/*
 *  Vanilla Javascript timepicker that allows setting of minTime and maxTime
 *
 *        View below code for a list of available methods
 *
 *  Developer: Lance Jernigan
 *  Version: 1.0.4
 *
 */


/*
 *  Setup our arguments to pass to our timepicker
 *
 *  @args - format (boolean) - Whether to format the input value or leave in 24 hour
 *          minTime (string) - Minimum time the timepicker should reach (any valid time string Javascript's Date() will accept)
 *          maxTime (string) - Maximum time the timepicker should reach (any valid time string Javascript's Date() will accept)
 *          meridiem (boolean) - Whether the timepicker should display the meridiem (defaults to true if format is true and false if format is false)
 *          arrowColor (string) - Any valid color (Hex, RGB, RGBA, etc.) to use for the arrows
 *
 */

var args = {
    // format: true,
    // minTime: '2:00 am',
    // maxTime: '1:00 pm',
    // meridiem: false
}

/*
 *  Create a new timepicker for our input and pass it our args
 */

var tpicker = new timepicker(document.querySelector('input.timepicker'), args)

/*
 *  Starts our Timepicker Functionality
 */

function timepicker(element, args) {

    this.initialized = false
    this.element = null
    this.elements = {}
    this.timepicker = null
    this.time = new Date()
    this.settings = {
        format: true,
        meridiem: true,
        minTime: new Date(new Date().toDateString() + ' 00:00'),
        maxTime: new Date(new Date().toDateString() + ' 24:00'),
        onChange: false
    }
    this.active = false

    this.updateSettings = function (args) {

        args = args || {}

        for (a = 0; a < Object.keys(args).length; a++) {

            var key = Object.keys(args)[a]
            var val = args[Object.keys(args)[a]]

            this.settings[key] = args[Object.keys(args)[a]]

        }

        if (!this.settings.format && typeof args.meridiem == 'undefined') {

            this.settings.meridiem = false

        }

        this.settings.meridiem = this.settings.format ? true : this.settings.meridiem
        this.settings.minTime = !(this.settings.minTime.getDate !== undefined || this.settings.minTime.getDate !== null) ? new Date(new Date().toDateString() + ' ' + this.settings.minTime) : new Date(new Date().toDateString() + ' 00:00')
        this.settings.maxTime = !(this.settings.maxTime.getDate !== undefined || this.settings.maxTime.getDate !== null) ? new Date(new Date().toDateString() + ' ' + this.settings.maxTime) : this.settings.maxTime

        if (this.settings.maxTime.toString() == this.settings.minTime.toString()) {

            var maxTime = new Date(this.settings.minTime)

            maxTime.setHours(maxTime.getHours() + 24)

            this.settings.maxTime = maxTime

        }

        if (this.element.value) {

            var newTime = new Date(new Date().toDateString() + ' ' + this.element.value)

            this.time = !isNaN(newTime.getTime()) ? newTime : this.time

        }

        this.time.setMilliseconds(0)

        if (Object.keys(this.elements).length) {

            this.updateTime('minute', true, 0)

            this.render()

        }

        if (!this.validateTime()) {

            this.time = this.settings.minTime ? this.settings.minTime : this.settings.maxTime

        }

    }

    this.buildTimepicker = function () {

        var wrapper = document.createElement('div')
        var elements = ['hour', 'minute', 'meridiem']

        wrapper.className = 'timepicker__wrapper'
        wrapper.setAttribute('id', 'tp_' + (Math.floor(Math.random() * 100) + 1))

        if (!Object.keys(this.elements).length) {

            for (e = 0; e < elements.length; e++) {

                this.elements[elements[e]] = document.createElement('div')
                this.elements[elements[e]].className = 'timepicker__' + elements[e]

                var up = document.createElement('div')
                up.appendChild(document.createElement('div'))
                var display = document.createElement('p')
                var down = document.createElement('div')
                down.appendChild(document.createElement('div'))

                up.className = 'timepicker__button timepicker__button__up'
                display.className = 'display'
                down.className = 'timepicker__button timepicker__button__down'

                if (this.settings.arrowColor) {

                    up.childNodes[0].style['border-bottom-color'] = this.settings.arrowColor
                    down.childNodes[0].style['border-top-color'] = this.settings.arrowColor

                }

                this.elements[elements[e]].appendChild(up)
                this.elements[elements[e]].appendChild(display)
                this.elements[elements[e]].appendChild(down)

            }

        }

        this.timepicker = wrapper

        this.element.parentNode.insertBefore(wrapper, this.element.nextSibling)

        this.addListeners()

        this.render()

    }

    this.render = function () {

        var wrapper = this.cleanWrapper(this.timepicker)

        if (this.settings.meridiem) {

            wrapper.className = wrapper.className.indexOf(' timepicker__wrapper-full') >= 0 ? wrapper.className : wrapper.className + ' timepicker__wrapper-full'

        }

        for (e = 0; e < Object.keys(this.elements).length; e++) {

            var key = Object.keys(this.elements)[e]
            var element = this.elements[key]
            var func = 'get' + key.charAt(0).toUpperCase() + key.slice(1)

            element.querySelector('.display').innerText = this[func]()

            if (Object.keys(this.elements)[e] == 'meridiem' && !this.settings.meridiem) {

                continue

            }

            wrapper.appendChild(element)

        }

        this.timepicker = wrapper

        this.updateInput()

    }

    this.cleanWrapper = function (wrapper) {

        while (wrapper.hasChildNodes()) {

            wrapper.removeChild(wrapper.lastChild)

        }

        return wrapper

    }

    this.handleClick = function (e) {

        var element = e.currentTarget

        var parent = element.parentNode.className.replace('timepicker__', '')
        var add = element.className.indexOf('up') !== -1 ? true : false

        this.updateTime(parent, add)

    }

    this.validateInput = function (e) {

        var value = e.currentTarget.value
        var date = value.length ? new Date(new Date().toDateString() + ' ' + value) : false

        if (date && !isNaN(date.getTime())) {

            this.time = date

        }

        if (!this.validateTime()) {

            var after = date.getTime() > this.settings.maxTime.getTime()
            date = after ? new Date(this.settings.maxTime) : new Date(this.settings.minTime)
            after ? date.setMinutes(date.getMinutes() - 1) : date.setMinutes(date.getMinutes() + 1)

            this.time = date

        }

        this.render()

    }

    this.updateTime = function (method, add, amount) {

        var amount = amount || 1

        switch (method) {

            case 'meridiem':

                this.time.getHours() > 12 ? this.time.setHours(this.time.getHours() - 12) : this.time.setHours(this.time.getHours() + 12)

                break

            default:

                if (add) {

                    this.add(method, amount)

                } else {

                    this.subtract(method, amount)

                }

        }

        if (!this.validateTime()) {

            var date = add ? new Date(this.settings.minTime) : new Date(this.settings.maxTime)
            add ? date.setMinutes(date.getMinutes() + 1) : date.setMinutes(date.getMinutes() - 1)

            this.time = date

        }

        this.render()

    }

    this.add = function (method, amount) {

        var amount = amount || 1

        switch (method) {

            case 'minute':

                this.time.setMinutes(this.time.getMinutes() + amount)

                break

            case 'hour':

                this.time.setHours(this.time.getHours() + amount)

                break

        }

    }

    this.subtract = function (method, amount) {

        var amount = amount || 1

        switch (method) {

            case 'minute':

                this.time.setMinutes(this.time.getMinutes() - amount)

                break

            case 'hour':

                this.time.setHours(this.time.getHours() - amount)

                break

        }

    }

    this.validateTime = function () {

        if (this.settings.minTime) {

            this.settings.maxTime = this.settings.maxTime

            this.time.setDate(new Date().getDate())

            return this.time.getTime() < this.settings.maxTime.getTime() && this.time.getTime() > this.settings.minTime.getTime()

        }

        return true

    }

    this.updateInput = function (parent) {

        if (this.initialized) {

            this.element.value = this.buildString()

        }

    }

    this.buildString = function () {

        return (this.getHour() + ":" + this.getMinute() + ' ' + this.getMeridiem()).trim()

    }

    this.toggleActive = function (e) {

        if (e.target == this.element) {

            if (!this.initialized) {

                this.initialized = true

                this.updateInput()

            }

            this.updateBounds(this.timepicker, e.target)

            this.active = true

        } else if (e.target.className.indexOf('timepicker__') == -1 && e.target.parentElement.className.indexOf('timepicker__') == -1) {

            this.active = false

        }

        this.timepicker.className = this.active ? this.timepicker.className.indexOf(' timepicker__wrapper-active') >= 0 ? this.timepicker.className : this.timepicker.className + ' timepicker__wrapper-active' : this.timepicker.className.replace(' timepicker__wrapper-active', '')

    }

    this.updateBounds = function () {

        var bounds = this.element.getBoundingClientRect()

        this.timepicker.style.top = this.element.offsetTop + this.element.innerHeight + 'px'
        this.timepicker.style.width = bounds.width + 'px'

    }

    this.addListeners = function () {

        var elements = Object.keys(this.elements)

        for (e = 0; e < elements.length; e++) {

            var element = this.elements[elements[e]]
            var buttons = [].slice.call(element.childNodes).filter(function (node) {

                return node.className.indexOf('button') !== -1

            })

            for (c = 0; c < buttons.length; c++) {

                var button = buttons[c]

                button.addEventListener('click', this.handleClick.bind(this))

            }

        }

        this.element.addEventListener('change', this.validateInput.bind(this))
        document.body.addEventListener('click', this.toggleActive.bind(this))
        window.addEventListener('resize', this.updateBounds.bind(this))

    }

    this.getTime = function () {
        return this.time
    }

    this.getHour = function () {

        if (!this.settings.format) {

            return this.time.getHours() < 10 ? '0' + this.time.getHours() : this.time.getHours()

        } else {

            return this.time.getHours() > 12 ? this.time.getHours() % 12 : this.time.getHours() == 0 ? 12 : this.time.getHours()

        }

    }

    this.getMinute = function () {

        var minutes = this.time.getMinutes()

        return minutes < 10 ? '0' + minutes : minutes

    }

    this.getMeridiem = function () {

        if (!this.settings.meridiem) {

            return ''

        } else {

            return this.time.getHours() >= 12 ? 'pm' : 'am'

        }

    }

    this.init = function () {

        if (element.length) {

            console.warn('Timepicker selector must be for a specific element, not a list of elements.')

            return

        }

        this.element = element

        this.updateSettings(args)
        this.buildTimepicker()

    }

    this.init()

}

/*
 *  Timepicker Methods
 *
 *  updateSettings()
 *
 *     Update the settings originally passed to your timepicker
 *
 *     @parameters - args (a list of available arguments is provided above the code)
 *
 *
 *  updateTime()
 *
 *     Update the time based on parameters passed
 *
 *     @parameters - method (string) - What method to affect ('hour', 'minute', 'meridiem')
 *                   add (boolean) - True to add amount, false to subtract amount
 *                   amount /optional/ (number) - Number to add or subtract from method (defaults to 1)
 *
 *
 *  add()
 *
 *     Add amount to selected method
 *
 *     @parameters - method (string) - What method to affect ('hour', 'minute')
 *                   amount /optional/ (number) - Number to add to method (defaults to 1)
 *
 *
 *  subtract()
 *
 *     subtract amount from selected method
 *
 *     @parameters - method (string) - What method to affect ('hour', 'minute')
 *                   amount /optional/ (number) - Number to subtract from method (defaults to 1)
 *
 *
 *  buildString()
 *
 *     Returns the string that will be sent to the input
 *
 *
 *  getTime()
 *
 *     Returns the date object for the current selected time
 *
 *
 *  getHour()
 *
 *     Returns the current hour for the timepicker
 *
 *
 *  getMinute()
 *
 *     Returns the current Minute for the timepicker
 *
 *
 *  getMeridiem()
 *
 *     Returns the current Meridiem for the timepicker
 *
 *
 *  get
 */
tpicker.updateSettings({ minTime: '2:00 am' })
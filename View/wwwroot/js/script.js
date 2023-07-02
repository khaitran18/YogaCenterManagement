(function () {
    "use strict";
    var yg = {
        init: function () {
            this.cacheDom();
            this.bindEvents();
            this.initSlider();
            this.navOverlay();
            this.totopButton();
            this.stickyHeader();
            this.enableGridGallery();
            this.enablePopupGallery();
        }
        , cacheDom: function () {
            this.toTop = $('.totop');
            this._body = $('body');
            this.ygHomepageSlider = $('.yg-slider');
            this.ygMenuTrigger = $('.yg-hamburger-trigger');
            this.ygMainMenu = $('.yg-nav-overlay-main-nav');
            this.ygOverlayMenuHolder = $('.yg-nav-overlay');
            this.ygOverlayMenuClose = $('.yg-nav-overlay-close');
            this.ygMenuLinks = $('.yg-nav-overlay-main-nav li a');
            this.ygGalleryTabs = $('.yg-toolbar-item');
            this.ygGalleryItem = $('.yg-gallery-item');
        }
        , bindEvents: function () {
            var self = this;
            this.ygGalleryTabs.on('click', self.changeActiveTab);
            this.ygGalleryTabs.on('click', self.addGalleryFilter);
            $(window).on('load', self.enablePreloader);
        }
        , /* popup gallery */
        enablePopupGallery: function () {
            $('.yg-popup-gallery').each(function () {
                $(this).magnificPopup({
                    delegate: 'a'
                    , type: 'image'
                    , gallery: {
                        enabled: true
                    }
                });
            });
        }
        , /* preloader */
        enablePreloader: function () {
            var preloader = $('#yg-page-loading');
            if (preloader.length > 0) {
                preloader.fadeOut("slow", function () {
                    preloader.remove();
                });
            }
        }
        , /* gallery tab */
        changeActiveTab: function () {
            $(this).closest('.yg-gallery-toolbar').find('.active').removeClass('active');
            $(this).addClass('active');
        }
        , /* gallery filter */
        addGalleryFilter: function () {
            var value = $(this).attr('data-filter');
            if (value === 'all') {
                yg.ygGalleryItem.show('3000');
            }
            else {
                yg.ygGalleryItem.not('.' + value).hide('3000');
                yg.ygGalleryItem.filter('.' + value).show('3000');
            }
        }
        , /* slider */
        initSlider: function () {
            var self = this;
            /* homepage slider */
            self.ygHomepageSlider.slick({
                infinite: true
                , dots: true
                , autoplay: true
                , autoplaySpeed: 4000
                , arrows: true
                , slidesToShow: 1
                , slidesToScroll: 1
                , responsive: [
                    {
                        breakpoint: 768
                        , settings: {
                            slidesToShow: 1
                            , slidesToScroll: 1
                        }
			}
			]
            });
        }
        , /* navigation overlay*/
        navOverlay: function () {
            var self = this;
            if (self.ygMainMenu.length > 0) {
                var closeMenu = function () {
                    self.ygOverlayMenuHolder.removeClass('is-active');
                    self.ygOverlayMenuHolder.addClass('yg-nav-overlay-closed');
                    self.ygMenuTrigger.removeClass('is-active');
                    setTimeout(function () {
                        self._body.css('overflow', '');
                    }, 700);
                };
                var openMenu = function () {
                    self.ygOverlayMenuHolder.addClass('is-active');
                    self.ygOverlayMenuHolder.removeClass('yg-nav-overlay-closed');
                    self.ygMenuTrigger.addClass('is-active');
                    self._body.css('overflow', 'hidden');
                };
                var toggleOpen = function () {
                    if (self.ygOverlayMenuHolder.hasClass('is-active')) {
                        closeMenu();
                    }
                    else {
                        openMenu();
                    }
                };
                /* Open menu trigger */
                self.ygMenuTrigger.on('click', function (e) {
                    e.preventDefault();
                    toggleOpen();
                });
                /* Close Button */
                self.ygOverlayMenuClose.on('click', function (e) {
                    e.preventDefault();
                    toggleOpen();
                });
                /* Close menu if the menu links are clicked */
                self.ygMenuLinks.on('click', function (e) {
                    self.ygMainMenu.find('li .active').removeClass('active');
                    $(this).addClass('active');
                    toggleOpen();
                    // Get the link id
                    var $link = $(this)
                        , linkAttribute = $link.attr('href')
                        , sectionId = linkAttribute.substring(linkAttribute.indexOf('#'))
                        , $section = $(sectionId);
                    if ($section.length !== 0) {
                        e.preventDefault();
                    }
                    var positionToTop = $section.offset().top
                        , topOffset = $link.data('offset');
                    // Check if link has offset
                    if (topOffset) {
                        positionToTop = positionToTop + topOffset;
                    }
                    // Scroll to element
                    $('html, body').animate({
                        scrollTop: positionToTop
                    }, 'slow');
                });
            }
        }
        , /* ======= toTop ======= */
        totopButton: function () {
            var self = this;
            /* Show totop button*/
            $(window).scroll(function () {
                var toTopOffset = self.toTop.offset().top;
                var toTopHidden = 1000;
                if (toTopOffset > toTopHidden) {
                    self.toTop.addClass('totop-vissible');
                }
                else {
                    self.toTop.removeClass('totop-vissible');
                }
            });
            /* totop button animation */
            if (self.toTop && self.toTop.length > 0) {
                self.toTop.on('click', function (e) {
                    e.preventDefault();
                    $('html, body').animate({
                        scrollTop: 0
                    }, 'slow');
                });
            }
        }
        , /* ======= sticky header ======= */
        stickyHeader: function () {
            var $el = $(".yg-sticky-header")
                , headerHeight = $el.find('.yg-navbar-container').outerHeight();
            $(window).on('scroll', function (event) {
                if ($(window).scrollTop() > headerHeight) {
                    $el.removeClass('header--not-sticked');
                    $el.addClass('header--is-sticked');
                }
                else {
                    $el.removeClass('header--is-sticked');
                    $el.addClass('header--not-sticked');
                }
            });
        }
        , /* ======= grid gallery ======= */
        enableGridGallery: function () {
            $('.yg-grid-gallery').each(function (i, el) {
                var item = $(el).find('.yg-grid-item');
                $(el).masonry({
                    itemSelector: '.yg-grid-item'
                    , columnWidth: '.yg-grid-item'
                    , horizontalOrder: true
                });
            });
        }
    };
    // Animations
    var contentWayPoint = function () {
        var i = 0;
        $('.animate-box').waypoint(function (direction) {
            if (direction === 'down' && !$(this.element).hasClass('animated')) {
                i++;
                $(this.element).addClass('item-animate');
                setTimeout(function () {
                    $('body .animate-box.item-animate').each(function (k) {
                        var el = $(this);
                        setTimeout(function () {
                            var effect = el.data('animate-effect');
                            if (effect === 'fadeIn') {
                                el.addClass('fadeIn animated');
                            }
                            else if (effect === 'fadeInLeft') {
                                el.addClass('fadeInLeft animated');
                            }
                            else if (effect === 'fadeInRight') {
                                el.addClass('fadeInRight animated');
                            }
                            else {
                                el.addClass('fadeInUp animated');
                            }
                            el.removeClass('item-animate');
                        }, k * 200, 'easeInOutExpo');
                    });
                }, 100);
            }
        }, {
            offset: '85%'
        });
    };
    $(function () {
        contentWayPoint();
    });
    yg.init();
})();

    // Mouse effect 
    function mousecursor() {
        if ($("body")) {
            const e = document.querySelector(".cursor-inner"),
                t = document.querySelector(".cursor-outer");
            let n, i = 0,
                o = !1;
            window.onmousemove = function (s) {
                o || (t.style.transform = "translate(" + s.clientX + "px, " + s.clientY + "px)"), e.style.transform = "translate(" + s.clientX + "px, " + s.clientY + "px)", n = s.clientY, i = s.clientX
            }, $("body").on("mouseenter", "a, .cursor-pointer", function () {
                e.classList.add("cursor-hover"), t.classList.add("cursor-hover")
            }), $("body").on("mouseleave", "a, .cursor-pointer", function () {
                $(this).is("a") && $(this).closest(".cursor-pointer").length || (e.classList.remove("cursor-hover"), t.classList.remove("cursor-hover"))
            }), e.style.visibility = "visible", t.style.visibility = "visible"
        }
    };
    $(function () {
        mousecursor();
    });


// Contact Form
    var form = $('.contact__form'),
        message = $('.contact__msg'),
        form_data;
    // success function
    function done_func(response) {
        message.fadeIn().removeClass('alert-danger').addClass('alert-success');
        message.text(response);
        setTimeout(function () {
            message.fadeOut();
        }, 2000);
        form.find('input:not([type="submit"]), textarea').val('');
    }
    // fail function
    function fail_func(data) {
        message.fadeIn().removeClass('alert-success').addClass('alert-success');
        message.text(data.responseText);
        setTimeout(function () {
            message.fadeOut();
        }, 2000);
    }
    form.submit(function (e) {
        e.preventDefault();
        form_data = $(this).serialize();
        $.ajax({
            type: 'POST',
            url: form.attr('action'),
            data: form_data
        })
        .done(done_func)
        .fail(fail_func);
    });

var currentPage = window.location.pathname;
var links = document.querySelectorAll('.navbar__list a, .pagination a');

for (var i = 0; i < links.length; i++) {
    var linkHref = links[i].getAttribute('href');
    var linkPath = linkHref.split('?')[0];

    if (linkPath === currentPage) {
        links[i].parentNode.classList.add('active');
        break;
    }
}

document.addEventListener('DOMContentLoaded', function () {
    // Filter button click event
    var filterBtn = document.getElementById('filterBtn');
    filterBtn.addEventListener('click', function () {
        applyFilters();
    });

    // Selection change event for filter dropdowns
    var filterSelects = document.querySelectorAll('.filter-select');
    filterSelects.forEach(function (select) {
        select.addEventListener('change', function () {
            applyFilters();
        });
    });

    // Function to apply filters
    function applyFilters() {
        var roleIds = getSelectedValues('roleIds'); // Get selected roleIds
        var disabled = null; // Get selected disabled status
        var verified = null; // Get selected verified status
        var sortBy = null; // Get selected sort option

        // Build the new URL with selected filter/sort parameters
        var newUrl = '@Url.Action("Users", "Admin")' +
            '?roleIds=' + roleIds +
            '&disabled=' + disabled +
            '&verified=' + verified +
            '&sortBy=' + sortBy;

        // Redirect to the new URL
        window.location.href = newUrl;
    }

    // Function to get selected values from a multi-select dropdown
    function getSelectedValues(selectName) {
        var select = document.querySelector('select[name="' + selectName + '"]');
        var selectedOptions = Array.from(select.options)
            .filter(option => option.selected)
            .map(option => option.value);
        return selectedOptions.join(',');
    }
});
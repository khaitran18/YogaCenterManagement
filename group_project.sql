create database YGC
use YGC

create table [Roles](
role_id int primary key identity (1,1),
role_name varchar(20) not null,
)

create table [Users](
[uid] int primary key identity(1,1),
[user_name] varchar(50) not null,
[password] varchar(500) not null,
full_name nvarchar(50) not null,
[address] nvarchar(150) null,
phone varchar(12) null,
[role_id] int foreign key references [Roles](role_id)
)

create table DateOfWeek(
day_id int primary key identity(1,1),
[day] varchar(30) not null
)

create table StudySlot(
slot_id int primary key identity(1,1),
start_time time not null,
end_time time not null
)

create table StudySlotDay(
slot_id int foreign key references StudySlot(slot_id) not null,
day_id int foreign key references DateOfWeek(day_id) not null,
primary key (slot_id, day_id)
)

create table AvailableDate(
lecturer_id int,
[date] datetime ,
slot_id int foreign key references [StudySlot] (slot_id), foreign key (lecturer_id) references Users(uid)
)

create table Class(
class_id int primary key identity (1,1),
class_name nvarchar(255) not null,
[start_date] datetime2 not null,
[end_date] datetime2 not null,

lecturer_id int foreign key references Users([uid])
)

create table Enrollment(
student_id int foreign key references Users([uid]),
class_id int foreign key references Class(class_id)

primary key (student_id, class_id)

)

create table Schedule(
id int primary key identity (1,1),
class_id int foreign key references Class(class_id),
slot_id int foreign key references StudySlot(slot_id),
[date] datetime2 not null,
daily_note nvarchar(255)
)

create table Feedback(
feedback_id int primary key identity (1,1),
[time] datetime2,
content nvarchar(255),
student_id int foreign key references Users([uid]),
lecturer_id int foreign key references Users([uid])

)

create table Payment(
payment_id int primary key identity(1,1),
student_id int foreign key references Users([uid]),
amount money not null,
method nvarchar(50)
)
USE [master]
GO
/****** Object:  Database [YGC2]    Script Date: 7/6/2023 9:19:59 PM ******/
CREATE DATABASE [YGC2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'YGC2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\YGC2.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'YGC2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\YGC2_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [YGC2] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [YGC2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [YGC2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [YGC2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [YGC2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [YGC2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [YGC2] SET ARITHABORT OFF 
GO
ALTER DATABASE [YGC2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [YGC2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [YGC2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [YGC2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [YGC2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [YGC2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [YGC2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [YGC2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [YGC2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [YGC2] SET  ENABLE_BROKER 
GO
ALTER DATABASE [YGC2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [YGC2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [YGC2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [YGC2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [YGC2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [YGC2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [YGC2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [YGC2] SET RECOVERY FULL 
GO
ALTER DATABASE [YGC2] SET  MULTI_USER 
GO
ALTER DATABASE [YGC2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [YGC2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [YGC2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [YGC2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [YGC2] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'YGC2', N'ON'
GO
ALTER DATABASE [YGC2] SET QUERY_STORE = OFF
GO
USE [YGC2]
GO
/****** Object:  Table [dbo].[AvailableDate]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvailableDate](
	[lecturer_id] [int] NOT NULL,
	[date] [datetime] NULL,
	[slot_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[lecturer_id] ASC,
	[slot_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChangeClassRequests]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChangeClassRequests](
	[request_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[class_id] [int] NULL,
	[content] [nvarchar](500) NULL,
	[request_class_id] [int] NULL,
	[is_approved] [smallint] NULL,
PRIMARY KEY CLUSTERED 
(
	[request_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[class_id] [int] IDENTITY(1,1) NOT NULL,
	[class_name] [nvarchar](255) NOT NULL,
	[start_date] [datetime2](7) NOT NULL,
	[end_date] [datetime2](7) NOT NULL,
	[lecturer_id] [int] NULL,
	[price] [float] NOT NULL,
	[class_capacity] [int] NOT NULL,
	[description] [nvarchar](max) NULL,
	[image] [nvarchar](255) NULL,
	[class_status] [smallint] NULL,
PRIMARY KEY CLUSTERED 
(
	[class_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DateOfWeek]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DateOfWeek](
	[day_id] [int] NOT NULL,
	[day] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[day_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollment]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment](
	[student_id] [int] NOT NULL,
	[class_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[student_id] ASC,
	[class_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[feedback_id] [int] IDENTITY(1,1) NOT NULL,
	[time] [datetime2](7) NULL,
	[content] [nvarchar](255) NULL,
	[student_id] [int] NULL,
	[lecturer_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[feedback_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[student_id] [int] NULL,
	[amount] [money] NOT NULL,
	[method] [nvarchar](50) NULL,
	[class_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[role_id] [int] NOT NULL,
	[role_name] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedule]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedule](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[class_id] [int] NULL,
	[slot_id] [int] NULL,
	[date] [datetime2](7) NOT NULL,
	[daily_note] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudySlot]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudySlot](
	[slot_id] [int] IDENTITY(1,1) NOT NULL,
	[start_time] [time](7) NOT NULL,
	[end_time] [time](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[slot_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudySlotDay]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudySlotDay](
	[slot_id] [int] NOT NULL,
	[day_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[slot_id] ASC,
	[day_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/6/2023 9:20:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[uid] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](50) NOT NULL,
	[password] [varchar](500) NOT NULL,
	[full_name] [nvarchar](50) NOT NULL,
	[address] [nvarchar](150) NULL,
	[phone] [varchar](12) NULL,
	[role_id] [int] NULL,
	[is_verified] [bit] NOT NULL,
	[is_disabled] [bit] NOT NULL,
	[disabled_reason] [nvarchar](max) NULL,
	[verification_token] [nchar](36) NULL,
	[email] [varchar](320) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[AvailableDate] ([lecturer_id], [date], [slot_id]) VALUES (5, NULL, 1)
GO
INSERT [dbo].[AvailableDate] ([lecturer_id], [date], [slot_id]) VALUES (5, NULL, 2)
GO
INSERT [dbo].[AvailableDate] ([lecturer_id], [date], [slot_id]) VALUES (5, NULL, 3)
GO
INSERT [dbo].[AvailableDate] ([lecturer_id], [date], [slot_id]) VALUES (39, CAST(N'2023-07-06T16:01:15.300' AS DateTime), 1)
GO
INSERT [dbo].[AvailableDate] ([lecturer_id], [date], [slot_id]) VALUES (41, CAST(N'2023-07-06T19:22:10.987' AS DateTime), 1)
GO
INSERT [dbo].[AvailableDate] ([lecturer_id], [date], [slot_id]) VALUES (41, CAST(N'2023-07-06T19:28:14.063' AS DateTime), 9)
GO
SET IDENTITY_INSERT [dbo].[ChangeClassRequests] ON 
GO
INSERT [dbo].[ChangeClassRequests] ([request_id], [user_id], [class_id], [content], [request_class_id], [is_approved]) VALUES (1, 22, 1, N'Reason', 2, 0)
GO
INSERT [dbo].[ChangeClassRequests] ([request_id], [user_id], [class_id], [content], [request_class_id], [is_approved]) VALUES (2, 35, 33, N'Thichs', 34, 1)
GO
INSERT [dbo].[ChangeClassRequests] ([request_id], [user_id], [class_id], [content], [request_class_id], [is_approved]) VALUES (3, 40, 36, N'I enrolled the wrong class, sorry', 37, 1)
GO
SET IDENTITY_INSERT [dbo].[ChangeClassRequests] OFF
GO
SET IDENTITY_INSERT [dbo].[Class] ON 
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (1, N'Hatha Yoga', CAST(N'2023-10-10T00:00:00.0000000' AS DateTime2), CAST(N'2023-10-11T00:00:00.0000000' AS DateTime2), 5, 200000, 35, N'Join our Hatha Yoga class and experience the perfect blend of relaxation and physical postures. This class focuses on balancing mind and body through gentle stretches, controlled breathing, and meditation. Suitable for all fitness levels, Hatha Yoga is ideal for beginners and those seeking a peaceful practice that enhances flexibility, strength, and overall well-being.', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FTest?alt=media&token=1688379378117485', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (2, N'Vinyasa Flow Yoga', CAST(N'2023-10-10T00:00:00.0000000' AS DateTime2), CAST(N'2023-10-11T00:00:00.0000000' AS DateTime2), 5, 300000, 35, N'Step into the dynamic world of Vinyasa Flow Yoga, where movement and breath unite in a seamless flow. This energizing class emphasizes fluid transitions between poses, synchronizing each movement with deep inhales and exhales. With its emphasis on building strength, improving flexibility, and cultivating mindfulness, Vinyasa Flow Yoga is perfect for those seeking a more invigorating and empowering practice.', N'https://i.ytimg.com/vi/wLC7a1XuPxU/maxresdefault.jpg', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (3, N'Restorative Yoga', CAST(N'2023-06-25T17:45:12.1850000' AS DateTime2), CAST(N'2023-07-01T17:45:12.1850000' AS DateTime2), NULL, 100000, 30, N'Indulge in the ultimate relaxation with our Restorative Yoga class. Designed to release tension and restore balance, this gentle practice incorporates supported postures, deep breathing, and guided relaxation. Ideal for stress relief and rejuvenation, Restorative Yoga offers a nurturing space to unwind, soothe the nervous system, and promote deep relaxation for both body and mind.', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FTest?alt=media&token=1688379378117485', 3)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (4, N'Power Yoga', CAST(N'2023-06-26T18:06:55.2720000' AS DateTime2), CAST(N'2023-07-04T18:06:55.2720000' AS DateTime2), NULL, 200000, 35, N'Ignite your inner strength with Power Yoga, a dynamic and challenging practice that combines strength, flexibility, and endurance. This class incorporates a faster-paced sequence of postures, empowering you to build heat, increase stamina, and develop core stability. Whether you''re looking to enhance physical fitness or elevate your yoga practice, Power Yoga offers a vigorous and rewarding experience.', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FTest?alt=media&token=1688379378117485', 3)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (5, N'Test', CAST(N'2023-07-03T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-11T00:00:00.0000000' AS DateTime2), NULL, 10000, 30, N'This is the description of the class', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FTest?alt=media&token=1688379378117485', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (17, N'Update class', CAST(N'2023-07-04T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-11T00:00:00.0000000' AS DateTime2), NULL, 200000, 40, NULL, N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FClass%20Yoga?alt=media&token=1688399533982486', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (18, N'Class Yoga Gud kaka', CAST(N'2023-07-04T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-11T00:00:00.0000000' AS DateTime2), NULL, 300000, 30, N'wioer', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FClass%20Yoga%20Gud?alt=media&token=1688399674863410', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (20, N'Beginner Class', CAST(N'2023-07-04T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-18T00:00:00.0000000' AS DateTime2), 29, 200000, 30, N'Hehe', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FBeginner%20Class?alt=media&token=1688399981334655', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (21, N'Intermediate Class', CAST(N'2023-07-05T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL, 4000000, 30, N'wioefjo', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FIntermediate%20Class?alt=media&token=1688400521182278', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (24, N'new class', CAST(N'2023-07-04T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL, 300000, 40, N'This is the new class', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2Fnew%20class?alt=media&token=1688477027767154', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (25, N'new class', CAST(N'2023-07-04T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL, 100000, 30, N'This is another class', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2Fnew%20class?alt=media&token=1688477233076518', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (26, N'new class', CAST(N'2023-07-04T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL, 200000, 50, N'This is the new class', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2Fnew%20class?alt=media&token=1688478192923463', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (27, N'The advance guru class', CAST(N'2023-07-04T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-18T00:00:00.0000000' AS DateTime2), 29, 500000, 10, N'This the class for people who have already had experience in yoga for more than 1 year.', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FThe%20advance%20class?alt=media&token=1688478826922247', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (28, N'Yoga center monthly class', CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL, 100000, 12, N'123', N'', 0)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (29, N'Yoga center monthly class', CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), NULL, 100000, 10, N'123', N'', 0)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (30, N'aaa', CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), NULL, 100000, 50, N'123', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2Faaa1512546722?alt=media&token=1688633488625078', 0)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (31, N'hello123', CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), NULL, 123123, 50, N'aaaa', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2Fhello123986495553?alt=media&token=1688634538442565', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (32, N'hahaterst', CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), 5, 100000, 50, N'123', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2Fhahaterst1524152900?alt=media&token=1688634581284346', 2)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (33, N'a', CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL, 100000, 50, N'1231', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2Fa1078108320?alt=media&token=1688634850742026', 1)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (34, N'aaaa', CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL, 123123, 50, N'123123', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2Faaaa1525095087?alt=media&token=1688635993657622', 0)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (35, N'Extra advance class', CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), CAST(N'2023-08-03T00:00:00.0000000' AS DateTime2), NULL, 500000, 15, N'A very advanced class for people with experience', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FExtra%20advance%20class672774069?alt=media&token=1688644469911135', 0)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (36, N'Extra advance class', CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), 41, 500000, 15, N'Extra advanced class for people with experience', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2FExtra%20advance%20class1248686652?alt=media&token=1688645171588986', 1)
GO
INSERT [dbo].[Class] ([class_id], [class_name], [start_date], [end_date], [lecturer_id], [price], [class_capacity], [description], [image], [class_status]) VALUES (37, N'aaaa', CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), 41, 500000, 50, N'huhu', N'https://firebasestorage.googleapis.com/v0/b/yoga-guru-391213.appspot.com/o/image%2Faaaa1555795063?alt=media&token=1688646325082608', 1)
GO
SET IDENTITY_INSERT [dbo].[Class] OFF
GO
INSERT [dbo].[DateOfWeek] ([day_id], [day]) VALUES (1, N'Monday')
GO
INSERT [dbo].[DateOfWeek] ([day_id], [day]) VALUES (2, N'Tuesday')
GO
INSERT [dbo].[DateOfWeek] ([day_id], [day]) VALUES (3, N'Wednesday')
GO
INSERT [dbo].[DateOfWeek] ([day_id], [day]) VALUES (4, N'Thursday')
GO
INSERT [dbo].[DateOfWeek] ([day_id], [day]) VALUES (5, N'Friday')
GO
INSERT [dbo].[DateOfWeek] ([day_id], [day]) VALUES (6, N'Saturday')
GO
INSERT [dbo].[DateOfWeek] ([day_id], [day]) VALUES (7, N'Sunday')
GO
INSERT [dbo].[Enrollment] ([student_id], [class_id]) VALUES (3, 2)
GO
INSERT [dbo].[Enrollment] ([student_id], [class_id]) VALUES (9, 1)
GO
INSERT [dbo].[Enrollment] ([student_id], [class_id]) VALUES (22, 1)
GO
INSERT [dbo].[Enrollment] ([student_id], [class_id]) VALUES (35, 20)
GO
INSERT [dbo].[Enrollment] ([student_id], [class_id]) VALUES (35, 27)
GO
INSERT [dbo].[Enrollment] ([student_id], [class_id]) VALUES (35, 34)
GO
INSERT [dbo].[Enrollment] ([student_id], [class_id]) VALUES (36, 27)
GO
INSERT [dbo].[Enrollment] ([student_id], [class_id]) VALUES (37, 33)
GO
INSERT [dbo].[Enrollment] ([student_id], [class_id]) VALUES (40, 37)
GO
SET IDENTITY_INSERT [dbo].[Feedback] ON 
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (1, CAST(N'2023-06-28T18:37:34.8537411' AS DateTime2), N'Good teacher!', 7, 5)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (2, CAST(N'2023-06-28T19:04:44.8155444' AS DateTime2), N'Cool!', 9, 5)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (3, CAST(N'2023-06-28T19:25:10.9193502' AS DateTime2), N'Nice!', 9, 7)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (4, CAST(N'2023-06-28T19:33:55.0464069' AS DateTime2), N'Yeah', 9, 5)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (5, CAST(N'2023-06-28T19:38:38.8173184' AS DateTime2), N'Marvelous', 9, 5)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (6, CAST(N'2023-06-28T20:02:31.8609857' AS DateTime2), N'Alright!', 9, 5)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (7, CAST(N'2023-07-04T20:52:19.3345672' AS DateTime2), N'Test!', 8, 5)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (8, CAST(N'2023-07-04T20:52:19.3340000' AS DateTime2), N'Tuyet voi xuat sac qua troi lun', 8, 19)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (9, CAST(N'2023-07-04T20:52:19.3340000' AS DateTime2), N'Nice sir', 8, 19)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (10, CAST(N'2023-07-04T20:52:19.3340000' AS DateTime2), N'Marvelous', 8, 19)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (11, CAST(N'2023-07-04T20:52:19.3340000' AS DateTime2), N'Alright!', 8, 19)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (12, CAST(N'2023-07-04T20:52:19.3340000' AS DateTime2), N'Nice sir', 8, 19)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (13, CAST(N'2023-07-06T14:34:17.6068267' AS DateTime2), N'aloalo', 35, 29)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (14, CAST(N'2023-07-06T14:35:29.7322071' AS DateTime2), N'aloalo kaka huhu', 35, 29)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (15, CAST(N'2023-07-06T14:35:47.0007679' AS DateTime2), N'aloalo kaka huhu', 35, 29)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (16, CAST(N'2023-07-06T14:36:20.5181480' AS DateTime2), N'aloalo kaka huhu', 35, 29)
GO
INSERT [dbo].[Feedback] ([feedback_id], [time], [content], [student_id], [lecturer_id]) VALUES (17, CAST(N'2023-07-06T16:39:59.0485144' AS DateTime2), N'aloaloaloalo', 35, 29)
GO
SET IDENTITY_INSERT [dbo].[Feedback] OFF
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 
GO
INSERT [dbo].[Payment] ([payment_id], [student_id], [amount], [method], [class_id]) VALUES (1, 9, 200000.0000, N'Internet', 1)
GO
INSERT [dbo].[Payment] ([payment_id], [student_id], [amount], [method], [class_id]) VALUES (2, 22, 200000.0000, N'Method', 1)
GO
INSERT [dbo].[Payment] ([payment_id], [student_id], [amount], [method], [class_id]) VALUES (3, 37, 100000.0000, N'Online', 33)
GO
INSERT [dbo].[Payment] ([payment_id], [student_id], [amount], [method], [class_id]) VALUES (4, 35, 100000.0000, N'Online', 33)
GO
INSERT [dbo].[Payment] ([payment_id], [student_id], [amount], [method], [class_id]) VALUES (5, 40, 500000.0000, N'Online', 36)
GO
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
INSERT [dbo].[Roles] ([role_id], [role_name]) VALUES (1, N'User')
GO
INSERT [dbo].[Roles] ([role_id], [role_name]) VALUES (2, N'Lecturer')
GO
INSERT [dbo].[Roles] ([role_id], [role_name]) VALUES (3, N'Staff')
GO
INSERT [dbo].[Roles] ([role_id], [role_name]) VALUES (4, N'Admin')
GO
SET IDENTITY_INSERT [dbo].[Schedule] ON 
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (3, 1, 1, CAST(N'2023-10-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (4, 2, 1, CAST(N'2023-10-12T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (5, 3, 1, CAST(N'2023-06-26T17:45:12.1850000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (6, 4, 1, CAST(N'2023-06-26T18:06:55.2720000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (7, 4, 1, CAST(N'2023-06-30T18:06:55.2720000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (8, 4, 1, CAST(N'2023-07-03T18:06:55.2720000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (9, 5, 1, CAST(N'2023-07-03T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (10, 5, 1, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (11, 18, 1, CAST(N'2023-07-05T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (12, 20, 2, CAST(N'2023-07-04T00:00:00.0000000' AS DateTime2), N'Bring mattress')
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (13, 20, 2, CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), N'huhu')
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (14, 20, 2, CAST(N'2023-07-11T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (15, 20, 2, CAST(N'2023-07-13T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (16, 20, 2, CAST(N'2023-07-18T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (17, 21, 2, CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (18, 21, 2, CAST(N'2023-07-08T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (19, 26, 2, CAST(N'2023-07-04T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (20, 26, 2, CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (21, 26, 2, CAST(N'2023-07-08T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (22, 26, 2, CAST(N'2023-07-11T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (23, 27, 4, CAST(N'2023-07-05T00:00:00.0000000' AS DateTime2), N'aaaaa')
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (24, 27, 4, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (25, 27, 4, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), N'Bring mattress')
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (26, 27, 4, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), N'Start 30 min early')
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (27, 27, 4, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (28, 27, 4, CAST(N'2023-07-17T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (29, 28, 1, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (30, 28, 1, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (31, 28, 1, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (32, 28, 1, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (33, 29, 1, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (34, 29, 1, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (35, 29, 1, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (36, 29, 1, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (37, 29, 1, CAST(N'2023-07-17T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (38, 29, 1, CAST(N'2023-07-19T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (39, 29, 1, CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (40, 29, 1, CAST(N'2023-07-24T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (41, 29, 1, CAST(N'2023-07-26T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (42, 29, 1, CAST(N'2023-07-28T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (43, 29, 1, CAST(N'2023-07-31T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (44, 29, 1, CAST(N'2023-08-02T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (45, 29, 1, CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (46, 30, 1, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (47, 30, 1, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (48, 30, 1, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (49, 30, 1, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (50, 30, 1, CAST(N'2023-07-17T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (51, 30, 1, CAST(N'2023-07-19T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (52, 30, 1, CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (53, 30, 1, CAST(N'2023-07-24T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (54, 30, 1, CAST(N'2023-07-26T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (55, 30, 1, CAST(N'2023-07-28T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (56, 30, 1, CAST(N'2023-07-31T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (57, 30, 1, CAST(N'2023-08-02T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (58, 30, 1, CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (59, 31, 1, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (60, 31, 1, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (61, 31, 1, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (62, 31, 1, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (63, 31, 1, CAST(N'2023-07-17T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (64, 31, 1, CAST(N'2023-07-19T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (65, 31, 1, CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (66, 31, 1, CAST(N'2023-07-24T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (67, 31, 1, CAST(N'2023-07-26T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (68, 31, 1, CAST(N'2023-07-28T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (69, 31, 1, CAST(N'2023-07-31T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (70, 31, 1, CAST(N'2023-08-02T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (71, 31, 1, CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (72, 32, 2, CAST(N'2023-07-06T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (73, 32, 2, CAST(N'2023-07-08T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (74, 32, 2, CAST(N'2023-07-11T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (75, 32, 2, CAST(N'2023-07-13T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (76, 33, 1, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (77, 33, 1, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (78, 33, 1, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (79, 33, 1, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (80, 33, 1, CAST(N'2023-07-17T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (81, 33, 1, CAST(N'2023-07-19T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (82, 33, 1, CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (83, 34, 1, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (84, 34, 1, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (85, 34, 1, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (86, 34, 1, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (87, 34, 1, CAST(N'2023-07-17T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (88, 34, 1, CAST(N'2023-07-19T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (89, 34, 1, CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (90, 35, 1, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (91, 35, 1, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (92, 35, 1, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (93, 35, 1, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (94, 35, 1, CAST(N'2023-07-17T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (95, 35, 1, CAST(N'2023-07-19T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (96, 35, 1, CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (97, 35, 1, CAST(N'2023-07-24T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (98, 35, 1, CAST(N'2023-07-26T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (99, 35, 1, CAST(N'2023-07-28T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (100, 35, 1, CAST(N'2023-07-31T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (101, 35, 1, CAST(N'2023-08-02T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (102, 36, 1, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), N'aaaa')
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (103, 36, 1, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (104, 36, 1, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), N'aaa')
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (105, 36, 1, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (106, 36, 1, CAST(N'2023-07-17T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (107, 36, 1, CAST(N'2023-07-19T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (108, 36, 1, CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (109, 36, 1, CAST(N'2023-07-24T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (110, 36, 1, CAST(N'2023-07-26T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (111, 36, 1, CAST(N'2023-07-28T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (112, 36, 1, CAST(N'2023-07-31T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (113, 36, 1, CAST(N'2023-08-02T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (114, 36, 1, CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (115, 37, 9, CAST(N'2023-07-07T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (116, 37, 9, CAST(N'2023-07-10T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (117, 37, 9, CAST(N'2023-07-12T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (118, 37, 9, CAST(N'2023-07-14T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (119, 37, 9, CAST(N'2023-07-17T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (120, 37, 9, CAST(N'2023-07-19T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (121, 37, 9, CAST(N'2023-07-21T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (122, 37, 9, CAST(N'2023-07-24T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (123, 37, 9, CAST(N'2023-07-26T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (124, 37, 9, CAST(N'2023-07-28T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (125, 37, 9, CAST(N'2023-07-31T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (126, 37, 9, CAST(N'2023-08-02T00:00:00.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[Schedule] ([id], [class_id], [slot_id], [date], [daily_note]) VALUES (127, 37, 9, CAST(N'2023-08-04T00:00:00.0000000' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[Schedule] OFF
GO
SET IDENTITY_INSERT [dbo].[StudySlot] ON 
GO
INSERT [dbo].[StudySlot] ([slot_id], [start_time], [end_time]) VALUES (1, CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time))
GO
INSERT [dbo].[StudySlot] ([slot_id], [start_time], [end_time]) VALUES (2, CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time))
GO
INSERT [dbo].[StudySlot] ([slot_id], [start_time], [end_time]) VALUES (3, CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time))
GO
INSERT [dbo].[StudySlot] ([slot_id], [start_time], [end_time]) VALUES (4, CAST(N'10:32:00' AS Time), CAST(N'11:32:00' AS Time))
GO
INSERT [dbo].[StudySlot] ([slot_id], [start_time], [end_time]) VALUES (5, CAST(N'10:37:00' AS Time), CAST(N'11:37:00' AS Time))
GO
INSERT [dbo].[StudySlot] ([slot_id], [start_time], [end_time]) VALUES (6, CAST(N'00:00:00' AS Time), CAST(N'00:00:00' AS Time))
GO
INSERT [dbo].[StudySlot] ([slot_id], [start_time], [end_time]) VALUES (7, CAST(N'09:00:00' AS Time), CAST(N'22:30:00' AS Time))
GO
INSERT [dbo].[StudySlot] ([slot_id], [start_time], [end_time]) VALUES (8, CAST(N'09:00:00' AS Time), CAST(N'10:30:00' AS Time))
GO
INSERT [dbo].[StudySlot] ([slot_id], [start_time], [end_time]) VALUES (9, CAST(N'20:00:00' AS Time), CAST(N'21:30:00' AS Time))
GO
SET IDENTITY_INSERT [dbo].[StudySlot] OFF
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (1, 1)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (1, 3)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (1, 5)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (2, 2)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (2, 4)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (2, 6)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (3, 2)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (3, 4)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (3, 6)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (4, 1)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (4, 3)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (4, 5)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (5, 1)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (5, 3)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (5, 5)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (6, 1)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (6, 3)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (6, 5)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (7, 1)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (7, 3)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (7, 4)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (8, 1)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (8, 3)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (8, 5)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (9, 1)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (9, 3)
GO
INSERT [dbo].[StudySlotDay] ([slot_id], [day_id]) VALUES (9, 5)
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (3, N'khaitran', N'1', N'Tran Quang Khai', N'23 Nguyen Hue', N'0919xxxx', 1, 0, 1, N'You have been banned for abusing other students', NULL, N'aaa@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (5, N'lecturer_1', N'1', N'Lecturer 1', N'23 ABC', N'0123456789', 2, 1, 0, NULL, NULL, N'hehe@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (6, N'Kabu', N'123', N'Nguyen', N'56 HCM', N'0947285615', 4, 1, 0, NULL, NULL, N'123@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (7, N'khaitran123', N'1', N'Tran Quang Khai', N'23 Nguyen Hue', N'0919xxxx', 1, 0, 1, N'You have been banned!', N'27b11440-784d-41d0-ba39-83c0a536f880', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (8, N'khaitran1234', N'1', N'Tran Quang Khai', N'23 Nguyen Hue', N'0919xxxx', 1, 0, 1, N'You have been banned!', N'1aa258f0-3b56-4a73-9cf4-a7c8202fe461', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (9, N'khaitran2', N'1', N'Tran Quang Khai', N'23 Nguyen Hue', N'0919xxxx', 1, 1, 0, NULL, N'73281ba6-058a-4d50-981c-5fa3445e1424', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (10, N'khaitran3', N'1', N'Tran Quang Khai', N'23 Nguyen Hue', N'0919xxxx', 1, 0, 0, NULL, N'bc32d565-0469-4bf5-a56b-b21f43167242', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (11, N'khaitran4', N'1', N'Tran Quang Khai', N'23 Nguyen Hue', N'0919xxxx', 1, 0, 0, NULL, N'89306044-5c01-4f96-8bd9-b3771d96a881', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (12, N'khaitran5', N'1', N'Tran Quang Khai', N'23 Nguyen Hue', N'0919xxxx', 1, 0, 0, NULL, N'8b130509-6f38-485d-83c9-0f7213d003d6', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (14, N'staff1', N'1', N'Nguyen Tho Nguyen', N'25 Nguyen Trai', N'129381923', 3, 0, 0, NULL, NULL, N'gasshu9@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (15, N'a', N'a', N'a', N'a', N'a', 1, 1, 0, NULL, N'165b0139-f0d0-4a54-8c81-b4aa9549ff02', N'admin@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (16, N'e', N'e', N'e', N'e', N'e', 1, 0, 0, NULL, N'13999012-0534-4a10-964d-218061f9629a', N'e@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (17, N'khaitranquang123', N'bGA4xA/2vf/FTb/EjfaMCA==;WFnsKG8BeN8eJRQdQU7EVJzhbGef8ab0unanIOwKf2s=', N'Khai Tran', N'aaaaa', N'123', 1, 1, 0, NULL, N'055178f0-dcb6-4741-b6b1-260f3c0af831', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (18, N'khaitranquang', N'PQmLi+8LV4edvGvLv52WrQ==;Zk20wO9zj9B3qSloc6oUj4JmQxdRiLGgU9uaC5Mrz/8=', N'Khai', N'a', N'a', 4, 1, 0, NULL, N'83168a54-3be2-4463-b5fb-29c37ab63ffe', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (19, N'KabuVG', N'nclhmpvafPzerhsu7F0WVQ==;3CSI7Y5SgeNRFZ/yaNmvPwcoS0mLNTbD2yKUF6PDZ6A=', N'NguyenThoNguyen', N'VietNam', N'0933424515', 2, 1, 0, NULL, N'c380f82b-845a-427d-b1b8-3f6d1a152e56', N'gasshu@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (20, N'Staff123', N'VUlc9D9Hch6eG2J6RNbAZA==;/y8ZpHpSTtJOL2qakBiGcjY4S5DG7HXpkCgM+CeRAcI=', N'Staff ne', N'haha', N'cuibap', 3, 1, 0, NULL, N'f4562e3a-bf64-4f38-a1ed-ba9aae7e647e', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (21, N'khaitranquang111', N'39njeH4YoaAhxx0Z+zZKvw==;A5CX9LVkJzJ/H/KJZQZiInQmpneMQuXnmOOubXYmFQM=', N'Tran Quang Khai', N'23 Nguyen Hue', N'0919xxxx', 1, 0, 0, NULL, N'ff51cc51-5408-4571-95ad-28d28e52d399', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (22, N'khaitranquang1112', N'ySEc9WiI2BDNHlWIyFxF5A==;Cbwcxt99FeICayTEr+v+UC1zFnOs7qow1wTsX6Wx7zY=', N'Tran Quang Khai', N'23 Nguyen Hue', N'0919xxxx', 1, 1, 0, NULL, N'558bd586-e741-424a-8e2b-7a55b91b5052', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (23, N'thinhngtri', N'MnxAC5j4baeqAZOIE/H4Cg==;EHfDdsv7qe87tBSRW4ywCQfzyOg1tI/I0t5k7r7aAo8=', N'Thinh', N'asdfasdf', N'0703100937', 4, 1, 0, NULL, N'42ee1834-7a6b-4b63-aa89-b2d54223f911', N'thinn2002@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (24, N'khaitranquangabc', N'Tgmksx8+c8yFz97/IcASyw==;vCa/joAn30iNL0JXYQc2YWq+l1a7HCzHXxiV/4GqnHY=', N'khai ne', NULL, NULL, 1, 1, 0, NULL, N'c533db16-ab41-4719-a654-9dcb356598e8', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (25, N'StaffKhaiTran', N'CREUlzMwXR507H6T0EyWQQ==;gnHHIgMTc6sg7iGIYFv6bO9AtX7npi3WxD41KkdlOOs=', N'Khai Tran Quang', NULL, NULL, 3, 0, 0, NULL, N'6da39661-bd88-48e3-9f88-448e84bdf889', N'a@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (26, N'khaiStaff2', N'XhXNS6GnrsEKbEWy7xnJsA==;Fa9lDyUjpnrF4pzfzOvYzO/eKI9d9UVQmYw4taP4Eng=', N'Khaitran', NULL, NULL, 3, 1, 0, NULL, N'a38b7bae-620c-47fa-a0d2-4f4141a3c0b4', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (27, N'KhainhungmalaStaff', N'63Rxajg6RaCvLujbbsmgTQ==;lHjLizv2HarjPRgfDOitR1bon0pmxQdo94jNZq7QpMk=', N'khai', NULL, NULL, 3, 1, 0, NULL, N'1c49b1d0-bdd1-4bc4-a7b1-cece810ec781', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (28, N'KhainhungmalaUser', N'86vqydieQLEb54svihVrDw==;m8VfJ2BuquX4tRzLXyFmf/UB3m5y4SJ6OpZP1rmNhSM=', N'a', NULL, NULL, 1, 1, 0, NULL, N'0142b378-37e2-4a52-8b73-ceb89ef9c8f6', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (29, N'KhainhungmalaLec', N'QXqdPa1ljvyC4yfLidow4w==;vkANOkm1cZzlTslDrgGBCianW2tTagd2TT1Avf4EGjM=', N'Khai', NULL, NULL, 2, 1, 0, NULL, N'3e2222a5-8b31-4948-8750-430843c644e2', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (30, N'khaiStaff01', N'Mc1WOfB7GgWQoxooSL23QQ==;XCe5mlG3Mq+ttUOHhxE62YWntxDPtyvBhJHOtxsQsKA=', N'Khai Tran Quang', NULL, NULL, 1, 1, 0, NULL, N'90cf1940-700a-425a-835a-b9b6b277c75f', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (31, N'khainhungmalaUser123', N'PqwlNfjIpWWKfeXMgkaEyA==;fbZ093VfXFlwPWt+ic6w7Dnp74GRtVIFfOBrZBAdk/g=', N'k', NULL, NULL, 1, 1, 0, NULL, N'61cfd594-3fc9-49f2-b0e9-c947b9906516', N'khaitqse161901@fpt.edu.vn')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (32, N'KhainhungmalaUserAbc', N'34UFVHZjWkb/nuqhK5TTFQ==;vTiaAw7nEC9L06ZjW2GM6tPEJV11SPx8KUkqpz+yCIA=', N'k', NULL, NULL, 1, 1, 0, NULL, N'0276e3c9-b714-4803-b83c-5e77e36691f0', N'khaitqse161901@fpt.edu.vn')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (33, N'Khainenhe', N'EvnIMnrZmor35dnii+lBEQ==;d1VIhakrzAe9cjvUETyacZ719wtZQvEYHCnC1MzyOAE=', N'Khai', NULL, NULL, 1, 1, 0, NULL, N'2746c661-fbff-43c1-9a05-a0ada9b79205', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (34, N'khainenha01', N'cID3Z+bivlrU0xDyz532Sg==;1at43O8V1gMkNKBjY95RuhUSNyLLtl83NLjfy5cJTL4=', N'Khai', NULL, NULL, 1, 1, 0, NULL, N'b95718f8-cec1-4300-8c75-de97d1e62cd8', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (35, N'khainuanenhe', N'Hofr8qBlWiZWKoj/ToglIg==;oobekl9mkVoxiaR8E9V/EkaBdpAloy24RwlgRzL5KVc=', N'a', NULL, NULL, 1, 1, 0, NULL, N'b99b7fe7-74ba-41fd-817f-2ae8b582bffc', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (36, N'KabuVG1', N'yq5Z1Y2SHiNvv3s/leYFOA==;iLqM9Dc9oJExDL350xH4lvE0a7mLo4Y+FXG8Un1nyYw=', N'NguyenStudent', N'Ho Chi Minh', N'092349283', 1, 1, 0, NULL, N'0e00990e-908c-4c6c-af46-ea28862e6424', N'gasshu9@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (37, N'khaitranquangUser', N'P+UjTFPHOhY6PDylzVlxCA==;mplxuJ4pVC0olF9tjDVUfQUC7E4K6dURrbyaCxL7dW8=', N'Khai', NULL, NULL, 1, 1, 0, NULL, N'f6f47027-05ea-4514-aed5-60de7b0f91b9', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (38, N'KhainhungmalaStaff2', N'qkVOLY/l1ktQWy2K5AF9lg==;7fiJgG+FsMgs9DSlC70SQT5qb0RPkpeXFoQnfn8t96c=', N'Khai Staff', NULL, NULL, 3, 1, 0, NULL, N'94ec5965-ab1e-4d4e-9864-433e13dd4319', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (39, N'KhainhungmalaLec2', N'kzqPTpoljW6TX+cC6nEhqg==;y6rFxwBXpppLoJqNp+H4PnW4kmq87VGDPSQrEfDivHU=', N'Khai Lec', NULL, NULL, 2, 1, 0, NULL, N'e042ce9a-b857-4120-be7d-200d203e8b55', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (40, N'khaitranquangtestAccount', N'NiItTwUJwJKDGXD2AntmHg==;ZjxXvdSK0gD+VNA6oLedlDWArFJECgIx0Ayejdy3mIE=', N'khaitranquang', NULL, NULL, 1, 1, 0, NULL, N'aab6485a-6b84-4736-9255-fd3d13b59c12', N'khaitranquang987@gmail.com')
GO
INSERT [dbo].[Users] ([uid], [user_name], [password], [full_name], [address], [phone], [role_id], [is_verified], [is_disabled], [disabled_reason], [verification_token], [email]) VALUES (41, N'KhainhungmalaLec3', N'1OcZuxqYNCgukqPBAmlqNQ==;cJYZ5l4KWZ5YL2/AoueXOL4gbAxUiF3bEi87oouKJwo=', N'Khai lecturer', NULL, NULL, 2, 1, 0, NULL, N'54a2180a-8fc5-49c2-aed4-5dd0552a9acd', N'khaitranquang987@gmail.com')
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__is_disabl__1EA48E88]  DEFAULT ((0)) FOR [is_disabled]
GO
ALTER TABLE [dbo].[AvailableDate]  WITH CHECK ADD  CONSTRAINT [FK__Available__lectu__30F848ED] FOREIGN KEY([lecturer_id])
REFERENCES [dbo].[Users] ([uid])
GO
ALTER TABLE [dbo].[AvailableDate] CHECK CONSTRAINT [FK__Available__lectu__30F848ED]
GO
ALTER TABLE [dbo].[AvailableDate]  WITH CHECK ADD  CONSTRAINT [FK__Available__slot___73BA3083] FOREIGN KEY([slot_id])
REFERENCES [dbo].[StudySlot] ([slot_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AvailableDate] CHECK CONSTRAINT [FK__Available__slot___73BA3083]
GO
ALTER TABLE [dbo].[ChangeClassRequests]  WITH CHECK ADD  CONSTRAINT [FK__ChangeCla__class__74AE54BC] FOREIGN KEY([class_id])
REFERENCES [dbo].[Class] ([class_id])
GO
ALTER TABLE [dbo].[ChangeClassRequests] CHECK CONSTRAINT [FK__ChangeCla__class__74AE54BC]
GO
ALTER TABLE [dbo].[ChangeClassRequests]  WITH CHECK ADD  CONSTRAINT [FK__ChangeCla__reque__75A278F5] FOREIGN KEY([request_class_id])
REFERENCES [dbo].[Class] ([class_id])
GO
ALTER TABLE [dbo].[ChangeClassRequests] CHECK CONSTRAINT [FK__ChangeCla__reque__75A278F5]
GO
ALTER TABLE [dbo].[ChangeClassRequests]  WITH CHECK ADD  CONSTRAINT [FK__ChangeCla__user___76969D2E] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([uid])
GO
ALTER TABLE [dbo].[ChangeClassRequests] CHECK CONSTRAINT [FK__ChangeCla__user___76969D2E]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK__Class__lecturer___33D4B598] FOREIGN KEY([lecturer_id])
REFERENCES [dbo].[Users] ([uid])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK__Class__lecturer___33D4B598]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK__Enrollmen__class__787EE5A0] FOREIGN KEY([class_id])
REFERENCES [dbo].[Class] ([class_id])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK__Enrollmen__class__787EE5A0]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK__Enrollmen__stude__36B12243] FOREIGN KEY([student_id])
REFERENCES [dbo].[Users] ([uid])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK__Enrollmen__stude__36B12243]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK__Feedback__lectur__3F466844] FOREIGN KEY([lecturer_id])
REFERENCES [dbo].[Users] ([uid])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK__Feedback__lectur__3F466844]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK__Feedback__studen__3E52440B] FOREIGN KEY([student_id])
REFERENCES [dbo].[Users] ([uid])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK__Feedback__studen__3E52440B]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([class_id])
REFERENCES [dbo].[Class] ([class_id])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK__Payment__student__4222D4EF] FOREIGN KEY([student_id])
REFERENCES [dbo].[Users] ([uid])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK__Payment__student__4222D4EF]
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK__Schedule__class___7D439ABD] FOREIGN KEY([class_id])
REFERENCES [dbo].[Class] ([class_id])
GO
ALTER TABLE [dbo].[Schedule] CHECK CONSTRAINT [FK__Schedule__class___7D439ABD]
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK__Schedule__slot_i__7E37BEF6] FOREIGN KEY([slot_id])
REFERENCES [dbo].[StudySlot] ([slot_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Schedule] CHECK CONSTRAINT [FK__Schedule__slot_i__7E37BEF6]
GO
ALTER TABLE [dbo].[StudySlotDay]  WITH CHECK ADD  CONSTRAINT [FK__StudySlot__day_i__2E1BDC42] FOREIGN KEY([day_id])
REFERENCES [dbo].[DateOfWeek] ([day_id])
GO
ALTER TABLE [dbo].[StudySlotDay] CHECK CONSTRAINT [FK__StudySlot__day_i__2E1BDC42]
GO
ALTER TABLE [dbo].[StudySlotDay]  WITH CHECK ADD  CONSTRAINT [FK__StudySlot__slot___00200768] FOREIGN KEY([slot_id])
REFERENCES [dbo].[StudySlot] ([slot_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudySlotDay] CHECK CONSTRAINT [FK__StudySlot__slot___00200768]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK__Users__role_id__267ABA7A] FOREIGN KEY([role_id])
REFERENCES [dbo].[Roles] ([role_id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK__Users__role_id__267ABA7A]
GO
USE [master]
GO
ALTER DATABASE [YGC2] SET  READ_WRITE 
GO

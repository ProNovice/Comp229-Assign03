USE [Comp229Assign03]

/* Object: Table Comp229Assign03.[dbo].[Students] */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE Comp229Assign03.[dbo].[Students](
	[StudentID] [int] IDENTITY (300000, 1) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[FirstMidName] [varchar](50) NOT NULL,
	[EnrollmentDate] [date] NOT NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/* Object: Table Comp229Assign03.[dbo].[Enrollments] */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE Comp229Assign03.[dbo].[Enrollments](
	[EnrollmentID] [int] IDENTITY (1, 1) NOT NULL,
	[CourseID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[Grade] [int] NOT NULL,
 CONSTRAINT [PK_Enrollments] PRIMARY KEY CLUSTERED 
(
	[EnrollmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/* Object: Table Comp229Assign03.[dbo].[Departments] */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE Comp229Assign03.[dbo].[Departments](
	[DepartmentID] [int] IDENTITY (1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Budget] [decimal](18,2) NOT NULL,
 CONSTRAINT [PK_Deparments] PRIMARY KEY CLUSTERED 
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/* Object: Table Comp229Assign03.[dbo].[Courses] */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE Comp229Assign03.[dbo].[Courses](
	[CourseID] [int] IDENTITY (4000, 1) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Credits] [int] NOT NULL,
	[DepartmentID] [int] NOT NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[CourseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

ALTER TABLE Comp229Assign03.[dbo].Courses
ADD CONSTRAINT FK_Courses_DepartmentID FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)

ALTER TABLE Comp229Assign03.[dbo].Enrollments
ADD CONSTRAINT FK_Enrollments_CourseID FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)

ALTER TABLE Comp229Assign03.[dbo].Enrollments
ADD CONSTRAINT FK_Enrollments_StudentID FOREIGN KEY (StudentID) REFERENCES Students(StudentID)

--Departments
INSERT INTO Comp229Assign03.[dbo].Departments (Name, Budget)
VALUES ('English', 350000)

INSERT INTO Comp229Assign03.[dbo].Departments (Name, Budget)
VALUES ('Mathematics', 100000)

INSERT INTO Comp229Assign03.[dbo].Departments (Name, Budget)
VALUES ('Engineering', 350000)

INSERT INTO Comp229Assign03.[dbo].Departments (Name, Budget)
VALUES ('Economics', 100000)

--Courses
INSERT INTO Comp229Assign03.[dbo].Courses (Title, Credits, DepartmentID)
VALUES ('Chemistry', 3, 3)

INSERT INTO Comp229Assign03.[dbo].Courses (Title, Credits, DepartmentID)
VALUES ('Microeconomics', 3, 4)

INSERT INTO Comp229Assign03.[dbo].Courses (Title, Credits, DepartmentID)
VALUES ('Macroeconomics', 3, 4)

INSERT INTO Comp229Assign03.[dbo].Courses (Title, Credits, DepartmentID)
VALUES ('Calculus', 4, 2)

INSERT INTO Comp229Assign03.[dbo].Courses (Title, Credits, DepartmentID)
VALUES ('Trigonometry', 4, 2)

INSERT INTO Comp229Assign03.[dbo].Courses (Title, Credits, DepartmentID)
VALUES ('Literature', 4, 1)

--Students
INSERT INTO Comp229Assign03.[dbo].Students (LastName, FirstMidName, EnrollmentDate)
VALUES ('Alexander', 'Carson', '2010-09-01')

INSERT INTO Comp229Assign03.[dbo].Students (LastName, FirstMidName, EnrollmentDate)
VALUES ('Alonso', 'Meredith', '2012-09-01')

INSERT INTO Comp229Assign03.[dbo].Students (LastName, FirstMidName, EnrollmentDate)
VALUES ('Anand', 'Arturo', '2013-09-01')

INSERT INTO Comp229Assign03.[dbo].Students (LastName, FirstMidName, EnrollmentDate)
VALUES ('Barzdukas', 'Gytis', '2012-09-01')

INSERT INTO Comp229Assign03.[dbo].Students (LastName, FirstMidName, EnrollmentDate)
VALUES ('Li', 'Yan', '2012-09-01')

INSERT INTO Comp229Assign03.[dbo].Students (LastName, FirstMidName, EnrollmentDate)
VALUES ('Justice', 'Peggy', '2011-09-01')

INSERT INTO Comp229Assign03.[dbo].Students (LastName, FirstMidName, EnrollmentDate)
VALUES ('Norman', 'Laura', '2013-09-01')

INSERT INTO Comp229Assign03.[dbo].Students (LastName, FirstMidName, EnrollmentDate)
VALUES ('Olivetto', 'Nino', '2005-09-01')

--Enrollments
INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4000, 300000, 65)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4000, 300001, 83)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4001, 300002, 74)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4001, 300003, 71)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4002, 300002, 95)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4002, 300003, 87)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4003, 300004, 50)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4003, 300005, 88)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4004, 300004, 61)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4004, 300005, 92)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4005, 300006, 74)

INSERT INTO Comp229Assign03.[dbo].Enrollments (CourseID, StudentID, Grade)
VALUES (4005, 300007, 73)
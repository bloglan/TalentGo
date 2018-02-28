/*
   2018年2月28日16:15:18
   用户: 
   服务器: db01
   数据库: RecruitmentDataDev
   应用程序: 
*/

/* 为了防止任何可能出现的数据丢失问题，您应该先仔细检查此脚本，然后再在数据库设计器的上下文之外运行此脚本。*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT FK_EnrollmentData_Job
GO
ALTER TABLE dbo.Job SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Job', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_User_WhenCreated
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_User_WhenChanged
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_User_LoginCount
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_User_Enabled
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_User_MobileValid
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_User_EmailValid
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_Users_AccessFailedCount
GO
CREATE TABLE dbo.Tmp_People
	(
	Id uniqueidentifier NOT NULL,
	IDCardNumber varchar(25) NOT NULL,
	Mobile varchar(15) NOT NULL,
	WhenCreated datetime NOT NULL,
	WhenChanged datetime NOT NULL,
	LastLogin datetime NULL,
	DisplayName nvarchar(10) NOT NULL,
	Email nvarchar(150) NULL,
	IDCardFrontFileId uniqueidentifier NULL,
	IDCardBackFileId uniqueidentifier NULL,
	HeadImageId uniqueidentifier NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_People SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_People ADD CONSTRAINT
	DF_People_Id DEFAULT NEWID() FOR Id
GO
ALTER TABLE dbo.Tmp_People ADD CONSTRAINT
	DF_User_WhenCreated DEFAULT (getdate()) FOR WhenCreated
GO
ALTER TABLE dbo.Tmp_People ADD CONSTRAINT
	DF_User_WhenChanged DEFAULT (getdate()) FOR WhenChanged
GO
IF EXISTS(SELECT * FROM dbo.Users)
	 EXEC('INSERT INTO dbo.Tmp_People (IDCardNumber, Mobile, WhenCreated, WhenChanged, LastLogin, DisplayName, Email)
		SELECT IDCardNumber, Mobile, WhenCreated, WhenChanged, LastLogin, DisplayName, Email FROM dbo.Users WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.UserLogins
	DROP CONSTRAINT FK_UserLogins_Users
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT FK_EnrollmentData_Users
GO
DROP TABLE dbo.Users
GO
EXECUTE sp_rename N'dbo.Tmp_People', N'People', 'OBJECT' 
GO
ALTER TABLE dbo.People ADD CONSTRAINT
	PK_People PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE UNIQUE NONCLUSTERED INDEX IX_Users_Email ON dbo.People
	(
	Email
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX IX_Users_Mobile ON dbo.People
	(
	Mobile
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT
select Has_Perms_By_Name(N'dbo.People', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.WebUser
	(
	Id uniqueidentifier NOT NULL,
	UserName nchar(10) NULL,
	HashPassword nchar(10) NULL,
	LoginCount nchar(10) NULL,
	Enabled nchar(10) NULL,
	MobileValid nchar(10) NULL,
	EmailValid nchar(10) NULL,
	SecurityStamp nchar(10) NULL,
	LockoutEndDateUTC nchar(10) NULL,
	LockoutEnabled nchar(10) NULL,
	AccessFailedCount nchar(10) NULL,
	TwoFactorEnabled nchar(10) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.WebUser ADD CONSTRAINT
	PK_WebUser PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.WebUser ADD CONSTRAINT
	FK_WebUser_People FOREIGN KEY
	(
	Id
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.WebUser SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.WebUser', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.WebUser', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.WebUser', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT DF_RecruitData_WhenCreated
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT DF_EnrollmentData_IgnoreAnnounced
GO
CREATE TABLE dbo.Tmp_EnrollmentData
	(
	Id int NOT NULL IDENTITY (10000, 1),
	JobId int NOT NULL,
	UserId uniqueidentifier NOT NULL,
	Name nvarchar(5) NOT NULL,
	Sex nvarchar(2) NOT NULL,
	DateOfBirth date NOT NULL,
	Nationality nvarchar(5) NOT NULL,
	PlaceOfBirth nvarchar(50) NOT NULL,
	Source nvarchar(100) NOT NULL,
	PoliticalStatus nvarchar(15) NOT NULL,
	Health nvarchar(10) NOT NULL,
	Marriage nvarchar(5) NOT NULL,
	IDCardNumber varchar(18) NOT NULL,
	Mobile varchar(15) NOT NULL,
	School nvarchar(50) NOT NULL,
	Major nvarchar(50) NOT NULL,
	YearOfGraduated int NOT NULL,
	SelectedMajor nvarchar(15) NOT NULL,
	EducationBackground nvarchar(15) NOT NULL,
	Degree nvarchar(15) NOT NULL,
	Resume nvarchar(MAX) NOT NULL,
	Accomplishments nvarchar(MAX) NULL,
	WhenCreated datetime NOT NULL,
	WhenChanged datetime NULL,
	WhenCommited datetime NULL,
	WhenAudit datetime NULL,
	Approved bit NULL,
	AuditMessage nvarchar(50) NULL,
	WhenAnnounced datetime NULL,
	IsTakeExam bit NULL,
	IgnoreAnnounced bit NOT NULL,
	ExamId varchar(20) NULL,
	ExamRoom nvarchar(50) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_EnrollmentData SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_EnrollmentData ADD CONSTRAINT
	DF_RecruitData_WhenCreated DEFAULT (getdate()) FOR WhenCreated
GO
ALTER TABLE dbo.Tmp_EnrollmentData ADD CONSTRAINT
	DF_EnrollmentData_IgnoreAnnounced DEFAULT ((0)) FOR IgnoreAnnounced
GO
SET IDENTITY_INSERT dbo.Tmp_EnrollmentData ON
GO
IF EXISTS(SELECT * FROM dbo.EnrollmentData)
	 EXEC('INSERT INTO dbo.Tmp_EnrollmentData (Id, JobId, Name, Sex, DateOfBirth, Nationality, PlaceOfBirth, Source, PoliticalStatus, Health, Marriage, IDCardNumber, Mobile, School, Major, YearOfGraduated, SelectedMajor, EducationBackground, Degree, Resume, Accomplishments, WhenCreated, WhenChanged, WhenCommited, WhenAudit, Approved, AuditMessage, WhenAnnounced, IsTakeExam, IgnoreAnnounced, ExamId, ExamRoom)
		SELECT Id, JobId, Name, Sex, DateOfBirth, Nationality, PlaceOfBirth, Source, PoliticalStatus, Health, Marriage, IDCardNumber, Mobile, School, Major, YearOfGraduated, SelectedMajor, EducationBackground, Degree, Resume, Accomplishments, WhenCreated, WhenChanged, WhenCommited, WhenAudit, Approved, AuditMessage, WhenAnnounced, IsTakeExam, IgnoreAnnounced, ExamId, ExamRoom FROM dbo.EnrollmentData WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_EnrollmentData OFF
GO
DROP TABLE dbo.EnrollmentData
GO
EXECUTE sp_rename N'dbo.Tmp_EnrollmentData', N'EnrollmentData', 'OBJECT' 
GO
ALTER TABLE dbo.EnrollmentData ADD CONSTRAINT
	PK_EnrollmentData_1 PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.EnrollmentData ADD CONSTRAINT
	FK_EnrollmentData_Job FOREIGN KEY
	(
	JobId
	) REFERENCES dbo.Job
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.EnrollmentData ADD CONSTRAINT
	FK_EnrollmentData_People FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.EnrollmentData', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.EnrollmentData', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.EnrollmentData', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.UserLogins SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.UserLogins', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.UserLogins', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.UserLogins', 'Object', 'CONTROL') as Contr_Per 
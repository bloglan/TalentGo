/*
   2018年2月28日17:52:47
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
ALTER TABLE dbo.[File]
	DROP CONSTRAINT PK_File
GO
ALTER TABLE dbo.[File] ADD CONSTRAINT
	PK_File PRIMARY KEY NONCLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE CLUSTERED INDEX IX_File_WhenCreated ON dbo.[File]
	(
	WhenCreated
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.[File] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.[File]', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.[File]', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.[File]', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Article
	DROP CONSTRAINT FK_Article_RecruitmentPlan
GO
ALTER TABLE dbo.ArchiveRequirements
	DROP CONSTRAINT FK_ArchiveRequirements_RecruitmentPlan
GO
ALTER TABLE dbo.RecruitmentPlan SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.People
	DROP CONSTRAINT DF_People_Id
GO
ALTER TABLE dbo.People
	DROP CONSTRAINT DF_User_WhenCreated
GO
ALTER TABLE dbo.People
	DROP CONSTRAINT DF_User_WhenChanged
GO
CREATE TABLE dbo.Tmp_People
	(
	Id uniqueidentifier NOT NULL,
	IDCardNumber varchar(25) NOT NULL,
	Sex int NOT NULL,
	DateOfBirth date NOT NULL,
	Ethnicity nvarchar(50) NOT NULL,
	Address nvarchar(150) NOT NULL,
	Issuer nvarchar(50) NOT NULL,
	IssueDate date NOT NULL,
	ExpiresAt date NULL,
	Mobile varchar(15) NOT NULL,
	WhenCreated datetime2(7) NOT NULL,
	WhenChanged datetime2(7) NOT NULL,
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
	DF_People_Id DEFAULT (newid()) FOR Id
GO
ALTER TABLE dbo.Tmp_People ADD CONSTRAINT
	DF_User_WhenCreated DEFAULT (getdate()) FOR WhenCreated
GO
ALTER TABLE dbo.Tmp_People ADD CONSTRAINT
	DF_User_WhenChanged DEFAULT (getdate()) FOR WhenChanged
GO
IF EXISTS(SELECT * FROM dbo.People)
	 EXEC('INSERT INTO dbo.Tmp_People (Id, IDCardNumber, Mobile, WhenCreated, WhenChanged, DisplayName, Email, IDCardFrontFileId, IDCardBackFileId, HeadImageId)
		SELECT Id, IDCardNumber, Mobile, CONVERT(datetime2(7), WhenCreated), CONVERT(datetime2(7), WhenChanged), DisplayName, Email, IDCardFrontFileId, IDCardBackFileId, HeadImageId FROM dbo.People WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.WebUser
	DROP CONSTRAINT FK_WebUser_People
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT FK_EnrollmentData_People
GO
DROP TABLE dbo.People
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
ALTER TABLE dbo.ArchiveRequirements SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ArchiveRequirements', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ArchiveRequirements', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ArchiveRequirements', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Article ADD
	WhenPublished datetime2(0) NULL
GO
ALTER TABLE dbo.Article
	DROP COLUMN Summary, RelatedPlan, IsPublic
GO
ALTER TABLE dbo.Article SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Article', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Article', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Article', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT FK_EnrollmentData_Job
GO
ALTER TABLE dbo.Job SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Job', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT DF_RecruitData_WhenCreated
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT DF_EnrollmentData_IgnoreAnnounced
GO
CREATE TABLE dbo.Tmp_ApplicationForm
	(
	Id int NOT NULL IDENTITY (10000, 1),
	JobId int NOT NULL,
	UserId uniqueidentifier NOT NULL,
	Name nvarchar(5) NOT NULL,
	NativePlace nvarchar(50) NOT NULL,
	Source nvarchar(100) NOT NULL,
	PoliticalStatus nvarchar(15) NOT NULL,
	Health nvarchar(10) NOT NULL,
	Marriage nvarchar(5) NOT NULL,
	School nvarchar(50) NOT NULL,
	Major nvarchar(50) NOT NULL,
	YearOfGraduated int NOT NULL,
	EducationBackground nvarchar(15) NOT NULL,
	Degree nvarchar(15) NOT NULL,
	Resume nvarchar(MAX) NOT NULL,
	Accomplishments nvarchar(MAX) NULL,
	WhenCreated datetime2(7) NOT NULL,
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
ALTER TABLE dbo.Tmp_ApplicationForm SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_ApplicationForm ADD CONSTRAINT
	DF_RecruitData_WhenCreated DEFAULT (getdate()) FOR WhenCreated
GO
ALTER TABLE dbo.Tmp_ApplicationForm ADD CONSTRAINT
	DF_EnrollmentData_IgnoreAnnounced DEFAULT ((0)) FOR IgnoreAnnounced
GO
SET IDENTITY_INSERT dbo.Tmp_ApplicationForm ON
GO
IF EXISTS(SELECT * FROM dbo.EnrollmentData)
	 EXEC('INSERT INTO dbo.Tmp_ApplicationForm (Id, JobId, UserId, Name, NativePlace, Source, PoliticalStatus, Health, Marriage, School, Major, YearOfGraduated, EducationBackground, Degree, Resume, Accomplishments, WhenCreated, WhenChanged, WhenCommited, WhenAudit, Approved, AuditMessage, WhenAnnounced, IsTakeExam, IgnoreAnnounced, ExamId, ExamRoom)
		SELECT Id, JobId, UserId, Name, PlaceOfBirth, Source, PoliticalStatus, Health, Marriage, School, Major, YearOfGraduated, EducationBackground, Degree, Resume, Accomplishments, CONVERT(datetime2(7), WhenCreated), WhenChanged, WhenCommited, WhenAudit, Approved, AuditMessage, WhenAnnounced, IsTakeExam, IgnoreAnnounced, ExamId, ExamRoom FROM dbo.EnrollmentData WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ApplicationForm OFF
GO
DROP TABLE dbo.EnrollmentData
GO
EXECUTE sp_rename N'dbo.Tmp_ApplicationForm', N'ApplicationForm', 'OBJECT' 
GO
ALTER TABLE dbo.ApplicationForm ADD CONSTRAINT
	PK_ApplicationForm PRIMARY KEY NONCLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE CLUSTERED INDEX IX_ApplicationForm_WhenCreated ON dbo.ApplicationForm
	(
	WhenCreated
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.ApplicationForm ADD CONSTRAINT
	FK_ApplicationForm_Job FOREIGN KEY
	(
	JobId
	) REFERENCES dbo.Job
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ApplicationForm ADD CONSTRAINT
	FK_ApplicationForm_People FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_WebUser
	(
	Id uniqueidentifier NOT NULL,
	UserName nvarchar(50) NOT NULL,
	HashPassword varchar(256) NOT NULL,
	LoginCount int NOT NULL,
	Enabled bit NOT NULL,
	MobileValid bit NOT NULL,
	EmailValid bit NOT NULL,
	SecurityStamp nvarchar(50) NULL,
	LockoutEndDateUTC datetime2(7) NULL,
	LockoutEnabled bit NOT NULL,
	AccessFailedCount int NOT NULL,
	TwoFactorEnabled bit NOT NULL,
	LastLogin datetime2(7) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_WebUser SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.WebUser)
	 EXEC('INSERT INTO dbo.Tmp_WebUser (Id, UserName, HashPassword, LoginCount, Enabled, MobileValid, EmailValid, SecurityStamp, LockoutEndDateUTC, LockoutEnabled, AccessFailedCount, TwoFactorEnabled)
		SELECT Id, CONVERT(nvarchar(50), UserName), CONVERT(varchar(256), HashPassword), CONVERT(int, LoginCount), CONVERT(bit, Enabled), CONVERT(bit, MobileValid), CONVERT(bit, EmailValid), CONVERT(nvarchar(50), SecurityStamp), CONVERT(datetime2(7), LockoutEndDateUTC), CONVERT(bit, LockoutEnabled), CONVERT(int, AccessFailedCount), CONVERT(bit, TwoFactorEnabled) FROM dbo.WebUser WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.WebUser
GO
EXECUTE sp_rename N'dbo.Tmp_WebUser', N'WebUser', 'OBJECT' 
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
COMMIT
select Has_Perms_By_Name(N'dbo.WebUser', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.WebUser', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.WebUser', 'Object', 'CONTROL') as Contr_Per 
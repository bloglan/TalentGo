/*
   2018年3月22日22:51:27
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
ALTER TABLE dbo.ApplicationForm
	DROP CONSTRAINT FK_ApplicationForm_People
GO
ALTER TABLE dbo.People SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.People', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.ApplicationForm
	DROP CONSTRAINT FK_ApplicationForm_Job1
GO
ALTER TABLE dbo.Job SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Job', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.ApplicationForm
	DROP CONSTRAINT DF_ApplicationForm_AcademicCertFiles
GO
ALTER TABLE dbo.ApplicationForm
	DROP CONSTRAINT DF_ApplicationForm_DegreeCertFiles
GO
ALTER TABLE dbo.ApplicationForm
	DROP CONSTRAINT DF_ApplicationForm_OtherFiles
GO
ALTER TABLE dbo.ApplicationForm
	DROP CONSTRAINT DF_RecruitData_WhenCreated
GO
ALTER TABLE dbo.ApplicationForm
	DROP CONSTRAINT DF_ApplicationForm_AuditFlag
GO
CREATE TABLE dbo.Tmp_ApplicationForm
	(
	Id int NOT NULL IDENTITY (10000, 1),
	JobId int NOT NULL,
	PlanId int NOT NULL,
	PersonId uniqueidentifier NOT NULL,
	NativePlace nvarchar(50) NOT NULL,
	Source nvarchar(100) NOT NULL,
	PoliticalStatus nvarchar(15) NOT NULL,
	Health nvarchar(10) NOT NULL,
	Marriage nvarchar(5) NOT NULL,
	School nvarchar(50) NOT NULL,
	Major nvarchar(50) NOT NULL,
	SelectedMajor nvarchar(50) NOT NULL,
	YearOfGraduated int NOT NULL,
	EducationalBackground nvarchar(15) NOT NULL,
	AcademicCertNumber nvarchar(50) NULL,
	Degree nvarchar(15) NOT NULL,
	DegreeCertNumber nvarchar(50) NULL,
	Resume nvarchar(MAX) NOT NULL,
	Accomplishments nvarchar(MAX) NULL,
	HeadImageFile nvarchar(50) NULL,
	AcademicCertFiles varchar(500) NOT NULL,
	DegreeCertFiles varchar(500) NOT NULL,
	OtherFiles varchar(500) NOT NULL,
	WhenCreated datetime2(7) NOT NULL,
	WhenChanged datetime NULL,
	WhenCommited datetime NULL,
	CommitCount int NOT NULL,
	FileReviewAccepted bit NULL,
	FileReviewMessage nvarchar(100) NULL,
	WhenFileReviewed datetime2(0) NULL,
	FileReviewedBy nvarchar(20) NULL,
	AuditFlag bit NOT NULL,
	AuditMessage nvarchar(50) NULL,
	AuditBy nvarchar(20) NULL,
	WhenAudit datetime2(0) NULL,
	WhenAuditComplete datetime2(0) NULL,
	Tags nvarchar(50) NULL,
	WhenAnnounced datetime NULL,
	IsTakeExam bit NULL,
	ChangeLog nvarchar(1000) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ApplicationForm SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_ApplicationForm ADD CONSTRAINT
	DF_ApplicationForm_AcademicCertFiles DEFAULT ('') FOR AcademicCertFiles
GO
ALTER TABLE dbo.Tmp_ApplicationForm ADD CONSTRAINT
	DF_ApplicationForm_DegreeCertFiles DEFAULT ('') FOR DegreeCertFiles
GO
ALTER TABLE dbo.Tmp_ApplicationForm ADD CONSTRAINT
	DF_ApplicationForm_OtherFiles DEFAULT ('') FOR OtherFiles
GO
ALTER TABLE dbo.Tmp_ApplicationForm ADD CONSTRAINT
	DF_RecruitData_WhenCreated DEFAULT (getdate()) FOR WhenCreated
GO
ALTER TABLE dbo.Tmp_ApplicationForm ADD CONSTRAINT
	DF_ApplicationForm_CommitCount DEFAULT 0 FOR CommitCount
GO
ALTER TABLE dbo.Tmp_ApplicationForm ADD CONSTRAINT
	DF_ApplicationForm_AuditFlag DEFAULT ((0)) FOR AuditFlag
GO
SET IDENTITY_INSERT dbo.Tmp_ApplicationForm ON
GO
IF EXISTS(SELECT * FROM dbo.ApplicationForm)
	 EXEC('INSERT INTO dbo.Tmp_ApplicationForm (Id, JobId, PlanId, PersonId, NativePlace, Source, PoliticalStatus, Health, Marriage, School, Major, SelectedMajor, YearOfGraduated, EducationalBackground, AcademicCertNumber, Degree, DegreeCertNumber, Resume, Accomplishments, HeadImageFile, AcademicCertFiles, DegreeCertFiles, OtherFiles, WhenCreated, WhenChanged, WhenCommited, FileReviewAccepted, FileReviewMessage, WhenFileReviewed, FileReviewedBy, AuditFlag, AuditMessage, AuditBy, WhenAudit, WhenAuditComplete, Tags, WhenAnnounced, IsTakeExam, ChangeLog)
		SELECT Id, JobId, PlanId, PersonId, NativePlace, Source, PoliticalStatus, Health, Marriage, School, Major, SelectedMajor, YearOfGraduated, EducationalBackground, AcademicCertNumber, Degree, DegreeCertNumber, Resume, Accomplishments, HeadImageFile, AcademicCertFiles, DegreeCertFiles, OtherFiles, WhenCreated, WhenChanged, WhenCommited, FileReviewAccepted, FileReviewMessage, WhenFileReviewed, FileReviewedBy, AuditFlag, AuditMessage, AuditBy, WhenAudit, WhenAuditComplete, Tags, WhenAnnounced, IsTakeExam, ChangeLog FROM dbo.ApplicationForm WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ApplicationForm OFF
GO
DROP TABLE dbo.ApplicationForm
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
	FK_ApplicationForm_Job1 FOREIGN KEY
	(
	JobId,
	PlanId
	) REFERENCES dbo.Job
	(
	Id,
	PlanId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ApplicationForm ADD CONSTRAINT
	FK_ApplicationForm_People FOREIGN KEY
	(
	PersonId
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'CONTROL') as Contr_Per 
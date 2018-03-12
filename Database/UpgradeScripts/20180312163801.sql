/*
   2018年3月12日16:38:08
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
	DROP CONSTRAINT FK_ApplicationForm_Job
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
CREATE TABLE dbo.Tmp_ApplicationForm
	(
	Id int NOT NULL IDENTITY (10000, 1),
	JobId int NOT NULL,
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
	EducationBackground nvarchar(15) NOT NULL,
	Degree nvarchar(15) NOT NULL,
	Resume nvarchar(MAX) NOT NULL,
	Accomplishments nvarchar(MAX) NULL,
	HeadImageFile nvarchar(50) NULL,
	AcademicCertFiles varchar(500) NOT NULL,
	DegreeCertFiles varchar(500) NOT NULL,
	OtherFiles varchar(500) NOT NULL,
	WhenCreated datetime2(7) NOT NULL,
	WhenChanged datetime NULL,
	WhenCommited datetime NULL,
	FileReviewAccepted bit NULL,
	WhenFileReview datetime2(0) NULL,
	WhenAudit datetime NULL,
	Approved bit NULL,
	AuditMessage nvarchar(50) NULL,
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
SET IDENTITY_INSERT dbo.Tmp_ApplicationForm ON
GO
IF EXISTS(SELECT * FROM dbo.ApplicationForm)
	 EXEC('INSERT INTO dbo.Tmp_ApplicationForm (Id, JobId, PersonId, NativePlace, Source, PoliticalStatus, Health, Marriage, School, Major, YearOfGraduated, EducationBackground, Degree, Resume, Accomplishments, HeadImageFile, AcademicCertFiles, DegreeCertFiles, OtherFiles, WhenCreated, WhenChanged, WhenCommited, FileReviewAccepted, WhenFileReview, WhenAudit, Approved, AuditMessage, WhenAnnounced, IsTakeExam, ChangeLog)
		SELECT Id, JobId, PersonId, NativePlace, Source, PoliticalStatus, Health, Marriage, School, Major, YearOfGraduated, EducationBackground, Degree, Resume, Accomplishments, HeadImageFile, AcademicCertFiles, DegreeCertFiles, OtherFiles, WhenCreated, WhenChanged, WhenCommited, FileReviewAccepted, WhenFileReview, WhenAudit, Approved, AuditMessage, WhenAnnounced, IsTakeExam, ChangeLog FROM dbo.ApplicationForm WITH (HOLDLOCK TABLOCKX)')
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
	PersonId
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'CONTROL') as Contr_Per 
/*
   2018年3月24日23:34:32
   用户: yangyaojing0314
   服务器: db01.qjyc.cn
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
ALTER TABLE dbo.ArchiveRequirements
	DROP CONSTRAINT FK_ArchiveRequirements_ArchiveCategory
GO
ALTER TABLE dbo.EnrollmentArchives
	DROP CONSTRAINT FK_EnrollmentArchives_ArchiveCategory
GO
DROP TABLE dbo.ArchiveCategory
GO
COMMIT
BEGIN TRANSACTION
GO
DROP TABLE dbo.EnrollmentArchives
GO
COMMIT
BEGIN TRANSACTION
GO
DROP TABLE dbo.ArchiveRequirements
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.ExaminationPlan
	(
	Id int NOT NULL IDENTITY (1000, 1),
	Title nvarchar(50) NOT NULL,
	WhenCreated datetime2(0) NOT NULL,
	WhenChanged datetime2(0) NOT NULL,
	WhenPublished datetime2(0) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ExaminationPlan ADD CONSTRAINT
	DF_ExaminationPlan_WhenCreated DEFAULT getdate() FOR WhenCreated
GO
ALTER TABLE dbo.ExaminationPlan ADD CONSTRAINT
	DF_ExaminationPlan_WhenChanged DEFAULT getdate() FOR WhenChanged
GO
ALTER TABLE dbo.ExaminationPlan ADD CONSTRAINT
	PK_ExaminationPlan PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ExaminationPlan SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ExaminationPlan', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ExaminationPlan', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ExaminationPlan', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.ExaminationSubject
	(
	Id int NOT NULL IDENTITY (1000, 1),
	PlanId int NOT NULL,
	Subject nvarchar(50) NULL,
	StartTime datetime2(7) NULL,
	EndTime datetime2(7) NULL,
	Address nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ExaminationSubject ADD CONSTRAINT
	PK_ExaminationSubject PRIMARY KEY CLUSTERED 
	(
	Id,
	PlanId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ExaminationSubject ADD CONSTRAINT
	FK_ExaminationSubject_ExaminationPlan FOREIGN KEY
	(
	PlanId
	) REFERENCES dbo.ExaminationPlan
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.ExaminationSubject SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ExaminationSubject', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ExaminationSubject', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ExaminationSubject', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.People SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.People', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Examinee
	(
	Id int NOT NULL,
	AdmissionNumber nvarchar(50) NULL,
	PlanId int NOT NULL,
	SubjectId int NOT NULL,
	PersonId uniqueidentifier NOT NULL,
	Room nvarchar(50) NULL,
	Seat nvarchar(50) NULL,
	AttendanceConfirmed bit NULL,
	WhenAttendanceConfirmed datetime2(7) NULL,
	Score decimal(5, 2) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Examinee ADD CONSTRAINT
	PK_Examinee PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Examinee ADD CONSTRAINT
	FK_Examinee_ExaminationSubject FOREIGN KEY
	(
	SubjectId,
	PlanId
	) REFERENCES dbo.ExaminationSubject
	(
	Id,
	PlanId
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Examinee ADD CONSTRAINT
	FK_Examinee_People FOREIGN KEY
	(
	PersonId
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Examinee SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Examinee', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Examinee', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Examinee', 'Object', 'CONTROL') as Contr_Per 
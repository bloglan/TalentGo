/*
   2018年3月6日12:08:35
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
ALTER TABLE dbo.Job
	DROP CONSTRAINT FK_Job_RecruitmentPlan
GO
ALTER TABLE dbo.RecruitmentPlan SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Job
	(
	Id int NOT NULL IDENTITY (3285, 7),
	PlanId int NOT NULL,
	Name nvarchar(50) NOT NULL,
	Description nvarchar(2000) NULL,
	WorkLocation nvarchar(300) NOT NULL,
	EducationBackgroundRequirement nvarchar(500) NOT NULL,
	DegreeRequirement nvarchar(500) NOT NULL,
	MajorRequirement nvarchar(500) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Job SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Job ADD CONSTRAINT
	DF_Job_WorkLocation DEFAULT N'聘用时分配' FOR WorkLocation
GO
ALTER TABLE dbo.Tmp_Job ADD CONSTRAINT
	DF_Job_EducationBackgroundRequirement DEFAULT N'本科' FOR EducationBackgroundRequirement
GO
ALTER TABLE dbo.Tmp_Job ADD CONSTRAINT
	DF_Job_DegreeRequirement DEFAULT N'学士' FOR DegreeRequirement
GO
ALTER TABLE dbo.Tmp_Job ADD CONSTRAINT
	DF_Job_MajorRequirement DEFAULT N'不限' FOR MajorRequirement
GO
SET IDENTITY_INSERT dbo.Tmp_Job ON
GO
IF EXISTS(SELECT * FROM dbo.Job)
	 EXEC('INSERT INTO dbo.Tmp_Job (Id, PlanId, Name, Description, WorkLocation)
		SELECT Id, PlanId, Name, Description, WorkLocation FROM dbo.Job WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Job OFF
GO
ALTER TABLE dbo.ApplicationForm
	DROP CONSTRAINT FK_ApplicationForm_Job
GO
DROP TABLE dbo.Job
GO
EXECUTE sp_rename N'dbo.Tmp_Job', N'Job', 'OBJECT' 
GO
ALTER TABLE dbo.Job ADD CONSTRAINT
	PK_Job PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Job ADD CONSTRAINT
	FK_Job_RecruitmentPlan FOREIGN KEY
	(
	PlanId
	) REFERENCES dbo.RecruitmentPlan
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Job', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
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
ALTER TABLE dbo.ApplicationForm SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'CONTROL') as Contr_Per 
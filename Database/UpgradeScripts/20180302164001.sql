/*
   2018年3月2日16:40:36
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
ALTER TABLE dbo.RecruitmentPlan
	DROP CONSTRAINT DF_RecruitPlan_WhenCreated
GO
CREATE TABLE dbo.Tmp_RecruitmentPlan
	(
	Id int NOT NULL IDENTITY (1, 1),
	Title nvarchar(50) NOT NULL,
	Recruitment nvarchar(MAX) NOT NULL,
	WhenCreated datetime NOT NULL,
	Publisher nvarchar(20) NOT NULL,
	EnrollExpirationDate datetime NOT NULL,
	WhenAuditCommited datetime NULL,
	AnnounceExpirationDate datetime NULL,
	WhenPublished datetime NULL,
	ExamStartTime datetime NULL,
	ExamEndTime datetime NULL,
	ExamLocation nvarchar(100) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_RecruitmentPlan SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_RecruitmentPlan ADD CONSTRAINT
	DF_RecruitPlan_WhenCreated DEFAULT (getdate()) FOR WhenCreated
GO
SET IDENTITY_INSERT dbo.Tmp_RecruitmentPlan ON
GO
IF EXISTS(SELECT * FROM dbo.RecruitmentPlan)
	 EXEC('INSERT INTO dbo.Tmp_RecruitmentPlan (Id, Title, Recruitment, WhenCreated, Publisher, EnrollExpirationDate, WhenAuditCommited, AnnounceExpirationDate, WhenPublished, ExamStartTime, ExamEndTime, ExamLocation)
		SELECT Id, Title, Recruitment, WhenCreated, Publisher, EnrollExpirationDate, WhenAuditCommited, AnnounceExpirationDate, WhenPublished, ExamStartTime, ExamEndTime, ExamLocation FROM dbo.RecruitmentPlan WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_RecruitmentPlan OFF
GO
ALTER TABLE dbo.Job
	DROP CONSTRAINT FK_Job_RecruitmentPlan
GO
ALTER TABLE dbo.Notification
	DROP CONSTRAINT FK_Notification_RecruitmentPlan
GO
DROP TABLE dbo.RecruitmentPlan
GO
EXECUTE sp_rename N'dbo.Tmp_RecruitmentPlan', N'RecruitmentPlan', 'OBJECT' 
GO
ALTER TABLE dbo.RecruitmentPlan ADD CONSTRAINT
	PK_RecruitPlan PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
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
ALTER TABLE dbo.Job SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Job', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Job', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Notification ADD CONSTRAINT
	FK_Notification_RecruitmentPlan FOREIGN KEY
	(
	PlanId
	) REFERENCES dbo.RecruitmentPlan
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Notification SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Notification', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Notification', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Notification', 'Object', 'CONTROL') as Contr_Per 
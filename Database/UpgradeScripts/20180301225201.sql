/*
   2018年3月1日22:52:28
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
ALTER TABLE dbo.RecruitmentPlan
	DROP CONSTRAINT DF_RecruitPlan_ExpirationDate
GO
ALTER TABLE dbo.RecruitmentPlan
	DROP CONSTRAINT DF_RecruitPlan_IsPublic
GO
ALTER TABLE dbo.RecruitmentPlan
	DROP COLUMN Year, ExpirationDate, IsPublic
GO
ALTER TABLE dbo.RecruitmentPlan SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RecruitmentPlan', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Notification
	(
	Id int NOT NULL IDENTITY (1, 1),
	PlanId int NOT NULL,
	Title nvarchar(100) NOT NULL,
	[Content] nvarchar(MAX) NOT NULL,
	WhenCreated datetime2(0) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Notification ADD CONSTRAINT
	DF_Notification_WhenCreated DEFAULT GETDATE() FOR WhenCreated
GO
ALTER TABLE dbo.Notification ADD CONSTRAINT
	PK_Notification PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

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
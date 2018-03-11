/*
   2018年3月11日3:21:50
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
ALTER TABLE dbo.ApplicationForm SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.ApplicationFormFile
	(
	ApplicationFormId int NOT NULL,
	FileId nvarchar(256) NOT NULL,
	Category int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ApplicationFormFile ADD CONSTRAINT
	DF_ApplicationFormFile_Category DEFAULT 0 FOR Category
GO
ALTER TABLE dbo.ApplicationFormFile ADD CONSTRAINT
	PK_ApplicationFormFile PRIMARY KEY CLUSTERED 
	(
	ApplicationFormId,
	FileId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ApplicationFormFile ADD CONSTRAINT
	FK_ApplicationFormFile_ApplicationForm FOREIGN KEY
	(
	ApplicationFormId
	) REFERENCES dbo.ApplicationForm
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.ApplicationFormFile SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ApplicationFormFile', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ApplicationFormFile', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ApplicationFormFile', 'Object', 'CONTROL') as Contr_Per 
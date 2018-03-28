/*
   2018年3月28日17:31:56
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
ALTER TABLE dbo.ExaminationSubject
	DROP CONSTRAINT FK_ExaminationSubject_Examination
GO
ALTER TABLE dbo.Candidate
	DROP CONSTRAINT FK_Candidate_Examination
GO
EXECUTE sp_rename N'dbo.Examination', N'ExaminationPlan', 'OBJECT' 
GO
ALTER TABLE dbo.ExaminationPlan SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ExaminationPlan', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ExaminationPlan', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ExaminationPlan', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExaminationSubject ADD CONSTRAINT
	FK_ExaminationSubject_ExaminationPlan FOREIGN KEY
	(
	ExamId
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
ALTER TABLE dbo.Candidate ADD CONSTRAINT
	FK_Candidate_ExaminationPlan FOREIGN KEY
	(
	ExamId
	) REFERENCES dbo.ExaminationPlan
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Candidate SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'CONTROL') as Contr_Per 
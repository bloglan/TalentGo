/*
   2018年3月30日16:59:17
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
EXECUTE sp_rename N'dbo.Candidate.Confirmed', N'Tmp_Attendance', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Candidate.Tmp_Attendance', N'Attendance', 'COLUMN' 
GO
ALTER TABLE dbo.Candidate SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'CONTROL') as Contr_Per 
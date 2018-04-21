/*
   2018年4月21日1:08:28
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
ALTER TABLE dbo.Candidate ADD
	HeadImageFile nvarchar(50) NULL
GO
ALTER TABLE dbo.Candidate SET (LOCK_ESCALATION = TABLE)
GO
UPDATE Candidate SET HeadImageFile = (SELECT HeadImageFile FROM ApplicationForm WHERE Candidate.PersonId = ApplicationForm.PersonId)
GO
COMMIT

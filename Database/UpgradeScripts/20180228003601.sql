/*
   2018年2月28日0:34:56
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
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT FK_EnrollmentData_MajorCategory
GO
ALTER TABLE dbo.Major
	DROP CONSTRAINT FK_Major_MajorCategory
GO
DROP TABLE dbo.MajorCategory
GO
COMMIT
BEGIN TRANSACTION
GO
DROP TABLE dbo.Major
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT FK_RecruitData_EducationBakcground
GO
DROP TABLE dbo.EducationBackground
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EnrollmentData
	DROP CONSTRAINT FK_RecruitData_Degree
GO
DROP TABLE dbo.Degree
GO
COMMIT
BEGIN TRANSACTION
GO
DROP TABLE dbo.EmailValidationSession
GO
COMMIT
BEGIN TRANSACTION
GO
DROP TABLE dbo.Nationality
GO
COMMIT
BEGIN TRANSACTION
GO
DROP TABLE dbo.ExamInfo
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EnrollmentData SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.EnrollmentData', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.EnrollmentData', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.EnrollmentData', 'Object', 'CONTROL') as Contr_Per 
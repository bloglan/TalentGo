/*
   2018年3月27日1:25:05
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
ALTER TABLE dbo.ExaminationPlan
	DROP CONSTRAINT DF_ExaminationPlan_WhenCreated
GO
ALTER TABLE dbo.ExaminationPlan
	DROP CONSTRAINT DF_ExaminationPlan_WhenChanged
GO
CREATE TABLE dbo.Tmp_Examination
	(
	Id int NOT NULL IDENTITY (1000, 1),
	Title nvarchar(50) NOT NULL,
	WhenCreated datetime2(0) NOT NULL,
	WhenChanged datetime2(0) NOT NULL,
	WhenPublished datetime2(0) NULL,
	AttendanceConfirmationExpiresAt datetime2(0) NOT NULL,
	Address nvarchar(100) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Examination SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Examination ADD CONSTRAINT
	DF_ExaminationPlan_WhenCreated DEFAULT (getdate()) FOR WhenCreated
GO
ALTER TABLE dbo.Tmp_Examination ADD CONSTRAINT
	DF_ExaminationPlan_WhenChanged DEFAULT (getdate()) FOR WhenChanged
GO
SET IDENTITY_INSERT dbo.Tmp_Examination ON
GO
IF EXISTS(SELECT * FROM dbo.ExaminationPlan)
	 EXEC('INSERT INTO dbo.Tmp_Examination (Id, Title, WhenCreated, WhenChanged, WhenPublished, AttendanceConfirmationExpiresAt)
		SELECT Id, Title, WhenCreated, WhenChanged, WhenPublished, AttendanceConfirmationExpiresAt FROM dbo.ExaminationPlan WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Examination OFF
GO
ALTER TABLE dbo.ExaminationSubject
	DROP CONSTRAINT FK_ExaminationSubject_ExaminationPlan
GO
ALTER TABLE dbo.Candidate
	DROP CONSTRAINT FK_Candidate_ExaminationPlan
GO
DROP TABLE dbo.ExaminationPlan
GO
EXECUTE sp_rename N'dbo.Tmp_Examination', N'Examination', 'OBJECT' 
GO
ALTER TABLE dbo.Examination ADD CONSTRAINT
	PK_ExaminationPlan PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.Examination', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Examination', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Examination', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.ExaminationSubject.PlanId', N'Tmp_ExamId', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.ExaminationSubject.Tmp_ExamId', N'ExamId', 'COLUMN' 
GO
ALTER TABLE dbo.ExaminationSubject ADD CONSTRAINT
	FK_ExaminationSubject_Examination FOREIGN KEY
	(
	ExamId
	) REFERENCES dbo.Examination
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.ExaminationSubject
	DROP COLUMN Address
GO
ALTER TABLE dbo.ExaminationSubject SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ExaminationSubject', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ExaminationSubject', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ExaminationSubject', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.Examinee.PlanId', N'Tmp_ExamId_1', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Examinee.Tmp_ExamId_1', N'ExamId', 'COLUMN' 
GO
ALTER TABLE dbo.Examinee
	DROP COLUMN Room, Seat
GO
ALTER TABLE dbo.Examinee SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Examinee', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Examinee', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Examinee', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.Candidate.PlanId', N'Tmp_ExamId_2', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Candidate.Tmp_ExamId_2', N'ExamId', 'COLUMN' 
GO
ALTER TABLE dbo.Candidate ADD
	Room nvarchar(50) NULL,
	Seat nvarchar(50) NULL
GO
ALTER TABLE dbo.Candidate ADD CONSTRAINT
	FK_Candidate_Examination FOREIGN KEY
	(
	ExamId
	) REFERENCES dbo.Examination
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Candidate SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'CONTROL') as Contr_Per 
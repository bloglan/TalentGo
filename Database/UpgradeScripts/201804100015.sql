/*
   2018年4月10日00:14:58
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
ALTER TABLE dbo.ExaminationPlan
	DROP CONSTRAINT DF_ExaminationPlan_WhenCreated
GO
ALTER TABLE dbo.ExaminationPlan
	DROP CONSTRAINT DF_ExaminationPlan_WhenChanged
GO
CREATE TABLE dbo.Tmp_ExaminationPlan
	(
	Id int NOT NULL IDENTITY (1000, 1),
	Title nvarchar(50) NOT NULL,
	WhenCreated datetime2(0) NOT NULL,
	WhenChanged datetime2(0) NOT NULL,
	WhenPublished datetime2(0) NULL,
	AttendanceConfirmationExpiresAt datetime2(0) NULL,
	Address nvarchar(100) NOT NULL,
	Notes nvarchar(MAX) NULL,
	WhenAdmissionTicketReleased datetime2(0) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ExaminationPlan SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_ExaminationPlan ADD CONSTRAINT
	DF_ExaminationPlan_WhenCreated DEFAULT (getdate()) FOR WhenCreated
GO
ALTER TABLE dbo.Tmp_ExaminationPlan ADD CONSTRAINT
	DF_ExaminationPlan_WhenChanged DEFAULT (getdate()) FOR WhenChanged
GO
SET IDENTITY_INSERT dbo.Tmp_ExaminationPlan ON
GO
IF EXISTS(SELECT * FROM dbo.ExaminationPlan)
	 EXEC('INSERT INTO dbo.Tmp_ExaminationPlan (Id, Title, WhenCreated, WhenChanged, WhenPublished, AttendanceConfirmationExpiresAt, Address, WhenAdmissionTicketReleased)
		SELECT Id, Title, WhenCreated, WhenChanged, WhenPublished, AttendanceConfirmationExpiresAt, Address, WhenAdmissionTicketReleased FROM dbo.ExaminationPlan WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ExaminationPlan OFF
GO
ALTER TABLE dbo.Candidate
	DROP CONSTRAINT FK_Candidate_ExaminationPlan
GO
ALTER TABLE dbo.ExaminationSubject
	DROP CONSTRAINT FK_ExaminationSubject_ExaminationPlan
GO
DROP TABLE dbo.ExaminationPlan
GO
EXECUTE sp_rename N'dbo.Tmp_ExaminationPlan', N'ExaminationPlan', 'OBJECT' 
GO
ALTER TABLE dbo.ExaminationPlan ADD CONSTRAINT
	PK_ExaminationPlan PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

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
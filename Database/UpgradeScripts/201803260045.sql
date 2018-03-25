/*
   2018年3月26日0:45:12
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
ALTER TABLE dbo.Examinee
	DROP CONSTRAINT FK_Examinee_ExaminationSubject
GO
ALTER TABLE dbo.ExaminationSubject SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ExaminationSubject', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ExaminationSubject', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ExaminationSubject', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Examinee
	DROP CONSTRAINT FK_Examinee_People
GO
ALTER TABLE dbo.People SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.People', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExaminationPlan SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ExaminationPlan', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ExaminationPlan', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ExaminationPlan', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Candidate
	(
	PersonId uniqueidentifier NOT NULL,
	PlanId int NOT NULL,
	Confirmed bit NULL,
	WhenConfirmed datetime2(0) NULL,
	AdmissionNumber varchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Candidate ADD CONSTRAINT
	PK_Candidate PRIMARY KEY CLUSTERED 
	(
	PersonId,
	PlanId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Candidate ADD CONSTRAINT
	FK_Candidate_People FOREIGN KEY
	(
	PersonId
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Candidate ADD CONSTRAINT
	FK_Candidate_ExaminationPlan FOREIGN KEY
	(
	PlanId
	) REFERENCES dbo.ExaminationPlan
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Candidate SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Candidate', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Examinee
	(
	PlanId int NOT NULL,
	SubjectId int NOT NULL,
	PersonId uniqueidentifier NOT NULL,
	Room nvarchar(50) NULL,
	Seat nvarchar(50) NULL,
	Score decimal(7, 4) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Examinee SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.Examinee)
	 EXEC('INSERT INTO dbo.Tmp_Examinee (PlanId, SubjectId, PersonId, Room, Seat, Score)
		SELECT PlanId, SubjectId, PersonId, Room, Seat, Score FROM dbo.Examinee WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.Examinee
GO
EXECUTE sp_rename N'dbo.Tmp_Examinee', N'Examinee', 'OBJECT' 
GO
ALTER TABLE dbo.Examinee ADD CONSTRAINT
	PK_Examinee_1 PRIMARY KEY CLUSTERED 
	(
	PlanId,
	SubjectId,
	PersonId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Examinee ADD CONSTRAINT
	FK_Examinee_ExaminationSubject FOREIGN KEY
	(
	SubjectId,
	PlanId
	) REFERENCES dbo.ExaminationSubject
	(
	Id,
	PlanId
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Examinee ADD CONSTRAINT
	FK_Examinee_People FOREIGN KEY
	(
	PersonId
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Examinee', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Examinee', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Examinee', 'Object', 'CONTROL') as Contr_Per 
/*
   2018年3月6日19:58:08
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
ALTER TABLE dbo.People
	DROP CONSTRAINT DF_People_Id
GO
ALTER TABLE dbo.People
	DROP CONSTRAINT DF_People_IDCardValid
GO
ALTER TABLE dbo.People
	DROP CONSTRAINT DF_User_WhenCreated
GO
ALTER TABLE dbo.People
	DROP CONSTRAINT DF_User_WhenChanged
GO
CREATE TABLE dbo.Tmp_People
	(
	Id uniqueidentifier NOT NULL,
	IDCardNumber varchar(25) NOT NULL,
	Surname nvarchar(10) NOT NULL,
	GivenName nvarchar(10) NOT NULL,
	Sex int NOT NULL,
	DateOfBirth date NOT NULL,
	Ethnicity nvarchar(50) NULL,
	Address nvarchar(150) NULL,
	Issuer nvarchar(50) NULL,
	IssueDate date NULL,
	ExpiresAt date NULL,
	WhenRealIdCommited datetime2(7) NULL,
	WhenRealIdValid datetime2(7) NULL,
	RealIdValid bit NULL,
	RealIdValidBy nvarchar(50) NULL,
	Mobile varchar(15) NOT NULL,
	WhenCreated datetime2(0) NOT NULL,
	WhenChanged datetime2(0) NOT NULL,
	DisplayName  AS ([Surname]+[GivenName]) PERSISTED ,
	Email nvarchar(150) NOT NULL,
	IDCardFrontFileId uniqueidentifier NULL,
	IDCardBackFileId uniqueidentifier NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_People SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_People ADD CONSTRAINT
	DF_People_Id DEFAULT (newid()) FOR Id
GO
ALTER TABLE dbo.Tmp_People ADD CONSTRAINT
	DF_People_IDCardValid DEFAULT ((0)) FOR RealIdValid
GO
ALTER TABLE dbo.Tmp_People ADD CONSTRAINT
	DF_User_WhenCreated DEFAULT (getdate()) FOR WhenCreated
GO
ALTER TABLE dbo.Tmp_People ADD CONSTRAINT
	DF_User_WhenChanged DEFAULT (getdate()) FOR WhenChanged
GO
IF EXISTS(SELECT * FROM dbo.People)
	 EXEC('INSERT INTO dbo.Tmp_People (Id, IDCardNumber, Surname, GivenName, Sex, DateOfBirth, Ethnicity, Address, Issuer, IssueDate, ExpiresAt, WhenRealIdCommited, WhenRealIdValid, RealIdValid, RealIdValidBy, Mobile, WhenCreated, WhenChanged, Email, IDCardFrontFileId, IDCardBackFileId)
		SELECT Id, IDCardNumber, Surname, GivenName, Sex, DateOfBirth, Ethnicity, Address, Issuer, IssueDate, ExpiresAt, WhenRealIdCommited, WhenRealIdValid, RealIdValid, RealIdValidBy, Mobile, WhenCreated, WhenChanged, Email, IDCardFrontFileId, IDCardBackFileId FROM dbo.People WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.ApplicationForm
	DROP CONSTRAINT FK_ApplicationForm_People
GO
ALTER TABLE dbo.WebUser
	DROP CONSTRAINT FK_WebUser_People
GO
DROP TABLE dbo.People
GO
EXECUTE sp_rename N'dbo.Tmp_People', N'People', 'OBJECT' 
GO
ALTER TABLE dbo.People ADD CONSTRAINT
	PK_People PRIMARY KEY NONCLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE UNIQUE NONCLUSTERED INDEX IX_People_Email ON dbo.People
	(
	Email
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX IX_People_Mobile ON dbo.People
	(
	Mobile
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE CLUSTERED INDEX IX_People_WhenCreated ON dbo.People
	(
	WhenCreated
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX IX_People_IDCardNumber ON dbo.People
	(
	IDCardNumber
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT
select Has_Perms_By_Name(N'dbo.People', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.People', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.ApplicationForm ADD CONSTRAINT
	FK_ApplicationForm_People FOREIGN KEY
	(
	PersonId
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.ApplicationForm SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ApplicationForm', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.WebUser ADD CONSTRAINT
	FK_WebUser_People FOREIGN KEY
	(
	Id
	) REFERENCES dbo.People
	(
	Id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.WebUser SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.WebUser', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.WebUser', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.WebUser', 'Object', 'CONTROL') as Contr_Per 
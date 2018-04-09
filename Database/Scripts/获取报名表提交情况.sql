USE TalentGo
GO

SELECT COUNT(*) AS Uncommited
FROM [TalentGo].[dbo].[ApplicationForm]
WHERE WhenCommited IS NULL

SELECT COUNT(*) AS ReturnedBack
FROM [TalentGo].[dbo].[ApplicationForm]
WHERE WhenCommited IS NULL AND WhenFileReviewed IS NOT NULL

SELECT COUNT(*) AS Recommited
FROM [TalentGo].[dbo].[ApplicationForm]
WHERE CommitCount > 1
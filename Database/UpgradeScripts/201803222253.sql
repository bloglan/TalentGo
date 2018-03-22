UPDATE People SET RealIdCommitCount = 1
WHERE WhenRealIdCommited is not null

update ApplicationForm set CommitCount = 1
where WhenFileReviewed is not null
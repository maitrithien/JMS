IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[Jobs]') AND TYPE IN (N'U'))
BEGIN
     CREATE TABLE [dbo].[Jobs]
     (
		[APK] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		[Status] NVARCHAR(50) NULL,
		[StatusConfirm] NVARCHAR(50) NULL,
		[Priority] NVARCHAR(50) NULL,
		[Complex] NVARCHAR(50) NULL,
		[Rate] NVARCHAR(50) NULL,
		[RateComment] NVARCHAR(MAX) NULL,
		[Deadline] DATETIME NULL,
		[JobID] NVARCHAR(50) NULL,
		[JobName] NVARCHAR(250) NULL,
		[Poster] NVARCHAR(50) NULL,
		[PosterRead] BIT NOT NULL DEFAULT(0),
		[Recipient] NVARCHAR(50) NULL,
		[RecipientRead] BIT NOT NULL DEFAULT(0),
		[Confirmer] NVARCHAR(50) NULL,
		[ConfirmerRead] BIT NOT NULL DEFAULT(0),
		[Sender] NVARCHAR(50) NULL,
		[Note] NVARCHAR(MAX) NULL,
		[DepartmentID] NVARCHAR(50) NULL,
		[CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
		[LastModifyDate] DATETIME NOT NULL DEFAULT GETDATE(),
		[CreatedUserID] NVARCHAR(50) NULL,
		[LastModifyUserID] NVARCHAR(50) NULL,
		CONSTRAINT [PK_JOBS_APK] PRIMARY KEY CLUSTERED
		(
		[APK]
		)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	)
	ON [PRIMARY]
END
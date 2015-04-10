IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[Jobs]') AND TYPE IN (N'U'))
BEGIN
     CREATE TABLE [dbo].[Jobs]
     (
		[APK] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		[Status] TINYINT NOT NULL,
		[Priority] TINYINT NULL,
		[Complex] TINYINT NULL,
		[Rate] TINYINT NULL,
		[Deadline] DATETIME NOT NULL,
		[JobID] NVARCHAR(50) NOT NULL,
		[JobName] NVARCHAR(250) NOT NULL,
		[Poster] NVARCHAR(50) NOT NULL,
		[Recipient] NVARCHAR(50) NULL,
		[Confirmer] NVARCHAR(50) NULL,
		[Note] NVARCHAR(500) NULL,
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
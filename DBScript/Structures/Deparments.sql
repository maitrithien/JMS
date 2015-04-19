IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[Departments]') AND TYPE IN (N'U'))
BEGIN
     CREATE TABLE [dbo].[Departments]
     (
		[APK] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		[DepartmentID] NVARCHAR(50) NOT NULL,
		[DepartmentName] NVARCHAR(250) NULL,
		[Note] NVARCHAR(500) NULL,
		[CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
		[LastModifyDate] DATETIME NOT NULL DEFAULT GETDATE(),
		[CreatedUserID] NVARCHAR(50) NULL,
		[LastModifyUserID] NVARCHAR(50) NULL,
		CONSTRAINT [PK_DEPARTMENTS_APK] PRIMARY KEY CLUSTERED
		(
			[APK]
		)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	)
	ON [PRIMARY]
END
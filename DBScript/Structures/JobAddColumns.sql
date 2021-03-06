IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE NAME='JOBS' AND xtype='U')
	BEGIN
		IF NOT EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id=tab.id WHERE tab.name='JOBS' AND col.name='ReJobID')
		ALTER TABLE JOBS ADD ReJobID NVARCHAR(50) NULL
	END
IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE NAME='JOBS' AND xtype='U')
	BEGIN
		IF NOT EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id=tab.id WHERE tab.name='JOBS' AND col.name='SentMessage')
		ALTER TABLE JOBS ADD SentMessage NVARCHAR(MAX) NULL
	END
IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE NAME='JOBS' AND xtype='U')
	BEGIN
		IF NOT EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id=tab.id WHERE tab.name='JOBS' AND col.name='ReAPK')
		ALTER TABLE JOBS ADD ReAPK UNIQUEIDENTIFIER NULL
	END
IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE NAME='JOBS' AND xtype='U')
	BEGIN
		IF NOT EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id=tab.id WHERE tab.name='JOBS' AND col.name='Completed')
		ALTER TABLE JOBS ADD Completed NVARCHAR(50) NULL
	END
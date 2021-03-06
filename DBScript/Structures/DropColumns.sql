IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE NAME='EMPLOYEES' AND xtype='U')
	BEGIN
		IF EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id=tab.id WHERE tab.name='EMPLOYEES' AND col.name='ManagerID')
		ALTER TABLE EMPLOYEES DROP COLUMN ManagerID
	END
IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE NAME='EMPLOYEES' AND xtype='U')
	BEGIN
		IF EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id=tab.id WHERE tab.name='EMPLOYEES' AND col.name='BirthDay')
		ALTER TABLE EMPLOYEES DROP COLUMN BirthDay
	END
IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE NAME='EMPLOYEES' AND xtype='U')
	BEGIN
		IF EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id=tab.id WHERE tab.name='EMPLOYEES' AND col.name='IDCardNo')
		ALTER TABLE EMPLOYEES DROP COLUMN IDCardNo
	END
IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE NAME='EMPLOYEES' AND xtype='U')
	BEGIN
		IF EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id=tab.id WHERE tab.name='EMPLOYEES' AND col.name='Description')
		ALTER TABLE EMPLOYEES DROP COLUMN [Description]
	END	
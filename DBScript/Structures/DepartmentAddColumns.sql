IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE NAME='Departments' AND xtype='U')
	BEGIN
		IF NOT EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id=tab.id WHERE tab.name='Departments' AND col.name='AssignedPerson')
		ALTER TABLE Departments ADD AssignedPerson NVARCHAR(50) NULL
	END
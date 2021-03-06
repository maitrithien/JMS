IF EXISTS (SELECT TOP 1 1 FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[DBO].[SP]') AND  OBJECTPROPERTY(ID, N'IsProcedure') = 1)			
DROP PROCEDURE [DBO].[SP]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


-- <Summary>
--- Load cấu trúc bảng, sinh script add-alter-drop column
--- Load nội dung store, view, trigger
-- <Param>
----
-- <Return>
----
-- <Reference>
----
-- <History>
----Created by: Thanh Sơn on 02/06/2014
-- <Example>
/*
    EXEC SP 
*/

 CREATE PROCEDURE SP
(
     @ObjectID VARCHAR(50),
     @Mode TINYINT = 0, --0: cấu trúc, 1: addcolumn, 2: altercolumn, 3: dropcolumn, 9:Cấu trúc order tên cột theo abc, 10: Lấy version
     @Column VARCHAR(100) = NULL,
     @ColumnType VARCHAR(50) = NULL
 )   
AS
DECLARE @Type NVARCHAR(5), 
		@object_id VARCHAR(4000),
		@Error NVARCHAR(1000), 
		@Infor NVARCHAR(MAX), 
		@Contranint NVARCHAR(1000),
		@End NVARCHAR(100),
		@Header NVARCHAR(2000),
		@Footer NVARCHAR(2000),
		@sSQL NVARCHAR(MAX),
		@sSQL1 NVARCHAR(100) = '',
		@Order NVARCHAR(500)
IF (@Mode = 10) SELECT DBVersion FROM AT0001
IF (@Mode = 11)
BEGIN
	SET @sSQL1 = 'INTO THANHSON'
	IF EXISTS (SELECT  TOP 1 1  FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[THANHSON]') AND TYPE IN (N'U')) DROP TABLE THANHSON 
END

SET @ObjectID = UPPER(@ObjectID)		
SET @Error = ''	
SET @Infor = ''	
SET @Contranint = ''
SET @End = ''
SET @sSQL = ''
SET @Order = 'col.colorder'
IF NOT EXISTS (SELECT TOP 1 1 [type] FROM sysobjects WHERE [name] = @ObjectID) 
	BEGIN
		SET @Error = N'Không có '+@ObjectID+' trong Database '+DB_NAME()
		PRINT (@Error)
	END
ELSE 
	BEGIN	
		SELECT @object_id = OBJECT_ID, @Type= [type] FROM  sys.objects WHERE [name] = @ObjectID
		IF @Type = 'U'
		BEGIN
			IF EXISTS (SELECT TOP 1 1 FROM sys.tables t JOIN sys.default_constraints d ON d.parent_object_id = t.object_id  
									JOIN sys.columns c ON  c.object_id = t.object_id AND c.column_id = d.parent_column_id
									WHERE t.name = @ObjectID AND c.name = @Column)
				BEGIN
					SET @Contranint = '
		BEGIN
		EXEC DROPCONSTRAINT '''+@ObjectID+''','''+@Column+''' '
					SET @End = '
		END'
				END				
			IF @Mode IN (0,9, 11)
			BEGIN
				SET NOCOUNT ON
				IF @Mode = 9 SET @Order = 'col.[name]'
				SET @sSQL = '					
					SELECT CASE WHEN OBJECTPROPERTY(OBJECT_ID(inf.constraint_name), ''IsPrimaryKey'') = 1 THEN ''X'' ELSE '''' END IsKey, col.[name] ColumnName,
					CASE WHEN typ.name LIKE ''%CHAR%'' AND typ.name LIKE ''N%''THEN UPPER(typ.name) + ''('' + LTRIM(RTRIM(STR(col.[length]/2))) + '')''
						 WHEN typ.name LIKE ''%CHAR%'' AND typ.name <> ''N%''THEN UPPER(typ.name) + ''('' + LTRIM(RTRIM(STR(col.[length]))) + '')''
						 WHEN typ.name = ''DECIMAL'' THEN  UPPER(typ.name) + ''('' + LTRIM(RTRIM(STR(col.xprec)))+ '','' + LTRIM(RTRIM(STR(col.xscale))) + '')'' ELSE UPPER(typ.name) 
					END DataType,
					CASE WHEN col.isnullable = 0 THEN '''' ELSE ''X'' END [IsNull], 
					CASE WHEN ISNULL(com.[text],'''') = '''' THEN '''' ELSE SUBSTRING(com.[Text],2,LEN(com.[Text])-2) END [Default] '+@sSQL1+'		
					FROM syscolumns col
					LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE inf ON inf.column_name = col.name AND table_name = '''+@ObjectID+''' 						
					INNER JOIN sysobjects tab ON tab.ID = col.ID
					INNER JOIN systypes typ ON typ.xtype = col.xtype
					LEFT JOIN sysobjects tab1 ON tab1.ID = col.cdefault
					LEFT JOIN syscomments com ON com.id = tab1.id
					WHERE tab.name = '''+@ObjectID+'''
					AND typ.name <> ''SYSNAME''
					ORDER BY '+@Order+''	
					EXEC (@sSQL)					
			END
					
			IF @Mode = 1 SET @Infor = 
'IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE [name] = '''+@ObjectID+''' AND xtype = ''U'')
	BEGIN
		IF NOT EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id = tab.id WHERE tab.name = '''+@ObjectID+''' AND col.name = '''+@Column+''')
		ALTER TABLE '+@ObjectID+' ADD '+@Column+' '+@ColumnType+' NULL
	END'
			IF @Mode = 2 SET @Infor = 
'IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE [name] = '''+@ObjectID+''' AND xtype = ''U'')
	BEGIN
		IF EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id = tab.id WHERE tab.name = '''+@ObjectID+''' AND col.name = '''+@Column+''')'+@Contranint+'
		ALTER TABLE '+@ObjectID+' ALTER COLUMN '+@Column+' '+@ColumnType+' NULL '+@End+'
	END'
			IF @Mode = 3 SET @Infor = 
'IF EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE [name] = '''+@ObjectID+''' AND xtype = ''U'')
	BEGIN
		IF EXISTS (SELECT TOP 1 1 FROM syscolumns col INNER JOIN sysobjects tab 
		ON col.id = tab.id WHERE tab.name = '''+@ObjectID+''' AND col.name = '''+@Column+''')'+@Contranint+'
		ALTER TABLE '+@ObjectID+' DROP COLUMN '+@Column+@End+'
	END'						
			PRINT(@Infor)			                  		
		END	
		IF @Type IN ('FN', 'IF', 'TF','P','V','S','TR')
		BEGIN
			SET @Header ='IF EXISTS (SELECT TOP 1 1 FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N''[DBO].['+@ObjectID+']'') AND '
			IF @Type = 'P' SET @Header = @Header + ' OBJECTPROPERTY(ID, N''IsProcedure'') = 1)			
DROP PROCEDURE [DBO].'
			IF @Type = 'V' SET @Header = @Header + 'OBJECTPROPERTY(ID, N''IsView'') = 1)
DROP VIEW [DBO].'
			IF @Type = 'TR' SET @Header = @Header + 'OBJECTPROPERTY(ID, N''IsTrigger'') = 1)
DROP TRIGGER [DBO].'
			IF @Type IN ('FN','IF','TF') SET @Header = @Header + 'XTYPE IN (N''FN'', N''IF'', N''TF''))
DROP FUNCTION [dbo].'
			SET @Header = @Header +'[' +@ObjectID+ ']
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
'
			SET @Footer = 'GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO'
			PRINT (@Header) 
			DECLARE @chuoi NVARCHAR(MAX), @data NVARCHAR(MAX), @line INT
			SELECT @chuoi = [definition] FROM Sys.sql_modules WHERE OBJECT_ID = (SELECT ID FROM sysobjects WHERE NAME = @ObjectID)
			WHILE LEN(@chuoi)>4000
				BEGIN
					SET @data = LEFT(@chuoi,4000)
					SET @data = REVERSE(@data)
					SET @line = CHARINDEX(CHAR(10) + CHAR(13), @data)
					PRINT LEFT(@chuoi, 4000 - @line + 1)
					SET @chuoi = RIGHT(@chuoi, LEN(@chuoi) - 4000 + @line - 1)
				END
			 IF (LEN(@chuoi) > 0) PRINT @chuoi			
			PRINT (@Footer)			
		END
	END	

IF @Mode = 11
BEGIN

	DECLARE @Cur CURSOR, @IsKey VARCHAR(1), @ColumnName VARCHAR(50), @DataType VARCHAR(50), @IsNull VARCHAR(10), @Default VARCHAR(50),
			@sSQL2 NVARCHAR(MAX) = '', @sSQL3 NVARCHAR(2000) = ''
			
	SET @sSQL2 = 
'IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N''[dbo].['+@ObjectID+']'') AND TYPE IN (N''U''))
BEGIN
     CREATE TABLE [dbo].['+@ObjectID+']
     ('
	SET @Cur = CURSOR SCROLL KEYSET FOR
	SELECT IsKey, ColumnName, DataType, [IsNull], [Default]	FROM THANHSON
	OPEN @Cur
	FETCH NEXT FROM @Cur INTO @IsKey, @ColumnName, @DataType, @IsNull, @Default
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @sSQL2 = @sSQL2 + '
		[' + @ColumnName+ '] ' + @DataType + CASE WHEN ISNULL(@Default, '') <> '' THEN ' DEFAULT ' + UPPER(@Default) ELSE '' END + ' ' +
		+ CASE WHEN @IsNull = 'X' THEN 'NULL,' ELSE 'NOT NULL,' END
		
		IF @IsKey = 'X' SET @sSQL3 = @sSQL3 + '
		[' + @ColumnName + '],'
		
		FETCH NEXT FROM @Cur INTO @IsKey, @ColumnName, @DataType, @IsNull, @Default
	END
	CLOSE @Cur
	SET @sSQL2 = LEFT (@sSQL2, LEN(@sSQL2) - 1) + '
	CONSTRAINT [PK_'+@ObjectID+'] PRIMARY KEY CLUSTERED
      (' + LEFT(@sSQL3, LEN(@sSQL3) - 1) + '
      )
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		)
	ON [PRIMARY]
END' 
	WHILE LEN(@sSQL2)> 4000
				BEGIN
					SET @data = LEFT(@sSQL2,4000)
					SET @data = REVERSE(@data)
					SET @line = CHARINDEX(CHAR(10) + CHAR(13), @data)
					PRINT LEFT(@sSQL2, 4000 - @line + 1)
					SET @sSQL2 = RIGHT(@sSQL2, LEN(@sSQL2) - 4000 + @line - 1)
				END
			 IF (LEN(@sSQL2) > 0) PRINT @sSQL2				
	DROP TABLE THANHSON
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

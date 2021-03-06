IF EXISTS (SELECT TOP 1 1 FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[DBO].[GETEMPLOYEEBYMANAGER]') AND  OBJECTPROPERTY(ID, N'IsProcedure') = 1)			
DROP PROCEDURE [DBO].[GETEMPLOYEEBYMANAGER]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Admin>
-- Create date: <19/04/2015>
-- Description:	<Get all employee by manager>
-- EXEC GetEmployeeByManager 'kiem.daoxuan'
-- =============================================
CREATE PROCEDURE GetEmployeeByManager
	-- ManagerID
	@ManagerID NVARCHAR(50) = N''
AS
BEGIN
	DECLARE @DepartmentID NVARCHAR(50),
			@EmployeeID NVARCHAR(50),
			@Group NVARCHAR(50)
	
	-- SET NOCOUNT ON added to prevent extra result sets FROM
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Get all employee by manager
	SELECT TOP 1 
		@Group = GroupID, @EmployeeID = EmployeeID, @DepartmentID = DepartmentID 
	FROM Employees e WHERE e.UserName = @ManagerID

	print @DepartmentID
	Print @Group
	
	IF(@DepartmentID = N'01')
		BEGIN
			SELECT EmployeeID FROM Employees
			WHERE UserName <> @ManagerID
		END
	ELSE
		BEGIN
			SELECT EmployeeID FROM Employees
			WHERE DepartmentID = @DepartmentID AND UserName <> @ManagerID AND (GroupID = @Group OR GroupID = '0')
		END
END
GO

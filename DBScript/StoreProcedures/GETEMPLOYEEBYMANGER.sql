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
			@Group NVARCHAR(50)
	
	-- SET NOCOUNT ON added to prevent extra result sets FROM
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Get all employee by manager
	SELECT TOP 1 @DepartmentID = DepartmentID FROM Departments d WHERE d.ManagerID = @ManagerID
	SELECT TOP 1 @Group = GroupID FROM Employees e WHERE e.EmployeeID = @ManagerID AND e.DepartmentID = @DepartmentID
	
	IF(@DepartmentID = N'GD')
		BEGIN
			SELECT EmployeeID FROM Employees
			WHERE GroupID <> @Group
		END
	ELSE
		BEGIN
			SELECT EmployeeID FROM Employees
			WHERE DepartmentID = @DepartmentID AND GroupID <> @Group
		END
END
GO

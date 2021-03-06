IF EXISTS (SELECT TOP 1 1 FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[DBO].[GETMANAGER]') AND  OBJECTPROPERTY(ID, N'IsProcedure') = 1)			
DROP PROCEDURE [DBO].[GETMANAGER]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Admin>
-- Create date: <19/04/2015>
-- Description:	<Get all employee by manager>
-- EXEC GetManager 'c.nguyenvan'
-- EXEC GetManager 'anhpha.lethi'
-- EXEC GetManager 'kiem.daoxuan'
-- =============================================
CREATE PROCEDURE GetManager
	-- ManagerID
	@EmployeeID NVARCHAR(50) = N''
AS
BEGIN
	DECLARE @ManagerID NVARCHAR(50)
	
	-- SET NOCOUNT ON added to prevent extra result sets FROM
	SET NOCOUNT ON;
	
	SELECT @ManagerID = d.ManagerID
	FROM Departments d
		LEFT JOIN Employees e ON d.DepartmentID = e.DepartmentID
	WHERE e.EmployeeID = @EmployeeID
	
	IF(@EmployeeID = @ManagerID)
	BEGIN
		SELECT @ManagerID = 
		(
			SELECT TOP 1 e.EmployeeID FROM Departments d
			INNER JOIN Employees e ON	e.EmployeeID = d.ManagerID AND 
										e.GroupID IN (	SELECT MAX(c.CodeID) 
														FROM Codes c WHERE c.CodeGroupID = 'EMPLOYEE_LEVEL')
		)
	END
	
	SELECT @ManagerID ManagerID
	
END
GO

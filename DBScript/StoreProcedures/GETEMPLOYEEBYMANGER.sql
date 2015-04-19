SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Admin>
-- Create date: <19/04/2015>
-- Description:	<Get all employee by manager>
-- =============================================
CREATE PROCEDURE GetEmployeeByManager
	-- ManagerID
	@ManagerID NVARCHAR(50) = N''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets FROM
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Get all employee by manager
	SELECT EmployeeID, ManagerID FROM Employees
	WHERE ManagerID IS NOT NULL
	AND ( 
		EmployeeID IN (SELECT EmployeeID
			FROM Employees
			WHERE ManagerID = @ManagerID) 
		OR
		ManagerID IN (SELECT EmployeeID 
			FROM Employees
			WHERE ManagerID = @ManagerID)
	)
END
GO

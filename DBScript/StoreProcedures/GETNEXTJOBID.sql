IF EXISTS (SELECT TOP 1 1 FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[DBO].[GetNextJobID]') AND  OBJECTPROPERTY(ID, N'IsProcedure') = 1)			
DROP PROCEDURE [DBO].[GetNextJobID]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Admin>
-- Create date: <19/04/2015>
-- Description:	<Get job id>
-- EXEC GetNextJobID  @UserName='f.nguyenvan'
-- =============================================
CREATE PROCEDURE GetNextJobID
	@UserName NVARCHAR(56)
AS
BEGIN
	SELECT 
		e.DepartmentID, e.EmployeeID, YEAR(GETDATE()) [Year], MONTH(GETDATE()) [Month], COUNT(*) OVER () + 1 [Next]
	FROM Jobs j
		LEFT JOIN Employees e ON e.UserName = j.CreatedUserID
	WHERE j.CreatedUserID = @UserName
	AND MONTH(j.CreatedDate) = MONTH(GETDATE()) AND YEAR(j.CreatedDate) = YEAR(GETDATE())

END
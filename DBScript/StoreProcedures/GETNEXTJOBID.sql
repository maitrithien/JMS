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
-- EXEC GetNextJobID  @UserName='anh.phamdong'
-- =============================================
CREATE PROCEDURE GetNextJobID
	@UserName NVARCHAR(56)
AS
BEGIN
	DECLARE @numrow INT = 0,
	@EmployeeID NVARCHAR(50) = N''
	
	-- Get EmployeeID by UserName
	SELECT @EmployeeID = e.EmployeeID FROM Employees e WHERE e.UserName = @UserName
	PRINT(@EmployeeID)
	
	SELECT @numrow = COUNT(*)
		FROM Jobs j WHERE 
		j.Poster = @EmployeeID 
		AND MONTH(j.CreatedDate) = MONTH(GETDATE()) AND YEAR(j.CreatedDate) = YEAR(GETDATE())
		
	IF(@numrow > 0)
	BEGIN
		SELECT TOP 1
		e.DepartmentID, e.EmployeeID, YEAR(GETDATE()) [Year], MONTH(GETDATE()) [Month], @numrow + 1 [Next]
		FROM Jobs j
			LEFT JOIN Employees e ON e.EmployeeID = j.Poster
		WHERE j.Poster = @EmployeeID
		AND MONTH(j.CreatedDate) = MONTH(GETDATE()) AND YEAR(j.CreatedDate) = YEAR(GETDATE())
	END
	ELSE
		BEGIN
			SELECT e.DepartmentID, e.EmployeeID, YEAR(GETDATE()) [Year], MONTH(GETDATE()) [Month], 1 [Next]
			FROM Employees e
			WHERE e.UserName = @UserName
		END

END
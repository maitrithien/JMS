IF EXISTS (SELECT TOP 1 1 FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[DBO].[GETCOUNTERJOBS]') AND  OBJECTPROPERTY(ID, N'IsProcedure') = 1)			
DROP PROCEDURE [DBO].[GETCOUNTERJOBS]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Admin>
-- Create date: <19/04/2015>
-- Description:	<Get all employee by manager>
-- EXEC GetCounterJobs @UserName='thu.lethi'
-- =============================================
CREATE PROCEDURE GetCounterJobs
	@UserName NVARCHAR(56)
AS
BEGIN
	DECLARE 
		@EmployeeID NVARCHAR(50) = N'',
		@countP INT = 0, -- Personal
		@countE INT = 0, -- Employee
		@countO INT = 0, -- Overdue
		@countS INT = 0, -- Sent
		@countR INT = 0 -- Received
		
	-- Get EmployeeID by UserName
	SELECT @EmployeeID = e.EmployeeID
	FROM Employees e WHERE e.UserName = @UserName
	
	-- SET NOCOUNT ON added to prevent extra result sets FROM
	SET NOCOUNT ON;
	
	-- =============================================
	-- Grid Jobs
	-- =============================================
	--SELECT @countP = COUNT(*)
	--FROM Jobs j
	--WHERE j.Sender IS NOT NULL AND
	--(
	--	j.CreatedUserID = @UserName OR
	--	(
	--		j.Recipient = @EmployeeID
	--		AND ISNULL(j.StatusConfirm, '0') = '1' -- Is Confirmed
	--		AND ISNULL(j.[Status], '0') <> '0' -- not new
	--	)
	--)
	-- =============================================
	-- Grid Jobs Employee
	-- =============================================
	---- Get all with-out filter
	--DECLARE @DepartmentID NVARCHAR(50),
	--		@Group NVARCHAR(50)
	
	---- Get all employee by manager
	--SELECT TOP 1 @DepartmentID = DepartmentID FROM Departments d WHERE d.ManagerID = @EmployeeID
	--SELECT TOP 1 @Group = GroupID FROM Employees e WHERE e.EmployeeID = @EmployeeID-- AND e.DepartmentID = @DepartmentID
	--IF(@Group <> '0')
	--BEGIN
	--	IF(@DepartmentID = N'01')
	--	SELECT @countE = COUNT(*)
	--	FROM Jobs j
	--	WHERE
	--		j.CreatedUserID = @UserName OR
	--		j.Sender IN (SELECT EmployeeID FROM Employees WHERE GroupID <> @Group) OR
	--		(j.Poster IN (SELECT EmployeeID FROM Employees WHERE GroupID <> @Group) AND j.PosterRead = 0) OR
	--		(j.Confirmer IN (SELECT EmployeeID FROM Employees WHERE GroupID <> @Group) AND j.ConfirmerRead = 0) OR
	--		(
	--			j.Recipient IN (SELECT EmployeeID FROM Employees WHERE GroupID <> @Group) AND
	--			j.RecipientRead = 0 AND
	--			ISNULL(j.StatusConfirm,'0') = '1'
	--		)
	--	ELSE
	--		SELECT @countE = COUNT(*)
	--		FROM Jobs j
	--		WHERE 
	--			j.CreatedUserID = @UserName OR
	--			(j.Poster IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @DepartmentID AND GroupID <> @Group) AND j.PosterRead = 0) OR
	--			(j.Sender IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @DepartmentID AND GroupID <> @Group) AND j.ConfirmerRead = 0) OR
	--			j.Confirmer IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @DepartmentID AND GroupID <> @Group) OR
	--			(
	--				j.Recipient IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @DepartmentID AND GroupID <> @Group) AND
	--				j.RecipientRead = 0 AND
	--				ISNULL(j.StatusConfirm,'0') = '1'
	--			)
	--END
	
	-- =============================================
	-- Grid Jobs Received
	-- =============================================
	---- Get all with-out filter
	SELECT @countR = COUNT(*)
	FROM Jobs j
	WHERE 
		(
			(j.Recipient = @EmployeeID AND j.RecipientRead = 0 AND ISNULL(j.StatusConfirm, '0') = '1') OR 
			(j.Confirmer = @EmployeeID AND j.ConfirmerRead = 0)
		) AND j.Sender IS NOT NULL
	
	-- =============================================
	-- Grid Jobs Sent
	-- =============================================
	
	---- Get all with-out filter
	--SELECT @countS = COUNT(*)
	--FROM Jobs j
	--WHERE
	--	j.Sender = @EmployeeID
	-- =============================================
	-- Grid Jobs Overdue
	-- =============================================
	
	---- Get all with-out filter
	SELECT @countO = COUNT(*)
	FROM Jobs j
	WHERE DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) > 0
	AND ISNULL(j.[Status], '0') NOT IN ('2', '9') -- Khác hoàn tất/Đã hủy
	AND (
			j.CreatedUserID = @UserName OR
			j.Confirmer = @EmployeeID OR (
				j.Recipient = @EmployeeID
				AND ISNULL(j.StatusConfirm, '0') = '1' -- Is Confirmed
			)
		)
		
	SELECT @countP CountP, @countE CountE, @countO CountO, @countS CountS, @countR CountR
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

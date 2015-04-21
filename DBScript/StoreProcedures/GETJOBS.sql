IF EXISTS (SELECT TOP 1 1 FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[DBO].[GETJOBS]') AND  OBJECTPROPERTY(ID, N'IsProcedure') = 1)			
DROP PROCEDURE [DBO].[GETJOBS]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Admin>
-- Create date: <19/04/2015>
-- Description:	<Get all employee by manager>
-- EXEC GetJobs @JobID=N'312', @UserName='f.nguyenvan', @Type = 3
-- =============================================
CREATE PROCEDURE GetJobs
	@IsFilter INT = 0, ---- 0: Non-Filter, 1: Filter
	@JobID NVARCHAR(50) = NULL, 
	@JobName NVARCHAR(250)  = NULL,
	@Status NVARCHAR(50) = NULL,
	@Poster NVARCHAR(50) = NULL,
	@Recipient NVARCHAR(50) = NULL,
	@Confirmer NVARCHAR(50) = NULL,
	@Deadline DATETIME = NULL,
	@Priority NVARCHAR(50) = NULL,
	@Rate NVARCHAR(50) = NULL,
	@Complex NVARCHAR(50) = NULL,
	@DepartmentID NVARCHAR(50) = NULL,
	@UserName NVARCHAR(56),
	@Type INT = 0 ---- 0: Grid Jobs, 1: Grid Jobs Received, 2: Grid Jobs Sent, 3: Grid Jobs Overdue, 4: Grid Jobs of Employee
AS
BEGIN
	DECLARE @EmployeeID NVARCHAR(50) = N''
	
	-- Get EmployeeID by UserName
	SELECT @EmployeeID = e.EmployeeID
	FROM Employees e WHERE e.UserName = @UserName
	
	-- SET NOCOUNT ON added to prevent extra result sets FROM
	SET NOCOUNT ON;
	
	-- =============================================
	-- Grid Jobs
	-- =============================================
	IF(@Type = 0)
	BEGIN
		IF(@IsFilter = 0)
		BEGIN
			---- Get all with-out filter
			SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
			WHERE 
				j.CreatedUserID = @UserName OR
				j.Confirmer = @EmployeeID OR (
					j.Recipient = @EmployeeID
                    AND ISNULL(j.StatusConfirm, '0') = '1' -- Is Confirmed
                    AND ISNULL(j.[Status], '0') <> '0' -- not new
                )
		END
		ELSE
			BEGIN
				---- Get all with-out filter
				SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
				WHERE 
					(
						j.JobID LIKE N'%' + ISNULL(@JobID, j.JobID) + '%' AND
						j.JobName LIKE N'%' + ISNULL(@JobName, j.JobName) + '%'AND
						j.[Status] = ISNULL(@Status, j.[Status]) AND
						j.Poster = ISNULL(@Poster, j.Poster) AND
						j.Confirmer = ISNULL(@Confirmer, j.Confirmer) AND
						j.Recipient = ISNULL(@Recipient, j.Recipient) AND
						Convert(VARCHAR, j.Deadline, 112) = Convert(VARCHAR, ISNULL(@Deadline, j.Deadline), 112) AND
						j.DepartmentID = ISNULL(@DepartmentID, j.DepartmentID) AND
						j.Complex = ISNULL(@Complex, j.Complex) AND
						j.Rate = ISNULL(@Rate, j.Rate) AND
						j.[Priority] = ISNULL(@Priority, j.[Priority])
					)
					AND (
							j.CreatedUserID = @UserName OR
							j.Confirmer = @EmployeeID OR (
							j.Recipient = @EmployeeID
							AND ISNULL(j.StatusConfirm, '0') = '1' -- Is Confirmed
							AND ISNULL(j.[Status], '0') <> '0' -- not new
							)
						)
			END
	END
	
	-- =============================================
	-- Grid Jobs Received
	-- =============================================
	ELSE IF (@Type = 1)
	BEGIN
		IF(@IsFilter = 0)
		BEGIN
			---- Get all with-out filter
			SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
			WHERE 
				j.Confirmer = @EmployeeID OR (
					j.Recipient = @EmployeeID
                    AND ISNULL(j.StatusConfirm, '0') = '1' -- Is Confirmed
                    AND ISNULL(j.[Status], '0') <> '0' -- not new
                )
		END
		ELSE
			BEGIN
				---- Get all with-out filter
				SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
				WHERE 
					(
						j.JobID LIKE N'%' + ISNULL(@JobID, j.JobID) + '%' AND
						j.JobName LIKE N'%' + ISNULL(@JobName, j.JobName) + '%'AND
						j.[Status] = ISNULL(@Status, j.[Status]) AND
						j.Poster = ISNULL(@Poster, j.Poster) AND
						j.Confirmer = ISNULL(@Confirmer, j.Confirmer) AND
						j.Recipient = ISNULL(@Recipient, j.Recipient) AND
						Convert(VARCHAR, j.Deadline, 112) = Convert(VARCHAR, ISNULL(@Deadline, j.Deadline), 112) AND
						j.DepartmentID = ISNULL(@DepartmentID, j.DepartmentID) AND
						j.Complex = ISNULL(@Complex, j.Complex) AND
						j.Rate = ISNULL(@Rate, j.Rate) AND
						j.[Priority] = ISNULL(@Priority, j.[Priority])
					)
					AND (
							j.Confirmer = @EmployeeID OR (
							j.Recipient = @EmployeeID
							AND ISNULL(j.StatusConfirm, '0') = '1' -- Is Confirmed
							AND ISNULL(j.[Status], '0') <> '0' -- not new
							)
						)
			END
	END
	
	-- =============================================
	-- Grid Jobs Sent
	-- =============================================
	ELSE IF (@Type = 2)
	BEGIN
		IF(@IsFilter = 0)
		BEGIN
			---- Get all with-out filter
			SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
			WHERE 
				j.CreatedUserID = @UserName OR
				j.Poster = @EmployeeID
		END
		ELSE
			BEGIN
				---- Get all with-out filter
				SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
				WHERE 
					(
						j.JobID LIKE N'%' + ISNULL(@JobID, j.JobID) + '%' AND
						j.JobName LIKE N'%' + ISNULL(@JobName, j.JobName) + '%'AND
						j.[Status] = ISNULL(@Status, j.[Status]) AND
						j.Poster = ISNULL(@Poster, j.Poster) AND
						j.Confirmer = ISNULL(@Confirmer, j.Confirmer) AND
						j.Recipient = ISNULL(@Recipient, j.Recipient) AND
						Convert(VARCHAR, j.Deadline, 112) = Convert(VARCHAR, ISNULL(@Deadline, j.Deadline), 112) AND
						j.DepartmentID = ISNULL(@DepartmentID, j.DepartmentID) AND
						j.Complex = ISNULL(@Complex, j.Complex) AND
						j.Rate = ISNULL(@Rate, j.Rate) AND
						j.[Priority] = ISNULL(@Priority, j.[Priority])
					)
					AND (
							j.CreatedUserID = @UserName OR
							j.Poster = @EmployeeID
						)
			END
	END
	
	-- =============================================
	-- Grid Jobs Overdue
	-- =============================================
	ELSE IF (@Type = 3)
	BEGIN
		IF(@IsFilter = 0)
		BEGIN
			---- Get all with-out filter
			SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
			WHERE DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) > 0
			AND (
					j.CreatedUserID = @UserName OR
					j.Confirmer = @EmployeeID OR (
						j.Recipient = @EmployeeID
						AND ISNULL(j.StatusConfirm, '0') = '1' -- Is Confirmed
						AND ISNULL(j.[Status], '0') <> '0' -- not new
					)
				)
		END
		ELSE
			BEGIN
				---- Get all with-out filter
				SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, 
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
				WHERE 
					(
						j.JobID LIKE N'%' + ISNULL(@JobID, j.JobID) + '%' AND
						j.JobName LIKE N'%' + ISNULL(@JobName, j.JobName) + '%'AND
						j.[Status] = ISNULL(@Status, j.[Status]) AND
						j.Poster = ISNULL(@Poster, j.Poster) AND
						j.Confirmer = ISNULL(@Confirmer, j.Confirmer) AND
						j.Recipient = ISNULL(@Recipient, j.Recipient) AND
						Convert(VARCHAR, j.Deadline, 112) = Convert(VARCHAR, ISNULL(@Deadline, j.Deadline), 112) AND
						j.DepartmentID = ISNULL(@DepartmentID, j.DepartmentID) AND
						j.Complex = ISNULL(@Complex, j.Complex) AND
						j.Rate = ISNULL(@Rate, j.Rate) AND
						j.[Priority] = ISNULL(@Priority, j.[Priority]) AND
						DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) > 0
					)
					AND (
							j.CreatedUserID = @UserName OR
							j.Confirmer = @EmployeeID OR (
								j.Recipient = @EmployeeID
								AND ISNULL(j.StatusConfirm, '0') = '1' -- Is Confirmed
								AND ISNULL(j.[Status], '0') <> '0' -- not new
							)
						)
			END
	END

	-- =============================================
	-- Grid Jobs of Employee
	-- =============================================
	ELSE IF (@Type = 4)
	BEGIN
		DECLARE @ManagerDepartmentID NVARCHAR(50),
				@Group NVARCHAR(50)
	
		-- Get all employee by manager
		SELECT TOP 1 @ManagerDepartmentID = DepartmentID FROM Departments d WHERE d.ManagerID = @EmployeeID
		SELECT TOP 1 @Group = GroupID FROM Employees e WHERE e.EmployeeID = @EmployeeID AND e.DepartmentID = @ManagerDepartmentID

		IF(@IsFilter = 0)
		BEGIN
			---- Get all with-out filter
			SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
			WHERE DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) > 0
			AND (
					j.CreatedUserID = @UserName OR
					j.Confirmer IN (SELECT EmployeeID FROM Employees WHERE GroupID <> @Group) OR
					j.Recipient IN (SELECT EmployeeID FROM Employees WHERE GroupID <> @Group)
				)
		END
		ELSE
			BEGIN
				---- Get all with-out filter
				SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, 
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON er.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c4.CodeGroupID = N'RATE'
				WHERE 
					(
						j.JobID LIKE N'%' + ISNULL(@JobID, j.JobID) + '%' AND
						j.JobName LIKE N'%' + ISNULL(@JobName, j.JobName) + '%'AND
						j.[Status] = ISNULL(@Status, j.[Status]) AND
						j.Poster = ISNULL(@Poster, j.Poster) AND
						j.Confirmer = ISNULL(@Confirmer, j.Confirmer) AND
						j.Recipient = ISNULL(@Recipient, j.Recipient) AND
						Convert(VARCHAR, j.Deadline, 112) = Convert(VARCHAR, ISNULL(@Deadline, j.Deadline), 112) AND
						j.DepartmentID = ISNULL(@DepartmentID, j.DepartmentID) AND
						j.Complex = ISNULL(@Complex, j.Complex) AND
						j.Rate = ISNULL(@Rate, j.Rate) AND
						j.[Priority] = ISNULL(@Priority, j.[Priority]) AND
						DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) > 0
					)
					AND (
							j.CreatedUserID = @UserName OR
							j.Confirmer IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @ManagerDepartmentID AND GroupID <> @Group) OR
							j.Recipient IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @ManagerDepartmentID AND GroupID <> @Group)
						)
			END
	END
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

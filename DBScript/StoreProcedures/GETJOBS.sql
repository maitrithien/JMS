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
-- EXEC GetJobs @IsFilter=0, @UserName='thanhphuong.tranngo', @Type = 0
-- =============================================
CREATE PROCEDURE GetJobs
	@IsFilter INT = 0, ---- 0: Non-Filter, 1: Filter
	@JobID NVARCHAR(50) = NULL, 
	@JobName NVARCHAR(250) = NULL,
	@Status NVARCHAR(50) = '0',
	@Poster NVARCHAR(50) = NULL,
	@Recipient NVARCHAR(50) = NULL,
	@Confirmer NVARCHAR(50) = NULL,
	@Deadline DATETIME = NULL,
	@Priority NVARCHAR(50) = '1',
	@Rate NVARCHAR(50) = NULL,
	@Complex NVARCHAR(50) = '1',
	@DepartmentID NVARCHAR(50) = NULL,
	@UserName NVARCHAR(56),
	@CreatedDate DATETIME = NULL,
	@Type INT = 0 ---- 0: Grid Jobs, 1: Grid Jobs Received, 2: Grid Jobs Sent, 3: Grid Jobs Overdue, 4: Grid Jobs of Employee
AS
BEGIN
	DECLARE @EmployeeID NVARCHAR(50) = N'',
			@Group NVARCHAR(50) = N'',
			@ManagerDepartmentID NVARCHAR(50) = N''
	
	-- Get EmployeeID by UserName
	SELECT @EmployeeID = e.EmployeeID
	FROM Employees e WHERE e.UserName = @UserName
	
	SELECT TOP 1 @Group = GroupID FROM Employees e WHERE e.EmployeeID = @EmployeeID
	
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
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
			  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
			WHERE 
				j.CreatedUserID = @UserName OR
				(
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
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
				  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
				WHERE 
					(
						ISNULL(j.JobID, '') LIKE N'%' + ISNULL(@JobID, ISNULL(j.JobID, '')) + '%'
						AND ISNULL(j.JobName, '') LIKE N'%' + ISNULL(@JobName, ISNULL(j.JobName, '')) + '%'
						AND ISNULL(j.[Status], '') = ISNULL(@Status, ISNULL(j.[Status], ''))
						AND ISNULL(j.Poster, '') = ISNULL(@Poster, ISNULL(j.Poster, ''))
						AND ISNULL(j.Confirmer, '') = ISNULL(@Confirmer, ISNULL(j.Confirmer, ''))
						AND ISNULL(j.Recipient, '') = ISNULL(@Recipient, ISNULL(j.Recipient, ''))
						AND Convert(VARCHAR, ISNULL(j.Deadline, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@Deadline, ISNULL(j.Deadline, GETDATE())), 112)
						AND ISNULL(j.DepartmentID, '') = ISNULL(@DepartmentID, ISNULL(j.DepartmentID, ''))
						AND ISNULL(j.Complex, '') = ISNULL(@Complex, ISNULL(j.Complex, ''))
						AND ISNULL(j.Rate, '') = ISNULL(@Rate, ISNULL(j.Rate, ''))
						AND ISNULL(j.[Priority], '') = ISNULL(@Priority, ISNULL(j.[Priority], ''))
						AND Convert(VARCHAR, ISNULL(j.CreatedDate, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@CreatedDate, ISNULL(j.CreatedDate, GETDATE())), 112)
					)
					AND (
							j.CreatedUserID = @UserName OR
							(
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
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
			  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
			WHERE 
				((j.Recipient = @EmployeeID AND ISNULL(j.StatusConfirm, '0') = '1') OR j.Confirmer = @EmployeeID) AND j.Sender IS NOT NULL
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
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
				  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
				WHERE 
					(
						ISNULL(j.JobID, '') LIKE N'%' + ISNULL(@JobID, ISNULL(j.JobID, '')) + '%'
						AND ISNULL(j.JobName, '') LIKE N'%' + ISNULL(@JobName, ISNULL(j.JobName, '')) + '%'
						AND ISNULL(j.[Status], '') = ISNULL(@Status, ISNULL(j.[Status], ''))
						AND ISNULL(j.Poster, '') = ISNULL(@Poster, ISNULL(j.Poster, ''))
						AND ISNULL(j.Confirmer, '') = ISNULL(@Confirmer, ISNULL(j.Confirmer, ''))
						AND ISNULL(j.Recipient, '') = ISNULL(@Recipient, ISNULL(j.Recipient, ''))
						AND Convert(VARCHAR, ISNULL(j.Deadline, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@Deadline, ISNULL(j.Deadline, GETDATE())), 112)
						AND ISNULL(j.DepartmentID, '') = ISNULL(@DepartmentID, ISNULL(j.DepartmentID, ''))
						AND ISNULL(j.Complex, '') = ISNULL(@Complex, ISNULL(j.Complex, ''))
						AND ISNULL(j.Rate, '') = ISNULL(@Rate, ISNULL(j.Rate, ''))
						AND ISNULL(j.[Priority], '') = ISNULL(@Priority, ISNULL(j.[Priority], ''))
						AND Convert(VARCHAR, ISNULL(j.CreatedDate, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@CreatedDate, ISNULL(j.CreatedDate, GETDATE())), 112)
					)
					AND ((j.Recipient = @EmployeeID AND ISNULL(j.StatusConfirm, '0') = '1') OR j.Confirmer = @EmployeeID) AND j.Sender IS NOT NULL
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
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
			  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
			WHERE 
				j.Sender = @EmployeeID
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
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
				  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
				WHERE 
					(
						ISNULL(j.JobID, '') LIKE N'%' + ISNULL(@JobID, ISNULL(j.JobID, '')) + '%'
						AND ISNULL(j.JobName, '') LIKE N'%' + ISNULL(@JobName, ISNULL(j.JobName, '')) + '%'
						AND ISNULL(j.[Status], '') = ISNULL(@Status, ISNULL(j.[Status], ''))
						AND ISNULL(j.Poster, '') = ISNULL(@Poster, ISNULL(j.Poster, ''))
						AND ISNULL(j.Confirmer, '') = ISNULL(@Confirmer, ISNULL(j.Confirmer, ''))
						AND ISNULL(j.Recipient, '') = ISNULL(@Recipient, ISNULL(j.Recipient, ''))
						AND Convert(VARCHAR, ISNULL(j.Deadline, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@Deadline, ISNULL(j.Deadline, GETDATE())), 112)
						AND ISNULL(j.DepartmentID, '') = ISNULL(@DepartmentID, ISNULL(j.DepartmentID, ''))
						AND ISNULL(j.Complex, '') = ISNULL(@Complex, ISNULL(j.Complex, ''))
						AND ISNULL(j.Rate, '') = ISNULL(@Rate, ISNULL(j.Rate, ''))
						AND ISNULL(j.[Priority], '') = ISNULL(@Priority, ISNULL(j.[Priority], ''))
						AND Convert(VARCHAR, ISNULL(j.CreatedDate, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@CreatedDate, ISNULL(j.CreatedDate, GETDATE())), 112)
					)
					AND (
							j.Sender = @EmployeeID
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
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
			  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
			WHERE DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) > 0
			AND ISNULL(j.[Status], '0') NOT IN ('2', '9') -- Khác hoàn tất/Đã hủy
			AND (
					j.CreatedUserID = @UserName OR
					j.Confirmer = @EmployeeID OR (
						j.Recipient = @EmployeeID
						AND ISNULL(j.StatusConfirm, '0') = '1' -- Is Confirmed
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
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
				  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
				WHERE 
					(
						ISNULL(j.JobID, '') LIKE N'%' + ISNULL(@JobID, ISNULL(j.JobID, '')) + '%'
						AND ISNULL(j.JobName, '') LIKE N'%' + ISNULL(@JobName, ISNULL(j.JobName, '')) + '%'
						AND ISNULL(j.[Status], '') = ISNULL(@Status, ISNULL(j.[Status], ''))
						AND ISNULL(j.Poster, '') = ISNULL(@Poster, ISNULL(j.Poster, ''))
						AND ISNULL(j.Confirmer, '') = ISNULL(@Confirmer, ISNULL(j.Confirmer, ''))
						AND ISNULL(j.Recipient, '') = ISNULL(@Recipient, ISNULL(j.Recipient, ''))
						AND Convert(VARCHAR, ISNULL(j.Deadline, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@Deadline, ISNULL(j.Deadline, GETDATE())), 112)
						AND ISNULL(j.DepartmentID, '') = ISNULL(@DepartmentID, ISNULL(j.DepartmentID, ''))
						AND ISNULL(j.Complex, '') = ISNULL(@Complex, ISNULL(j.Complex, ''))
						AND ISNULL(j.Rate, '') = ISNULL(@Rate, ISNULL(j.Rate, ''))
						AND ISNULL(j.[Priority], '') = ISNULL(@Priority, ISNULL(j.[Priority], '')) 
						AND Convert(VARCHAR, ISNULL(j.CreatedDate, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@CreatedDate, ISNULL(j.CreatedDate, GETDATE())), 112)
						AND DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) > 0
					)
					AND ISNULL(j.[Status], '0') NOT IN ('2', '9') -- Khác hoàn tất/Đã hủy
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
		DECLARE @CurrentDepartment NVARCHAR(50)

		SELECT TOP 1 
			@Group = GroupID, @EmployeeID = EmployeeID, @CurrentDepartment = DepartmentID 
		FROM Employees e WHERE e.UserName = @UserName

		CREATE TABLE #Temp
		(
			EmployeeID NVARCHAR(50) NULL
		)

		IF(@CurrentDepartment = N'01')
			BEGIN
				INSERT INTO #Temp (EmployeeID)
				SELECT EmployeeID FROM Employees
				WHERE UserName <> @UserName
			END
		ELSE
			BEGIN
				INSERT INTO #Temp (EmployeeID)
				SELECT EmployeeID FROM Employees
				WHERE DepartmentID = @CurrentDepartment AND UserName <> @UserName AND (GroupID = @Group OR GroupID = '0')
			END

		IF(@IsFilter = 0)
		BEGIN
			SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
			FROM Jobs j
			  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
			  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
			  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
			  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
			  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
			  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
			  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
			  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
			  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
			  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
			  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
			WHERE 
				j.Poster IN (SELECT EmployeeID FROM #Temp) OR
				(
					j.Recipient IN (SELECT EmployeeID FROM #Temp) AND
					ISNULL(j.StatusConfirm,'0') = '1'
				)
		END
		ELSE
			BEGIN
				SELECT  j.APK, j.[Status], c1.CodeName StatusName, j.StatusConfirm, c2.CodeName StatusConfirmName, 
					j.[Priority], c3.CodeName PriorityName, j.Complex, c4.CodeName ComplexName,
					j.Rate, c5.CodeName RateName, j.Deadline, j.JobID, j.JobName, 
					j.Poster, ep.FullName PosterName, 
					j.Recipient, er.FullName RecipientName,
					j.Confirmer, ec.FullName ConfirmerName,
					j.Sender, es.FullName SenderName, j.Completed, ISNULL(c6.CodeName, '') CompletedName,
					j.Note, j.CreatedDate, j.LastModifyDate, j.CreatedUserID, j.LastModifyUserID,
					j.DepartmentID, d.DepartmentName, j.RateComment, j.PosterRead, j.RecipientRead, j.ConfirmerRead,
					DATEDIFF(DAY, ISNULL(j.Deadline, GETDATE()), GETDATE()) OverDeadlineNumber
				FROM Jobs j
				  LEFT JOIN Departments d ON d.DepartmentID = j.DepartmentID
				  LEFT JOIN Employees ec ON ec.EmployeeID = j.Confirmer
				  LEFT JOIN Employees ep ON ep.EmployeeID = j.Poster
				  LEFT JOIN Employees er ON er.EmployeeID = j.Recipient
				  LEFT JOIN Employees es ON es.EmployeeID = j.Sender
				  LEFT JOIN Codes c1 ON c1.CodeID = j.[Status] AND c1.CodeGroupID = N'STATUS'
				  LEFT JOIN Codes c2 ON c2.CodeID = j.StatusConfirm AND c2.CodeGroupID = N'CONFIRM_STATUS'
				  LEFT JOIN Codes c3 ON c3.CodeID = j.[Priority] AND c3.CodeGroupID = N'PRIORITY'
				  LEFT JOIN Codes c4 ON c4.CodeID = j.Complex AND c4.CodeGroupID = N'COMPLEX'
				  LEFT JOIN Codes c5 ON c5.CodeID = j.Rate AND c5.CodeGroupID = N'RATE'
				  LEFT JOIN Codes c6 ON c6.CodeID = j.Completed AND c6.CodeGroupID = N'STATUS_COMPLETED'
				WHERE 
				(
					ISNULL(j.JobID, '') LIKE N'%' + ISNULL(@JobID, ISNULL(j.JobID, '')) + '%'
					AND ISNULL(j.JobName, '') LIKE N'%' + ISNULL(@JobName, ISNULL(j.JobName, '')) + '%'
					AND ISNULL(j.[Status], '') = ISNULL(@Status, ISNULL(j.[Status], ''))
					AND ISNULL(j.Poster, '') = ISNULL(@Poster, ISNULL(j.Poster, ''))
					AND ISNULL(j.Confirmer, '') = ISNULL(@Confirmer, ISNULL(j.Confirmer, ''))
					AND ISNULL(j.Recipient, '') = ISNULL(@Recipient, ISNULL(j.Recipient, ''))
					AND Convert(VARCHAR, ISNULL(j.Deadline, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@Deadline, ISNULL(j.Deadline, GETDATE())), 112)
					AND ISNULL(j.DepartmentID, '') = ISNULL(@DepartmentID, ISNULL(j.DepartmentID, ''))
					AND ISNULL(j.Complex, '') = ISNULL(@Complex, ISNULL(j.Complex, ''))
					AND ISNULL(j.Rate, '') = ISNULL(@Rate, ISNULL(j.Rate, ''))
					AND ISNULL(j.[Priority], '') = ISNULL(@Priority, ISNULL(j.[Priority], '')) 
					AND Convert(VARCHAR, ISNULL(j.CreatedDate, GETDATE()), 112) = Convert(VARCHAR, ISNULL(@CreatedDate, ISNULL(j.CreatedDate, GETDATE())), 112)
				)
				AND
				(	
					j.Poster IN (SELECT EmployeeID FROM #Temp) OR
					(
						j.Recipient IN (SELECT EmployeeID FROM #Temp) AND
						ISNULL(j.StatusConfirm,'0') = '1'
					)
				)
			END

		DROP TABLE #Temp
	END
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

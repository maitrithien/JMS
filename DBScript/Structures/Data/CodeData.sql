/*************************************************************
Xóa bảng Codes
**************************************************************/
Delete Codes

/*************************************************************
--- Tình trạng hoàn tất ---- delete Codes
**************************************************************/
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'0' AND CodeGroupID = N'STATUS_COMPLETED')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'0', N'Chưa hoàn tất', N'Tình trạng hoàn tất', 'STATUS_COMPLETED')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'1' AND CodeGroupID = N'STATUS_COMPLETED')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'1', N'Đã hoàn tất', N'Tình trạng hoàn tất', 'STATUS_COMPLETED')
END
/*************************************************************
--- Tình trạng ----
**************************************************************/
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'0' AND CodeGroupID = N'STATUS')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'0', N'JOBS mới', N'Tình trạng JOBS', 'STATUS')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'1' AND CodeGroupID = N'STATUS')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'1', N'Đang xử lý', N'Tình trạng JOBS', 'STATUS')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'2' AND CodeGroupID = N'STATUS')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'2', N'Đã hoàn tất', N'Tình trạng JOBS', 'STATUS')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'3' AND CodeGroupID = N'STATUS')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'2', N'Tạm hoãn', N'Tình trạng JOBS', 'STATUS')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'9' AND CodeGroupID = N'STATUS')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'9', N'Đã hủy', N'Tình trạng JOBS', 'STATUS')
END


/*************************************************************
--- Tình trạng duyệt ----
**************************************************************/

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'0' AND CodeGroupID = N'CONFIRM_STATUS')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'0', N'Chưa chấp nhận', N'Tình trạng duyệt JOBS', 'CONFIRM_STATUS')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'1' AND CodeGroupID = N'CONFIRM_STATUS')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'1', N'Chấp nhận', N'Tình trạng duyệt JOBS', 'CONFIRM_STATUS')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'9' AND CodeGroupID = N'CONFIRM_STATUS')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'9', N'Không chuyển', N'Tình trạng duyệt JOBS', 'CONFIRM_STATUS')
END
/*************************************************************
--- ĐỘ PHỨC TẠP ----
**************************************************************/
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'1' AND CodeGroupID = N'COMPLEX')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'1', N'1', N'Độ phức tạp', 'COMPLEX')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'2' AND CodeGroupID = N'COMPLEX')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'2', N'2', N'Độ phức tạp', 'COMPLEX')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'3' AND CodeGroupID = N'COMPLEX')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'3', N'3', N'Độ phức tạp', 'COMPLEX')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'4' AND CodeGroupID = N'COMPLEX')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'4', N'4', N'Độ phức tạp', 'COMPLEX')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'5' AND CodeGroupID = N'COMPLEX')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'5', N'5', N'Độ phức tạp', 'COMPLEX')
END

/*************************************************************
--- ĐỘ ƯU TIÊN ----
**************************************************************/
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'1' AND CodeGroupID = N'PRIORITY')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'1', N'1', N'Độ ưu tiên', 'PRIORITY')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'2' AND CodeGroupID = N'PRIORITY')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'2', N'2', N'Độ ưu tiên', 'PRIORITY')
END
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'3' AND CodeGroupID = N'PRIORITY')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'3', N'3', N'Độ ưu tiên', 'PRIORITY')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'4' AND CodeGroupID = N'PRIORITY')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'4', N'4', N'Độ ưu tiên', 'PRIORITY')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'5' AND CodeGroupID = N'PRIORITY')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'5', N'5', N'Độ ưu tiên', 'PRIORITY')
END

/*************************************************************
--- Đánh giá ----
**************************************************************/
IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'A' AND CodeGroupID = N'RATE')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'A', N'A', N'Xuất sắc', 'RATE')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'B' AND CodeGroupID = N'RATE')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'B', N'B', N'Tốt', 'RATE')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'C' AND CodeGroupID = N'RATE')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'C', N'C', N'Trung bình', 'RATE')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'D' AND CodeGroupID = N'RATE')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'D', N'D', N'Đạt', 'RATE')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'E' AND CodeGroupID = N'RATE')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'E', N'E', N'Kém', 'RATE')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'F' AND CodeGroupID = N'RATE')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'F', N'F', N'Không đạt', 'RATE')
END

/*************************************************************
--- Tình trạng duyệt ----
**************************************************************/

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'0' AND CodeGroupID = N'EMPLOYEE_LEVEL')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'0', N'Nhân viên', N'Chức vụ', 'EMPLOYEE_LEVEL')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'1' AND CodeGroupID = N'EMPLOYEE_LEVEL')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'1', N'Trưởng phòng', N'Chức vụ', 'EMPLOYEE_LEVEL')
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Codes Where CodeID = N'9' AND CodeGroupID = N'EMPLOYEE_LEVEL')
BEGIN
	INSERT INTO Codes(CodeID, CodeName, CodeDescription, CodeGroupID) VALUES(N'9', N'Giám đốc', N'Chức vụ', 'EMPLOYEE_LEVEL')
END


CREATE VIEW ViewUserRoles
AS
(
	SELECT DISTINCT u.UserId, u.UserName,
	(
		SELECT LEFT(Roles.Id, LEN(Roles.Id) - 2)
		FROM
		(
			SELECT 
			stuff(
				ISNULL((SELECT Convert(NVARCHAR, r.RoleId) + ' | ' From webpages_Roles r
				LEFT JOIN webpages_UsersInRoles ur ON r.RoleId = ur.RoleId
				WHERE UserId = u.UserId
				FOR XML PATH('')), '')
			, 1, 0, '') Id
		) Roles
	) AS UserRoleId,
	(
		SELECT LEFT(Roles.Name, LEN(Roles.Name) - 2)
		FROM
		(
			SELECT 
			stuff(
				ISNULL((SELECT r.RoleName + ' | ' From webpages_Roles r
				LEFT JOIN webpages_UsersInRoles ur ON r.RoleId = ur.RoleId
				WHERE UserId = u.UserId
				FOR XML PATH('')), '')
			, 1, 0, '') Name 
		) Roles
	) AS UserRoleName
	FROM UserProfile u 
)

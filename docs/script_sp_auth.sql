CREATE PROCEDURE sp_GetAuthByProfileId
 @profileId as int
AS
select p.ProfileId,p.NickName,r.Name 'Rol',u.Name 'User',u.Email,s.SystemId,s.Code 'SytemCode',s.SystemType,s.Name 'SystemName',s.Path 'SystemPath',
	   m.ModuleId,m.Name 'ModuleName',m.Path 'ModulePath',a.CanRead 'ModuleCanRead',
	   op.OptionId,op.Name 'OptionName',op.Path 'OptionPath',ai.CanRead 'OptionCanRead'
from Options op 
inner join Modules m on m.ModuleId = op.ModuleId
inner join AssignmentItems ai on ai.OptionId = op.OptionId
inner join Assignments a on a.ModuleId = m.ModuleId
inner join Profiles p on p.ProfileId = a.ProfileId
inner join Users u on u.UserId = p.UserId
inner join Roles r on r.RolId = p.RoleId 
inner join Systems s on s.SystemId = r.SystemId
where p.ProfileId = @profileId and p.Status = 1
GO

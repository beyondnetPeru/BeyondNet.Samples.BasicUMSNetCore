use FargoSecurity
go

select s.SystemId,s.Name,r.RolId,r.Name 
from Roles r
inner join Systems s on s.SystemId = r.SystemId

select p.ProfileId,p.NickName, u.UserId,u.Name,u.UserName,u.Email 
from Profiles p
inner join Users u on u.UserId = p.UserId

select s.SystemId,s.Name, m.ModuleId, m.Name, o.OptionId, o.Name 
from Options o
inner join Modules m on m.ModuleId = o.ModuleId
inner join Systems S on s.SystemId = m.SystemId

select * from AssignmentItems


declare @username nvarchar(50) = 'glaforge'

/*
delete from duty where id in (20,21)
update duty set EndDate = null where id = 6
*/

select * from Duty d
	join Astronaut a on a.id = d.astronautid
	join Person p on p.id = a.PersonId
where UserName = @username
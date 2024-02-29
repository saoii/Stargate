declare @username nvarchar(50) = 'jpicard'

/*
delete from duty where id in (33,34)
update duty set EndDate = null where id = 6
*/

select * from Duty d
	join Astronaut a on a.id = d.astronautid
	join Person p on p.id = a.PersonId
where UserName = @username
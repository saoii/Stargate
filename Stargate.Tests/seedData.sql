USE [AlfredTech]
GO

SET IDENTITY_INSERT person ON;

truncate table person
delete from Astronaut
truncate table duty


INSERT INTO [dbo].[Person](Id, FirstName,LastName)
VALUES 
(1,'Jean-Luc','Picard'),
(2,'William','Riker'),
(3,'Data','Soong'),
(4,'Geordi','La Forge'),
(5,'Beverly','Crusher')

SET IDENTITY_INSERT Astronaut ON;
INSERT INTO [dbo].[Astronaut](Id, PersonId) VALUES (1,1),(2,2),(3,3),(4,4)

SET IDENTITY_INSERT duty ON;
INSERT INTO [dbo].[Duty]
           ([AstronautId]
           ,[Rank]
           ,[Title]
           ,[StartDate]
           ,[EndDate])
VALUES 
(1,4,1,'01-01-1987','01-01-2004'),
(1,4,5,'01-01-2004',null),
(2,0,1,'01-01-1987','12-31-1987'),
(2,1,1,'01-01-1988','12-31-1988'),
(2,2,1,'01-01-1989','12-31-1988'),
(2,3,1,'01-01-1990',null),
(3,0,1,'01-01-1987','12-31-1987'),
(3,1,1,'01-01-1988','12-31-1988'),
(3,2,1,'01-01-1989',null),
(4,0,3,'01-01-1987','12-31-1987'),
(4,1,3,'01-01-1988','12-31-1988'),
(4,2,3,'01-01-1989',null)


SET IDENTITY_INSERT person OFF;
SET IDENTITY_INSERT Astronaut OFF;
SET IDENTITY_INSERT duty OFF;
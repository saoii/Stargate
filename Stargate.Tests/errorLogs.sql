SELECT TOP (1000) [Id]
      ,[Message]
      ,[MessageTemplate]
      ,[Level]
      ,[TimeStamp]
      ,[Exception]
      ,[Properties]
  FROM [dbo].[LogEvents]
  order by TimeStamp desc
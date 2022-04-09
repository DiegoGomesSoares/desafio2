USE [Payment]
GO

INSERT INTO [dbo].[Pessoas]
           ([nome]
           ,[cpf]
           ,[dataNascimento])
     VALUES
           ('Diego teste2'
           ,'389.215.780-48'
           ,GetDate())
GO

/****** Script for SelectTopNRows command from SSMS  ******/
UPDATE [Smithy].[dbo].[Stock] SET InMaintance = 0  WHERE InMaintance is NULL

ALTER TABLE STOCK
ADD CONSTRAINT InMaintanceNoNull 
DEFAULT 0 FOR InMaintance

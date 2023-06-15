USE Smithy;   
GO  
ALTER TABLE AccountData   
ADD CONSTRAINT AK_Account UNIQUE (login,userid);   
GO 
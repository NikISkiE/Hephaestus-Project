CREATE FUNCTION StockCalc(@eqid int)  
RETURNS int
AS  
BEGIN  
    DECLARE @WeaponCount int;
    SELECT @WeaponCount = COUNT(@eqid)
    FROM Stock
    WHERE EquipmentID = @eqid; 
    RETURN @WeaponCount;
END;
GO
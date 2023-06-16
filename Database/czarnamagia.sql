CREATE FUNCTION StockCalc(@eqid int)  
RETURNS int
AS  
BEGIN  
    DECLARE @WeaponCount int;
    SELECT @WeaponCount = COUNT(@eqid)
    FROM Stock
    WHERE EquipmentID = @eqid and InMaintance = 0 and UserIDL IS NULL
    RETURN @WeaponCount;
END;
GO
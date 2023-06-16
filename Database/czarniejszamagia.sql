ALTER TABLE Equipment
DROP COLUMN Stocked

ALTER TABLE Equipment
ADD Stocked AS dbo.StockCalc(ID)
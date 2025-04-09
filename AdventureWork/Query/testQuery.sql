SELECT 
    h.SalesOrderID, 
    h.OrderDate, 
    CONCAT(p.FirstName, ' ', p.LastName) as CustomerName, 
    h.TotalDue
FROM Sales.SalesOrderHeader h
JOIN Sales.Customer c ON h.CustomerID = c.CustomerID
JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
ORDER BY h.OrderDate DESC

SELECT *
FROM Sales.SalesOrderDetail sod
WHERE sod.SalesOrderID = 75084

        SELECT 
            h.SalesOrderID,
            h.OrderDate,
            h.DueDate,
            h.ShipDate,
            h.Status,
            h.AccountNumber,
            h.SubTotal,
            h.TaxAmt,
            h.Freight,
            h.TotalDue,
            s.Name AS ShipMethod,
            CONCAT(p.FirstName, ' ', p.LastName) AS CustomerName
        FROM Sales.SalesOrderHeader h
        JOIN Sales.Customer c ON h.CustomerID = c.CustomerID
        JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
		JOIN Purchasing.ShipMethod s ON h.ShipMethodID = s.ShipMethodID
        WHERE h.SalesOrderID = 75084

SELECT 
*
FROM Sales.SalesOrderHeader 
ORDER BY TotalDue desc;

SELECT 
    CONCAT(p.FirstName, ' ', p.LastName) AS CustomerName,
    SUM(h.TotalDue) AS TotalSales
FROM Sales.SalesOrderHeader h
JOIN Sales.Customer c ON h.CustomerID = c.CustomerID
JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
GROUP BY p.FirstName, p.LastName
ORDER BY TotalSales DESC
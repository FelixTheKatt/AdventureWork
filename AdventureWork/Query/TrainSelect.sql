select top 10 * from Person.Person

select P.EmailAddress from HumanResources.Employee as E
inner join Person.EmailAddress as P on E.BusinessEntityID = P.BusinessEntityID

SELECT SOD.SalesOrderID,SOH.OrderDate, SOH.SubTotal FROM SALES.SalesOrderDetail as SOD
inner join Sales.SalesOrderHeader as SOH on SOD.SalesOrderID = SOH.SalesOrderID
where SOH.SubTotal > 1000

select * from Sales.SalesORderHeader h
JOIN Sales.Customer c ON h.CustomerID = c.CustomerID
JOIN Person.Person p ON c.PersonID = p.BusinessEntityID

select top 5 * from Production.Product ORDER BY StandardCost

SELECT COUNT(*) from Production.Product

select * from Person.Person as P
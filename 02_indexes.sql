USE SalesPulse;
GO
CREATE NONCLUSTERED INDEX IX_Orders_OrderDate
    ON dbo.Orders (OrderDate)
    INCLUDE (TotalAmount, Status, CustomerId, CityId, PaymentMethod);
GO
CREATE NONCLUSTERED INDEX IX_Orders_CityId_OrderDate
    ON dbo.Orders (CityId, OrderDate)
    INCLUDE (TotalAmount, Status, CustomerId);
GO
CREATE NONCLUSTERED INDEX IX_Orders_Status_OrderDate
    ON dbo.Orders (Status, OrderDate)
    INCLUDE (TotalAmount, CustomerId, CityId, PaymentMethod, OrderNumber);
GO
CREATE NONCLUSTERED INDEX IX_Orders_CustomerId_OrderDate
    ON dbo.Orders (CustomerId, OrderDate)
    INCLUDE (TotalAmount, Status);
GO
CREATE NONCLUSTERED INDEX IX_OrderDetails_ProductId
    ON dbo.OrderDetails (ProductId)
    INCLUDE (OrderId, Quantity, LineTotal);
GO
CREATE NONCLUSTERED INDEX IX_OrderDetails_OrderId
    ON dbo.OrderDetails (OrderId)
    INCLUDE (ProductId, Quantity, UnitPrice, LineTotal);
GO
CREATE NONCLUSTERED INDEX IX_Products_CategoryId
    ON dbo.Products (CategoryId)
    INCLUDE (Name, UnitPrice);
GO
CREATE NONCLUSTERED INDEX IX_Customers_CityId
    ON dbo.Customers (CityId)
    INCLUDE (FullName, Email);
GO
CREATE NONCLUSTERED INDEX IX_Customers_FullName
    ON dbo.Customers (FullName);
GO

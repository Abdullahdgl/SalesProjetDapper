/*
SalesPulse e-ticaret þemasý
Tablolar: Cities, Categories, Products, Customers, Orders, OrderDetails, Payments
*/
IF DB_ID(N'SalesPulse') IS NULL
BEGIN
    CREATE DATABASE SalesPulse;
END
GO
USE SalesPulse;
GO
IF OBJECT_ID(N'dbo.Payments', N'U') IS NOT NULL DROP TABLE dbo.Payments;
IF OBJECT_ID(N'dbo.OrderDetails', N'U') IS NOT NULL DROP TABLE dbo.OrderDetails;
IF OBJECT_ID(N'dbo.Orders', N'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID(N'dbo.Customers', N'U') IS NOT NULL DROP TABLE dbo.Customers;
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID(N'dbo.Categories', N'U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID(N'dbo.Cities', N'U') IS NOT NULL DROP TABLE dbo.Cities;
GO
CREATE TABLE dbo.Cities
(
    CityId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Cities PRIMARY KEY,
    Name NVARCHAR(80) NOT NULL,
    Region NVARCHAR(40) NOT NULL,
    CONSTRAINT UQ_Cities_Name UNIQUE (Name)
);
CREATE TABLE dbo.Categories
(
    CategoryId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categories PRIMARY KEY,
    Name NVARCHAR(80) NOT NULL,
    CONSTRAINT UQ_Categories_Name UNIQUE (Name)
);
CREATE TABLE dbo.Products
(
    ProductId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
    CategoryId INT NOT NULL CONSTRAINT FK_Products_Categories REFERENCES dbo.Categories (CategoryId),
    Name NVARCHAR(160) NOT NULL,
    UnitPrice DECIMAL(12,2) NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Products_IsActive DEFAULT (1)
);
CREATE TABLE dbo.Customers
(
    CustomerId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Customers PRIMARY KEY,
    FullName NVARCHAR(120) NOT NULL,
    Email NVARCHAR(160) NOT NULL,
    CityId INT NOT NULL CONSTRAINT FK_Customers_Cities REFERENCES dbo.Cities (CityId),
    RegisteredAt DATETIME2(0) NOT NULL,
    CONSTRAINT UQ_Customers_Email UNIQUE (Email)
);
CREATE TABLE dbo.Orders
(
    OrderId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Orders PRIMARY KEY,
    OrderNumber VARCHAR(20) NOT NULL,
    CustomerId INT NOT NULL CONSTRAINT FK_Orders_Customers REFERENCES dbo.Customers (CustomerId),
    CityId INT NOT NULL CONSTRAINT FK_Orders_Cities REFERENCES dbo.Cities (CityId),
    OrderDate DATETIME2(0) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    TotalAmount DECIMAL(14,2) NOT NULL,
    PaymentMethod VARCHAR(20) NOT NULL,
    CONSTRAINT UQ_Orders_OrderNumber UNIQUE (OrderNumber),
    CONSTRAINT CK_Orders_Status CHECK (Status IN ('Completed','Shipped','Pending','Cancelled','Returned')),
    CONSTRAINT CK_Orders_Payment CHECK (PaymentMethod IN ('CreditCard','DebitCard','BankTransfer','Wallet','CashOnDelivery'))
);
CREATE TABLE dbo.OrderDetails
(
    OrderDetailId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_OrderDetails PRIMARY KEY,
    OrderId BIGINT NOT NULL CONSTRAINT FK_OrderDetails_Orders REFERENCES dbo.Orders (OrderId),
    ProductId INT NOT NULL CONSTRAINT FK_OrderDetails_Products REFERENCES dbo.Products (ProductId),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(12,2) NOT NULL,
    LineTotal DECIMAL(14,2) NOT NULL,
    CONSTRAINT CK_OrderDetails_Qty CHECK (Quantity > 0)
);
CREATE TABLE dbo.Payments
(
    PaymentId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Payments PRIMARY KEY,
    OrderId BIGINT NOT NULL CONSTRAINT FK_Payments_Orders REFERENCES dbo.Orders (OrderId),
    Method VARCHAR(20) NOT NULL,
    Amount DECIMAL(14,2) NOT NULL,
    PaidAt DATETIME2(0) NULL,
    Status VARCHAR(20) NOT NULL,
    CONSTRAINT UQ_Payments_OrderId UNIQUE (OrderId)
);
GO

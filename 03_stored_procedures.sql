USE SalesPulse;
GO
CREATE OR ALTER PROCEDURE dbo.sp_GetDashboardKpis
   @fromDate DATETIME2,
   @toDate DATETIME2
   AS
   BEGIN

   SET NOCOUNT ON;
    SELECT
       COUNT(*) AS TotalOrders,
       ISNULL(SUM(TotalAmount), 0) AS TotalRevenue,
       COUNT(DISTINCT CustomerId) AS TotalCustomers,
       ISNULL(AVG(TotalAmount),0) AS AverageBasket
       FROM dbo.Orders
       WHERE OrderDate >= @FromDate
       AND OrderDate < DATEADD(DAY, 1, @ToDate);
       END
       GO
       CREATE OR ALTER PROCEDURE dbo.sp_GetOrdersPaged
       @Search NVARCHAR(80) = NULL,
       @FromDate DATETIME2 = NULL,
       @ToDate DATETIME2 = NULL,
       @CityId INT =NULL,
       @Status NVARCHAR(20) = NULL,
       @MinAmount DECIMAL(14,2) = NULL,
       @MaxAmount DECIMAL(14,2) = NULL,
       @SortColumn VARCHAR(20) = 'OrderDate',
       @SortDirection VARCHAR(4) = 'DESC',
       @Page INT =1,
       @PageSize INT = 25
       AS

       BEGIN
          SET NOCOUNT ON;
          IF @Page <1 SET @Page = 1;
          IF @PageSize <1 SET @PageSize = 25;
          IF @PageSize > 10000 SET @PageSize = 10000;
          DECLARE @Offset INT = (@Page - 1) * @PageSize;
          DECLARE @SearchLike NVARCHAR(82) = NULL;
          IF @Search IS NOT NULL AND LTRIM(RTRIM(@Search)) <> ''
             SET @SearchLike = N'%' + LTRIM(RTRIM(@Search)) + N'%';
             ;WITH Filtered AS
             (
              SELECT
               O.OrderId,
               O.OrderNumber,
               c.FullName AS CustomerName,
               ci.Name AS CityName,
               O.OrderDate,
               o.TotalAmount,
               o.PaymentMethod,
               o.Status,
               COUNT(*) OVER() AS TotalCount
               FROM dbo.Orders o
               INNER JOIN dbo.Customers c ON o.CustomerId = c.CustomerId
               INNER JOIN dbo.Cities ci ON ci.CityId = o.CityId
               WHERE (@FromDate IS NULL OR o.OrderDate >= @FromDate)
               AND (@ToDate IS NULL OR o.OrderDate < DATEADD(DAY, 1, @ToDate))
               AND (@CityId IS NULL OR o.CityId = @CityId)
               AND (@Status IS NULL OR o.Status = @Status)
               AND (@MinAmount IS NULL OR o.TotalAmount >= @MinAmount)
               AND (@MaxAmount IS NULL OR o.TotalAmount <= @MaxAmount)
               AND (
               @SearchLike IS NULL 
               OR o.OrderNumber LIKE @SearchLike
               OR c.FullName LIKE @SearchLike
               )
              )

              SELECT
        OrderId,OrderNumber,CustomerName,CityName,OrderDate,TotalAmount,PaymentMethod,Status,TotalCount FROM Filtered
        ORDER BY
        CASE WHEN @SortColumn = 'OrderNumber' AND @SortDirection ='ASC' THEN OrderNumber END ASC,
        CASE WHEN @SortColumn = 'OrderNumber' AND @SortDirection ='DESC' THEN OrderNumber END DESC,
        
        CASE WHEN @SortColumn = 'CustomerName' AND @SortDirection = 'ASC' THEN CustomerName END ASC,
        CASE WHEN @SortColumn = 'CustomerName' AND @SortDirection = 'DESC' THEN CustomerName END DESC,

        CASE WHEN @SortColumn ='CityName' AND @SortDirection = 'ASC' THEN CityName END ASC,
        CASE WHEN @SortColumn ='CityName' AND @SortDirection = 'DESC' THEN CityName END DESC,

        CASE WHEN @SortColumn = 'TotalAmount' AND @SortDirection = 'ASC' THEN TotalAmount END ASC,
        CASE WHEN @SortColumn = 'TotalAmount' AND @SortDirection = 'DESC' THEN TotalAmount END DESC,

        CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'ASC' THEN Status END ASC,
        CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'DESC' THEN Status END DESC,

        CASE WHEN @SortColumn = 'PaymentMethod' AND @SortDirection = 'ASC' THEN PaymentMethod END ASC,
        CASE WHEN @SortColumn = 'PaymentMethod' AND @SortDirection = 'DESC' THEN PaymentMethod END DESC,

        CASE WHEN @SortDirection ='ASC' THEN OrderDate END ASC,
        CASE WHEN @SortDirection ='DESC' THEN OrderDate END DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
        END;


        GO
        CREATE OR ALTER PROCEDURE dbo.sp_GetTopCustomersByRevenue
        @FromDate DATETIME2,
        @ToDate DATETIME2,
        @TopN INT = 20
        AS
        BEGIN
           SET NOCOUNT ON;
           SELECT TOP (@TopN)
           c.CustomerId,
           c.FullName,
           ci.Name AS CityName,
           COUNT(*) AS OrderCount,
           SUM(o.TotalAmount) AS Revenue
           FROM dbo.Orders o
          INNER JOIN dbo.Customers c ON c.CustomerId = o.CustomerId
          INNER JOIN dbo.Cities ci ON ci.CityId = c.CityId
          WHERE o.OrderDate >= @FromDate
            AND o.OrderDate  < DATEADD(DAY,1, @ToDate)
            AND o.Status IN ('Completed', 'Shipped')
        GROUP BY c.CustomerId,c.FullName, ci.Name
        HAVING SUM(o.TotalAmount) >0
        ORDER BY Revenue DESC;
        END;

        GO
        CREATE OR ALTER PROCEDURE dbo.sp_GetOrderDetail
         @OrderId BIGINT
         AS
         BEGIN
         SET NOCOUNT ON;
         SELECT
            
            o.OrderId,
            o.OrderNumber,
            
            c.FullName AS CustomerName,
            c.Email,

            ci.Name AS CityName,

            o.OrderDate,
            o.Status,
            o.TotalAmount,
            o.PaymentMethod,

            p.Status AS PaymentStatus,
            p.PaidAt

            FROM dbo.Orders o
    INNER JOIN dbo.Customers c ON c.CustomerId = o.CustomerId
    INNER JOIN dbo.Cities ci ON ci.CityId = o.CityId
    LEFT JOIN dbo.Payments p ON p.OrderId = o.OrderId
    WHERE o.OrderId = @OrderId;

    SELECT
        od.OrderDetailId,
        pr.Name AS ProductName,
        cat.Name AS CategoryName,
        od.Quantity,
        od.UnitPrice,
        od.LineTotal
            
   FROM dbo.OrderDetails od
    INNER JOIN dbo.Products pr ON pr.ProductId = od.ProductId
    INNER JOIN dbo.Categories cat ON cat.CategoryId = pr.CategoryId
    WHERE od.OrderId = @OrderId
    ORDER BY od.OrderDetailId;
END
GO
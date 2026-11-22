using Dapper;
using SalesProjetDapper.Web.Context;
using SalesProjetDapper.Web.Dtos.Common;
using SalesProjetDapper.Web.Dtos.Dashboard;
using System.Data;

namespace SalesProjetDapper.Web.Services.Dashboard
{
	public class DashboardService : IDashboardService
	{
		private readonly AppDbContext _context;

		public DashboardService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<DashboardPayload> GetAsync(DateRange range, CancellationToken ct = default)
		{
			return new DashboardPayload
			{
				Kpis = await GetKpisAsync(range, ct),
				MonthlySales = await GetMonthlySalesAsync(range,ct),
				TopCategories = await GetTopCategoriesAsync(range,10, ct),
				TopCities = await GetTopCitiesAsync(range,10,ct),
				StatusBreakdown = await GetStatusBreakdownAsync(range,ct),
				TopProducts = await GetTopProductsAsync(range,10,ct)

			};
		}

		private async Task<DashboardKpis> GetKpisAsync(DateRange range, CancellationToken ct = default)
		{
		using var connection = _context.CreateConnection();
			return await connection.QuerySingleAsync<DashboardKpis>(
				new CommandDefinition(
					"dbo.sp_GetDashboardKpis",
					new { FromDate = range.From, ToDate = range.To },
					commandType: CommandType.StoredProcedure,
					cancellationToken:ct ));
		}

		private async Task<IReadOnlyList<MonthlySalesPoint>> GetMonthlySalesAsync(DateRange range, CancellationToken ct)
		{
			const string sql = """
					SELECT YEAR(OrderDate) AS [Year], MONTH(OrderDate) AS [Month],
					        COUNT(*) AS OrderCount, SUM(TotalAmount) AS Revenue
					FROM dbo.Orders
					WHERE OrderDate >= @From AND OrderDate < DATEADD(DAY,1,@To)
					GROUP BY YEAR(OrderDate), MONTH(OrderDate)
					ORDER BY [Year], [Month];
					""";
			using var connection = _context.CreateConnection();
			var rows = await connection.QueryAsync<MonthlySalesPoint>(
				new CommandDefinition(sql, new { range.From, range.To }, cancellationToken: ct));
			return rows.AsList();
				
		}

		private async Task<IReadOnlyList<NamedMetric>> GetTopCategoriesAsync(DateRange range, int top, CancellationToken ct)
		{
			const string sql = """
				SELECT TOP (@Top)
				  cat.Name AS Name,
				  SUM(od.LineTotal) AS Revenue,
				  SUM(od.Quantity) AS Quantity
				FROM dbo.OrderDetails od
				INNER JOIN dbo.Orders o ON o.OrderId = od.OrderId
				INNER JOIN dbo.Products p ON p.ProductId = od.ProductId
				INNER JOIN dbo.Categories cat ON cat.CategoryId = p.CategoryId
				WHERE o.OrderDate >= @From AND o.OrderDate <DATEADD(DAY,1,@To)
				  AND o.Status IN ('Completed', 'Shipped')
				GROUP BY cat.Name
				ORDER BY Revenue DESC;
				""";
			using var connection = _context.CreateConnection();
			var rows = await connection.QueryAsync<NamedMetric>(
				new CommandDefinition(sql, new {range.From, range.To, Top = top}, cancellationToken: ct));
			return rows.AsList();
		}

		private async Task<IReadOnlyList<NamedMetric>> GetTopCitiesAsync(DateRange range, int top, CancellationToken ct)
		{
			const string sql = """
				SELECT TOP (@Top)
				   c.Name AS Name,
				   SUM(o.TotalAmount) AS Revenue,
				   COUNT(*) AS Quantity
				FROM dbo.Orders o
				INNER JOIN dbo.Cities c ON c.CityId = o.CityId
				WHERE o.OrderDate >= @From AND o.OrderDate < DATEADD(DAY,1,@To)
				GROUP BY c.Name
				ORDER BY Revenue DESC;
				""";
			using var connection = _context.CreateConnection();
			var rows = await connection.QueryAsync<NamedMetric>(
				new CommandDefinition(sql, new { range.From, range.To, Top = top }, cancellationToken: ct));
			return rows.AsList();

		}

		private async Task<IReadOnlyList<OrderStatusCount>> GetStatusBreakdownAsync(DateRange range, CancellationToken ct)
		{
			const string sql = """
				SELECT Status, COUNT(*) AS Count
				FROM dbo.Orders
				WHERE OrderDate >= @From AND OrderDate < DATEADD(DAY,1,@To)
				GROUP BY Status
				ORDER BY Count DESC;
				""";
			using var connection = _context.CreateConnection();
			var rows = await connection.QueryAsync<OrderStatusCount>(
				new CommandDefinition(sql,new {range.From,range.To}, cancellationToken: ct));
			return rows.AsList();
		}

		private async Task<IReadOnlyList<NamedMetric>> GetTopProductsAsync(DateRange range, int top, CancellationToken ct)
		{
			const string sql = """
				SELECT TOP (@Top)
				  p.Name AS Name,
				  SUM(od.LineTotal) AS Revenue,
				  sum(od.Quantity) AS Quantity
				FROM dbo.OrderDetails od
				INNER JOIN dbo.Orders o ON o.OrderId = od.OrderId
				INNER JOIN dbo.Products p ON p.ProductId = od.ProductId
				WHERE o.OrderDate >= @From AND o.OrderDate < DATEADD(DAY,1,@To)
				 AND o.Status IN ('Completed','Shipped')
				GROUP BY p.Name
				ORDER BY Quantity DESC, Revenue DESC;
				""";
			using var connection = _context.CreateConnection();
			var rows = await connection.QueryAsync<NamedMetric>(
				new CommandDefinition(sql, new { range.From, range.To, Top = top }, cancellationToken: ct));
			return rows.AsList();
		}

	}
}

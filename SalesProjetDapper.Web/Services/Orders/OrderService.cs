using Dapper;
using SalesProjetDapper.Web.Context;
using SalesProjetDapper.Web.Dtos.Common;
using SalesProjetDapper.Web.Dtos.Orders;
using System.Data;

namespace SalesProjetDapper.Web.Services.Orders
{
	public class OrderService : IOrderService
	{

		private readonly AppDbContext _appDbContext;

		public OrderService(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}


		public async Task<PagedResult<OrderListItem>> SearchAsync(OrderFilter filter, CancellationToken ct = default)
		{
			using var connection = _appDbContext.CreateConnection();
			var rows = (await connection.QueryAsync<OrderListItem>(
				new CommandDefinition(
					"dbo.sp_GetOrdersPaged",
					new
					{
						filter.Search,
						filter.FromDate,
						filter.ToDate,
						filter.CityId,
						filter.Status,
						filter.MinAmount,
						filter.MaxAmount,
						filter.SortColumn,
						filter.SortDirection,
						filter.Page,
						filter.PageSize,

					},

					commandType: CommandType.StoredProcedure,
					cancellationToken: ct))).AsList();


			var total = rows.Count == 0 ? 0 : rows[0].TotalCount;

			return new PagedResult<OrderListItem>
			{
				Items = rows,
				TotalCount = total,
				Page =filter.Page,
				PageSize = filter.PageSize
			};


		}

		public async Task<IReadOnlyList<LookupItem>> GetCitiesAsync(CancellationToken ct = default)
		{
			const string sql = """
				SELECT CityId AS Id, Name
				FROM dbo.Cities
				ORDER BY Name;
				""";
			using var connection = _appDbContext.CreateConnection();
			var rows = await connection.QueryAsync<LookupItem>(
				new CommandDefinition(sql, cancellationToken: ct));
			return rows.AsList();


		}

		
	}
}


using SalesProjetDapper.Web.Dtos.Common;
using SalesProjetDapper.Web.Dtos.Orders;

namespace SalesProjetDapper.Web.Services.Orders
{
	public interface IOrderService
	{
		Task<PagedResult<OrderListItem>> SearchAsync(OrderFilter filter, CancellationToken ct = default);
		Task<IReadOnlyList<LookupItem>> GetCitiesAsync( CancellationToken ct = default);

	}
}

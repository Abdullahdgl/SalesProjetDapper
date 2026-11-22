using SalesProjetDapper.Web.Dtos.Common;
using SalesProjetDapper.Web.Dtos.Dashboard;

namespace SalesProjetDapper.Web.Services.Dashboard
{
	public interface IDashboardService
	{
		Task<DashboardPayload> GetAsync(DateRange range, CancellationToken ct = default);

	}
}

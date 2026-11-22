using SalesProjetDapper.Web.Dtos.Common;

namespace SalesProjetDapper.Web.Dtos.Dashboard
{
	public sealed class DashboardPayload
	{
		public DashboardKpis Kpis { get; set; } = new();
		public IReadOnlyList<MonthlySalesPoint> MonthlySales { get; set; } = [];
		public IReadOnlyList<NamedMetric> TopCategories { get; set; } = [];
		public IReadOnlyList<OrderStatusCount> StatusBreakDown { get; set; } = [];
		public IReadOnlyList<NamedMetric> TopProducts { get; set; } = [];


	}
}

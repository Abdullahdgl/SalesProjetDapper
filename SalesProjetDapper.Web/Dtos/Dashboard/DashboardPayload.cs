using SalesProjetDapper.Web.Dtos.Common;

namespace SalesProjetDapper.Web.Dtos.Dashboard
{
	public sealed class DashboardPayload
	{
		public DashboardKpis Kpis { get; set; } = new();
		public IReadOnlyList<MonthlySalesPoint> MonthlySales { get; set; } = [];
		public IReadOnlyList<NamedMetric> TopCategories { get; set; } = [];
		public IReadOnlyList<NamedMetric> TopCities { get; set; } = [];
		public IReadOnlyList<OrderStatusCount> StatusBreakdown { get; set; } = [];
		public IReadOnlyList<NamedMetric> TopProducts { get; set; } = [];


	}
}

using SalesProjetDapper.Web.Dtos.Common;

namespace SalesProjetDapper.Web.Dtos.Analytics
{
	public sealed class AnalyticsPayload
	{
		public IReadOnlyList<TopCustomerRow> TopCustomers { get; set; } = [];
		public IReadOnlyList<NamedMetric> TopCities { get; set; } = [];
		public IReadOnlyList<CategoryMonthlyRow> CategoryMonthly { get; set; } = [];
		public IReadOnlyList<NamedMetric> TopProducts { get; set; } = [];
		public RepeatCustomerStats RepeatCustomers { get; set; } = new();


	}
}

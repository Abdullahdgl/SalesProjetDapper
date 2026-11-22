namespace SalesProjetDapper.Web.Dtos.Dashboard
{
	public sealed class DashboardKpis
	{
		public long TotalOrders { get; set; }
		public decimal TotalRevenue { get; set; }
		public int TotalCustomers { get; set; }
		public decimal AverageBasket { get; set; }
	}
}

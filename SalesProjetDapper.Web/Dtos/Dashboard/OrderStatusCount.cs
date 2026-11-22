namespace SalesProjetDapper.Web.Dtos.Dashboard
{
	public sealed class OrderStatusCount
	{
		public string Status { get; set; } = string.Empty;
		public int Count { get; set; }
	}
}

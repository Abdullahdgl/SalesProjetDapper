namespace SalesProjetDapper.Web.Dtos.Orders
{
	public sealed class OrderListItem
	{
		public long OrderID { get; set; }
		public string OrderNumber { get; set; } = string.Empty;
		public string CustomerName { get; set; } = string.Empty;
		public string CityName { get; set; } = string.Empty;
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public string PaymentMethod { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;

		public int TotalCount { get; set; }
	}
}

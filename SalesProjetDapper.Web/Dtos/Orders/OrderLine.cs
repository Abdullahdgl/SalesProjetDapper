namespace SalesProjetDapper.Web.Dtos.Orders
{
	public sealed class OrderLine
	{
		public long OrderDetailId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public string CategoryName { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal LineTotal { get; set; }
	}
}

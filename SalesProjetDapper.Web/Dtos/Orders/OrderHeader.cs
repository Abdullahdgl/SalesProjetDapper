namespace SalesProjetDapper.Web.Dtos.Orders
{
	public sealed class OrderHeader
	{
		public long OrderId { get; set; }
		public string OrderNumber { get; set; } = string.Empty;
		public string CustomerName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string CityName { get; set; } = string.Empty;
		public DateTime OrderDate { get; set; }
		public string Status { get; set; } = string.Empty;
		public decimal TotalAmount { get; set; }
		public string PaymentMethod { get; set; } = string.Empty;
		public string? PaymentStatus { get; set; }
		public DateTime? PaidAt { get; set; }
	}
}

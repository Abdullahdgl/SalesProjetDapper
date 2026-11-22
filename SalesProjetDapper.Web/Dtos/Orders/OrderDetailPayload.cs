namespace SalesProjetDapper.Web.Dtos.Orders
{
	public  sealed class OrderDetailPayload
	{
		public OrderHeader? Header { get; set; }
		public IReadOnlyList<OrderLine> Lines { get; set; } = [];

	}
}

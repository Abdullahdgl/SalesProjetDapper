namespace SalesProjetDapper.Web.Dtos.Analytics
{
	public sealed class TopCustomerRow
	{
		public int CustomerId { get; set; }
		public string FullName { get; set; } = string.Empty;
		public string CityName { get; set; } = string.Empty;
		public int OrderCount{ get; set; }
		public decimal Revenue { get; set; }
	}
}

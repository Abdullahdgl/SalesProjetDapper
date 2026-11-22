namespace SalesProjetDapper.Web.Dtos.Analytics
{
	public sealed class CategoryMonthlyRow
	{
		public string CategoryName { get; set; } = string.Empty;
		public int Year { get; set; }
		public int Month { get; set; }
		public decimal Revenue { get; set; }
		public string Label => $"{Year}-{Month:00}";
	}
}

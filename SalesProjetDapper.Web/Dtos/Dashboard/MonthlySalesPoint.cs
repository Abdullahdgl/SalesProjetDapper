namespace SalesProjetDapper.Web.Dtos.Dashboard
{
	public sealed class MonthlySalesPoint
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public string Label => $"{Year}-{Month:00}";
		public int OrderCount { get; set; }
		public decimal Revenue { get; set; }
	}
}

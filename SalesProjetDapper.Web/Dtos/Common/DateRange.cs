namespace SalesProjetDapper.Web.Dtos.Common
{

	//rapor aralığı. Last12Months(): bugün ve 11 ay önceki ayın 1’i.
	public sealed record DateRange(DateTime from, DateTime To)
	{
		public static DateRange Last12Months()
		{
			var to = DateTime.Today;
			var from = to.AddMonths(-11);
			from = new DateTime(from.Year, from.Month, 1);
			return new DateRange(from, to);
		}
	}
	
}

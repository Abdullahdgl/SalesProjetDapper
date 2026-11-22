namespace SalesProjetDapper.Web.Dtos.Analytics
{
	public sealed class RepeatCustomerStats
	{
		public int DistinctCustomers { get; set; } //Dönemden aslında en az 1 sipariş verendir.
		public int RepeatCustomers { get; set; } //bu da aynı dönemden 2+ sipariş
		public decimal RepeatRate { get; set; }//Yüzdeyi yansıtır



		//Aynı uzunlukta önceki dönem
		public int PreviousDistinctCustomers { get; set; } 
		public int PreviousRepeatCustomers { get; set; }
		public decimal PreviousRepeatRate { get; set; }
	}
}

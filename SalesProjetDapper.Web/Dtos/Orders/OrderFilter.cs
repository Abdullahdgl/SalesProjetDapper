namespace SalesProjetDapper.Web.Dtos.Orders
{
	public sealed class OrderFilter
	{
		//burdaki propertyleri yazmaktaki amacım aslında bir arama formu oluşturmak tarayıcıda!!!
		//misal tarayıcıda ahmet diye arattığımızda ?search=Abdullah&page=2 ..... gibi gönderir ama mvc bunu görmeyebilir. ? ise aslında burada bir filtre yok db de 


		public string? Search { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public int? CityId { get; set; }
		public string? Status { get; set; }
		public decimal? MinAmount { get; set; }
		public decimal? MaxAmount { get; set; }
		public string SortColumn { get; set; } = "OrderDate";
		public string SortDirection { get; set; } = "DESC";
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 25;
	}
}

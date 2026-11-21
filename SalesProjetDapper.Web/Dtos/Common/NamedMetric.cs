namespace SalesProjetDapper.Web.Dtos.Common
{
	//grafik çubuğu: ad, ciro, adet(kategori / şehir / ürün ortak).
	public sealed record NamedMetric
	{
		public string Name { get; set; } = string.Empty;
		public decimal Revenue { get; set; }
		public int Quantity { get; set; }

	}
}

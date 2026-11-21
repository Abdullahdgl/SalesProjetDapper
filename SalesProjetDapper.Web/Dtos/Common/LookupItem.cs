namespace SalesProjetDapper.Web.Dtos.Common
{
	//şehir dropdown: CityId AS Id.
	public sealed record LookupItem
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}

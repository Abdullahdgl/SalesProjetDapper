using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using SalesProjetDapper.DataGenerator;
using System.Data;

var config = new ConfigurationBuilder()
	.SetBasePath(AppContext.BaseDirectory)
	.AddJsonFile("appsettings.json", optional: false)
	.Build();

var connectionString = config["ConnectionString"] ?? throw new InvalidOperationException("ConnectionString missing");
var customerCount = int.Parse(config["Customers"] ?? "100000");
var productCount = int.Parse(config["Products"] ?? "5000");
var orderCount = int.Parse(config["Orders"] ?? "2000000");
var batchSize = int.Parse(config["BatchSize"] ?? "50000");

Console.WriteLine("SalesPulse veri üretimi başladı.");
Console.WriteLine($"Hedef: {customerCount:N0} müşteri, {productCount:N0} ürün, {orderCount:N0} sipariş");

await using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

var cityCount = Catalog.Cities.Length;
var cityWeights = Catalog.Cities.Select(c => c.Weight).ToArray();
var rng = new Random(2026);

await TruncateAsync(connection);
await BulkCitiesAsync(connection);
await BulkCategoriesAsync(connection);
await BulkProductsAsync(connection, productCount, rng);
await BulkCustomersAsync(connection, customerCount, cityWeights, rng);
await BulkOrdersAsync(connection, orderCount, customerCount, productCount, cityWeights, batchSize, rng);

Console.WriteLine("Tamamlandı. Ardından sql/02_indexes.sql ve sql/03_stored_procedures.sql çalıştırın.");

static async Task TruncateAsync(SqlConnection connection)
{
	Console.WriteLine("Mevcut işlem tabloları temizleniyor...");
	await using var cmd = connection.CreateCommand();
	cmd.CommandTimeout = 0;
	cmd.CommandText = """
		DELETE FROM dbo.Payments;
		DELETE FROM dbo.OrderDetails;
		DELETE FROM dbo.Orders;
		DELETE FROM dbo.Customers;
		DELETE FROM dbo.Products;
		DELETE FROM dbo.Categories;
		DELETE FROM dbo.Cities;
		""";
	await cmd.ExecuteNonQueryAsync();
}

static async Task BulkCitiesAsync(SqlConnection connection)
{
	var table = new DataTable();
	table.Columns.Add("CityId", typeof(int));
	table.Columns.Add("Name", typeof(string));
	table.Columns.Add("Region", typeof(string));
	for (var i = 0; i < Catalog.Cities.Length; i++)
		table.Rows.Add(i + 1, Catalog.Cities[i].Name, Catalog.Cities[i].Region);

	await BulkAsync(connection, "dbo.Cities", table, keepIdentity: true);
	Console.WriteLine($"Cities: {table.Rows.Count}");
}

static async Task BulkCategoriesAsync(SqlConnection connection)
{
	var table = new DataTable();
	table.Columns.Add("CategoryId", typeof(int));
	table.Columns.Add("Name", typeof(string));
	for (var i = 0; i < Catalog.Categories.Length; i++)
		table.Rows.Add(i + 1, Catalog.Categories[i]);

	await BulkAsync(connection, "dbo.Categories", table, keepIdentity: true);
	Console.WriteLine($"Categories: {table.Rows.Count}");
}

static async Task BulkProductsAsync(SqlConnection connection, int productCount, Random rng)
{
	var table = new DataTable();
	table.Columns.Add("ProductId", typeof(int));
	table.Columns.Add("CategoryId", typeof(int));
	table.Columns.Add("Name", typeof(string));
	table.Columns.Add("UnitPrice", typeof(decimal));
	table.Columns.Add("IsActive", typeof(bool));
	for (var i = 1; i <= productCount; i++)
	{
		var categoryId = ((i - 1) % Catalog.Categories.Length) + 1;
		var price = Math.Round((decimal)(rng.NextDouble() * 4800 + 19.90), 2);
		table.Rows.Add(i, categoryId, $"{Catalog.Categories[categoryId - 1]} #{i:0000}", price, rng.NextDouble() > 0.04);
	}

	await BulkAsync(connection, "dbo.Products", table, keepIdentity: true);
	Console.WriteLine($"Products: {productCount:N0}");
}

static async Task BulkCustomersAsync(SqlConnection connection, int customerCount, int[] cityWeights, Random rng)
{
	var table = new DataTable();
	table.Columns.Add("CustomerId", typeof(int));
	table.Columns.Add("FullName", typeof(string));
	table.Columns.Add("Email", typeof(string));
	table.Columns.Add("CityId", typeof(int));
	table.Columns.Add("RegisteredAt", typeof(DateTime));
	const int batch = 20000;
	var written = 0;
	for (var i = 1; i <= customerCount; i++)
	{
		var first = Catalog.FirstNames[rng.Next(Catalog.FirstNames.Length)];
		var last = Catalog.LastNames[rng.Next(Catalog.LastNames.Length)];
		table.Rows.Add(
			i,
			$"{first} {last}",
			$"user{i}@mail.salespulse.local",
			WeightedIndex(cityWeights, rng) + 1,
			new DateTime(2021, 1, 1).AddDays(rng.Next(0, 1700)));

		if (table.Rows.Count >= batch || i == customerCount)
		{
			await BulkAsync(connection, "dbo.Customers", table, keepIdentity: true);
			written += table.Rows.Count;
			table.Clear();
			Console.WriteLine($"Customers: {written:N0}");
		}
	}
}

static async Task BulkOrdersAsync(
	SqlConnection connection,
	int orderCount,
	int customerCount,
	int productCount,
	int[] cityWeights,
	int batchSize,
	Random rng)
{
	var orders = new DataTable();
	orders.Columns.Add("OrderId", typeof(long));
	orders.Columns.Add("OrderNumber", typeof(string));
	orders.Columns.Add("CustomerId", typeof(int));
	orders.Columns.Add("CityId", typeof(int));
	orders.Columns.Add("OrderDate", typeof(DateTime));
	orders.Columns.Add("Status", typeof(string));
	orders.Columns.Add("TotalAmount", typeof(decimal));
	orders.Columns.Add("PaymentMethod", typeof(string));

	var details = new DataTable();
	details.Columns.Add("OrderId", typeof(long));
	details.Columns.Add("ProductId", typeof(int));
	details.Columns.Add("Quantity", typeof(int));
	details.Columns.Add("UnitPrice", typeof(decimal));
	details.Columns.Add("LineTotal", typeof(decimal));

	var payments = new DataTable();
	payments.Columns.Add("OrderId", typeof(long));
	payments.Columns.Add("Method", typeof(string));
	payments.Columns.Add("Amount", typeof(decimal));
	var paidAt = payments.Columns.Add("PaidAt", typeof(DateTime));
	paidAt.AllowDBNull = true;
	payments.Columns.Add("Status", typeof(string));

	var start = new DateTime(2023, 1, 1);
	var days = (DateTime.Today - start).Days;
	long detailIdHint = 0;

	for (var i = 1; i <= orderCount; i++)
	{
		var customerId = 1 + (int)(Math.Pow(rng.NextDouble(), 2.15) * (customerCount - 1));
		var cityId = WeightedIndex(cityWeights, rng) + 1;
		var day = WeightedDay(start, days, rng);
		var status = Catalog.Statuses[rng.Next(Catalog.Statuses.Length)];
		var payment = Catalog.Payments[rng.Next(Catalog.Payments.Length)];
		var lineCount = rng.NextDouble() < 0.55 ? 1 : rng.Next(2, 5);
		decimal total = 0;
		for (var l = 0; l < lineCount; l++)
		{
			var productId = 1 + (int)(Math.Pow(rng.NextDouble(), 1.8) * (productCount - 1));
			var qty = rng.Next(1, 4);
			var price = Math.Round((decimal)(rng.NextDouble() * 1800 + 29), 2);
			var line = price * qty;
			total += line;
			details.Rows.Add(i, productId, qty, price, line);
			detailIdHint++;
		}

		orders.Rows.Add(i, $"SP{i:0000000000}", customerId, cityId, day, status, total, payment);
		payments.Rows.Add(
			i,
			payment,
			total,
			status is "Cancelled" or "Pending" ? DBNull.Value : day.AddHours(rng.Next(1, 48)),
			status is "Cancelled" ? "Failed" : status == "Pending" ? "Pending" : "Paid");

		if (orders.Rows.Count >= batchSize || i == orderCount)
		{
			await BulkAsync(connection, "dbo.Orders", orders, keepIdentity: true);
			await BulkAsync(connection, "dbo.OrderDetails", details, keepIdentity: false);
			await BulkAsync(connection, "dbo.Payments", payments, keepIdentity: false);
			Console.WriteLine($"Orders: {i:N0} | OrderDetails ~ {detailIdHint:N0}");
			orders.Clear();
			details.Clear();
			payments.Clear();
		}
	}
}

static DateTime WeightedDay(DateTime start, int days, Random rng)
{
	DateTime date;
	do
	{
		date = start.AddDays(rng.Next(days + 1)).AddHours(rng.Next(8, 23)).AddMinutes(rng.Next(0, 60));
		var accept = rng.Next(0, 170) < Catalog.MonthWeight[date.Month - 1];
		if (accept) return date;
	} while (true);
}

static int WeightedIndex(int[] weights, Random rng)
{
	var total = weights.Sum();
	var pick = rng.Next(total);
	var acc = 0;
	for (var i = 0; i < weights.Length; i++)
	{
		acc += weights[i];
		if (pick < acc) return i;
	}
	return weights.Length - 1;
}

static async Task BulkAsync(SqlConnection connection, string tableName, DataTable table, bool keepIdentity)
{
	using var bulk = new SqlBulkCopy(connection, keepIdentity ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default, null)
	{
		DestinationTableName = tableName,
		BatchSize = table.Rows.Count,
		BulkCopyTimeout = 0
	};
	foreach (DataColumn column in table.Columns)
		bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
	await bulk.WriteToServerAsync(table);
}

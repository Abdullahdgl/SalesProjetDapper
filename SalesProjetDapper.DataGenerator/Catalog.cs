using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjetDapper.DataGenerator
{
	internal class Catalog
	{
		public static readonly (string Name, string Region, int Weight)[] Cities =
			[
			("İstanbul", "Marmara", 180), ("Ankara", "İç Anadolu", 80), ("İzmir", "Ege", 70),
		("Bursa", "Marmara", 40), ("Antalya", "Akdeniz", 40), ("Konya", "İç Anadolu", 25),
		("Adana", "Akdeniz", 25), ("Gaziantep", "Güneydoğu", 22), ("Mersin", "Akdeniz", 20),
		("Kayseri", "İç Anadolu", 18), ("Kocaeli", "Marmara", 18), ("Diyarbakır", "Güneydoğu", 16),
		("Hatay", "Akdeniz", 14), ("Manisa", "Ege", 14), ("Samsun", "Karadeniz", 14),
		("Kahramanmaraş", "Akdeniz", 12), ("Balıkesir", "Marmara", 12), ("Van", "Doğu Anadolu", 10),
		("Denizli", "Ege", 12), ("Malatya", "Doğu Anadolu", 10), ("Elazığ", "Doğu Anadolu", 8),
		("Erzurum", "Doğu Anadolu", 8), ("Trabzon", "Karadeniz", 10), ("Aydın", "Ege", 10),
		("Tekirdağ", "Marmara", 10), ("Sakarya", "Marmara", 10), ("Muğla", "Ege", 12),
		("Eskişehir", "İç Anadolu", 12), ("Mardin", "Güneydoğu", 8), ("Şanlıurfa", "Güneydoğu", 12),
		("Ordu", "Karadeniz", 7), ("Afyonkarahisar", "Ege", 7), ("Sivas", "İç Anadolu", 7),
		("Tokat", "Karadeniz", 6), ("Zonguldak", "Karadeniz", 6), ("Çorum", "Karadeniz", 6),
		("Adıyaman", "Güneydoğu", 6), ("Osmaniye", "Akdeniz", 6), ("Isparta", "Akdeniz", 6),
		("Çanakkale", "Marmara", 7), ("Edirne", "Marmara", 6), ("Kütahya", "Ege", 6),
		("Ağrı", "Doğu Anadolu", 4), ("Batman", "Güneydoğu", 6), ("Rize", "Karadeniz", 5),
		("Giresun", "Karadeniz", 5), ("Amasya", "Karadeniz", 4), ("Kastamonu", "Karadeniz", 4),
		("Uşak", "Ege", 5), ("Nevşehir", "İç Anadolu", 4), ("Aksaray", "İç Anadolu", 5),
		("Niğde", "İç Anadolu", 4), ("Yozgat", "İç Anadolu", 4), ("Kırıkkale", "İç Anadolu", 4),
		("Düzce", "Karadeniz", 5), ("Bolu", "Karadeniz", 4), ("Karabük", "Karadeniz", 3),
		("Bartın", "Karadeniz", 3), ("Sinop", "Karadeniz", 3), ("Artvin", "Karadeniz", 3),
		("Gümüşhane", "Karadeniz", 2), ("Bayburt", "Karadeniz", 2), ("Erzincan", "Doğu Anadolu", 3),
		("Kars", "Doğu Anadolu", 3), ("Iğdır", "Doğu Anadolu", 2), ("Ardahan", "Doğu Anadolu", 2),
		("Bitlis", "Doğu Anadolu", 3), ("Muş", "Doğu Anadolu", 3), ("Bingöl", "Doğu Anadolu", 3),
		("Tunceli", "Doğu Anadolu", 2), ("Hakkari", "Doğu Anadolu", 2), ("Siirt", "Güneydoğu", 3),
		("Şırnak", "Güneydoğu", 3), ("Kilis", "Güneydoğu", 2), ("Çankırı", "İç Anadolu", 3),
		("Burdur", "Akdeniz", 3), ("Karaman", "İç Anadolu", 3), ("Kırşehir", "İç Anadolu", 3),
		("Kırklareli", "Marmara", 4), ("Yalova", "Marmara", 4), ("Bilecik", "Marmara", 3)
			];


		public static readonly string[] Categories =
			[
			"Elektronik", "Cep Telefonu", "Bilgisayar", "TV & Ses", "Beyaz Eşya",
		"Mutfak", "Mobilya", "Ev Tekstili", "Aydınlatma", "Yapı Market",
		"Giyim Kadın", "Giyim Erkek", "Ayakkabı", "Çanta", "Saat & Aksesuar",
		"Kozmetik", "Kişisel Bakım", "Bebek", "Oyuncak", "Kitap",
		"Kırtasiye", "Spor", "Outdoor", "Otomotiv", "Pet Shop",
		"Market", "Atıştırmalık", "İçecek", "Sağlık", "Hobi"
			];

		public static readonly string[] FirstNames =
			[
					"Ahmet", "Mehmet", "Ayşe", "Fatma", "Elif", "Mustafa", "Emre", "Zeynep",
		"Can", "Deniz", "Burak", "Selin", "Hakan", "Merve", "Cem", "Ece",
		"Yusuf", "Seda", "Oğuz", "Derya", "Kaan", "Pınar", "Onur", "Gül"
			];

		public static readonly string[] LastNames =
			[
				"Yılmaz", "Kaya", "Demir", "Şahin", "Çelik", "Yıldız", "Yıldırım", "Öztürk",
		"Aydın", "Özdemir", "Arslan", "Doğan", "Kilic", "Aslan", "Koç", "Kurt",
		"Özkan", "Şimşek", "Polat", "Aksoy", "Erdoğan", "Güneş", "Bulut", "Acar"
			];

		public static readonly int[] MonthWeight = [0, 65, 80, 85, 95, 100, 105, 90, 95, 110, 140, 160];


		public static readonly string[] Statuses =
			[
			"Completed", "Completed", "Completed", "Completed", "Completed",
		"Completed", "Completed", "Shipped", "Pending", "Cancelled", "Returned"


			];

		public static readonly string[] Payments =
			[
			"CreditCard", "CreditCard", "CreditCard", "DebitCard", "BankTransfer", "Wallet", "CashOnDelivery"

			];




	}
}

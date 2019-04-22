using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
	public class DataBase
	{
		public static List<Card> CardList { get; set; } =
			new List<Card>()
			{
				new Card ()
				{
					CardNumber = "5376439009021994",
					CardName = "MyCard1",
					Currency = Infrastructure.Currency.RUR,
					Money = 10_000,
					Validity = new DateTime(2021, 3, 1)
				},
				new Card ()
				{
					CardNumber = "4083969009020320",
					CardName = "MyCard2",
					Currency = Infrastructure.Currency.EUR,
					Money = 50_000,
					Validity = new DateTime(2022, 5, 1)
				}
			};

		public static List<User> UserList { get; set; } =
			new List<User>()
			{
				new User("Maxim", CardList[0]),
				new User("Denis", CardList[1])

			};

		public static List<Transaction> TransactionaList { get; set; } =
			new List<Transaction>()
			{
				new Transaction()
				{
					CardNumberFrom = "5376439009021994",
					CardNumberTo = "4083969009020320",
					Money = 3_000,
					TransactionTime = new DateTime(2019, 4, 15, 10, 15, 30)
				},
				new Transaction()
				{
					CardNumberFrom = "4083969009020320",
					CardNumberTo = "5376439009021994",
					Money = 1_500,
					TransactionTime = new DateTime(2019, 4, 20, 18, 30, 25)
				}
			};
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Data.Interface;
using Server.Models;
using Server.Services;

namespace Server.Data.Implementation
{
	public class CardRepository : ICardRepository
	{
		public Card GetCard(string cardNumber)
		{
			if(cardNumber == null)
			{
				return null;
			}

			return DataBase.CardList.Where(x => x.CardNumber == cardNumber).FirstOrDefault();
		}

		public IEnumerable<Card> GetCards() =>
			DataBase.CardList;
	}
}

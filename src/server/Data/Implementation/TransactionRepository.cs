using Server.Data.Interface;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data.Implementation
{
	public class TransactionRepository : ITransactionRepository
	{
		public IEnumerable<object> GetTransactions(string cardNumber, DateTime from, DateTime to) =>
			DataBase.TransactionaList?.Where(x => x.TransactionTime >= from && x.TransactionTime <= to);
	}
}

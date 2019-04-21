using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Infrastructure;

namespace Server.Services
{
    public class BankService : IBankService
    {
        public bool TryOpenNewCard(CardType cardType)
        {
            throw new NotImplementedException();
        }

        public bool TryTransferMoney(decimal sum, string from, string to)
        {
            throw new NotImplementedException();
        }
    }
}

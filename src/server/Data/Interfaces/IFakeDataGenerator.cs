using System.Collections.Generic;
using Server.Models;

namespace Server.Data.Interfaces
{
    public interface IFakeDataGenerator
    {
        IEnumerable<Card> GenerateFakeCards();

        User GenerateFakeUser();
    }
}
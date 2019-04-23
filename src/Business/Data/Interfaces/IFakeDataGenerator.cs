using System.Collections.Generic;
using Models;

namespace Business.Data.Interfaces
{
    public interface IFakeDataGenerator
    {
        IEnumerable<Card> GenerateFakeCards();

        User GenerateFakeUser();
    }
}
using System.Collections.Generic;
using AlfaBank.Core.Models;

namespace AlfaBank.Services.Interfaces
{
    public interface IFakeDataGenerator
    {
        IEnumerable<Card> GenerateFakeCards();

        User GenerateFakeUser();
    }
}
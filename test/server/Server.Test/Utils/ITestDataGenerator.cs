using System.Collections.Generic;
using AlfaBank.Core.Models;

namespace Server.Test.Utils
{
    public interface ITestDataGenerator
    {
        IEnumerable<Card> GenerateFakeCards();

        User GenerateFakeUser();
    }
}
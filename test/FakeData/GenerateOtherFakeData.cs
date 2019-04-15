using System;
using Xunit;
using Bogus;
using System.Text;

namespace test.FakeData
{
    public class GenerateOtherFakeData
    {
        public string GetNumberCard()
		{
			return GenerateNumberCard();
		}

		public string GetNumberCard(char firstNumber)
		{
			var numberCard = GenerateNumberCard();
			return firstNumber + numberCard.Substring(1);
		}

		public string GetNumberCard(string firstNumbers)
		{
			var numberCard = GenerateNumberCard();
			return (firstNumbers.Length > 15) ? numberCard : firstNumbers + numberCard.Substring(firstNumbers.Length);	
		}

		private string GenerateNumberCard()
		{
			var random = new Random();
			var fakeNumber = new StringBuilder();
			for(int i = 0; i < 16; i++)
			{
				fakeNumber.Append(random.Next(0, 9));
			}
			return fakeNumber.ToString();
		}
    }
}

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

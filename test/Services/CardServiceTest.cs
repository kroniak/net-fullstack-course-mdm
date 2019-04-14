using Server.Infrastructure;
using Server.Services;
using System;
using test.FakeData;
using Xunit;

namespace test.Services
{
    public class CardServiceTest
	{
		private readonly GenerateOtherFakeData _generateOtherFakeData;

		public CardServiceTest()
		{
			_generateOtherFakeData = new GenerateOtherFakeData();
		}

		[Fact]
		public void GetCardType_RandomNumber_CardType()
		{
			var fakeNumber = _generateOtherFakeData.GetNumberCard();
			var service = new CardService();

			var result = service.GetCardType(fakeNumber);

			Assert.IsType<CardType>(result);
		}
	}
}

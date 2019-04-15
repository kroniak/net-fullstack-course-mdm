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
		public void GetCardType_RandomCardNumber_CardTypeMIR()
		{
			var fakeNumber = _generateOtherFakeData.GetNumberCard('2');
			var service = new CardService();

			var result = service.GetCardType(fakeNumber);

			Assert.IsType<CardType>(result);
			Assert.Equal(CardType.MIR, result);
		}

		[Fact]
		public void GetCardType_RandomCardNumber_CardTypeVISA()
		{
			var fakeNumber = _generateOtherFakeData.GetNumberCard('4');
			var service = new CardService();

			var result = service.GetCardType(fakeNumber);

			Assert.IsType<CardType>(result);
			Assert.Equal(CardType.VISA, result);
		}
	}
}

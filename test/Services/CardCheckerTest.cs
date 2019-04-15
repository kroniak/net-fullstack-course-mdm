using Server.Services.Checkers;
using System;
using test.FakeData;
using Xunit;

namespace test.Services
{
    public class CardCheckerTest
	{
		private readonly GenerateOtherFakeData _generateOtherFakeData;

		public CardCheckerTest()
		{
			_generateOtherFakeData = new GenerateOtherFakeData();
		}

		[Fact]
        public void CheckCardEmitter_cardAlfaBankNumber_True()
        {
			var fakeNumber = _generateOtherFakeData.GetNumberCard("1234");
			var service = new CardChecker();

			var result = service.CheckCardEmitter(fakeNumber);

			Assert.IsType<bool>(result);
			Assert.Equal(true, result);
        }

		[Fact]
		public void CheckCardNumber_cardNoAlfaBankNumber_False()
		{
			var fakeNumber = _generateOtherFakeData.GetNumberCard("4276");
			var service = new CardChecker();

			var result = service.CheckCardNumber(fakeNumber);

			Assert.IsType<bool>(result);
			Assert.Equal(false, result);
		}

		[Fact]
		public void CheckCardNumber_cardNumberExists_True()
		{
			var fakeNumber = "4561261212345467";
			var service = new CardChecker();

			var result = service.CheckCardNumber(fakeNumber);

			Assert.IsType<bool>(result);
			Assert.Equal(true, result);
		}

		[Fact]
		public void CheckCardNumber_cardNumberNoExists_False()
		{
			var fakeNumber = "4561261212345464";
			var service = new CardChecker();

			var result = service.CheckCardNumber(fakeNumber);

			Assert.IsType<bool>(result);
			Assert.Equal(false, result);
		}
	}
}

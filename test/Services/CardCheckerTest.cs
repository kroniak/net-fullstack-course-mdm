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
        public void CheckCardEmitter_RandomNumber_Bool()
        {
			var fakeNumber = _generateOtherFakeData.GetNumberCard();
			var service = new CardChecker();

			var result = service.CheckCardEmitter(fakeNumber);

			Assert.IsType<bool>(result);
        }

		[Fact]
		public void CheckCardNumber_RandomNumber_Bool()
		{
			var fakeNumber = _generateOtherFakeData.GetNumberCard();
			var service = new CardChecker();

			var result = service.CheckCardNumber(fakeNumber);

			Assert.IsType<bool>(result);
		}
	}
}

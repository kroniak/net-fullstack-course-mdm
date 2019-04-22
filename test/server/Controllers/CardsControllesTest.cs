using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Server.Data;
using Server.Data.Interface;
using Server.Models;
using Server.Models.DTO;
using Server.Services;
using Server.Services.Checkers;
using Xunit;

namespace ServerTest.Controllers
{
    public class CardsControllerTest
    {
        private readonly ICardChecker _cardChecker = new CardChecker();
        private readonly Mock<ICardRepository> _cardRepository;
        private readonly Mock<ICardService> _cardService;
        private readonly CardsController _controller;

        public CardsControllerTest()
        {
            _cardRepository = new Mock<ICardRepository>();
			_cardService = new Mock<ICardService>();
            _controller = new CardsController(_cardChecker, _cardService.Object, _cardRepository.Object);
        }

        [Fact]
        public void GetCards_()
        {
            // Arrange
            var fakeCards = new List<Card>
			{
				new Card
				{
					CardName = "test card",
					CardNumber = "4790878827491205"
				}
			};

            _cardRepository.Setup(r => r.GetCards()).Returns(fakeCards);
            // Act
            var result = _controller.Get();
            // Assert
            _cardRepository.Verify(r => r.GetCards(), Times.AtMostOnce());
            Assert.IsType<ActionResult<IEnumerable<CardDTO>>>(result);
            var resultList = Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Server.Data;
using Server.Models;
using Server.Services.Checkers;
using Xunit;

namespace ServerTest.Controllers
{
    public class CardsControllerTest
    {
        private readonly ICardChecker _cardChecker = new CardChecker();
        private readonly Mock<ICardRepository> _cardRepository;
        private readonly CardsController _controller;

        public CardsControllerTest()
        {
            _cardRepository = new Mock<ICardRepository>();
            _controller = new CardsController(_cardChecker);
        }

        [Fact]
        public void GetCards_()
        {
            // Arrange
            var fakeCards = Enumerable.Repeat(new Card
            {
                CardName = "test card",
                    CardNumber = "4790878827491205"
            }, 5);

            _cardRepository.Setup(r => r.GetCards()).Returns(fakeCards);
            // Act
            var result = _controller.Get();
            // Assert
            _cardRepository.Verify(r => r.GetCards(), Times.AtMostOnce());
            Assert.IsType<ActionResult<IEnumerable<Card>>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(3, result.Value.Count());
        }
    }
}
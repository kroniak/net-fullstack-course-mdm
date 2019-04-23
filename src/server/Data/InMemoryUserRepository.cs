using System;
using Server.Data.Interfaces;
using Server.Exceptions;
using Server.Infrastructure;
using Server.Models;

namespace Server.Data
{
    /// <inheritdoc />
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly User _currentUser;
        private readonly IFakeDataGenerator _fakeDataGenerator;
        private readonly ICardRepository _cardRepository;

        /// <inheritdoc />
        public InMemoryUserRepository(IFakeDataGenerator fakeDataGenerator, ICardRepository cardRepository)
        {
            _fakeDataGenerator = fakeDataGenerator ?? throw new ArgumentNullException(nameof(fakeDataGenerator));
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));

            _currentUser = fakeDataGenerator.GenerateFakeUser();

            ProducedFakeData();
        }

        private void ProducedFakeData()
        {
            var cards = _cardRepository.GetCards(_currentUser);
            var fakeCards = _fakeDataGenerator.GenerateFakeCards();
            cards.AddRange(fakeCards);
        }

        /// <inheritdoc />
        public User GetCurrentUser() => _currentUser ??
                                        throw new CriticalException("User is null", TypeCriticalException.USER);
    }
}
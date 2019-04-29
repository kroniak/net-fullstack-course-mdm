using Moq;
// ReSharper disable UnusedMemberInSuper.Global

namespace Server.Test.Mocks
{
    public interface IMockFactory<T>
        where T : class
    {
        Mock<T> Mock();

        T MockObject();
    }
}
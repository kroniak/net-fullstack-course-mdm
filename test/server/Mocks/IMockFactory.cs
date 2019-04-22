using System.Diagnostics.CodeAnalysis;
using Moq;

namespace ServerTest.Mocks
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IMockFactory<T> where T : class
    {
        Mock<T> Mock();

        T MockObject();
    }
}
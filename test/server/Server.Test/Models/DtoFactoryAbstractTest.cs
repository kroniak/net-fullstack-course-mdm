using AlfaBank.Core.Models;
using AutoMapper;

namespace Server.Test.Models
{
    public class DtoFactoryAbstractTest
    {
        protected static IMapper GetMapper()
        {
            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new DomainToDtoProfile()); });
            return mockMapper.CreateMapper();
        }
    }
}
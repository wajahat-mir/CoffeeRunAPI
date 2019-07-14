using AutoMapper;
using CoffeeRunAPI.Profiles;

namespace CoffeRunAPITests.Services
{
    public static class MapperService
    {
        public static Mapper DefaultMapper()
        {
            var mapProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mapProfile));
            var mapper = new Mapper(configuration);

            return mapper;
        }
    }
}

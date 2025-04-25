using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Models;

namespace Finance_Manager_Tests.ServicesTests;

public static class AutoMapperFotTests
{
    public static IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        });       

        return config.CreateMapper();
    }
}

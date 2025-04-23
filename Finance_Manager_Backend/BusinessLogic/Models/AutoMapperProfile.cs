using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;

namespace Finance_Manager_Backend.BusinessLogic.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDTO>().ReverseMap();

        CreateMap<TransactionDTO, Transaction>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.InnerCategory, opt => opt.Ignore())
            .ForMember(dest => dest.InnerCategoryId, opt =>
                opt.MapFrom(src => src.InnerCategoryId == 0 ? null : src.InnerCategoryId));
        CreateMap<Transaction, TransactionDTO>();

        CreateMap<SavingDTO, Saving>()
            .ForMember(dest => dest.User, opt => opt.Ignore());
        CreateMap<Saving, SavingDTO>();

        CreateMap<CategoryDTO, Category>()
            .ForMember(dest => dest.ParentCategory, opt => opt.Ignore());
        CreateMap<Category, CategoryDTO>();
    }
}

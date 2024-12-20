using AutoMapper;
using SachNoiOnline.Domain.Entities;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.Responses;

namespace SachNoiOnline.Domain.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Map between Category and CategoryRequest (bi-directional if needed)
            CreateMap<Category, CategoryRequest>();
            CreateMap<CategoryRequest, Category>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())  // Ignore CreatedAt
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())  // Ignore UpdatedAt
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore()); // Ignore DeletedAt

            // Map between Category and CategoryResponse
            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.TotalStories, opt => opt.MapFrom(src => src.Stories != null ? src.Stories.Count : 0));
        }
    }
}

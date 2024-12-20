using AutoMapper;
using SachNoiOnline.Domain.Entities;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.Responses;

namespace SachNoiOnline.Application.Mappings
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            // Mapping from Rating entity to RatingResponse
            CreateMap<Rating, RatingResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Account.Username))
                .ForMember(dest => dest.StoryTitle, opt => opt.MapFrom(src => src.Story.Title));

            // Mapping from RatingRequest to Rating entity
            CreateMap<RatingRequest, Rating>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore()); // Optional, depending on your business logic
        }
    }
}

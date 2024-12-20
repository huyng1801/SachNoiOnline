using AutoMapper;
using SachNoiOnline.Domain.Entities;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.Responses;

namespace SachNoiOnline.Domain.Mappers
{
    public class StoryProfile : Profile
    {
        public StoryProfile()
        {
            // Map from Story to StoryResponse
            CreateMap<Story, StoryResponse>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName))  // Map Author name
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))  // Map Category name
                .ForMember(dest => dest.NarratorName, opt => opt.MapFrom(src => src.Narrator.NarratorName))  // Map Narrator name
                .ForMember(dest => dest.TotalAudios, opt => opt.MapFrom(src => src.Audios.Count))  // Map total audios count
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(r => r.RatingValue) : 0))  // Map average rating
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))  // Map CreatedAt
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))  // Map UpdatedAt
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))  // Map DeletedAt
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.CoverImageUrl) ? null : "https://localhost:7246" + src.CoverImageUrl)); // Full URL for CoverImageUrl

            // Map from StoryRequest to Story (for creating or updating a story)
            CreateMap<StoryRequest, Story>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())  // Ignore CreatedAt (set by DB)
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())  // Ignore UpdatedAt (set by DB)
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())  // Ignore DeletedAt (for creation)
                .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore())  // Ignore CoverImageUrl (set later after file upload)
                .ForMember(dest => dest.Audios, opt => opt.Ignore()) // Ignore Audios during creation
                .ForMember(dest => dest.Ratings, opt => opt.Ignore()) // Ignore Ratings during creation
                .ForMember(dest => dest.Author, opt => opt.Ignore()) // We will set the AuthorId explicitly later
                .ForMember(dest => dest.Category, opt => opt.Ignore()) // We will set the CategoryId explicitly later
                .ForMember(dest => dest.Narrator, opt => opt.Ignore()); // We will set the NarratorId explicitly later
        }
    }
}

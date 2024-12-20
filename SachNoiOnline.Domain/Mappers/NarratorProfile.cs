using AutoMapper;
using SachNoiOnline.Domain.Entities;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.Responses;

namespace SachNoiOnline.Domain.Mappers
{
    public class NarratorProfile : Profile
    {
        public NarratorProfile()
        {
            // Map from Narrator to NarratorResponse
            CreateMap<Narrator, NarratorResponse>()
                .ForMember(dest => dest.TotalStories, opt => opt.MapFrom(src => src.Stories.Count)) // Map total number of stories
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))  // Include CreatedAt
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))  // Include UpdatedAt
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt)); // Include DeletedAt

            // Map from NarratorRequest to Narrator (for creating or updating a narrator)
            CreateMap<NarratorRequest, Narrator>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())  // Ignore CreatedAt (set by DB)
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())  // Ignore UpdatedAt (set by DB)
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore()) // Ignore DeletedAt (for creation)
                .ForMember(dest => dest.Stories, opt => opt.Ignore()); // Ignore Stories (they are not part of the request)
        }
    }
}

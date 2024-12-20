using AutoMapper;
using SachNoiOnline.Domain.Entities;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.Responses;

namespace SachNoiOnline.Domain.Mappers
{
    public class AudioProfile : Profile
    {
        public AudioProfile()
        {
            // Map from Audio to AudioResponse
            CreateMap<Audio, AudioResponse>()
                .ForMember(dest => dest.storyTitle, opt => opt.MapFrom(src => src.Story.Title))
                .ForMember(dest => dest.AudioFileUrl, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.AudioFileUrl) ? null : "https://localhost:7246" + src.AudioFileUrl)); // Map AudioFileUrl with base URL

            // Map from AudioRequest to Audio (for creation or update)
            CreateMap<AudioRequest, Audio>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id since it will be set in the database
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Ignore CreatedAt for create requests
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Ignore UpdatedAt for create requests
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore()); // Ignore DeletedAt for create requests
        }
    }
}

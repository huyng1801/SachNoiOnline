using AutoMapper;
using SachNoiOnline.Domain.Entities;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.Responses;

namespace SachNoiOnline.Domain.Mappers
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            // Map from Account to AccountResponse (for returning account info to the client)
            CreateMap<Account, AccountResponse>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))  // Include CreatedAt in the response
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))  // Include UpdatedAt in the response
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt));  // Include DeletedAt in the response

            CreateMap<AccountRequest, Account>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => HashPassword(src.Password))); // Map and hash Password separately

            CreateMap<AccountResponse, Account>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())  
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) 
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); 
        }

        private string HashPassword(string password)
        {
           
            return password; 
        }
    }
}

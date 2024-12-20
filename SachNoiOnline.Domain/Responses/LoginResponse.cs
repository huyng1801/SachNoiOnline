using SachNoiOnline.Domain.Entities;

namespace SachNoiOnline.Domain.Responses
{
    public class LoginResponse
    {
        public AccountResponse AccountResponse { get; set; }
        public string Token { get; set; }
    }
}

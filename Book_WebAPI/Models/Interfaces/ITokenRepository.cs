using Microsoft.AspNetCore.Identity;

namespace Book_WebAPI.Models.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}

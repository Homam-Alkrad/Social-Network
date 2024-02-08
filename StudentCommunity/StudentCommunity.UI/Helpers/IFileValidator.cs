using Microsoft.AspNetCore.Http;

namespace StudentCommunity.UI.Helpers
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile file);
    }
}

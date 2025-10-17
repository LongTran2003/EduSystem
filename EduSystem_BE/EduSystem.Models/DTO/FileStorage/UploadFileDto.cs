using Microsoft.AspNetCore.Http;

namespace EduSystem.Models.DTO.FileStorage
{
    public class UploadFileDto
    {
        public IFormFile? File { get; set; }
    }
}

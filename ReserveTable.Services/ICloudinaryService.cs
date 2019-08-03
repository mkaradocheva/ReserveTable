using System;
namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> UploadPicture(IFormFile pictureFile, string fileName);
    }
}

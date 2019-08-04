namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> UploadCityPicture(IFormFile pictureFile, string fileName);

        Task<string> UploadRestaurantPicture(IFormFile pictureFile, string fileName);
    }
}

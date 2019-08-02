namespace ReserveTable.Models.Cities
{
    using Microsoft.AspNetCore.Http;

    public class CreateCityViewModel
    {
        public string Name { get; set; }

        public IFormFile Photo { get; set; }
    }
}

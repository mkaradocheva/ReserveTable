namespace ReserveTable.Models.Cities
{
    using Microsoft.AspNetCore.Http;

    public class CreateCityBindingModel
    {
        public string Name { get; set; }

        public IFormFile Photo { get; set; }
    }
}

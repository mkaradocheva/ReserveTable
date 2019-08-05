namespace ReserveTable.Tests.Common
{
    using System.Reflection;
    using Domain;
    using Mapping;
    using Services.Models;
    public static class AutoMapperFactory
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ReviewServiceModel).GetTypeInfo().Assembly,
                typeof(Review).GetTypeInfo().Assembly);
        }
    }
}

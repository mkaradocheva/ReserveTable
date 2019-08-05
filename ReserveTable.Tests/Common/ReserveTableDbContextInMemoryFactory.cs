namespace ReserveTable.Tests.Common
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Data;

    public static class ReserveTableDbContextInMemoryFactory
    {
        public static ReserveTableDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<ReserveTableDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ReserveTableDbContext(options);
        }
    }
}

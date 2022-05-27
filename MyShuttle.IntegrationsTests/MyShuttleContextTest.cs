using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyShuttle.Data;

namespace MyShuttle.IntegrationTests
{
    public class MyShuttleContextTests
    {
        [Fact]
        public async Task Database_CreatedSuccessfully()
        {
            var optionsBuilder = new DbContextOptionsBuilder()
                    .UseInMemoryDatabase("MyShuttleTestDb");
            using var context = new MyShuttleContext(optionsBuilder.Options);

            var databaseCreated = await context.Database.EnsureCreatedAsync();
            Assert.True(databaseCreated);
        }

        [Fact]
        public async Task Database_DeletedSuccessfully()
        {
            var optionsBuilder = new DbContextOptionsBuilder()
                    .UseInMemoryDatabase("MyShuttleTestDb");
            using var context = new MyShuttleContext(optionsBuilder.Options);

            _ = await context.Database.EnsureCreatedAsync();

            var databaseDeleted = await context.Database.EnsureDeletedAsync();
            Assert.True(databaseDeleted);
        }
    }
}

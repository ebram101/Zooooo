using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoo.Controllers;
using Zoo.Data;
using Zoo.Models;
using Zoo.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TestProject1
{
    public class EnclosuresControllerTests
    {
        private ZooContext GetDbContext(List<Enclosure> data)
        {
            var options = new DbContextOptionsBuilder<ZooContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure a unique database for each test
                .Options;
            var context = new ZooContext(options);
            context.Enclosure.AddRange(data);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndAddsEnclosure_WhenModelStateIsValid()
        {
            // Arrange
            var context = GetDbContext(new List<Enclosure>());
            var controller = new EnclosuresController(context);

            var enclosure = new Enclosure
            {
                Id = 1,
                Name = "Savannah",
                Climate = Climate.Tropical,
                HabitatType = HabitatType.Grassland,
                SecurityLevel = SecurityLevel.Medium,
                Size = 1000
            };

            // Act
            var result = await controller.Create(enclosure);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, context.Enclosure.Count());
        }

        [Fact]
        public async Task Edit_ReturnsARedirect_WhenModelStateIsValid()
        {
            // Arrange
            var data = new List<Enclosure>
            {
                new Enclosure
                {
                    Id = 1,
                    Name = "Savannah",
                    Climate = Climate.Tropical,
                    HabitatType = HabitatType.Grassland,
                    SecurityLevel = SecurityLevel.Medium,
                    Size = 1000
                }
            };
            var context = GetDbContext(data);
            var controller = new EnclosuresController(context);

            var enclosure = new Enclosure
            {
                Id = 1,
                Name = "Rainforest",
                Climate = Climate.Tropical,
                HabitatType = HabitatType.Forest,
                SecurityLevel = SecurityLevel.High,
                Size = 1500
            };

            // Detach the original entity to avoid tracking conflicts
            context.Entry(data.First()).State = EntityState.Detached;

            // Act
            var result = await controller.Edit(1, enclosure);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            var editedEnclosure = context.Enclosure.FirstOrDefault(e => e.Id == 1);
            Assert.Equal("Rainforest", editedEnclosure.Name);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesEnclosure_AndRedirectsToIndex()
        {
            // Arrange
            var data = new List<Enclosure>
            {
                new Enclosure
                {
                    Id = 1,
                    Name = "Savannah",
                    Climate = Climate.Tropical,
                    HabitatType = HabitatType.Grassland,
                    SecurityLevel = SecurityLevel.Medium,
                    Size = 1000
                }
            };
            var context = GetDbContext(data);
            var controller = new EnclosuresController(context);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Empty(context.Enclosure);
        }
    }
}

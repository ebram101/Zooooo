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
    public class AnimalsControllerTests
    {
        private ZooContext GetDbContext(List<Animals> data)
        {
            var options = new DbContextOptionsBuilder<ZooContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure a unique database for each test
                .Options;
            var context = new ZooContext(options);
            context.Animals.AddRange(data);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndAddsAnimal_WhenModelStateIsValid()
        {
            // Arrange
            var context = GetDbContext(new List<Animals>());
            var controller = new AnimalsController(context);

            var animal = new Animals
            {
                Id = 1,
                Name = "Lion",
                Species = "Panthera leo",
                Size = CustomSize.Medium,
                DietaryClass = DietaryClass.Carnivore,
                ActivityPattern = ActivityPattern.Diurnal,
                EnclosureId = 1,
                SpaceRequirement = 500,
                SecurityRequirement = SecurityLevel.High,
                Prey = "Deer"
            };

            // Act
            var result = await controller.Create(animal);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, context.Animals.Count());
        }

        [Fact]
        public async Task Edit_ReturnsARedirect_WhenModelStateIsValid()
        {
            // Arrange
            var data = new List<Animals>
            {
                new Animals
                {
                    Id = 1,
                    Name = "Lion",
                    Species = "Panthera leo",
                    Size = CustomSize.Medium,
                    DietaryClass = DietaryClass.Carnivore,
                    ActivityPattern = ActivityPattern.Diurnal,
                    EnclosureId = 1,
                    SpaceRequirement = 500,
                    SecurityRequirement = SecurityLevel.High,
                    Prey = "Deer"
                }
            };
            var context = GetDbContext(data);
            var controller = new AnimalsController(context);

            var animal = new Animals
            {
                Id = 1,
                Name = "Tiger",
                Species = "Panthera tigris",
                Size = CustomSize.Large,
                DietaryClass = DietaryClass.Carnivore,
                ActivityPattern = ActivityPattern.Nocturnal,
                EnclosureId = 1,
                SpaceRequirement = 600,
                SecurityRequirement = SecurityLevel.High,
                Prey = "Boar"
            };

            // Detach the original entity to avoid tracking conflicts
            context.Entry(data.First()).State = EntityState.Detached;

            // Act
            var result = await controller.Edit(1, animal);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            var editedAnimal = context.Animals.FirstOrDefault(a => a.Id == 1);
            Assert.Equal("Tiger", editedAnimal.Name);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesAnimal_AndRedirectsToIndex()
        {
            // Arrange
            var data = new List<Animals>
            {
                new Animals
                {
                    Id = 1,
                    Name = "Lion",
                    Species = "Panthera leo",
                    Size = CustomSize.Medium,
                    DietaryClass = DietaryClass.Carnivore,
                    ActivityPattern = ActivityPattern.Diurnal,
                    EnclosureId = 1,
                    SpaceRequirement = 500,
                    SecurityRequirement = SecurityLevel.High,
                    Prey = "Deer"
                }
            };
            var context = GetDbContext(data);
            var controller = new AnimalsController(context);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Empty(context.Animals);
        }
    }
}

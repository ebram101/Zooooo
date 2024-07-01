using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoo.Controllers;
using Zoo.Data;
using Zoo.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TestProject1
{
    public class AnimalsControllerTests
    {
        private Mock<DbSet<Animals>> GetMockDbSet(List<Animals> data)
        {
            var queryableData = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<Animals>>();
            mockDbSet.As<IQueryable<Animals>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<Animals>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<Animals>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<Animals>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            return mockDbSet;
        }

        private ZooContext GetDbContext(List<Animals> data)
        {
            var options = new DbContextOptionsBuilder<ZooContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
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
            var mockContext = new Mock<ZooContext>();
            var animals = new List<Animals>();
            mockContext.Setup(m => m.Animals).Returns(GetMockDbSet(animals).Object);
            var controller = new AnimalsController(mockContext.Object);

            var animal = new Animals { Id = 1, Name = "Lion", Species = "Panthera leo", Size = CustomSize.Medium, DietaryClass = DietaryClass.Carnivore, ActivityPattern = ActivityPattern.Diurnal, EnclosureId = 1, SpaceRequirement = 500, SecurityRequirement = SecurityLevel.High };

            // Act
            var result = await controller.Create(animal);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockContext.Verify(m => m.Add(It.IsAny<Animals>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Edit_ReturnsARedirect_WhenModelStateIsValid()
        {
            // Arrange
            var data = new List<Animals>
        {
            new Animals { Id = 1, Name = "Lion", Species = "Panthera leo", Size = CustomSize.Medium, DietaryClass = DietaryClass.Carnivore, ActivityPattern = ActivityPattern.Diurnal, EnclosureId = 1, SpaceRequirement = 500, SecurityRequirement = SecurityLevel.High }
        };
            var context = GetDbContext(data);
            var controller = new AnimalsController(context);

            var animal = new Animals { Id = 1, Name = "Tiger", Species = "Panthera tigris", Size = CustomSize.Large, DietaryClass = DietaryClass.Carnivore, ActivityPattern = ActivityPattern.Nocturnal, EnclosureId = 1, SpaceRequirement = 600, SecurityRequirement = SecurityLevel.High };

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
            new Animals { Id = 1, Name = "Lion", Species = "Panthera leo", Size = CustomSize.Medium, DietaryClass = DietaryClass.Carnivore, ActivityPattern = ActivityPattern.Diurnal, EnclosureId = 1, SpaceRequirement = 500, SecurityRequirement = SecurityLevel.High }
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

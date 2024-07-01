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

    public class EnclosuresControllerTests
    {
        private Mock<DbSet<Enclosure>> GetMockDbSet(List<Enclosure> data)
        {
            var queryableData = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<Enclosure>>();
            mockDbSet.As<IQueryable<Enclosure>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<Enclosure>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<Enclosure>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<Enclosure>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            return mockDbSet;
        }

        private ZooContext GetDbContext(List<Enclosure> data)
        {
            var options = new DbContextOptionsBuilder<ZooContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
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
            var mockContext = new Mock<ZooContext>();
            var enclosures = new List<Enclosure>();
            mockContext.Setup(m => m.Enclosure).Returns(GetMockDbSet(enclosures).Object);
            var controller = new EnclosuresController(mockContext.Object);

            var enclosure = new Enclosure { Id = 1, Name = "Savannah", Climate = Climate.Warm, HabitatType = HabitatType.Open, SecurityLevel = SecurityLevel.Medium, Size = 1000 };

            // Act
            var result = await controller.Create(enclosure);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockContext.Verify(m => m.Add(It.IsAny<Enclosure>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Edit_ReturnsARedirect_WhenModelStateIsValid()
        {
            // Arrange
            var data = new List<Enclosure>
        {
            new Enclosure { Id = 1, Name = "Savannah", Climate = Climate.Warm, HabitatType = HabitatType.Open, SecurityLevel = SecurityLevel.Medium, Size = 1000 }
        };
            var context = GetDbContext(data);
            var controller = new EnclosuresController(context);

            var enclosure = new Enclosure { Id = 1, Name = "Rainforest", Climate = Climate.Humid, HabitatType = HabitatType.Forested, SecurityLevel = SecurityLevel.High, Size = 1500 };

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
            new Enclosure { Id = 1, Name = "Savannah", Climate = Climate.Warm, HabitatType = HabitatType.Open, SecurityLevel = SecurityLevel.Medium, Size = 1000 }
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

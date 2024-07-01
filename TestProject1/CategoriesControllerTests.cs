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
    public class CategoriesControllerTests
    {
        private Mock<DbSet<Category>> GetMockDbSet(List<Category> data)
        {
            var queryableData = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            return mockDbSet;
        }

        private ZooContext GetDbContext(List<Category> data)
        {
            var options = new DbContextOptionsBuilder<ZooContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new ZooContext(options);
            context.Category.AddRange(data);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndAddsCategory_WhenModelStateIsValid()
        {
            // Arrange
            var mockContext = new Mock<ZooContext>();
            var categories = new List<Category>();
            mockContext.Setup(m => m.Category).Returns(GetMockDbSet(categories).Object);
            var controller = new CategoriesController(mockContext.Object);

            var category = new Category { Id = 1, Name = "Mammals" };

            // Act
            var result = await controller.Create(category);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockContext.Verify(m => m.Add(It.IsAny<Category>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Edit_ReturnsARedirect_WhenModelStateIsValid()
        {
            // Arrange
            var data = new List<Category>
        {
            new Category { Id = 1, Name = "Mammals" }
        };
            var context = GetDbContext(data);
            var controller = new CategoriesController(context);

            var category = new Category { Id = 1, Name = "Birds" };

            // Act
            var result = await controller.Edit(1, category);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            var editedCategory = context.Category.FirstOrDefault(c => c.Id == 1);
            Assert.Equal("Birds", editedCategory.Name);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesCategory_AndRedirectsToIndex()
        {
            // Arrange
            var data = new List<Category>
        {
            new Category { Id = 1, Name = "Mammals" }
        };
            var context = GetDbContext(data);
            var controller = new CategoriesController(context);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Empty(context.Category);
        }
    }

}
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
        private ZooContext GetDbContext(List<Category> data)
        {
            var options = new DbContextOptionsBuilder<ZooContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure a unique database for each test
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
            var context = GetDbContext(new List<Category>());
            var controller = new CategoriesController(context);

            var category = new Category { Id = 1, Name = "Mammals" };

            // Act
            var result = await controller.Create(category);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, context.Category.Count());
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

            // Detach the original entity to avoid tracking conflicts
            context.Entry(data.First()).State = EntityState.Detached;

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

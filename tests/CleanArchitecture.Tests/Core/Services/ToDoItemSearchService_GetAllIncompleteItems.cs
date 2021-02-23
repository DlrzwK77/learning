using Ardalis.Specification;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Services;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.UnitTests.Core.Services
{
    public class ToDoItemSearchService_GetAllIncompleteItems
    {
        [Fact]
        public async Task ReturnsInvalidGivenNullSearchString()
        {
            var repo = new Mock<IRepository>();
            var service = new ToDoItemSearchService(repo.Object);
            var result = await service.GetAllIncompleteItemsAsync(null);
            Assert.Equal(Ardalis.Result.ResultStatus.Invalid, result.Status);

            Assert.Equal("searchString is required.", result.ValidationErrors.First().ErrorMessage);
        }

        [Fact]
        public async Task ReturnsErrorGivenDataAccessException()
        {
            var repo = new Mock<IRepository>();
            var service = new ToDoItemSearchService(repo.Object);
            repo.Setup(r => r.ListAsync(It.IsAny<ISpecification<ToDoItem>>()))
                .ThrowsAsync(new Exception("Database not there."));
            var result = await service.GetAllIncompleteItemsAsync(null);
            Assert.Equal(Ardalis.Result.ResultStatus.Invalid, result.Status);
        }

        [Fact]
        public async Task ReturnsListGivenSearchString()
        {
            var repo = new Mock<IRepository>();
            var service = new ToDoItemSearchService(repo.Object);
            var result = await service.GetAllIncompleteItemsAsync("foo");
            Assert.Equal(Ardalis.Result.ResultStatus.Ok, result.Status);
        }

        /*
        private List<ToDoItem> GetTestItems()
        {
            // Note: could use AutoFixture
            var builder = new ToDoItemBuilder();

            var items = new List<ToDoItem>();

            var item1 = builder.WithDefaultValues().Build();
            //item1.MarkComplete();
            items.Add(item1);

            var item2 = builder.WithDefaultValues().Id(2).Build();
            items.Add(item2);

            var item3 = builder.WithDefaultValues().Id(3).Build();
            items.Add(item3);

            return items;
        }
        */
    }
}

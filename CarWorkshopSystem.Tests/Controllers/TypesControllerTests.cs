using CarWorkshopSystem.Core.Enums;
using CarWorkshopSystem.WebAPI.Controllers;
using CarWorkshopSystem.WebAPI.ViewModel.General;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopSystem.Tests.Controllers
{
    public class TypesControllerTests
    {
        private readonly TypesController _controller;

        public TypesControllerTests()
        {
            _controller = new TypesController();
        }

        [Fact]
        public void StatusTypeList_ReturnsOk()
        {
            var expectedTypes = GeneralProcessStatusTypeList.Map().Select(item => new KeyValue<string>
            {
                Id = item.Key,
                Name = item.Value
            });

            var result = _controller.StatusTypeList();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTypes = Assert.IsAssignableFrom<IEnumerable<KeyValue<string>>>(okResult.Value);

            Assert.Equal(expectedTypes.Count(), returnedTypes.Count());
            Assert.Equal(expectedTypes, returnedTypes, new KeyValueEqualityComparer<string>());
        }

        [Fact]
        public void RoleTypeList_ReturnsOk()
        {
            var expectedTypes = UserRoleTypeList.Map().Select(item => new KeyValue<string>
            {
                Id = item.Key,
                Name = item.Value
            });

            var result = _controller.RoleTypeList();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTypes = Assert.IsAssignableFrom<IEnumerable<KeyValue<string>>>(okResult.Value);

            Assert.Equal(expectedTypes.Count(), returnedTypes.Count());
            Assert.Equal(expectedTypes, returnedTypes, new KeyValueEqualityComparer<string>());
        }
    }

    public class KeyValueEqualityComparer<T> : IEqualityComparer<KeyValue<T>>
    {
        public bool Equals(KeyValue<T> x, KeyValue<T> y)
        {
            return x.Id.Equals(y.Id) && x.Name.Equals(y.Name);
        }

        public int GetHashCode(KeyValue<T> obj)
        {
            return obj.Id.GetHashCode() ^ obj.Name.GetHashCode();
        }
    }
}

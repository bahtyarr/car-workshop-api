using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.WebAPI.Controllers;
using CarWorkshopSystem.WebAPI.ViewModel.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarWorkshopSystem.Tests.Controllers
{
    public class ServicesControllerTests
    {
        private readonly ServicesController _controller;
        private readonly Mock<IServiceRepository> _mockServiceRepository;

        public ServicesControllerTests()
        {
            _mockServiceRepository = new Mock<IServiceRepository>();
            _controller = new ServicesController(_mockServiceRepository.Object);
        }

        [Fact]
        public async Task GetServiceList_ReturnsOk()
        {
            var services = new List<Service>
            {
                new Service { Id = 1, Name = "Service 1", Price = 100 },
                new Service { Id = 2, Name = "Service 2", Price = 150 }
            };
            _mockServiceRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(services);

            var result = await _controller.GetServiceList();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedServices = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);

            Assert.Equal(services.Count, returnedServices.Count());
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsService()
        {
            int serviceId = 1;
            var service = new Service { Id = serviceId, Name = "Service 1", Price = 100 };
            _mockServiceRepository.Setup(x => x.GetByIdAsync(serviceId)).ReturnsAsync(service);

            var result = await _controller.GetById(serviceId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedService = Assert.IsType<Service>(okResult.Value);

            Assert.Equal(service.Id, returnedService.Id);
            Assert.Equal(service.Name, returnedService.Name);
            Assert.Equal(service.Price, returnedService.Price);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            int nonExistingId = 999;
            _mockServiceRepository.Setup(x => x.GetByIdAsync(nonExistingId)).ReturnsAsync((Service)null);

            var result = await _controller.GetById(nonExistingId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidModel_ReturnsServiceId()
        {
            var createServiceVm = new CreateServiceVm { Name = "New Service", Price = 200 };
            var createdService = new Service { Id = 1, Name = createServiceVm.Name, Price = createServiceVm.Price };
            _mockServiceRepository.Setup(x => x.AddAsync(It.IsAny<Service>())).ReturnsAsync(createdService);

            var result = await _controller.Create(createServiceVm);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_ExistingId_ReturnsNoContent()
        {
            int serviceId = 1;
            var updateServiceVm = new CreateServiceVm { Name = "Updated Service", Price = 250 };
            var existingService = new Service { Id = serviceId, Name = "Service 1", Price = 100 };
            _mockServiceRepository.Setup(x => x.GetByIdAsync(serviceId)).ReturnsAsync(existingService);

            var result = await _controller.Update(serviceId, updateServiceVm);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(existingService.Name, updateServiceVm.Name);
            Assert.Equal(existingService.Price, updateServiceVm.Price);
        }

        [Fact]
        public async Task Update_NonExistingId_ReturnsNotFound()
        {
            int nonExistingId = 999;
            var updateServiceVm = new CreateServiceVm { Name = "Updated Service", Price = 250 };
            _mockServiceRepository.Setup(x => x.GetByIdAsync(nonExistingId)).ReturnsAsync((Service)null);

            var result = await _controller.Update(nonExistingId, updateServiceVm);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingId_ReturnsNoContent()
        {
            int serviceId = 1;
            var existingService = new Service { Id = serviceId, Name = "Service 1", Price = 100 };
            _mockServiceRepository.Setup(x => x.GetByIdAsync(serviceId)).ReturnsAsync(existingService);

            var result = await _controller.Delete(serviceId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            int nonExistingId = 999;
            _mockServiceRepository.Setup(x => x.GetByIdAsync(nonExistingId)).ReturnsAsync((Service)null);

            var result = await _controller.Delete(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
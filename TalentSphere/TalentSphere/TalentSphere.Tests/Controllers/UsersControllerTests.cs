using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TalentSphere.Controllers;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Tests.Controllers
{
    // Tests for UsersController.GetById endpoint
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            // Create a fake IUserService
            _userServiceMock = new Mock<IUserService>();

            // Other dependencies are not used by the endpoints we test, so pass null!
            _controller = new UsersController(
                _userServiceMock.Object,
                mapper: null!,
                roleRepository: null!,
                userRoleService: null!,
                userRoleRepository: null!,
                tokenService: null!,
                auditLogHelper: null!);
        }

        [Test]
        public async Task GetById_UserExists_ReturnsOkResult()
        {
            // Arrange: create a fake user and tell the mock to return it
            var fakeUser = new User { UserID = 1, Name = "Rakesh", Email = "rakesh@test.com" };
            _userServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(fakeUser);

            // Act: call the controller
            var result = await _controller.GetById(1);

            // Assert: result should be a 200 OK
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange: mock returns null (user not found)
            _userServiceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((User)null!);

            // Act
            var result = await _controller.GetById(99);

            // Assert: result should be 404 Not Found
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}

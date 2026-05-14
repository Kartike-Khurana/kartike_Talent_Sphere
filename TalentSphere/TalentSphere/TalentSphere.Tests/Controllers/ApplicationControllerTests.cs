using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TalentSphere.Controllers;
using TalentSphere.DTOs;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Tests.Controllers
{
    // Tests for ApplicationController.GetById endpoint
    [TestFixture]
    public class ApplicationControllerTests
    {
        private Mock<IApplicationService> _applicationServiceMock;
        private ApplicationController _controller;

        [SetUp]
        public void Setup()
        {
            // Create a fake IApplicationService
            _applicationServiceMock = new Mock<IApplicationService>();

            // AuditLogHelper is not used by GetById, so we pass null!
            _controller = new ApplicationController(
                _applicationServiceMock.Object,
                auditLogHelper: null!);
        }

        [Test]
        public async Task GetById_ApplicationExists_ReturnsOkResult()
        {
            // Arrange: return a fake application
            var app = new ApplicationResponseDTO { ApplicationID = 7, JobID = 2, CandidateID = 11 };
            _applicationServiceMock.Setup(s => s.GetByIdAsync(7)).ReturnsAsync(app);

            // Act
            var result = await _controller.GetById(7);

            // Assert: should be 200 OK
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_ApplicationDoesNotExist_ReturnsNotFound()
        {
            // Arrange: mock returns null (application not found)
            _applicationServiceMock.Setup(s => s.GetByIdAsync(404)).ReturnsAsync((ApplicationResponseDTO?)null);

            // Act
            var result = await _controller.GetById(404);

            // Assert: should be 404 Not Found
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using TalentSphere.Controllers;
using TalentSphere.DTOs.Interview;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Tests.Controllers
{
    // Tests for InterviewsController.GetInterviewById endpoint
    [TestFixture]
    public class InterviewsControllerTests
    {
        private Mock<IInterviewService> _interviewServiceMock;
        private InterviewsController _controller;

        [SetUp]
        public void Setup()
        {
            // Create a fake IInterviewService
            _interviewServiceMock = new Mock<IInterviewService>();

            // Use NullLogger for logging and null! for the audit helper (not used by GetInterviewById)
            _controller = new InterviewsController(
                _interviewServiceMock.Object,
                NullLogger<InterviewsController>.Instance,
                auditLogHelper: null!);
        }

        [Test]
        public async Task GetInterviewById_DetailedExists_ReturnsOkResult()
        {
            // Arrange: detailed lookup returns a DTO
            var detailed = new InterviewResponseDTO { InterviewID = 3, JobTitle = "Backend Dev" };
            _interviewServiceMock.Setup(s => s.GetDetailedByIdAsync(3)).ReturnsAsync(detailed);

            // Act
            var result = await _controller.GetInterviewById(3);

            // Assert: should be 200 OK
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetInterviewById_NotFound_ReturnsNotFound()
        {
            // Arrange: both detailed and basic lookup return null
            _interviewServiceMock.Setup(s => s.GetDetailedByIdAsync(99)).ReturnsAsync((InterviewResponseDTO?)null);
            _interviewServiceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Interview?)null);

            // Act
            var result = await _controller.GetInterviewById(99);

            // Assert: should be 404 Not Found
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }
    }
}

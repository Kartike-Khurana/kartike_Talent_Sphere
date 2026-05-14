using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using TalentSphere.Controllers;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Tests.Controllers
{
    // Tests for JobsController.GetJobById endpoint
    [TestFixture]
    public class JobsControllerTests
    {
        private Mock<IJobService> _jobServiceMock;
        private JobsController _controller;

        [SetUp]
        public void Setup()
        {
            // Create a fake IJobService
            _jobServiceMock = new Mock<IJobService>();

            // ILogger is required but we don't care about logging, so use NullLogger
            // AuditLogHelper is not used by GetJobById, so we pass null!
            _controller = new JobsController(
                _jobServiceMock.Object,
                NullLogger<JobsController>.Instance,
                auditLogHelper: null!);
        }

        [Test]
        public async Task GetJobById_JobExists_ReturnsOkResult()
        {
            // Arrange: prepare a fake job and tell the mock to return it
            var fakeJob = new Job { JobID = 10, Title = "Software Engineer" };
            _jobServiceMock.Setup(s => s.GetByIdAsync(10)).ReturnsAsync(fakeJob);

            // Act
            var result = await _controller.GetJobById(10);

            // Assert: should be 200 OK with the job in the body
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            Assert.That(ok!.Value, Is.EqualTo(fakeJob));
        }

        [Test]
        public async Task GetJobById_JobDoesNotExist_ReturnsNotFound()
        {
            // Arrange: mock returns null (no job found)
            _jobServiceMock.Setup(s => s.GetByIdAsync(123)).ReturnsAsync((Job?)null);

            // Act
            var result = await _controller.GetJobById(123);

            // Assert: should be 404 Not Found
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }
    }
}

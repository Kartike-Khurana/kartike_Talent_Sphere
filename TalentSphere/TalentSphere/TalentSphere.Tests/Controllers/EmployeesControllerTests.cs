using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TalentSphere.Controllers;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Tests.Controllers
{
    // Tests for EmployeesController GetAll and GetById endpoints
    [TestFixture]
    public class EmployeesControllerTests
    {
        private Mock<IEmployeeService> _employeeServiceMock;
        private EmployeesController _controller;

        [SetUp]
        public void Setup()
        {
            // Create a fake IEmployeeService
            _employeeServiceMock = new Mock<IEmployeeService>();

            // Mapper and AuditLogHelper are not used by GetAll/GetById, so pass null!
            _controller = new EmployeesController(
                _employeeServiceMock.Object,
                mapper: null!,
                auditLogHelper: null!);
        }

        [Test]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange: return a list of two employees
            var employees = new List<EmployeeResponseDto>
            {
                new EmployeeResponseDto { EmployeeID = 1, Name = "Alice" },
                new EmployeeResponseDto { EmployeeID = 2, Name = "Bob" }
            };
            _employeeServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetAll();

            // Assert: should be 200 OK
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_EmployeeExists_ReturnsOkResult()
        {
            // Arrange: mock returns a real employee
            var employee = new Employee { EmployeeID = 5, Name = "Charlie" };
            _employeeServiceMock.Setup(s => s.GetByIdAsync(5)).ReturnsAsync(employee);

            // Set up a fake HttpContext so role checks don't crash
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
            };

            // Act
            var result = await _controller.GetById(5);

            // Assert: should be 200 OK
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_EmployeeDoesNotExist_ReturnsNotFound()
        {
            // Arrange: mock returns null (employee not found)
            _employeeServiceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Employee)null!);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
            };

            // Act
            var result = await _controller.GetById(99);

            // Assert: should be 404 Not Found
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}

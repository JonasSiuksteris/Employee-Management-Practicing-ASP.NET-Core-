using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Practice.Models;
using Practice.ViewModels;

namespace Practice.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;

        public HomeController(IEmployeeRepository employeeRepository,
            IWebHostEnvironment webHostEnvironment,
            ILogger<HomeController> logger )
        {
            _employeeRepository = employeeRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            Employee employee = _employeeRepository.GetEmployee(id.Value);

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            var homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }


        [HttpGet]
        public ViewResult Edit(int id)
        {
            var employee = _employeeRepository.GetEmployee(id);
            var employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                            "img", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                _employeeRepository.Update(employee);
                return RedirectToAction("index");

            }

            return View();
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var uniqueFileName = ProcessUploadedFile(model);
                var newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new {id = newEmployee.Id});

            }

            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Photo.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}

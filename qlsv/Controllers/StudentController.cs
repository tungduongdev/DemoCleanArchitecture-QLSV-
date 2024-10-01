using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Interfaces;
using Application.UseCases;

namespace presentation.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentController : Controller
    {
        private readonly AddStudentUseCase _addStudentUseCase;
        private readonly UpdateStudentUseCase _updateStudentUseCase;
        private readonly DeleteStudentUseCase _deleteStudentUseCase;
        private readonly GetStudentByIdUseCase _getStudentByIdUseCase;
        private readonly GetAllStudentsUseCase _getAllStudentsUseCase;
        private readonly SearchStudentsByNameUseCase _searchStudentsByNameUseCase;

        public StudentController(
            AddStudentUseCase addStudentUseCase,
            UpdateStudentUseCase updateStudentUseCase,
            DeleteStudentUseCase deleteStudentUseCase,
            GetStudentByIdUseCase getStudentByIdUseCase,
            GetAllStudentsUseCase getAllStudentsUseCase,
            SearchStudentsByNameUseCase searchStudentsByNameUseCase)
        {
            _addStudentUseCase = addStudentUseCase;
            _updateStudentUseCase = updateStudentUseCase;
            _deleteStudentUseCase = deleteStudentUseCase;
            _getStudentByIdUseCase = getStudentByIdUseCase;
            _getAllStudentsUseCase = getAllStudentsUseCase;
            _searchStudentsByNameUseCase = searchStudentsByNameUseCase;
        }

        // Hiển thị danh sách sinh viên
        [HttpGet]
        public IActionResult Index()
        {
            var students = _getAllStudentsUseCase.Execute();
            return View(students);
        }

        // Hiển thị form thêm sinh viên
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] Student student) // Thêm [FromBody]
        {
            if (ModelState.IsValid)
            {
                _addStudentUseCase.Execute(student);
                return Json(new { success = true }); // Trả về JSON nếu thành công
            }
            return Json(new { success = false }); // Trả về JSON nếu thất bại
        }


        // Hiển thị form sửa sinh viên
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var student = _getStudentByIdUseCase.Execute(id);
            if (student == null) return NotFound();
            return View(student);  // Đảm bảo rằng student.Id có giá trị
        }
        [HttpPost("edit/{id}")]
        public IActionResult Edit([FromBody] Student student)
        {
            if (ModelState.IsValid)
            {
                // Thực thi cập nhật sinh viên thông qua UseCase
                _updateStudentUseCase.Execute(student);

                // Trả về thông báo thành công dưới dạng JSON
                return Json(new { success = true });
            }

            // Nếu dữ liệu không hợp lệ, trả về thông báo lỗi
            return Json(new { success = false, errors = ModelState });
        }



        // Hiển thị form xóa sinh viên
        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var student = _getStudentByIdUseCase.Execute(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            _deleteStudentUseCase.Execute(id);
            return RedirectToAction("Index");
        }

        // Hiển thị chi tiết sinh viên
        [HttpGet("details/{id}")]
        public IActionResult Details(int id)
        {
            var student = _getStudentByIdUseCase.Execute(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpGet("search")]
        public IActionResult Search(string name)
        {
            var students = _searchStudentsByNameUseCase.Execute(name);
            return View("Index", students);
        }
    }
}

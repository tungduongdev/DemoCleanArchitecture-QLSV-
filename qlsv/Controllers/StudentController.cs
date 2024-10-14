using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Interfaces;
using Application.UseCases;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;
using System.Linq;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly AddStudentUseCase _addStudentUseCase;
        private readonly UpdateStudentUseCase _updateStudentUseCase;
        private readonly DeleteStudentUseCase _deleteStudentUseCase;
        private readonly GetStudentByIdUseCase _getStudentByIdUseCase;
        private readonly GetAllStudentsUseCase _getAllStudentsUseCase;
        private readonly SearchStudentsByNameUseCase _searchStudentsByNameUseCase;

        public StudentController(
            IStudentRepository studentRepository,   // Inject repository
            AddStudentUseCase addStudentUseCase,
            UpdateStudentUseCase updateStudentUseCase,
            DeleteStudentUseCase deleteStudentUseCase,
            GetStudentByIdUseCase getStudentByIdUseCase,
            GetAllStudentsUseCase getAllStudentsUseCase,
            SearchStudentsByNameUseCase searchStudentsByNameUseCase)
        {
            _studentRepository = studentRepository;   // Assign repository
            _addStudentUseCase = addStudentUseCase;
            _updateStudentUseCase = updateStudentUseCase;
            _deleteStudentUseCase = deleteStudentUseCase;
            _getStudentByIdUseCase = getStudentByIdUseCase;
            _getAllStudentsUseCase = getAllStudentsUseCase;
            _searchStudentsByNameUseCase = searchStudentsByNameUseCase;
        }

        // Hiển thị danh sách sinh viên
        [HttpGet("index")]
        public IActionResult Index(int pageNumber = 1, int pageSize = 5)
        {
            var paginatedResult = _getAllStudentsUseCase.Execute(pageNumber, pageSize);
            return View(paginatedResult); ;
        }

        // Hiển thị form thêm sinh viên
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] Student student)
        {
            if (ModelState.IsValid)
            {
                _addStudentUseCase.Execute(student);
                return Ok(new { success = true });
            }
            return BadRequest(new { success = false, errors = ModelState });
        }

        // Hiển thị form sửa sinh viên
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var student = _getStudentByIdUseCase.Execute(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit([FromBody] Student student)
        {
            if (ModelState.IsValid)
            {
                _updateStudentUseCase.Execute(student);
                return Ok(new { success = true });
            }
            return BadRequest(new { success = false, errors = ModelState });
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
        public IActionResult Search(string name, int pageNumber = 1, int pageSize = 5)
        {
            var students = _searchStudentsByNameUseCase.Execute(name); // Giả sử trả về List<Student>
            if (string.IsNullOrWhiteSpace(name))
            {
                // Nếu không có tên, chuyển hướng về trang danh sách sinh viên với trang đầu tiên
                ViewBag.SearchSuccess = students.Any();
                return RedirectToAction("Index", new { pageNumber = 1 });
            }
            // Tìm kiếm sinh viên theo tên

            // Tạo PaginatedResult
            var paginatedResult = new PaginatedResult<Student>
            {
                Items = students.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                TotalCount = students.Count(), // Số lượng tổng sinh viên tìm được
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.SearchSuccess = students.Any();

            return View("Index", paginatedResult);
        }

        // Chức năng xuất danh sách sinh viên ra file Excel
        [HttpGet("export")]
        public async Task<IActionResult> ExportStudentsToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var students = _studentRepository.GetAll().ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Students");

                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Class";
                worksheet.Cells[1, 4].Value = "Age";

                for (int i = 0; i < students.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = students[i].Id;
                    worksheet.Cells[i + 2, 2].Value = students[i].Name;
                    worksheet.Cells[i + 2, 3].Value = students[i].Class;
                    worksheet.Cells[i + 2, 4].Value = students[i].Age;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"StudentList-{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                TempData["ExportSuccess"] = "Xuất file Excel thành công!";

                RedirectToAction("Index", "Students");

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportStudentsFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Chưa chọn file hoặc file rỗng.");
            }

            // Thiết lập LicenseContext cho EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await excelFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
                    var rowCount = worksheet.Dimension.Rows;

                    // Duyệt qua từng dòng và thêm sinh viên
                    for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2 vì dòng 1 là tiêu đề
                    {
                        var student = new Student
                        {
                            Name = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                            Age = int.Parse(worksheet.Cells[row, 3].Value?.ToString().Trim()),
                            Class = worksheet.Cells[row, 4].Value?.ToString().Trim()
                        };

                        // Thêm sinh viên vào repository
                        _addStudentUseCase.Execute(student);
                    }

                    // Sau khi thêm tất cả sinh viên, thiết lập thông báo và chuyển hướng
                    TempData["ImportSuccess"] = "Import file thành công!"; // Thông báo thành công
                    return RedirectToAction("Index", "Students"); // Chuyển hướng về trang chính
                }
            }

            // Nếu không có sinh viên nào được thêm, trả về thông báo lỗi
            ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình import.");
            return View(); // Trả về view nếu có lỗi
        }

    }
}

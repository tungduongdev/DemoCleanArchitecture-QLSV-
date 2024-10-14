using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class GetAllStudentsUseCase
    {
        private readonly IStudentRepository _studentRepository;

        public GetAllStudentsUseCase(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public PaginatedResult<Student> Execute(int pageNumber, int pageSize)
        {
            var students = _studentRepository.GetAll(); // Lấy tất cả sinh viên
            var totalCount = students.Count();

            var paginatedStudents = students.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedResult<Student>
            {
                Items = paginatedStudents,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}

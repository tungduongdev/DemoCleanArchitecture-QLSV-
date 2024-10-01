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

        public IEnumerable<Student> Execute()
        {
            return _studentRepository.GetAll();
        }
    }
}

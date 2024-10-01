using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class SearchStudentsByNameUseCase
    {
        private readonly IStudentRepository _studentRepository;
        public SearchStudentsByNameUseCase(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IEnumerable<Student> Execute(string name)
        {
            return _studentRepository.SearchStudent(name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class AddStudentUseCase
    {
        private readonly IStudentRepository _studentRepository;

        public AddStudentUseCase(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Execute(Student student)
        {
            _studentRepository.Add(student);
        }
    }
}

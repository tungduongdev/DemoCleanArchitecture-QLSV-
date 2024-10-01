using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class UpdateStudentUseCase
    {
        private readonly IStudentRepository _studentRepository;

        public UpdateStudentUseCase(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Execute(Student student)
        {
            _studentRepository.Update(student);
        }
    }
}

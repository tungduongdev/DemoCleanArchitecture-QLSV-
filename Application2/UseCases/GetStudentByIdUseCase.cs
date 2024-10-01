using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class GetStudentByIdUseCase
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentByIdUseCase(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Student Execute(int id)
        {
            return _studentRepository.GetById(id);
        }
    }
}

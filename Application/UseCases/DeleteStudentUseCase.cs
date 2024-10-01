using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class DeleteStudentUseCase
    {
        private readonly IStudentRepository _studentRepository;

        public DeleteStudentUseCase(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Execute(int id)
        {
            _studentRepository.Delete(id);
        }
    }
}

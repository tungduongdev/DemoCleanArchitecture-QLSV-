using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IStudentRepository
    {
        void Add(Student student);
        void Update(Student student);
        void Delete(int id);
        Student GetById(int id);
        IEnumerable<Student> GetAll();
        IEnumerable<Student> SearchStudent(string name);
    }
}

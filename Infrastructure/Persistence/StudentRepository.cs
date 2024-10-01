using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void Update(Student student)
        {
            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        public Student GetById(int id)
        {
            return _context.Students.Find(id);
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public IEnumerable<Student> SearchStudent(string name)
        {
            return _context.Students
                           .Where(s => s.Name.Contains(name))
                           .ToList();
        }

    }
}

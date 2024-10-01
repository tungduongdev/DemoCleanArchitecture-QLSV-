using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.Tests.UseCases
{
    public class AddStudentUseCaseTests
    {
        [Fact]
        public void Execute_ShouldCallAddMethodOnce()
        {
            // Arrange: Tạo một mock cho IStudentRepository
            var studentRepositoryMock = new Mock<IStudentRepository>();

            // Khởi tạo AddStudentUseCase với mock của IStudentRepository
            var useCase = new AddStudentUseCase(studentRepositoryMock.Object);

            // Tạo đối tượng sinh viên
            var student = new Student { Name = "Tùng Dương",Class ="KTPM K20C", Age = 20 };

            // Act: Gọi phương thức Execute
            useCase.Execute(student);

            // Assert: Kiểm tra xem phương thức Add() của repository có được gọi đúng không
            studentRepositoryMock.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Once);
        }
    }
}

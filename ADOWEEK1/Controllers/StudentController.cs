using ADOWEEK1.Model;
using ADOWEEK1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ADOWEEK1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService) //constructor of StudentController
        {
            _studentService = studentService;
        }

        [HttpGet]
        public List<Student> GetStudents()
        {
            return _studentService.GetStudents();
        }

        [HttpPost]

        public Student AddStudents(Student student)
        {
            return _studentService.AddStudent(student);
        }

        [HttpDelete]
        public void DeleteStudent(int id)
        {
            _studentService.DeleteStudent(id);
        }

        [HttpGet("{id}")]

        public Student? GetStudentById(int id)
        {
            return _studentService.GetStudentById(id);

        }

        [HttpPut("{id}")]
        public Student UpdateStudent(int id, Student student)
        {
            return _studentService.UpdateStudent(id, student);
        }

        [HttpGet("dataset")]
        public List<Student> GetStudentsUsingDataSet()       //using DataSet
        {
            DataSet Ds = _studentService.GetStudentsUsingDataSet();

            List<Student> student = new List<Student>();

            foreach (DataRow row in Ds.Tables[0].Rows)
            {
                student.Add(new Student {

                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Age = Convert.ToInt32(row["Age"])

                });

            }
                return student;
        }

        [HttpGet("FilteredDataset")]

        public List<Student> FilteredStudents()
        {
            DataTable FDS = _studentService.FilteredStudents();

            List<Student> students = new List<Student>();

            foreach (DataRow row in FDS.Rows)
            {
                students.Add(new Student
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString()!,
                    Age = Convert.ToInt32(row["Age"])
                });
            }
            return students;
     
        }

        //[HttpPost]

        //public List<Student> InsertingValueToATable()
        //{
                   
        //}
    }
}

using ADOWEEK1.Model;
using System.Data;

namespace ADOWEEK1.Services
{
    public interface IStudentService
    {
        List<Student> GetStudents();
        Student AddStudent(Student student);
        void DeleteStudent(int id);
        Student? GetStudentById(int id); // if no such student exists return Null

        Student UpdateStudent(int id, Student student);

        DataSet GetStudentsUsingDataSet();

        DataTable FilteredStudents();

        //DataSet InsertingValueToATable();
    }
}
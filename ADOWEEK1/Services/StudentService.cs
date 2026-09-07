using ADOWEEK1.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ADOWEEK1.Services
{
    public class StudentService : IStudentService
    {
        private readonly string _ConnectionString;

        public StudentService(IConfiguration configuration)
        {
           _ConnectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();

            using SqlConnection connection = new SqlConnection(_ConnectionString);

            connection.Open();

            string query = "SELECT Id, Name, Age FROM Students";

            using SqlCommand command = new SqlCommand(query, connection);

            using SqlDataReader reader = command.ExecuteReader();
            {

                while (reader.Read())
                {
                    Student student = new Student
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString()!,
                        Age = Convert.ToInt32(reader["Age"])
                    };
                    students.Add(student);
                }
                return students;
            }

        }

        public Student AddStudent(Student student)
        {
            using SqlConnection connection = new SqlConnection(_ConnectionString);
            connection.Open();

            string query =
                @"INSERT INTO Students (Name, Age)
                  VALUES(@Name,@Age);
                 
                   SELECT SCOPE_IDENTITY();
                ";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Age",student.Age);

            int newId = Convert.ToInt32(command.ExecuteScalar());//returns only the first column of the first row and return is object.

            student.Id = newId;

            return student;

            
        }

        public void DeleteStudent(int id)
        {
            using SqlConnection connection = new SqlConnection(_ConnectionString);
            connection.Open();

            string query = "DELETE FROM Students WHERE id = @id";
            using SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@id",id);

            command.ExecuteNonQuery();
            
        }

        public Student? GetStudentById(int id)
        {
            using SqlConnection connection = new SqlConnection(_ConnectionString);

            connection.Open();

            string query = "SELECT Id, Name, Age FROM Students WHERE Id = @id";

            using SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@id",id);

            using (SqlDataReader reader = command.ExecuteReader()) // It is used to read one or more rows from the database, returns sqlDataReader object
            {
                if (reader.Read())
                {
                    Student student = new Student
                    {


                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString()!,
                        Age = Convert.ToInt32(reader["Age"])


                    };


                    return student;

                }

                return null; // Return null if no student found with the given id, bcz we gave the return type Student?
            }

            
        }

        public Student UpdateStudent(int id, Student student)
        {
            using SqlConnection conn = new SqlConnection(_ConnectionString);

            conn.Open();

            string que = "UPDATE Students SET Name = @Name, Age = @Age WHERE Id = @id";

            using SqlCommand comm = new SqlCommand(que,conn);

            comm.Parameters.AddWithValue("@id",student.Id);
            comm.Parameters.AddWithValue("@Name", student.Name);
            comm.Parameters.AddWithValue("@Age",student.Age);
            int rowsAffected = comm.ExecuteNonQuery();        //used to when dont want to return a result set instead it will return an integer.

            return student;

        }

        public DataSet GetStudentsUsingDataSet()              //using DataSet
        {
            DataSet StudetDataSet = new DataSet();
            using SqlConnection connection = new SqlConnection(_ConnectionString);

            string query = "SELECT Id, Name, Age FROM Students";

            using SqlDataAdapter adapter = new SqlDataAdapter(query, connection); //we didnt call connection.open() bcz it will open and close the connection automatically when we use DataAdapter.

            adapter.Fill(StudetDataSet,"Students");           //here Students is a column 
            // Modifying the Age

            DataTable table = StudetDataSet.Tables["Students"]!;

            foreach (DataRow row in table.Rows)
            {
                if (Convert.ToInt32(row["Id"]) == 1)
                {
                    row["Age"] = 25; // Update the Age for the student with Id = 1
                }
            }

            using SqlCommandBuilder builder1 = new SqlCommandBuilder(adapter);

            adapter.Update(StudetDataSet,"Students");

            return StudetDataSet;
            
        }

        public DataTable FilteredStudents()
        {
            DataTable FilteredStudents = new DataTable();

            using SqlConnection connection = new SqlConnection(_ConnectionString);

            string query = "SELECT Id, Name, Age FROM Students";

            using SqlDataAdapter adapter = new SqlDataAdapter(query,connection);

            adapter.Fill(FilteredStudents);

            DataTable table = FilteredStudents;

            DataView view = new DataView(table);

            view.RowFilter = "Age > 22";
            
            return view.ToTable();

        }


        public DataTable CreatingTableAndInsertValues()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Id",typeof(int));
            table.Columns.Add("Name",typeof(string));
            table.Columns.Add("Age",typeof(int));

            var FirstMan = table.Rows.Add(1, "Sai", 26);
            var SeconMan = table.Rows.Add(2, "Anirudh",29);

            using SqlConnection connection = new SqlConnection(_ConnectionString);

            string query = @"CREATE TABLE NewStudents
                                (Id INT PRIMARY KEY,
                                 Name VARCHAR(100),
                                 Age INT)";
            using SqlCommand command = new SqlCommand(query, connection);

            command.ExecuteNonQuery();

            string insertQuery = @"INSERT INTO NewStudents (Id, Name, Age)VALUES(@Id,@Name,@Age) "

            using SqlCommand InsertCommand = new SqlCommand(insertQuery,connection);

            InsertCommand.Parameters.Add("Id",SqlDbType.Int);
            InsertCommand.Parameters.Add("Name",SqlDbType.VarChar,100);
            InsertCommand.Parameters.Add("Age",SqlDbType.Int);


            InsertCommand.Parameters["@Id"].Value = FirstMan["Id"];
            InsertCommand.Parameters["@Name"].Value = FirstMan["Name"];
            InsertCommand.Parameters["@Age"].Value = FirstMan["Age"];

            InsertCommand.Parameters["@Id"].Value = SeconMan["Id"];
            InsertCommand.Parameters["@Name"].Value = SeconMan["Name"];
            InsertCommand.Parameters["Age"].Value = SeconMan["Age"];

            InsertCommand.ExecuteNonQuery();

            return table;

        }

        //public List<Student> StoredProcedureMe()
        //{
        //    using SqlConnection connection = new SqlConnection(_ConnectionString);

        //    string query = "sd";

        //    using SqlCommand command = new SqlCommand(connection);
        //}

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Text;

namespace FrmClubRegistration
{
    internal class ClubRegistrationQuery
    {
        private SqlConnection sqlConnect;
        private SqlCommand sqlCommand;
        private SqlDataAdapter sqlAdapter;
        private SqlDataReader sqlReader;

        public DataTable dataTable;
        public BindingSource bindingSource;

        private string connectionString;

        public string _FirstName, _MiddleName, _LastName, _Gender, _Program;
        public int _Age;

        public string ConnectionString
        {
            get { return connectionString; }
        }
        public ClubRegistrationQuery()
        {
            connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Administrator\\OneDrive\\Documents\\ClubDB.mdf;Integrated Security=True;Connect Timeout=30";       
            sqlConnect = new SqlConnection(connectionString);
            dataTable = new DataTable();
            bindingSource = new BindingSource();
        }

        public bool DisplayList()
        {
            string ViewClubMembers = "SELECT StudentId, FirstName, MiddleName, LastName, Age, Gender, Program FROM ClubMembers";

            sqlAdapter = new SqlDataAdapter(ViewClubMembers, sqlConnect);

            try
            {
                dataTable.Clear();
                sqlAdapter.Fill(dataTable);
                bindingSource.DataSource = dataTable;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying list: " + ex.Message);
                return false;
            }
        }

        public bool RegisterStudent(int ID, long StudentID, string FirstName, string MiddleName, string LastName, int Age, string Gender, string Program)
        {
            sqlCommand = new SqlCommand("INSERT INTO ClubMembers VALUES(@ID, @StudentID, @FirstName, @MiddleName, @LastName, @Age, @Gender, @Program)", sqlConnect);

            sqlCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
            sqlCommand.Parameters.Add("@StudentID", SqlDbType.BigInt).Value = StudentID;
            sqlCommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCommand.Parameters.Add("@MiddleName", SqlDbType.VarChar).Value = MiddleName;
            sqlCommand.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;
            sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = Age;
            sqlCommand.Parameters.Add("@Gender", SqlDbType.VarChar).Value = Gender;
            sqlCommand.Parameters.Add("@Program", SqlDbType.VarChar).Value = Program;

            try
            {
                sqlConnect.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnect.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering student: " + ex.Message);
                if (sqlConnect.State == ConnectionState.Open)
                    sqlConnect.Close();
                return false;
            }
        }
        public bool UpdateStudent(int ID, long StudentID, string FirstName, string MiddleName, string LastName, int Age, string Gender, string Program)
        {
            sqlCommand = new SqlCommand("UPDATE ClubMembers SET StudentId=@StudentID, FirstName=@FirstName, MiddleName=@MiddleName, " +
                                      "LastName=@LastName, Age=@Age, Gender=@Gender, Program=@Program WHERE ID=@ID", sqlConnect);

            sqlCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
            sqlCommand.Parameters.Add("@StudentID", SqlDbType.BigInt).Value = StudentID;
            sqlCommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCommand.Parameters.Add("@MiddleName", SqlDbType.VarChar).Value = MiddleName;
            sqlCommand.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;
            sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = Age;
            sqlCommand.Parameters.Add("@Gender", SqlDbType.VarChar).Value = Gender;
            sqlCommand.Parameters.Add("@Program", SqlDbType.VarChar).Value = Program;

            try
            {
                sqlConnect.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnect.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating student: " + ex.Message);
                if (sqlConnect.State == ConnectionState.Open)
                    sqlConnect.Close();
                return false;
            }
        }

        public DataTable GetStudentByID(int ID)
        {
            string query = "SELECT * FROM ClubMembers WHERE ID = @ID";
            sqlAdapter = new SqlDataAdapter(query, sqlConnect);

            sqlAdapter.SelectCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

            DataTable studentTable = new DataTable();
            try
            {
                sqlAdapter.Fill(studentTable);
                return studentTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving student: " + ex.Message);
                return null;
            }
        }

        public bool DeleteStudent(long studentId)
        {
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                {
                    sqlConnect.Open();
                    string query = "DELETE FROM ClubMembers WHERE StudentId = @StudentId";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnect))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting student: " + ex.Message);
                return false;
            }
        }

    }
}


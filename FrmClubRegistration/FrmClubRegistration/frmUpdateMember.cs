using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmClubRegistration
{
    public partial class frmUpdateMember : Form
    {
        private ClubRegistrationQuery clubRegistrationQuery;
        private int selectedID;
        public frmUpdateMember()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmUpdateMember_Load(object sender, EventArgs e)
        {
            clubRegistrationQuery = new ClubRegistrationQuery();

            
            LoadStudentIDs();


            string[] programs = { "BS Information Technology", "BS Computer Science", "BS Computer Engineering", "BS Information System", "BS Artificial Intelligence" };
            comboBox1.Items.AddRange(programs);

            string[] genders = { "Male", "Female", "Prefer not to say" };
            comboBox3.Items.AddRange(genders);

        }

        private void LoadStudentIDs()
        {
            string query = "SELECT ID FROM ClubMembers";

            // Use the public property instead of private field
            using (SqlConnection connection = new SqlConnection(clubRegistrationQuery.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox2.Items.Clear();
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(reader["ID"].ToString());
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading student IDs: " + ex.Message);
                }
            }
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                if (int.TryParse(comboBox2.SelectedItem.ToString(), out selectedID))
                {
                    LoadStudentData(selectedID);
                }
            }
        }



        private void LoadStudentData(int ID)
        {
            DataTable studentData = clubRegistrationQuery.GetStudentByID(ID);

            if (studentData != null && studentData.Rows.Count > 0)
            {
                DataRow row = studentData.Rows[0];

                txtFirst.Text = row["FirstName"].ToString();
                txtMiddle.Text = row["MiddleName"].ToString();
                txtLast.Text = row["LastName"].ToString();
                comboBox2.Text = row["StudentId"].ToString();
                txtage.Text = row["Age"].ToString();

                // Set combo box selections
                comboBox3.SelectedItem = row["Gender"].ToString();
                comboBox1.SelectedItem = row["Program"].ToString();
            }
        }



        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(txtFirst.Text) ||
               string.IsNullOrWhiteSpace(txtLast.Text) ||
               string.IsNullOrWhiteSpace(comboBox2.Text) ||
               !int.TryParse(txtage.Text, out int Age) ||
               comboBox3.SelectedIndex == -1 ||
               comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Age validation
            if (Age < 16 || Age > 100)
            {
                MessageBox.Show("Please enter a valid age between 16 and 100.");
                txtage.Focus();
                return;
            }

            if (!long.TryParse(comboBox2.Text, out long StudentId))
            {
                MessageBox.Show("Invalid Student ID format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get form values
            string FirstName = txtFirst.Text;
            string MiddleName = txtMiddle.Text;
            string LastName = txtLast.Text;
            string Gender = comboBox3.Text;
            string Program = comboBox1.Text;

            // Update student
            bool result = clubRegistrationQuery.UpdateStudent(selectedID, StudentId, FirstName, MiddleName, LastName, Age, Gender, Program);

            if (result)
            {
                MessageBox.Show("Student information updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudentIDs(); // Refresh the list
                ClearForm();
            }
            else
            {
                MessageBox.Show("Failed to update student information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearForm()
        {
            comboBox2.SelectedIndex = -1;
            txtFirst.Text = "";
            txtMiddle.Text = "";
            txtLast.Text = "";
            comboBox2.Text = "";
            txtage.Text = "";
            comboBox3.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }
    }
}

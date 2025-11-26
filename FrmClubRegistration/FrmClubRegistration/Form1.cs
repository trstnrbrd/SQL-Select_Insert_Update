using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace FrmClubRegistration
{
    public partial class frmClubRegistration : Form
    {

        private ClubRegistrationQuery clubRegistrationQuery;
        private int ID, Age, count;
        private string FirstName, MiddleName, LastName, Gender, Program;
        private long StudentId;


        public frmClubRegistration()
        {
            InitializeComponent();
            LoadComboBox();
        }

        private void RefreshListOfClubMembers()
        {
            clubRegistrationQuery.DisplayList();
            dataGridView1.DataSource = clubRegistrationQuery.bindingSource;
        }

        private int RegistrationID()
        {
            count++;
            return count;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            frmUpdateMember updateForm = new frmUpdateMember();
            updateForm.ShowDialog();

           
            RefreshListOfClubMembers();
        }


        private void LoadComboBox()
        {
            string[] programs = { "BS Information Technology", "BS Computer Science", "BS Computer Engineering", "BS Information System", "BS Artificial Intelligence" };
            comboBox2.Items.AddRange(programs);


            string[] genders = { "Male", "Female", "Prefer not to say"};
            comboBox1.Items.AddRange(genders);

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshListOfClubMembers();
        }

        private void frmClubRegistration_Load(object sender, EventArgs e)
        {

            
            clubRegistrationQuery = new ClubRegistrationQuery();
            count = 0;
            RefreshListOfClubMembers();

        }


        private void btnRegister_Click(object sender, EventArgs e)
        {

           

            ID = RegistrationID();

            if (!long.TryParse(txtStudentID.Text.Trim(), out StudentId))
            {
                MessageBox.Show("Please enter a valid numeric Student ID.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First Name is required.");
                return;
            }
            FirstName = txtFirstName.Text.Trim();

            MiddleName = txtMiddleName.Text.Trim();
            LastName = txtLastName.Text.Trim();

            
            
            if (int.TryParse(txtAge.Text, out Age))
            {
                MessageBox.Show("Age must be a valid number.");
                return;
            }


            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Please select a Gender.");
                return;
            }
            Gender = comboBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Please select a Program.");
                return;
            }
            Program = comboBox2.Text.Trim();

            
            bool ok = clubRegistrationQuery.RegisterStudent(
                ID, StudentId, FirstName, MiddleName, LastName, Age, Gender, Program
            );

            if (ok)
            {
                MessageBox.Show("Registered Successfully!");
                RefreshListOfClubMembers();
          
            }
            else
            {
                MessageBox.Show("Registration Failed.");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                DataGridViewRow row = dataGridView1.CurrentRow;
                // Use the column name as it appears in your DataGridView
                long studentId = Convert.ToInt64(row.Cells["StudentId"].Value);

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to remove this student?",
                    "Confirm Remove",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    bool success = clubRegistrationQuery.DeleteStudent(studentId);
                    if (success)
                    {
                        MessageBox.Show("Student removed successfully!");
                        RefreshListOfClubMembers();
                    }
                    else
                    {
                        MessageBox.Show("Failed to remove the student.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a student to remove.");
            }

        }

        private void label1_Click(object sender, EventArgs e)
            {

            }

            private void groupBox2_Enter(object sender, EventArgs e)
            {

            }
        }
    }


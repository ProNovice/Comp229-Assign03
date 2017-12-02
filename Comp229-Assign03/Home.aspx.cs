using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Comp229_Assign03
{
    public partial class Home : System.Web.UI.Page
    {
        //3.	Your Landing Page will: 
        //    a.identify the brand.
        //    b.provide a list of all students’ names from the database.
        //    c.allow for the addition of new students to the database.
        //    d.allow a user to click on a student, loading the Student Page.

        // Creating a connection from server string
        private SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Comp229Assign03;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only work when the page is initialized
            if (!IsPostBack)
            {
                GetStudents();
            }
        }


        private void GetStudents()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
            {
                // declare SqlCommand to execute
                SqlCommand studentsComm = new SqlCommand(
                    "SELECT *, FirstMidName + ' ' + LastName AS Name, CONVERT(VARCHAR(10), EnrollmentDate, 110) AS Date FROM Students", conn);

                // open connection between Database and ASP.NET
                conn.Open();
                // put the result of SqlCommand into SqlDataReader
                SqlDataReader studentsReader = studentsComm.ExecuteReader();
                // put data in studentsRepeater
                studentsRepeater.DataSource = studentsReader;
                // databind
                studentsRepeater.DataBind();
                conn.Close();
            }
        }



        protected void studentsRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "deleteStudent")
            {
                SqlCommand deleteEnrollments = new SqlCommand("DELETE FROM Enrollments WHERE StudentID=@StudentID", connection);
                SqlCommand deleteStudent = new SqlCommand("DELETE FROM Students WHERE StudentID=@StudentID", connection);

                // parameterize values in SqlCommands
                deleteEnrollments.Parameters.AddWithValue("@StudentID", e.CommandArgument);
                deleteStudent.Parameters.AddWithValue("@StudentID", e.CommandArgument);

                connection.Open(); // open the connection

                // delete enrollments first, then delete the student to prevent a order conflict
                deleteEnrollments.ExecuteNonQuery();
                deleteStudent.ExecuteNonQuery();

                connection.Close(); // close the connection

                // Rebind changed students database
                // Here is the Home page, so Redirect to Home.aspx was not added
                GetStudents();

            }
            // redirect to Student page
            else if (e.CommandName == "linkStudent")
            {
                Session["StudentID"] = e.CommandArgument.ToString();
                Response.Redirect("Student.aspx");
            }
        }

        // Those actions cannot use ItemCommand in Repeater if th
        protected void studentAddButton_click(object sender, EventArgs e)
        {
            // put all values of a new student into Database to add the new student data

            string studentId = string.Empty;
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            DateTime enrollmentDate = DateTime.Today;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
            {
                // open connection between Database and ASP.NET
                conn.Open();

                // declare SqlCommand to execute
                SqlCommand findLastStudentIDComm = new SqlCommand(
                    "SELECT MAX(StudentID) FROM Students", conn);

                // insert new student
                SqlCommand insertNewStudent = new SqlCommand(
                    "INSERT INTO Students (FirstMidName, LastName, EnrollmentDate)" +
                    "VALUES (@FirstName, @LastName, @EnrollmentDate);", conn);
                insertNewStudent.Parameters.AddWithValue("@FirstName", firstName);
                insertNewStudent.Parameters.AddWithValue("@LastName", lastName);
                insertNewStudent.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

                insertNewStudent.ExecuteNonQuery();

                // close the connection
                conn.Close();

                GetStudents();
            }

        }

    }
}

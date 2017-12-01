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
            int studentID = Convert.ToInt32(Session["StudentID"]);

            if (studentID >= 300000)
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
                {
                    // declare SqlCommand to execute
                    SqlCommand coursesComm = new SqlCommand(
                        "SELECT * FROM Students", conn);

                    // open connection between Database and ASP.NET
                    conn.Open();
                    // put the result of SqlCommand into SqlDataReader
                    SqlDataReader coursesReader = coursesComm.ExecuteReader();
                    // put data in studentsRepeater
                    studentsRepeater.DataSource = coursesReader;
                    // databind
                    studentsRepeater.DataBind();
                    conn.Close();
                }
        }


        protected void studentCoursesRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // redirect to Course page
            if (e.CommandName == "linkStudent")
            {
                Session["StudentID"] = e.CommandArgument.ToString();
                Response.Redirect("Student.aspx");
            }
        }


        #region Buttons

        // Those actions cannot use ItemCommand in Repeater if th
        protected void studentAddButton_click(object sender, EventArgs e)
        {
            // put all values of a new student into Database to add the new student data

            string studentId = txtStudentID.ToString();
            string studentName = txtStudentName.ToString();

            // define lastname which is the last word in a full name
            int lastSpaceIndex = studentName.LastIndexOf(" ");

            string lastName = studentName.Substring(lastSpaceIndex + 1, studentName.Length);

            // the other parts of full name excluding lastname
            string firstName = studentName.Substring(0, lastSpaceIndex);
                        
            // put DateTime value into SQL DATE type value
            // reference: https://forums.asp.net/t/2003945.aspx?Convert+DateTime+in+C+into+Date+in+SQL
            DateTime studentEnrollmentDate = Convert.ToDateTime(txtEnrollmentDate.ToString());
            SqlParameter enrollmentDate = new SqlParameter(@"Date", System.Data.SqlDbType.Date);
            enrollmentDate.Direction = System.Data.ParameterDirection.Input;
            enrollmentDate.Value = studentEnrollmentDate;

        }

        protected void deleteButton_click(object sender, EventArgs e)
        {
            // to prevent exception
            if (Session["StudentID"] != null)
            {
                // delete enrollments first, then delete the student
                SqlCommand deleteEnrollment = new SqlCommand("DELETE FROM Enrollments WHERE StudentID=@StudentID", connection);
                SqlCommand deleteStudent = new SqlCommand("DELETE FROM Students WHERE StudentID=@StudentID", connection);

                deleteEnrollment.Parameters.AddWithValue("@StudentID", Session["StudentID"]);
                deleteStudent.Parameters.AddWithValue("@StudentID", Session["StudentID"]);

                connection.Open();

                deleteEnrollment.ExecuteNonQuery();
                deleteStudent.ExecuteNonQuery();

                connection.Close();

                // redirect to Home page
                Response.Redirect("Home.aspx");
            }
        }



        private void DeleteStudent()
        {
            ClientScriptManager CSM = Page.ClientScript;
            if (true)
            {
                string strconfirm = "<script>if(!window.confirm('Are you sure?')){window.location.href='Default.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            // popup alert
            // reference: https://stackoverflow.com/questions/1920397/how-to-popup-a-alert-on-button-click-from-code-behind-in-asp-net-2-0
            string script = "alert('abc');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
        }



        protected void myRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // use try-finally to ensure connection close
            try
            {
                if (e.CommandName == "deleteCommand")
                {
                    // You can't delete a record with references in other tables, so delete those references first
                    SqlCommand deleteEnrollments = new SqlCommand("DELETE FROM Enrollments WHERE CourseID=@CourseID", connection);
                    SqlCommand deleteCourse = new SqlCommand("DELETE FROM Courses WHERE CourseID=@CourseID", connection);

                    // Parameterize everything, even if the user isn't entering the values
                    deleteEnrollments.Parameters.AddWithValue("@CourseID", e.CommandArgument);
                    deleteCourse.Parameters.AddWithValue("@CourseID", e.CommandArgument);

                    connection.Open(); // open the cmd connection

                    // delete the references FIRST
                    deleteEnrollments.ExecuteNonQuery();
                    deleteCourse.ExecuteNonQuery();
                }
                else if (e.CommandName == "updateCommand")
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Courses SET Title=@UpdatedTitle WHERE Title=@Title", connection);
                    cmd.Parameters.AddWithValue("@Title", e.CommandArgument);
                    cmd.Parameters.AddWithValue("@UpdatedTitle", e.CommandArgument + " - Updated");

                    connection.Open(); // open the cmd connection

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }

            // Re-bind the data with the changed database records
            GetStudents();
        }


        #endregion
    }
}

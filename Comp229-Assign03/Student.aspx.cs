using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Comp229_Assign03
{
    public partial class Student : System.Web.UI.Page
    {
        
        //  4.	Your Student Page will:
        //      a.	collect and display personal data about the selected student as covered 
        //          by the SQL database’s Student table. 
        //      b.	list the selected student’s courses.
        //      i.	Clicking on a course will load that course's Course Page.
        //      c.	include an Update link to the Update Page. 
        //      d.	include parameterized SQL queries for all actions. 
        //      e.	include a delete button to remove the selected student (and redirect to the home page). 
        
         
        // Creating a connection from server string
        private SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Comp229Assign03;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only work when the page is initialized
            if (!IsPostBack)
            {
                GetStudentNames();
                GetStudentCourses();
                studentNamelbl.Text = Session["StudentName"].ToString();
            }
        }

        //p.400 in Textbook
        private void GetStudentNames()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
            {
                conn.Open();
                SqlCommand allStudentNamesComm = new SqlCommand("SELECT StudentID, FirstMidName + ' ' + LastName AS Name FROM Students", conn);
                SqlDataReader reader = allStudentNamesComm.ExecuteReader();
                studentNameList.DataSource = reader;
                //to specify the field that contains the value of each item in a list control. -MSDN
                studentNameList.DataValueField = "StudentID";
                //to specify a field to display as the items of the list in a list control.
                studentNameList.DataTextField = "Name";
                studentNameList.DataBind();
                reader.Close();
                conn.Close();
            }

            Session["StudentID"] = studentNameList.SelectedValue;
            Session["StudentName"] = studentNameList.SelectedItem.Text;
        }

        private void GetStudentCourses()
        {
            int studentID = Convert.ToInt32(Session["StudentID"]);

            if (studentID >= 300000)
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
                {
                    //JOIN tableName USING (columnName) does not work here.
                    //tableName cannnot be replaced by another word. 
                    //  Bad example: SELECT name FROM Student S JOIN Enrollments E ON S.StudentID = E.StudentID;
                    //INNER JOIN table2 ON table1.coumnName = table2.ColumnName WHERE A=B
                    //Reference: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/join-clause
                    SqlCommand coursesComm = new SqlCommand(
                        "SELECT * FROM Courses " +
                        "INNER JOIN Enrollments ON Courses.CourseID = Enrollments.CourseID " +
                        "INNER JOIN Students ON Students.StudentID = Enrollments.StudentID " +
                        "WHERE Students.StudentID = @StudentID;", conn);
                    coursesComm.Parameters.AddWithValue("@StudentID", studentID);
                    conn.Open();
                    SqlDataReader coursesReader = coursesComm.ExecuteReader();
                    studentCoursesRepeater.DataSource = coursesReader;
                    studentCoursesRepeater.DataBind();
                    conn.Close();
                }
        }

        protected void StudentNamesList_Change(object sender, EventArgs e)
        {
            Session["StudentID"] = Convert.ToInt32(studentNameList.SelectedValue);
            Session["StudentName"] = studentNameList.SelectedItem.Text;
            studentNamelbl.Text = Session["StudentName"].ToString();
            GetStudentCourses();
        }

        protected void studentCoursesRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // redirect to Course page
            if (e.CommandName == "linkCourse")
            {
                Session["Course"] = e.CommandArgument.ToString();
                Response.Redirect("Course.aspx");
            }
        }


        #region Buttons

        // Those actions cannot use ItemCommand in Repeater if th
        protected void updateButton_click(object sender, EventArgs e)
        {
            Session["StudentID"] = Convert.ToInt32(studentNameList.SelectedValue);
            Session["StudentName"] = studentNameList.SelectedItem.Text;
            Response.Redirect("Update.aspx");
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
        #endregion
    }
}
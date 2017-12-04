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
    public partial class Student : Page
    {

        //  4.	Your Student Page will:
        //      a.	collect and display personal data about the selected student as covered 
        //          by the SQL database’s Student table. 
        //      b.	list the selected student’s courses.
        //      i.	Clicking on a course will load that course's Course Page.   -> Home page is Course page in this assignment
        //      c.	include an Update link to the Update Page. 
        //      d.	include parameterized SQL queries for all actions. 
        //      e.	include a delete button to remove the selected student (and redirect to the home page). 


        // Creating a connection from server string
        private SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Comp229Assign03;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only work when the page is initialized
            if (!IsPostBack)
            {
                GetStudentNameList();
                GetStudentCourses();
                GetStudentInfo();
            }
        }

        //p.400 in Textbook
        private void GetStudentNameList()
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

            // if there is no value in session studentID
            if (Session["StudentID"] == null)
            {
                Session["StudentID"] = studentNameList.SelectedValue;
                Session["StudentName"] = studentNameList.SelectedItem.Text;
            }
            // if there is any value in session studentID
            else
            {
                bool found = false;
                for (int i = 0; i < studentNameList.Items.Count; i++)
                {
                    studentNameList.SelectedIndex = i;
                    if (Session["StudentID"].ToString() == studentNameList.SelectedValue)
                    {
                        found = true;
                        Session["StudentName"] = studentNameList.SelectedItem.Text;
                        break;
                    }
                }
                // if the session studentID value does not match any value in StudentNameList, set index 0
                if (!found)
                {
                    studentNameList.SelectedIndex = 0;
                    Session["StudentID"] = studentNameList.SelectedValue;
                    Session["StudentName"] = studentNameList.SelectedItem.Text;
                }
            }
        }

        private void GetStudentCourses()
        {
            if (Session["StudentID"] != null)
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
                {
                    int studentID = Convert.ToInt32(Session["StudentID"]);

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

        private void GetStudentInfo()
        {
            Session["StudentID"] = Convert.ToInt32(studentNameList.SelectedValue);
            Session["StudentName"] = studentNameList.SelectedItem.Text;
            lblStudentName.Text = Session["StudentName"].ToString();
            lblStudentID.Text = "Student ID: " + Session["StudentID"];
            string studentID = Session["StudentID"].ToString();
            string enrollmentDate = "";

            // find EnrollmentDate
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
            {
                conn.Open();

                // find enrollment date of student
                // source: https://stackoverflow.com/questions/1555320/store-value-in-a-variable-after-using-select-statement
                SqlCommand getEnrollmentDate = new SqlCommand(
                     "SELECT CONVERT(VARCHAR(10), EnrollmentDate, 101) AS EnrollmentDate FROM Students WHERE StudentID = @StudentID;", conn);
                getEnrollmentDate.Parameters.AddWithValue("@StudentID", studentID);
                enrollmentDate = getEnrollmentDate.ExecuteScalar().ToString();

                conn.Close();
            }
            lblEnrollmentDate.Text = enrollmentDate;
        }


        protected void StudentNamesList_Change(object sender, EventArgs e)
        {
            GetStudentInfo();
            GetStudentCourses();
        }

        protected void StudentCoursesRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // redirect to Course page
            if (e.CommandName == "linkCourse")
            {
                Session["CourseID"] = e.CommandArgument.ToString();
                Response.Redirect("Course.aspx");
            }
        }


        #region Buttons

        // Those actions cannot use ItemCommand in Repeater if th
        protected void UpdateButton_click(object sender, EventArgs e)
        {
            Session["StudentID"] = Convert.ToInt32(studentNameList.SelectedValue);
            Session["StudentName"] = studentNameList.SelectedItem.Text;
            Response.Redirect("Update.aspx");
        }

        protected void DeleteButton_click(object sender, EventArgs e)
        {
            if (Session["StudentID"] != null)
            {
                // delete enrollments first, then delete the student
                SqlCommand deleteEnrollment = new SqlCommand("DELETE FROM Enrollments WHERE StudentID=@StudentID", conn);
                SqlCommand deleteStudent = new SqlCommand("DELETE FROM Students WHERE StudentID=@StudentID", conn);
                deleteEnrollment.Parameters.AddWithValue("@StudentID", Session["StudentID"]);
                deleteStudent.Parameters.AddWithValue("@StudentID", Session["StudentID"]);

                // execute queries
                conn.Open();
                deleteEnrollment.ExecuteNonQuery();
                deleteStudent.ExecuteNonQuery();
                conn.Close();

                // redirect to Home page
                Response.Redirect("Course.aspx");
            }
        }

        #endregion
    }
}
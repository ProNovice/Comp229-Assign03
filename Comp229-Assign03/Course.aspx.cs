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
    public partial class Home : Page
    {
        // This Home page is integrated with Course Page.
        // Because the displayed contents are same.

        //3.	Your Landing Page will: 
        //    a.identify the brand.
        //    b.provide a list of all students’ names from the database.
        //    c.allow for the addition of new students to the database.
        //    d.allow a user to click on a student, loading the Student Page.


        //  5.	The Course Page will: 
        //      a.display all students enrolled in the selected course.
        //      b.allow for the removal and addition of a student to the selected course.

        // Creating a connection from server string
        private SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Comp229Assign03;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only work when the page is initialized
            if (!IsPostBack)
            {
                GetCourseList();
                GetStudents();
                GetCourseInfo();
            }
        }

        //p.400 in Textbook
        private void GetCourseList()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
            {
                conn.Open();
                SqlCommand allStudentNamesComm = new SqlCommand("SELECT * FROM Courses", conn);
                SqlDataReader reader = allStudentNamesComm.ExecuteReader();
                courseList.DataSource = reader;
                //to specify the field that contains the value of each item in a list control. -MSDN
                courseList.DataValueField = "CourseID";
                //to specify a field to display as the items of the list in a list control.
                courseList.DataTextField = "Title";
                courseList.DataBind();

                // also put 'All' item to show all students registered
                ListItem itemForAll = new ListItem("All", "-1");
                // add the item for 'all'
                courseList.Items.Add(itemForAll);
                // close reader
                reader.Close();
                // close connection
                conn.Close();
            }

            // to make selected course item chosen in the list
            // if there is no value in session CourseID or the value is -1
            if (Session["CourseID"] == null || Convert.ToInt32(Session["CourseID"]) == -1)
            {
                Session["CourseID"] = -1;
                Session["Title"] = courseList.SelectedItem.Text;
                courseList.SelectedIndex = courseList.Items.Count - 1;
            }
            // if there is any value in session CourseID
            else
            {
                string courseID = Session["CourseID"].ToString();
                bool found = false;
                for (int i = 0; i < courseList.Items.Count; i++)
                {
                    courseList.SelectedIndex = i;
                    if (courseID == courseList.SelectedValue)
                    {
                        found = true;
                        Session["Title"] = courseList.SelectedItem.Text;
                        break;
                    }
                }
                // if the session studentID value does not match any value in courseList, set index 0
                if (!found)
                {
                    courseList.SelectedIndex = 0;
                    Session["CourseID"] = courseList.SelectedValue;
                    Session["Title"] = courseList.SelectedItem.Text;
                }
            }
        }

        protected void CourseList_Change(object sender, EventArgs e)
        {
            GetCourseInfo();
        }

        private void GetCourseInfo()
        {
            string courseID = courseList.SelectedValue;
            string courseTitle = courseList.SelectedItem.Text;

            Session["CourseID"] = courseID;
            Session["Title"] = courseTitle;

            string credits = "";
            string departmentID = "";

            // prevent exception of 'All'
            if (Convert.ToInt32(courseID) > 4000)
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
                {
                    conn.Open();

                    // input specific values from Database into variables
                    // source: https://stackoverflow.com/questions/1555320/store-value-in-a-variable-after-using-select-statement
                    SqlCommand getCredits = new SqlCommand(
                         "SELECT Credits FROM Courses WHERE CourseID = @CourseID;", conn);
                    getCredits.Parameters.AddWithValue("@CourseID", courseID);
                    credits = "Credits: " + getCredits.ExecuteScalar().ToString();

                    SqlCommand getDepartmentID = new SqlCommand(
                         "SELECT DepartmentID FROM Courses WHERE CourseID = @CourseID;", conn);
                    getDepartmentID.Parameters.AddWithValue("@CourseID", courseID);
                    departmentID = "Department ID: " + getDepartmentID.ExecuteScalar().ToString();

                    conn.Close();
                }

            courselbl.Text = courseTitle;
            creditlbl.Text = credits;
            departmentIDlbl.Text = departmentID;
            GetStudents();
        }


        private void GetStudents()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
            {
                // declare SqlCommand to execute
                SqlCommand studentsComm;

                // if there is no value in session CourseID or the value is -1 to show all stuents
                if (Session["CourseID"] == null || Convert.ToInt32(Session["CourseID"]) == -1)
                {
                    studentsComm = new SqlCommand(
                    "SELECT *, FirstMidName + ' ' + LastName AS Name, CONVERT(VARCHAR(10), EnrollmentDate, 110) AS Date FROM Students", conn);
                }
                // when there is any selected course
                else
                {
                    int courseID = Convert.ToInt32(Session["CourseID"]);

                    // declare SqlCommand to execute
                    studentsComm = new SqlCommand(
                    "SELECT *, FirstMidName + ' ' + LastName AS Name, CONVERT(VARCHAR(10), EnrollmentDate, 110) AS Date FROM Students " +
                    "INNER JOIN Enrollments ON Students.StudentID = Enrollments.StudentID " +
                    "INNER JOIN Courses ON Courses.CourseID = Enrollments.CourseID " +
                    "WHERE Courses.CourseID = @CourseID;", conn);

                    studentsComm.Parameters.AddWithValue("@CourseID", courseID);
                }

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
                // Delete an enrollment
                SqlCommand deleteEnrollments = new SqlCommand("DELETE FROM Enrollments WHERE StudentID=@StudentID", connection);

                // parameterize values in SqlCommands
                deleteEnrollments.Parameters.AddWithValue("@StudentID", e.CommandArgument);

                connection.Open(); // open the connection

                // delete enrollments first, then delete the student to prevent a order conflict
                deleteEnrollments.ExecuteNonQuery();

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

            int courseID = -1;

            // if there is selected course
            if (Session["CourseID"] != null)
                courseID = Convert.ToInt32(Session["CourseID"]);

            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string enrollmentDate = inputDate.Value;

            SqlCommand insertStudent;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
            {
                // open connection between Database and ASP.NET
                conn.Open();

                // when there is no selected course, just add a student without any course
                if (courseID == -1)
                {
                    // insert new student
                    insertStudent = new SqlCommand(
                        "INSERT INTO Students (FirstMidName, LastName, EnrollmentDate) " +
                        "VALUES (@FirstName, @LastName, @EnrollmentDate);", conn);
                    insertStudent.Parameters.AddWithValue("@FirstName", firstName);
                    insertStudent.Parameters.AddWithValue("@LastName", lastName);
                    insertStudent.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

                    insertStudent.ExecuteNonQuery();
                }
                else if (courseID >= 4000)   // all courseID is higher than 4000
                {
                    // insert a new student
                    insertStudent = new SqlCommand(
                        "INSERT INTO Students (FirstMidName, LastName, EnrollmentDate) " +
                        "VALUES (@FirstName, @LastName, @EnrollmentDate);", conn);
                    insertStudent.Parameters.AddWithValue("@FirstName", firstName);
                    insertStudent.Parameters.AddWithValue("@LastName", lastName);
                    insertStudent.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);
                    insertStudent.ExecuteNonQuery();

                    // to find a new student's index
                    // source: https://stackoverflow.com/questions/1555320/store-value-in-a-variable-after-using-select-statement
                    SqlCommand findLastStudentIDComm = new SqlCommand(
                        "SELECT MAX(StudentID) AS StudentID FROM Students;", conn);

                    int newStudentID = Convert.ToInt32(findLastStudentIDComm.ExecuteScalar().ToString());

                    // insert a new enrollment
                    SqlCommand insertEnrollment = new SqlCommand(
                        "INSERT INTO Enrollments (CourseID, StudentID, Grade) " +
                        "VALUES (@CourseID, @StudentID, @Grade);", conn);
                    insertEnrollment.Parameters.AddWithValue("@CourseID", courseID);
                    insertEnrollment.Parameters.AddWithValue("@StudentID", newStudentID);
                    insertEnrollment.Parameters.AddWithValue("@Grade", 0);  // because Grade value cannot be null in Enrollments table 
                    insertEnrollment.ExecuteNonQuery();

                }

                // close the connection
                conn.Close();
                // refresh
                GetStudents();
            }

        }

    }
}

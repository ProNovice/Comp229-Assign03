using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Comp229_Assign03
{
    public partial class Update : Page
    {

        //  6.	The Update Page will:
        //    a.load already containing the selected student's data.
        //    b.allow for changing any data fields.
        //    c.update changed fields in the database.

        private SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Comp229Assign03;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only work when the page is initialized
            if (!IsPostBack)
            {
                GetStudentNameList();
                GetStudentCourses();
                GetStudentInfo();
                GetNewCourseList();
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
            string enrollmentDate = "4";

            // find EnrollmentDate
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
            {
                conn.Open();

                // find enrollment date of student
                // source: https://stackoverflow.com/questions/1555320/store-value-in-a-variable-after-using-select-statement
                SqlCommand getEnrollmentDate = new SqlCommand(
                     "SELECT CONVERT(VARCHAR(10), EnrollmentDate, 120) AS EnrollmentDate FROM Students WHERE StudentID = @StudentID;", conn);
                getEnrollmentDate.Parameters.AddWithValue("@StudentID", studentID);
                enrollmentDate = getEnrollmentDate.ExecuteScalar().ToString();

                conn.Close();
            }
            inputDate.Value = enrollmentDate;
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
            else if (e.CommandName == "updateCourse")
            {
            }
            else if (e.CommandName == "deleteCourse")
            {
                int enrollmentID = Convert.ToInt32(e.CommandArgument.ToString());
                int studentID = Convert.ToInt32(Session["StudentID"]);  // if the page was loaded well, it means there is a value in Session["StudentID"]

                // delete enrollments first, then delete the student
                SqlCommand deleteEnrollment = new SqlCommand("DELETE FROM Enrollments WHERE EnrollmentID = @EnrollmentID", conn);
                deleteEnrollment.Parameters.AddWithValue("EnrollmentID", enrollmentID);
                // execute query
                conn.Open();
                deleteEnrollment.ExecuteNonQuery();
                conn.Close();
                // refresh
                GetStudentCourses();
            }
        }

        #region Add new course

        private void GetNewCourseList()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString))
            {
                // open connection
                conn.Open();
                // Display all Course Titles
                SqlCommand allStudentNamesComm = new SqlCommand("SELECT * FROM Courses", conn);
                SqlDataReader reader = allStudentNamesComm.ExecuteReader();
                newCourseList.DataSource = reader;
                newCourseList.DataValueField = "CourseID";
                newCourseList.DataTextField = "Title";
                newCourseList.DataBind();
                // close reader
                reader.Close();
                // close connection
                conn.Close();
            }

        }

        protected void AddCourseButton_click(object sender, EventArgs e)
        {
            string courseID = newCourseList.SelectedValue;
            string studentID = Session["StudentID"].ToString();
            int grade = Convert.ToInt32(numNewGrade.Value);

            conn.Open();
            // check if there is same course in the student's enrollments
            SqlCommand findSameCourse = new SqlCommand(
                "SELECT CourseID FROM Enrollments WHERE StudentID = @StudentID AND CourseID = @CourseID", conn);
            findSameCourse.Parameters.AddWithValue("@CourseID", courseID);
            findSameCourse.Parameters.AddWithValue("@StudentID", studentID);
            SqlDataReader sameCourseReader = findSameCourse.ExecuteReader();

            string foundCourse = null;
            if(sameCourseReader.Read())
                foundCourse = Convert.ToString(sameCourseReader[0]);
            sameCourseReader.Close();

            if (foundCourse == null)
            {
                // insert a new enrollment
                SqlCommand insertEnrollment = new SqlCommand(
                    "INSERT INTO Enrollments (CourseID, StudentID, Grade) " +
                    "VALUES (@CourseID, @StudentID, @Grade);", conn);
                insertEnrollment.Parameters.AddWithValue("@CourseID", courseID);
                insertEnrollment.Parameters.AddWithValue("@StudentID", studentID);
                insertEnrollment.Parameters.AddWithValue("@Grade", grade);  // because Grade value cannot be null in Enrollments table 
                insertEnrollment.ExecuteNonQuery();

                lblNewCourseError.Visible = false;
            }
            else
            {
                lblNewCourseError.Visible = true;
            }

            conn.Close();

            GetStudentCourses();
        }

        #endregion
        
        #region Buttons & OnChanged


        protected void Grade_OnChanged(object sender, EventArgs s)
        {
            RepeaterItem item = (sender as TextBox).Parent as RepeaterItem;
            // .Text is always 'Value' in the Tag 
            string grade = (item.FindControl("txtGrade") as TextBox).Text;

            // identify if string grade consists of only number
            // source: https://stackoverflow.com/questions/894263/how-do-i-identify-if-a-string-is-a-number
            int n;
            bool isNumeric = int.TryParse(grade, out n);
            // prevent Error because of not pure numeric value
            if (!isNumeric)
            {
                // remove all not number characters
                Regex rgx = new Regex("[^0-9]");
                grade = rgx.Replace(grade, "");
            }
            (item.FindControl("txtGrade") as TextBox).Text = grade;
        }

        // Important function
        // find item in itemTemplate through FindControl
        protected void UpdateGradeButton_click(object sender, EventArgs e)
        {
            string grade;
            string enrollmentID;
            // find item's ID by using RepeaterItem, FindControl("original ID in itemTemplate");
            // source: https://www.aspsnippets.com/Articles/ASPNet-Repeater-CRUD-Select-Insert-Edit-Update-and-Delete-in-Repeater-using-C-and-VBNet.aspx
            RepeaterItem item = (sender as Button).Parent as RepeaterItem;
            // .Text is always 'Value' in the Tag 
            grade = (item.FindControl("txtGrade") as TextBox).Text;
            enrollmentID = (item.FindControl("lblEnrollmentID") as Label).Text;

            // identify if string grade consists of only number
            // source: https://stackoverflow.com/questions/894263/how-do-i-identify-if-a-string-is-a-number
            int n;
            bool isNumeric = int.TryParse(grade, out n);
            // prevent Error because of not pure numeric value
            if (isNumeric)
            {
                // execute queries
                conn.Open();
                SqlCommand updateGrade = new SqlCommand(
                    "UPDATE Enrollments SET Grade = @Grade WHERE EnrollmentID = @EnrollmentID;", conn);
                updateGrade.Parameters.AddWithValue("@Grade", grade);
                updateGrade.Parameters.AddWithValue("@EnrollmentID", enrollmentID);
                updateGrade.ExecuteNonQuery();
                conn.Close();
                lblStudentName.Text = grade + enrollmentID;
            }

        }

        // Those actions cannot use ItemCommand in Repeater if th
        protected void UpdateButton_click(object sender, EventArgs e)
        {
            // to prevent exception
            if (Session["StudentID"] != null)
            {
                string studentID = Session["StudentID"].ToString();
                string enrollmentDate = inputDate.Value;
                SqlCommand updateStudent = new SqlCommand(
                    "UPDATE Students SET EnrollmentDate = @EnrollmentDate WHERE StudentID = @StudentID", conn);
                updateStudent.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);
                updateStudent.Parameters.AddWithValue("@StudentID", studentID);

                // execute query
                conn.Open();
                updateStudent.ExecuteNonQuery();
                conn.Close();
            }
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
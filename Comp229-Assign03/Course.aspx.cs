using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Comp229_Assign03
{
    public partial class Course : System.Web.UI.Page
    {
        //  5.	The Course Page will: 
        //      a.display all students enrolled in the selected course.
        //      b.allow for the removal and addition of a student to the selected course.



        private SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Comp229Assign03;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");



        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void addButton_click(object sender, EventHandler e)
        {


        }
        protected void removeButton_click(object sender, EventHandler e)
        {


        }
    }
}
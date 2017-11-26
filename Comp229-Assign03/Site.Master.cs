﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Comp229_Assign03
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath);
            Page.Title = "Lawrence College :: " + pageName;
        }
    }
}
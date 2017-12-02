<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="Comp229_Assign03.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2><strong>Course Information</strong></h2>
        <p>
            <label>Course:&nbsp; </label>
            <!--AutoPostBack has to be "true" -->
            <asp:DropDownList ID="courseList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CourseList_Change">
            </asp:DropDownList>
        </p>
        <asp:Label runat="server" Text="Please select a student to see their courses.&nbsp;" />
        <div class="col-md-8" style="margin-right: auto; margin-left: 25%">
            <div class="container table-bordered">
                <h3><strong>
                    <asp:Label runat="server" ID="courselbl" Text="Course" />
                </strong></h3>
                <div class="row">
                    <div class="col-md-4">
                        <h4><strong>
                            <asp:Label runat="server" ID="creditlbl" Text="Credit" />
                        </strong></h4>
                    </div>
                    <div class="col-md-4">
                        <h4><strong>
                            <asp:Label runat="server" ID="departmentIDlbl" Text="DepartmentID" />
                        </strong></h4>
                    </div>
                </div>
                <p>To see courses of a student, Please click the student name in the list.</p>
            </div>
            <!-- Only ItemTemplate can do data binding and < %# Eval("") %>. They don't work in HeaderTemplate and FooterTemplaye -->
            <!-- https://stackoverflow.com/questions/1470472/net-repeater-headertemplate -->
            <table class="table table-striped courseTable align-center">
                <thead>
                    <tr>
                        <th class="col-md-3">Student ID</th>
                        <th class="col-md-4">Name</th>
                        <th class="col-md-3">Enrollment Date</th>
                        <th class="col-md-2"></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="studentsRepeater" runat="server" OnItemCommand="studentsRepeater_ItemCommand">
                        <ItemTemplate>
                            <!-- repeat students data-->
                            <tr class="text-center">
                                <td>
                                    <asp:LinkButton runat="server" CommandName="linkStudent" CssClass="commonText"
                                        CommandArgument='<%# Eval("StudentID") %>'><%# Eval("StudentID") %></asp:LinkButton></td>
                                <td>
                                    <asp:LinkButton runat="server" CommandName="linkStudent" CssClass="commonText"
                                        CommandArgument='<%# Eval("StudentID") %>'><%# Eval("Name") %></asp:LinkButton></td>
                                <td><%# Eval("Date") %></td>
                                <td>
                                    <asp:Button runat="server" CommandName="deleteStudent" CommandArgument='<%# Eval("StudentID") %>' CssClass="btn" Text="Delete" /></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
            </table>
            <!-- A textbox and a button for adding a new student -->
            <div class="text-center">
                <div id="divNewStudent" class="panel panel-default col-md-8 align-center" style="margin-left: 15%;">
                    <table class="align-center">
                        <thead>
                            <tr>
                                <th colspan="2" class="text-center table">
                                    <br />
                                    New Student Info<br />
                                    <br />
                                </th>
                            </tr>
                        </thead>
                        <tr>
                            <td>First Name:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RequiredFieldValidator ValidationGroup="groupNewStudent" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name is required" ForeColor="Red"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>Last Name:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLastName"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RequiredFieldValidator ValidationGroup="groupNewStudent" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last name is required" ForeColor="Red"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>Enrollment Date: </td>
                            <td>
                                <input runat="server" type="date" id="inputDate" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RequiredFieldValidator ValidationGroup="groupNewStudent" runat="server" ControlToValidate="inputDate" ErrorMessage="Date is required" ForeColor="Red"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="text-center" colspan="2">
                                <asp:Button ID="AddStudent" ValidationGroup="groupNewStudent" CssClass="btn align-center" runat="server" Text="Add new Student" OnClick="studentAddButton_click" /></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

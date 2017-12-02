<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Student.aspx.cs" Inherits="Comp229_Assign03.Student" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2><strong>Student Information</strong></h2>
        <p>
            <label>Student:&nbsp; </label>
            <!--AutoPostBack has to be "true" -->
            <asp:DropDownList ID="studentNameList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="StudentNamesList_Change">
            </asp:DropDownList>
        </p>
        <asp:Label runat="server" Text="Please select a student to see their courses.&nbsp;" />
        <div class="col-md-8" style="margin-right: auto; margin-left: 25%">
            <div class="container table-bordered">
                <h3><strong>
                    <asp:Label runat="server" ID="lblStudentName" Text="Student" />
                </strong></h3>
                <div class="row">
                    <div class="col-md-4">
                        <h4><strong>
                            <asp:Label runat="server" ID="lblStudentID" Text="StudentID" />
                        </strong></h4>
                    </div>
                    <div class="col-md-6">
                        <h4><strong>
                            <asp:Label runat="server" Text="Enrollment Date: " />
                            <asp:Label runat="server" ID="lblEnrollmentDate" Text="EnrollmentDate" />
                        </strong></h4>
                    </div>
                </div>
                <p>To see information of a course, please click the course name in the list.</p>
                To edit the student information, please click the Update button.
                <div style="float: right">
                    <asp:Button CssClass="btn" ID="studentAddBtn" runat="server" Text="Go to Update" OnClick="UpdateButton_click" />|
                    <asp:Button CssClass="btn" ID="studentDeleteBtn" runat="server" Text="Delete" OnClick="DeleteButton_click" />
                </div>
                <br />
                <br />
            </div>
            <asp:Repeater ID="studentCoursesRepeater" runat="server" OnItemCommand="StudentCoursesRepeater_ItemCommand">
                <HeaderTemplate>
                    <table class="table table-striped courseTable align-center">
                        <thead>
                            <tr>
                                <th class="col-md-2">Course ID</th>
                                <th class="col-md-4">Title</th>
                                <th class="col-md-2">Credits</th>
                                <th class="col-md-2">Grade</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="text-center">
                        <td>
                            <asp:LinkButton runat="server" CommandName="linkCourse" CssClass="commonText"
                                CommandArgument='<%# Eval("CourseID") %>'><%# Eval("CourseID") %></asp:LinkButton></td>
                        <td class="text-left">
                            <asp:LinkButton runat="server" CommandName="linkCourse" CssClass="commonText"
                                CommandArgument='<%# Eval("CourseID") %>'><%# Eval("Title") %></asp:LinkButton></td>
                        <td><%# Eval("Credits") %></td>
                        <td><%# Eval("Grade") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>

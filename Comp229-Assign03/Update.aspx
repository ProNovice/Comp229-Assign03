<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Comp229_Assign03.Update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2><strong>Update Student Info</strong></h2>
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
                    <div class="col-md-8">
                        <h4><strong>
                            <asp:Label runat="server" Text="Enrollment Date: " />
                            <!-- input date source: https://www.w3schools.com/jsref/dom_obj_date.asp -->
                            <input runat="server" type="date" id="inputDate" />
                        </strong></h4>
                    </div>
                </div>
                <div style="float: right">
                    <asp:Button CssClass="btn" ID="studentAddBtn" runat="server" Text="Update" OnClick="UpdateButton_click" />|
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
                                <th class="col-md-1">Credits</th>
                                <th class="col-md-1">Grade</th>
                                <th class="col-md-4"></th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="text-center">
                        <td>
                            <asp:LinkButton runat="server" CommandName="linkCourse" CssClass="commonText"
                                CommandArgument='<%# Eval("CourseID") %>' Text='<%# Eval("CourseID") %>'></asp:LinkButton></td>
                        <td class="text-left">
                            <asp:LinkButton runat="server" CommandName="linkCourse" CssClass="commonText"
                                CommandArgument='<%# Eval("CourseID") %>'><%# Eval("Title") %></asp:LinkButton></td>
                        <td><%# Eval("Credits") %></td>
                        <td>
                            <!-- <input type="number"> cannot be used in repeater. but text type works well -->
                            <asp:TextBox OnTextChanged="Grade_OnChanged" runat="server" Columns="3" ID="txtGrade" value='<%# Eval("Grade") %>'></asp:TextBox>
                            <!-- This element is for getting EnrollmentID -->
                            <asp:Label ID="lblEnrollmentID" runat="server" Text='<%# Eval("EnrollmentID") %>' Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:Button runat="server" OnClick="UpdateGradeButton_click" CssClass="btn" Text="Update" />
                            |                            
                            <asp:Button runat="server" CommandName="deleteCourse" CommandArgument='<%# Eval("EnrollmentID") %>' CssClass="btn" Text="Delete" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="panel panel-default col-md-8" style="margin-left: 15%; padding: 50px;">
                <h4 class="text-center"><strong>Add new course</strong></h4>
                <br />
                <div class="row">
                    <div class="col-md-4">
                        New Course: 
                    </div>
                    <div class="col-md-8">
                        <asp:DropDownList ID="newCourseList" runat="server" AutoPostBack="true"></asp:DropDownList>
                        <asp:Label runat="server" ID="lblNewCourseError" Visible="false" ForeColor="Red" Text="Same course is already enrolled"></asp:Label>
                    </div>
                </div>
                <p />
                <div class="row">
                    <div class="col-md-4">Grade:</div>
                    <div class="col-md-8">
                        <input runat="server" id="numNewGrade" type="number" value="0" style="width: 40px;" />
                    </div>
                    <br />
                    <br />
                    <div class="col-md-12 text-center">
                        <asp:Button runat="server" OnClick="AddCourseButton_click" CssClass="btn" Text="Add" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

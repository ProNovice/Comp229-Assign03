<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Comp229_Assign03.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div>
            <h2><strong>Student List</strong></h2>
            <p>To see courses of a student, Please click the student name in the list.</p>
        </div>
        <div class="col-md-8" style="margin-right: auto; margin-left: 25%">
            <div class="row">
                <div class="col-md-6">
                    <h4><strong>
                        <asp:Label runat="server" ID="studentNamelbl" Text="Name" />
                    </strong></h4>
                </div>
                <div style="float: right">
                    <asp:Button ID="studentUpdateBtn" runat="server" Text="Update" OnClick="updateButton_click" />
                    |                  
            <asp:Button ID="studentDeleteBtn" runat="server" Text="Delete" OnClick="deleteButton_click" />
                </div>
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
                            <tr class="text-center">
                                <td>
                                    <asp:LinkButton runat="server" CommandName="linkStudent" CssClass="commonText"
                                        CommandArgument='<%# Eval("StudentID") %>'><%# Eval("StudentID") %></asp:LinkButton><%# Eval("StudentID") %></td>
                                <td>
                                    <asp:LinkButton runat="server" CommandName="linkStudent" CssClass="commonText"
                                        CommandArgument='<%# Eval("StudentID") %>'><%# Eval("Name") %></asp:LinkButton></td>
                                <td><%# Eval("EnrollmentDate") %></td>
                                <td>
                                    <asp:Button runat="server" CommandName="DeleteStudent" CommandArgument="<%# Eval("StudentID") %>" CssClass="btn" Text="Delete" /></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="txtStudentID" Enabled="true" Columns="15"></asp:TextBox></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtStudentName" Columns="20"></asp:TextBox></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEnrollmentDate" Columns="15"></asp:TextBox></td>
                        <td>
                            <asp:Button ID="AddStudent" CssClass="btn" runat="server" Text="Add" OnClick="studentAddButton_click" /></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>

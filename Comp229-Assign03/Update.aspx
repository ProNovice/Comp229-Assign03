<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Comp229_Assign03.Update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:Label runat="server" Text="Please select a student to see their courses.&nbsp;" />
        <p>
            <label>Student:&nbsp; </label>
            <!--AutoPostBack has to be "true" -->
            <asp:DropDownList ID="studentNameList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="StudentNamesList_Change">
            </asp:DropDownList>
        </p>
        <div class="col-md-8" style="margin-right: auto; margin-left: 25%">
            <div class="row">
                <div class="col-md-6">
                    <h4><strong>
                        <asp:Label runat="server" ID="studentNamelbl" Text="Name" />
                    </strong></h4>
                </div>
                <div style="float: right">
                    <asp:Button CssClass="btn" ID="studentAddBtn" runat="server" Text="Update" OnClick="updateButton_click" />|
                    <asp:Button CssClass="btn" ID="studentDeleteBtn" runat="server" Text="Delete" OnClick="deleteButton_click" />
                </div>
            </div>
            <asp:Repeater ID="studentCoursesRepeater" runat="server" OnItemCommand="studentCoursesRepeater_ItemCommand">
                <HeaderTemplate>
                    <table class="table table-striped courseTable align-center">
                        <thead>
                            <tr>
                                <th class="col-md-2">Course ID</th>
                                <th class="col-md-4">Title</th>
                                <th class="col-md-2">Credits</th>
                                <th class="col-md-2">Grade</th>
                                <th class="col-md-2"></th>
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
                        <td>
                            <asp:Button runat="server" CommandName="UpdateEnrollment" CommandArgument='<%# Eval("EnrollmentID") %>' CssClass="btn" Text="Update" />
                            |                            
                            <asp:Button runat="server" CommandName="DeleteEnrollment" CommandArgument='<%# Eval("EnrollmentID") %>' CssClass="btn" Text="Delete" />
                        </td>
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

﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Student.aspx.cs" Inherits="Comp229_Assign03.Student" %>

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
                    <asp:Button ID="studentUpdateBtn" runat="server" Text="Update" OnClick="updateButton_click" />
                    |                  
            <asp:Button ID="studentDeleteBtn" runat="server" Text="Delete" OnClick="deleteButton_click" />
                </div>
            </div>
            <!-- Only ItemTemplate can do data binding and < %# Eval("") %>. They don't work in HeaderTemplate and FooterTemplaye -->
            <!-- https://stackoverflow.com/questions/1470472/net-repeater-headertemplate -->
            <asp:Repeater ID="studentCoursesRepeater" runat="server" OnItemCommand="studentCoursesRepeater_ItemCommand">
                <HeaderTemplate>
                    <table class="table table-striped courseTable align-center">
                        <thead>
                            <tr>
                                <th class="col-md-4">Title</th>
                                <th class="col-md-2">Credits</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" CommandName="linkCourse" CssClass="commonText" CommandArgument='<%# Eval("CourseID") %>'><%# Eval("Title") %></asp:LinkButton></td>
                        <td><%# Eval("Credits") %></td>
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
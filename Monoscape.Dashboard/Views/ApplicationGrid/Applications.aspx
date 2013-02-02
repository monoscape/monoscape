<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Monoscape.Common.Model.Application>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Applications")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Applications</h2>
    
    <% if (Model.Count == 0)
       { %>
       <p>There are no applications deployed in the Application Grid.</p>
    <% } %>
    <% else
        { %>
        <p>The following applications were found in the Application Grid.</p>
        <table>
            <tr>
                <th>
                    Application ID
                </th>
                <th>
                    Name
                </th>
                <th>
                    Version
                </th>
                <th>
                    State
                </th>
                <th>
                    File Name
                </th>
                <th></th>
            </tr>

        <% foreach (var item in Model) { %>
    
            <tr>
                <td>
                    <%= Html.Encode(item.Id) %>
                </td>
                <td>
                    <%= Html.ActionLink(Html.Encode(item.Name), "ApplicationInfo", new { applicationId = item.Id })%>
                    
                </td>
                <td>
                    <%= Html.Encode(item.Version) %>
                </td>
                <td>
                    <%= Html.Encode(item.State) %>
                </td>
                <td>
                    <%= Html.Encode(item.FileName) %>
                </td>
                <td>
                    <%= Html.ActionLink("Start", "startapplication", new { id = item.Id })%> | <%= Html.ActionLink("Instances", "applicationinstances", new { id = item.Id })%> | <%= Html.ActionLink("Remove", "removeapplication", new { applicationId = item.Id })%> 
                </td>
            </tr>    
        <% } %>
        </table>
    <% } %>
</asp:Content>


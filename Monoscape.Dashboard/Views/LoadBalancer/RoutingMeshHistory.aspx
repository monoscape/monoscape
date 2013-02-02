<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Monoscape.Common.Model.ApplicationInstance>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Routing Mesh History")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Routing Mesh History</h2>
    <% if (Model.Count == 0)
       { %>
       <p>There are no routing mesh history records found.</p>
    <% } %>
    <% else
        { %>
        <p>The following routing mesh history records found:</p>
    <table>
        <tr>
            <th>Instance ID</th>
            <th>
                Created Time
            </th>
            <th>
                Application Name
            </th>
            <th>
                NodeId
            </th>
            <th>
                Url
            </th>
            <th>State
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td><%= Html.Encode(item.Id) %></td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.CreatedTime)) %>
            </td>
            <td>
                <%= Html.Encode(item.ApplicationName) %>
            </td>
            <td>
                <%= Html.Encode(item.NodeId) %>
            </td>
            <td>
                <%= Html.Encode(item.Url) %>
            </td>
            <td><%= Html.Encode(item.State) %></td>
        </tr>
    
    <% } %>

    </table>
    <% } %>
</asp:Content>


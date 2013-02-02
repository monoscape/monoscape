<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Monoscape.Common.Model.Node>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Nodes")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Application Grid Nodes</h2>
    <% if (Model.Count == 0)
       { %>
       <p>There are no nodes subscribed with the Application Grid.</p>
    <% } %>
    <% else
        { %>
        <p>The following nodes are subscribed with the Application Grid.</p>

    <table>
        <tr>
            <th>
                Node ID
            </th>
            <th>
                Instance ID
            </th>
            <th>
                IP Address
            </th>
            <th>
                Subscribed On
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.Id) %>
            </td>
            <td>
                <%= Html.Encode(item.InstanceId) %>
            </td>           
            <td>
                <%= Html.Encode(item.IpAddress) %>
            </td>            
            <td>
                <%= Html.Encode(item.SubscribedOn) %>
            </td>
        </tr>
    
    <% } %>

    </table>
    <% } %>
</asp:Content>


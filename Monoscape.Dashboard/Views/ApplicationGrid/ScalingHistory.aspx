<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Monoscape.Common.Model.ScalingHistoryItem>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Scaling History
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="LeftMenuContent" runat="server">
<% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Scaling History</h2>
    <p>Following scaling history records were found:</p>
    <table>
        <tr>
            <th>
                Time
            </th>
            <th>
                Application ID
            </th>
            <th>
                Tenant ID
            </th>
            <th>
                Request Count
            </th>
            <th>
                Scale
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.Time) %>
            </td>
            <td>
                <%= Html.Encode(item.ApplicationId) %>
            </td>
            <td>
                <%= Html.Encode(item.TenantId) %>
            </td>
            <td>
                <%= Html.Encode(item.RequestCount) %>
            </td>
            <td>
                <%= Html.Encode(item.Scale) %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>


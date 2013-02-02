<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Monoscape.Common.Model.Subscription>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Subscriptions")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Subscriptions</h2>

    <% if (Model.Count == 0)
       { %>
       <p>Currently there are no subscriptions defined.</p>
    <% } %>
    <% else
        { %>
        <p>The following subscriptions were found in the cloud controller:</p>
        <table>
            <tr>                
                <th>
                    Access Key
                </th>
                <th>
                    Secret Key
                </th>
                <th>
                    Type
                </th>
                <th>
                    State
                </th>
                <th>
                    Created Date
                </th>
                <th></th>
            </tr>

        <% foreach (var item in Model) { %>
    
            <tr>                
                <td>
                    <%= Html.ActionLink(item.AccessKey, "subscription", new { subscriptionId = item.Id })%>
                </td>
                <td>
                    <%= Html.Encode(item.SecretKey) %>
                </td>
                <td>
                    <%= Html.Encode(item.Type) %>
                </td>
                <td>
                    <%= Html.Encode(item.State) %>
                </td>
                <td>
                    <%= Html.Encode(String.Format("{0:g}", item.CreatedDate)) %>
                </td>
                <td>
                    <%= Html.ActionLink("Remove", "RemoveSubscription", new { subscriptionId = item.Id })%>
                </td>
            </tr>
    
        <% } %>

        </table>
    <% } %>
</asp:Content>


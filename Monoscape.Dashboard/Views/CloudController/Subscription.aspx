<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Monoscape.Common.Model.Subscription>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Subscription
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Subscription Information</h2>

    <fieldset>
        
        <div class="display-label">Type: <%= Html.Encode(Model.Type) %></div>
                
        <div class="display-label">AccessKey: <%= Html.Encode(Model.AccessKey) %></div>
                
        <div class="display-label">SecretKey: <%= Html.Encode(Model.SecretKey) %></div>
                
        <div class="display-label">State: <%= Html.Encode(Model.State) %></div>
                
        <div class="display-label">CreatedDate: <%= Html.Encode(String.Format("{0:g}", Model.CreatedDate)) %></div>        
                
    </fieldset>

    <% if ((Model.Items == null) || (Model.Items.Count == 0))
       { %>
       <p>Currently this subscription has no applications granted.</p>
    <% } %>
    <% else
        { %>
        <p>The following applications are granted to this subscription:</p>
    <table>
        <tr>
            <th>ID</th>
            <th>
                Application
            </th>
        </tr>

    <% foreach (var item in Model.Items) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.Id) %>
            </td>
            <td>
                <%= Html.Encode(item.Application.Name) %>
            </td>
        </tr>
    
    <% } %>

    </table>
     <% } %>

</asp:Content>


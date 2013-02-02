<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Monoscape.Common.Model.Application>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle(Html.Encode(Model.Name))%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.Encode(Model.Name) %></h2>
    <p>Application information:</p>
    <fieldset>            
    
        <%= Html.HiddenFor(model => model.Id) %> 
        
        <div class="display-label">Name: <%= Html.Encode(Model.Name) %></div>
        <div class="display-label">Version: <%= Html.Encode(Model.Version) %></div>                
        <div class="display-label">FileName: <%= Html.Encode(Model.FileName) %></div>        
        
    </fieldset>

    <h3>Tenants</h3>
    <% if (Model.Tenants.Count == 0)
       { %>
       <p>There are no tenants defined for this application, the runtime default will be used.</p>
    <% } %>
    <% else
        { %>
        <p>The following tenants are defined for this application:</p>
        <table>
            <tr>
                <th>
                    Name
                </th>
                <th>
                    Upper Scale Limit
                </th>
                <th>
					Scaling Factor
                </th>
            </tr>

        <% foreach (var item in Model.Tenants) { %>    
            <tr>
                <td>
                    <%= Html.Encode(item.Name) %>
                </td>
                <td style="text-align: center;">
                    <%= Html.Encode(item.UpperScaleLimit) %>
                </td>
                <td style="text-align: center;">
                    <%= Html.Encode(item.ScalingFactor) %>
                </td>
            </tr>    
        <% } %>
        </table>
    <% } %>

    <br />
    <%= Html.ActionLink("Add Tenant", "AddApplicationTenant", new { applicationId = Model.Id })%>

</asp:Content>



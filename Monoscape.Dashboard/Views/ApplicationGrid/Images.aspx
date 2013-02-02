<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Monoscape.ApplicationGridController.Model.Image>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Virtual Machine Images")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Virtual Machine Images</h2>

    <% if (Model.Count == 0)
       { %>
       <p>There are no virtual images found in the IaaS.</p>
    <% } %>
    <% else
        { %>
        <p>The following virtual machine images were found in the IaaS.</p>
    <table>
        <tr>
            <th>Image ID</th>
            <th>Name</th>
            <th>State</th>
            <th></th>
        </tr>

    <% foreach (var item in Model)
       { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.ImageId)%>
            </td>
            <td>
                <%= Html.Encode(item.Name)%>
            </td>
            <td>
                <%= Html.Encode(item.State)%>
            </td>            
            <td>
                <%= Html.ActionLink("Remove", "DeregisterImage", new { imageId = item.ImageId })%> | 
                <%= Html.ActionLink("Start Instance", "RunInstance", new { imageId = item.ImageId }) %>
            </td>
        </tr>
    
    <% } %>
    </table>    
    <% } %>
</asp:Content>


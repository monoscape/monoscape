<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Monoscape.ApplicationGridController.Model.Instance>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Virtual Machine Instances")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Virtual Machine Instances</h2>
	
    <% if (Model.Count == 0)
       { %>
       <p>There are no virtual machine instances running.</p>
    <% } %>
    <% else
        { %>
        <p>The following instances were found in the IaaS.</p>
    <table>
        <tr>
            <th>
                Image ID
            </th>
            <th>
                Instance ID
            </th>
            <th>
                Private DNS Name
            </th>
            <th>
                IP Address
            </th>
            <th>
                Type
            </th>
            <th>
                State
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.ImageId) %>
            </td>
            <td>
                <%= Html.Encode(item.InstanceId) %>
            </td>
            <td>
                <%= Html.Encode(item.PrivateDnsName) %>
            </td>
            <td>
                <%= Html.Encode(item.IpAddress) %>
            </td>
            <td>
                <%= Html.Encode(item.Type) %>
            </td>
            <td>
                <%= Html.Encode(item.State) %>
            </td>
            <td>
                <%= Html.ActionLink("Console", "ConsoleOutput", new { instanceId = item.InstanceId })%> | 
                <%= Html.ActionLink("Assign Public IP", "AssignPublicIp", new { instanceId = item.InstanceId })%> <br />
                <%= Html.ActionLink("Reboot", "RebootInstance", new { instanceId = item.InstanceId })%> | 
                <%= Html.ActionLink("Terminate", "TerminateInstance", new { instanceId = item.InstanceId })%>
            </td>            
        </tr>
    
    <% } %>
    </table>
    <% } %>
</asp:Content>


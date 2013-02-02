<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Monoscape.Common.Model.Node>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Application Instances")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<h2>Application Instances</h2>
			
<% if (Model.Count == 0)
   { %>
   <p>There are no nodes subscribed with the Application Grid.</p>
<% } %>
<% else 
   { %>
    <p>The following applications are found in the Application Grid.</p>
    <% foreach (var node in Model)
       { %>
            <h2>Node <%= node.Id %>: <%= node.IpAddress %></h2>

            <% if (node.Applications.Count == 0)
               { %>
               <p>There are no running applications found in this node.</p>
            <% } %>
            <% else 
               { %>
                    <p>The following applications are currently running in this node.</p>
                    <% foreach (var application in node.Applications)
                     { %>
                            <h3><%= application.Id %>: <%= application.ToString() %></h3>
                            <table>
                                <tr>
                                    <th>
                                        Instance ID
                                    </th>                                    
                                    <th>
                                        Process ID
                                    </th>
                                    <th>
                                        Url
                                    </th>
                                    <th>
                                        Request Count
                                    </th>
                                    <th></th>
                                </tr>

                            <% foreach (var instance in application.ApplicationInstances)
                                { %>
    
                                <tr>
                                    <td>
                                        <%= Html.Encode(instance.Id)%>
                                    </td>                                    
                                    <td>
                                        <%= Html.Encode(instance.ProcessId)%>
                                    </td>
                                    <td>
                                        <a href="<%= Html.Encode(instance.Url)%>"><%= Html.Encode(instance.Url)%></a>
                                    </td>
                                    <td>
                                        <%= Html.Encode(instance.RequestCount)%>
                                    </td>
                                    <td>
                                        <%= Html.ActionLink("Stop", "stopapplicationinstance", new { nodeId = node.Id, applicationId = application.Id, instanceId = instance.Id }) %>
                                    </td>
                                </tr>
    
                            <% } %>
                            </table>
                    <% } %>
                <% } %>
        <% } %>
<% } %>
</asp:Content>


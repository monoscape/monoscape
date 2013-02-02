<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Monoscape.Common.Model.ApplicationRequest>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Request History")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Request History</h2>
    
    <% if (Model.Count == 0)
       { %>
       <p>There are no requests found in the request history.</p>
    <% } %>
    <% else
        { %>
        <p>The following requests were found in the request history.</p>
        <% Html.RenderPartial("RequestTable", Model); %>
    <% } %>
</asp:Content>


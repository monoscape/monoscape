<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Monoscape.Common.Model.ApplicationRequest>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Request Queue")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Request Queue</h2>

    <% if (Model.Count == 0)
       { %>
       <p>There are no requests in the Load Balancer Request Queue at the moment. This is the live status.</p>
    <% } %>
    <% else
        { %>
        <p>The following requests were found in the Load Balancer Request Queue. This is the live status.</p>
        <% Html.RenderPartial("RequestTable", Model); %>
    <% } %>
</asp:Content>


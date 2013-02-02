<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Load Balancer")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Load Balancer</h2>
    <p>The Load Balancer performs the provisioning of Monoscape Cloud. This includes the Reverse Proxy and the Routing Mesh.</p>
</asp:Content>


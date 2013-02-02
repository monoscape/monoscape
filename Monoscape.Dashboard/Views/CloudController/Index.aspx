<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Cloud Controller")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Cloud Controller</h2>
    <p>The Cloud Controller is the central point of access to the Monoscape Cloud. </p>
    <p>There are two types of cloud controller API subscriptions.</p>
    <ol>
        <li>
            Application Subscriptions: 
            <p>Applications can subscribe to Cloud Controller Application Service API and access its service methods. The application service access key and the secret key will be used for authentication.</p>
        </li>
        <li>
            External System Subscriptions:
            <p>External Systems can subscribe to Cloud Controller External System Service API and access its service methods. The external system access key and the secret key will be used for authentication.</p>
        </li>
    </ol>
</asp:Content>


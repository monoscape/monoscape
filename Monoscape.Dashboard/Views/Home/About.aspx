<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("About")%>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Monoscape</h2>
    <p>
        Monoscape is a Platform as a Service (PaaS) cloud computing solution for hosting Mono web applications.
    </p>
</asp:Content>

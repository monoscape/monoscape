<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Monoscape Dashboard")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Monoscape Dashboard</h2>
    <p>
        Monoscape dashboard is the central point of access to Monoscape Cloud.
    </p>
</asp:Content>

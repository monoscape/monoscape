<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Exception>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%= Model.Message %>
    </h2>
    <p>
    <%= Model.StackTrace %><br /><br />
    <% if (Model.InnerException != null)
       { %>
       <%= Model.InnerException.Message %><br />
       <%= Model.InnerException.StackTrace %><br /><br />
    <% } %>
    </p>
</asp:Content>

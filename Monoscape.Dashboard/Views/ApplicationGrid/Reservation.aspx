<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Monoscape.ApplicationGridController.Model.Reservation>" %>
<%@ Import Namespace="Monoscape.ApplicationGridController.Model" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Reservation Done")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Reservation Done</h2>
    <p>A reservation was made to start an instance.</p>
    <fieldset>
        
        <div class="display-label">Reservation ID: <%= Html.Encode(Model.ReservationId) %></div>                
        An instance has not started yet. Please check the <%= Html.ActionLink("instances", "instances") %> page.
    </fieldset>

</asp:Content>


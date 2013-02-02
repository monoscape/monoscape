<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Monoscape.ApplicationGridController.Model.ConsoleOutput>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Console Output")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Console Output | <%= Html.Encode(Model.InstanceId) %></h2>

    <fieldset>
        
        <div class="display-label">InstanceId: <%= Html.Encode(Model.InstanceId) %></div>

        <div class="display-label">Timestamp: <%= Html.Encode(Model.Timestamp) %></div>
        
        <div class="display-label">Output</div>
        <div class="console-output"><%= Html.TextAreaFor(m => m.Output) %></div>       
        
    </fieldset>

</asp:Content>


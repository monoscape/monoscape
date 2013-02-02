<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Monoscape.ApplicationGridController.Model.Instance>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Run Instance")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Run Instance</h2>
    <p>Enter the instance type to run an instance.</p>

    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>

        <fieldset>                      
            
            <div class="editor-label">
                Image ID: <%= Model.ImageId %>
            </div>
            <div class="editor-label">
                Image Name: <%= ViewData["ImageName"]%>
            </div>
            
            <div class="editor-label">
                Instance Type:
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Type) %>
                <%= Html.ValidationMessageFor(model => model.Type) %>
            </div>
            
            <input type="submit" value="Run Instance" />
            
        </fieldset>

    <% } %>

</asp:Content>


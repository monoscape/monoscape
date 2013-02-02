<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Monoscape.Dashboard.Models.StartApplicationModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Start Application Instances")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Start Application Instances</h2>
    <p>Enter the number of instances to start.</p>

     <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>

        <fieldset>                      
            
            <div class="editor-label">
                Application ID: <%= Model.ApplicationId %>
            </div>
            <div class="editor-label">
                Application Name: <%= Model.Name %>
            </div>

            <div class="editor-label">
                Tenant Name:
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.TenantName) %>
                <%= Html.ValidationMessageFor(model => model.TenantName)%>
            </div>

            <div class="editor-label">
                Tenants Available: <%= ViewData["Tenants"].ToString() %>
            </div>

            <div class="editor-label">
                Number of Instances:
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.NumberOfInstances) %>
                <%= Html.ValidationMessageFor(model => model.NumberOfInstances)%>
            </div>
            
            <input type="submit" value="Start" />
            
        </fieldset>

    <% } %>
</asp:Content>

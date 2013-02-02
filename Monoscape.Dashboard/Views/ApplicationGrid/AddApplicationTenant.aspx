<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Monoscape.Common.Model.Tenant>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Add Application Tenant")%>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= ViewData["ApplicationName"].ToString() %> | Add Tenant</h2>
    <p>Enter name and upper scale limit to create a new tenant for <%= ViewData["ApplicationName"].ToString() %> application.</p>
    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>

        <fieldset>           
            
            <%= Html.HiddenFor(model => model.ApplicationId) %>

            <div class="editor-label">
                <%= Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Name) %>
                <%= Html.ValidationMessageFor(model => model.Name) %>
            </div>
            
            <div class="editor-label">
                Upper Scale Limit
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.UpperScaleLimit) %>
                <%= Html.ValidationMessageFor(model => model.UpperScaleLimit) %>
            </div>

            <div class="editor-label">
                Scaling Factor
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.ScalingFactor) %>
                <%= Html.ValidationMessageFor(model => model.ScalingFactor)%>
            </div>
            Note: Scaling factor is the number of requests served by an application instance at a given moment.<br /><br />
                       
            <input type="submit" value="Add" />            
        </fieldset>

    <% } %>
</asp:Content>


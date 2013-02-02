<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Monoscape.Common.Model.Subscription>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Add External System Subscription")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Add External System Subscription</h2>
    <p>Create a new external system subscription to allow a third party application to access the cloud controller external system service API.</p>
    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>

        <fieldset>            
                        
            <div class="editor-label">
                <%= Html.LabelFor(model => model.AccessKey) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.AccessKey) %>
                <%= Html.ValidationMessageFor(model => model.AccessKey) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.SecretKey) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.SecretKey) %>
                <%= Html.ValidationMessageFor(model => model.SecretKey) %>
            </div>
                        
            <input type="submit" value="Add" />
            
        </fieldset>

    <% } %>

</asp:Content>
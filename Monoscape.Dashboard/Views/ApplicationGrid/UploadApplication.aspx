<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Monoscape.Common.Model.Application>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Upload Application")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Upload Application</h2>
    <p>Zip your Mono/ASP.NET web application to a single file and upload.</p>

    <% using (Html.BeginForm("uploadapplication", "ApplicationGrid", FormMethod.Post, new { enctype = "multipart/form-data" })) {%>
        <%= Html.ValidationSummary(true) %>

        <fieldset>                        
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Name) %>
                <%= Html.ValidationMessageFor(model => model.Name) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Version) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Version) %>
                <%= Html.ValidationMessageFor(model => model.Version) %>
            </div>
            
            <div class="editor-field">                
                <input type="file" name="applicationPackage" id="applicationPackage" />  
                <%= Html.ValidationMessage("applicationPackage") %>             
            </div>
                        
            <input type="submit" value="Upload" />
        </fieldset>

    <% } %>

</asp:Content>


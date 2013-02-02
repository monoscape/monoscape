<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Monoscape.Dashboard.Runtime.ApplicationUtil.GetTitle("Application Grid")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <% Html.RenderPartial("leftmenu"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Application Grid</h2>
    <p>The Application Grid is the heart of Monoscape Cloud. It deploys and executes Mono/ASP.NET web applications in a
    distributed environment.</p>

    <h3>Application Grid Configuration</h3>
    <div class="editor-field">Monoscape Access Key: <%= ViewData["MonoscapeAccessKey"] %></div>
    <div class="editor-field">Monoscape Secret Key: <%= ViewData["MonoscapeSecretKey"] %></div>
    <div class="editor-field">Application Grid Service URL: <%= ViewData["ApplicationGridEndPointURL"]%></div>
    <div class="editor-field">Application Grid Status: <%= ViewData["ApplicationGridStatus"]%></div>
    <% if((ViewData["RunningOnMono"] != null) && (ViewData["RunningOnMono"].ToString().Equals("TRUE")))
       { %>
		<div class="editor-field">Mono Runtime: <%= ViewData["MonoRuntime"]%></div>
	<% } %>	
	<div class="editor-field">.NET Runtime: <%= ViewData["DotNetRuntime"]%></div>
	<div class="editor-field">Operating System: <%= ViewData["OperatingSystem"]%></div>
	
    <% if (ViewData["ApplicationGridError"] != null)
       { %>
          <div class="editor-field">Error: <%= ViewData["ApplicationGridError"]%></div>
    <% } %>
    <% else 
       { %>
        <br />

        <h3>IaaS Configuration</h3>
        <div class="editor-field">IaaS Name: <%= ViewData["IaasName"] %></div>
        <div class="editor-field">IaaS Access Key: <%= ViewData["IaasAccessKey"] %></div>
        <div class="editor-field">IaaS Secret Key: <%= ViewData["IaasSecretKey"] %></div>
        <div class="editor-field">IaaS Service URL: <%= ViewData["IaasServiceURL"] %></div>
        <div class="editor-field">IaaS Key Name: <%= ViewData["IaasKeyName"] %></div>
        <div class="editor-field">IaaS Status: <%= ViewData["IaasStatus"] %></div>

        <% if (ViewData["IaasError"] != null)
           { %>
              <div class="editor-field">Error: <%= ViewData["IaasError"]%></div>
        <% } %>
    <% } %>
    <br />   
    
</asp:Content>

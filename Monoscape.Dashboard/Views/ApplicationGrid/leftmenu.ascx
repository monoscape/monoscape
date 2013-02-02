<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% Dictionary<string, string> menuItems = new Dictionary<string, string>();
   string url = "applicationgrid/";
   menuItems.Add(url + "images", "Virtual Machine Images");
   menuItems.Add(url + "instances", "Virtual Machine Instances");
   menuItems.Add(url + "nodes", "Application Grid Nodes");
   menuItems.Add(url + "applications", "Applications");
   menuItems.Add(url + "applicationinstances", "Application Instances");
   menuItems.Add(url + "uploadapplication", "Upload Application");
   menuItems.Add(url + "scalinghistory", "Scaling History");
 %>
<%= Html.LeftMenu(menuItems) %>

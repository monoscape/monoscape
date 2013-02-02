<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% Dictionary<string, string> menuItems = new Dictionary<string, string>();
   string url = "cloudcontroller/";
   menuItems.Add(url + "subscriptions", "Subscriptions");
   menuItems.Add(url + "addapplicationsubscription", "Add App. Subscription");
   menuItems.Add(url + "addexsystemsubscription", "Add Ex. Sys. Subscription");   
 %>
<%= Html.LeftMenu(menuItems) %>

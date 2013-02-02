<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% Dictionary<string, string> menuItems = new Dictionary<string, string>();
   string url = "loadbalancer/";
   menuItems.Add(url + "requestqueue", "Request Queue");
   menuItems.Add(url + "requesthistory", "Request History");
   menuItems.Add(url + "routingmeshhistory", "Routing Mesh History");
 %>
<%= Html.LeftMenu(menuItems) %>

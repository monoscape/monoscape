<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<Monoscape.Common.Model.ApplicationRequest>>" %>

<table>
    <tr>
        <th>ID</th>
        <th>
			Time
		</th>
        <th>
            Raw Url
        </th>
        <th>
            Request Type
        </th>
        <th>
            User Agent
        </th>
        <th>
            User Host Address
        </th>
    </tr>

<% foreach (var item in Model) { %>
    
    <tr>
		<td>
            <%= Html.Encode(item.Id) %>
        </td>
        <td>
            <%= Html.Encode(item.Time) %>
        </td>
        <td>
            <%= Html.Encode(item.RawUrl) %>
        </td>
        <td>
            <%= Html.Encode(item.RequestType) %>
        </td>
        <td>
            <%= Html.Encode(item.UserAgent) %>
        </td>
        <td>
            <%= Html.Encode(item.UserHostAddress) %>
        </td>
    </tr>
    
<% } %>

</table>
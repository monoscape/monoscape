/*
 *  Copyright 2013 Monoscape
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 *  History: 
 *  2011/11/10 Created.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace System.Web.Mvc
{
    public static class MenuExtensions
    {
        public static string Menu(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            string currentPath = HttpContext.Current.Request.Url.AbsolutePath;
            sb.AppendLine("<ul id='menu'>");            
            DrawMenuItem(sb, currentPath, "cloudcontroller", "Cloud Controller");
            DrawMenuItem(sb, currentPath, "applicationgrid", "Application Grid");
            DrawMenuItem(sb, currentPath, "loadbalancer", "Load Balancer");
            DrawMenuItem(sb, currentPath, "about", "About");            
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        public static string LeftMenu(this HtmlHelper helper, Dictionary<string, string> menuItems)
        {
            StringBuilder sb = new StringBuilder();
            string currentPath = HttpContext.Current.Request.Url.AbsolutePath;
            string value = string.Empty;

            sb.AppendLine("<ul id='leftmenu'>"); 
            foreach (string key in menuItems.Keys)
            {
                menuItems.TryGetValue(key, out value);
                DrawMenuItem(sb, currentPath, key, value);
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        private static void DrawMenuItem(StringBuilder sb, string currentPath, string url, string menuText)
        {
            if (currentPath.ToLower().StartsWith(ResolveUrl(url).ToLower()))
                sb.AppendLine("    <li class='selectedMenuItem'>" + "<a href=" + ResolveUrl(url) + ">" + menuText + "</a>" + "</li>");
            else
                sb.AppendLine("    <li class='menuItem'>" + "<a href=" + ResolveUrl(url) + ">" + menuText + "</a>" + "</li>");
        }

        private static string ResolveUrl(string url)
        {
            return VirtualPathUtility.ToAbsolute("~/" + url);
        }
    }
}
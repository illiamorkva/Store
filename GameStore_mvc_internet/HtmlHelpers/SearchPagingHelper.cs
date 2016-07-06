﻿using GameStore_mvc_internet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GameStore_mvc_internet.HtmlHelpers
{
    public static class SearchPagingHelper
    {
        public static MvcHtmlString PageLinksSearch(this HtmlHelper html,
                                             PagingInfo pagingInfo,
                                             Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++) // пробег по страницам
            {
                TagBuilder tag = new TagBuilder("a"); // ссыль
                tag.MergeAttribute("href", pageUrl(i)); // добавление страниці к ссілке
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage) // для выбранной
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}
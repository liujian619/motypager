using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Web.Mvc;

namespace Moty.Utils
{
	/// <summary>
	/// 服务器端分页器（刷新分页）。
	/// </summary>
	public static class MotyPagerHelper
	{
		/// <summary>
		/// 生成分页HTML文本。
		/// </summary>
		/// <param name="cur">当前页。</param>
		/// <param name="total">总页数。</param>
		/// <param name="option">分页选项。</param>
		/// <returns></returns>
		public static string Create(int cur, int total, MotyPagerOption option)
		{
			if (cur < 0 || total < 0)
			{
				//throw new ArgumentException("Initialization Argument Error('cur' or 'total' must be greater than 0).");
				return string.Empty;
			}
			if (cur > total)
			{
				//throw new ArgumentException("Initialization Argument Error('cur' must be less than 'total').");
				return string.Empty;
			}

			string html = string.Empty;
			if (!(total <= 1 && option.HideWhenOnlyOnePage))
			{
				html = CreateHtml(cur, total, option);
			}

			return html;
		}

		/// <summary>
		/// 生成分页HTML文本。
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="cur">当前页。</param>
		/// <param name="total">总页数。</param>
		/// <param name="option">分页选项。</param>
		/// <returns></returns>
		public static MvcHtmlString MotyPager(this HtmlHelper helper, int cur, int total, MotyPagerOption option)
		{
			return MvcHtmlString.Create(Create(cur, total, option));
		}

		//生成
		private static string CreateHtml(int cur, int total, MotyPagerOption option)
		{
			StringBuilder sb = new StringBuilder(string.Empty);
			sb.Append("<div class='motypc_1604261319'>");

			#region 首页
			if (option.ShowFirst)
			{
				if (cur == 1)
				{
					sb.AppendFormat("<span style='color:{0}'>{1}</span>",
						ColorTranslator.ToHtml(option.DisabledForeColor), option.FirstText);
				}
				else
				{
					sb.AppendFormat("<a href='{0}' style='color:{1}' onmouseover=\"this.style.color='{2}';this.style.borderColor='{3}'\" onmouseout=\"this.style.color='{4}';this.style.borderColor='{5}'\">{6}</a>",
						option.Url(1),
						ColorTranslator.ToHtml(option.ForeColor), ColorTranslator.ToHtml(option.HoverForeColor),
						ColorTranslator.ToHtml(option.HoverForeColor), ColorTranslator.ToHtml(option.ForeColor),
						"transparent", option.FirstText);
				}
			}
			#endregion
			#region 上一页
			if (option.ShowPrev)
			{
				if (cur == 1)
				{
					sb.AppendFormat("<span style='color:{0}'>{1}</span>",
						ColorTranslator.ToHtml(option.DisabledForeColor), option.PrevText);
				}
				else
				{
					sb.AppendFormat("<a href='{0}' style='color:{1}' onmouseover=\"this.style.color='{2}';this.style.borderColor='{3}'\" onmouseout=\"this.style.color='{4}';this.style.borderColor='{5}'\">{6}</a>",
						option.Url(cur - 1),
						ColorTranslator.ToHtml(option.ForeColor), ColorTranslator.ToHtml(option.HoverForeColor),
						ColorTranslator.ToHtml(option.HoverForeColor), ColorTranslator.ToHtml(option.ForeColor),
						"transparent", option.PrevText);
				}
			}
			#endregion
			#region 中间页面按钮
			if (option.Responsive)
			{
				sb.Append(CreatePageBtns(cur, total, option));
				sb.Append(CreateSelect(cur, total, option));
			}
			else
			{
				if (option.IsPcStyle)
					sb.Append(CreatePageBtns(cur, total, option));
				else
					sb.Append(CreateSelect(cur, total, option));
			}
			#endregion
			#region 下一页
			if (option.ShowNext)
			{
				if (cur == total)
				{
					sb.AppendFormat("<span style='color:{0}'>{1}</span>",
						ColorTranslator.ToHtml(option.DisabledForeColor), option.NextText);
				}
				else
				{
					sb.AppendFormat("<a href='{0}' style='color:{1}' onmouseover=\"this.style.color='{2}';this.style.borderColor='{3}'\" onmouseout=\"this.style.color='{4}';this.style.borderColor='{5}'\">{6}</a>",
						option.Url(cur + 1),
						ColorTranslator.ToHtml(option.ForeColor), ColorTranslator.ToHtml(option.HoverForeColor),
						ColorTranslator.ToHtml(option.HoverForeColor), ColorTranslator.ToHtml(option.ForeColor),
						"transparent", option.NextText);
				}
			}
			#endregion
			#region 尾页
			if (option.ShowLast)
			{
				if (cur == total)
				{
					sb.AppendFormat("<span style='color:{0}'>{1}</span>",
						ColorTranslator.ToHtml(option.DisabledForeColor), option.LastText);
				}
				else
				{
					sb.AppendFormat("<a href='{0}' style='color:{1}' onmouseover=\"this.style.color='{2}';this.style.borderColor='{3}'\" onmouseout=\"this.style.color='{4}';this.style.borderColor='{5}'\">{6}</a>",
						option.Url(total),
						ColorTranslator.ToHtml(option.ForeColor), ColorTranslator.ToHtml(option.HoverForeColor),
						ColorTranslator.ToHtml(option.HoverForeColor), ColorTranslator.ToHtml(option.ForeColor),
						"transparent", option.LastText);
				}
			}
			#endregion

			sb.Append("</div>");
			return sb.ToString();
		}

		private static string CreatePageBtns(int cur, int total, MotyPagerOption option)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<div class='motypc_horizontal_pager_btn'>");
			foreach (var page in GetPagesToShow(cur, total, option.PageBtnCount))
			{
				if (page == cur)
				{
					sb.AppendFormat("<a href='javascript:void(0)' style='color:{0}; background-color:{1};border-color:transparent'>{2}</a>",
						ColorTranslator.ToHtml(option.CurrentForeColor), ColorTranslator.ToHtml(option.CurrentBackColor), page);
				}
				else
				{
					sb.AppendFormat("<a href='{0}' style='color:{1}' onmouseover=\"this.style.color='{2}';this.style.borderColor='{3}'\" onmouseout=\"this.style.color='{4}';this.style.borderColor='{5}'\">{6}</a>",
						option.Url(page),
						ColorTranslator.ToHtml(option.ForeColor), ColorTranslator.ToHtml(option.HoverForeColor),
						ColorTranslator.ToHtml(option.HoverForeColor), ColorTranslator.ToHtml(option.ForeColor),
						"transparent", page);
				}
			}
			sb.Append("</div>");
			return sb.ToString();
		}

		private static string CreateSelect(int cur, int total, MotyPagerOption option)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("<select class='motypc_vertical_pager_btn' onchange=\"window.location=this.options[this.selectedIndex].getAttribute('data-url')\">");
			for (int i = 1; i <= total; i++)
			{
				if (i == cur)
					sb.AppendFormat("<option selected='selected' value='{0}' data-url='{1}'>{2}</option>", i, option.Url(i), i);
				else
					sb.AppendFormat("<option value='{0}' data-url='{1}'>{2}</option>", i, option.Url(i), i);
			}
			sb.Append("</select>");
			return sb.ToString();
		}

		//获取当前需展示的页面按钮。
		private static List<int> GetPagesToShow(int cur, int total, int pagesToShow)
		{
			var pages = new List<int>();

			int half = (int)Math.Floor((pagesToShow - 1) / 2.0);
			int pad = cur + half > total ? (cur + half - total) : 0;

			int min = Math.Max(1, cur - half - pad);
			for (var i = min; i <= cur; i++)
			{
				pages.Add(i);
			}

			var max = Math.Min(total, cur + pagesToShow - pages.Count);
			for (var i = cur + 1; i <= max; i++)
			{
				pages.Add(i);
			}

			return pages;
		}
	}

	/// <summary>
	/// 分页选项。
	/// </summary>
	public class MotyPagerOption
	{
		public MotyPagerOption()
		{
			ForeColor = ColorTranslator.FromHtml("#787d82");
			DisabledForeColor = ColorTranslator.FromHtml("#c8cdd2");
			HoverForeColor = ColorTranslator.FromHtml("#ec1500");
			CurrentForeColor = ColorTranslator.FromHtml("#fff");
			CurrentBackColor = ColorTranslator.FromHtml("#ec1500");

			ShowFirst = ShowLast = ShowNext = ShowPrev = IsPcStyle = HideWhenOnlyOnePage = true;
			Responsive = false;

			FirstText = "首页";
			LastText = "尾页";
			PrevText = "上一页";
			NextText = "下页";
			FirstText = "首页";

			PageBtnCount = 7;

			Url = index => { return "index_" + index; };
		}

		/// <summary>
		/// 获取或设置正常文本颜色。
		/// </summary>
		public Color ForeColor { get; set; }

		/// <summary>
		/// 获取或设置失效文本颜色。
		/// </summary>
		public Color DisabledForeColor { get; set; }

		/// <summary>
		/// 获取或设置鼠标悬停文本颜色及该项的下边界颜色。
		/// </summary>
		public Color HoverForeColor { get; set; }

		/// <summary>
		/// 获取或设置当前选中页文本颜色。
		/// </summary>
		public Color CurrentForeColor { get; set; }

		/// <summary>
		/// 获取或设置当前选中页背景颜色。
		/// </summary>
		public Color CurrentBackColor { get; set; }

		/// <summary>
		/// 获取或设置是否显示首页。
		/// </summary>
		public bool ShowFirst { get; set; }

		/// <summary>
		/// 获取或设置是否显示尾页。
		/// </summary>
		public bool ShowLast { get; set; }

		/// <summary>
		/// 获取或设置是否显示上一页。
		/// </summary>
		public bool ShowPrev { get; set; }

		/// <summary>
		/// 获取或设置是否显示下一页。
		/// </summary>
		public bool ShowNext { get; set; }

		/// <summary>
		/// 获取或设置是否在只有一页的情况下隐藏分页控件。
		/// </summary>
		public bool HideWhenOnlyOnePage { get; set; }

		/// <summary>
		/// 是否支持响应式。（如果设置为true，则在屏幕宽度小于768px时自动变为select显示。）
		/// </summary>
		public bool Responsive { get; set; }

		/// <summary>
		/// 获取或设置是否显示为pc样式，如果是，那么将所以页面按钮平铺展开，否则使用elect标签用于显示。（Responsive为true时无效。）
		/// </summary>
		public bool IsPcStyle { get; set; }

		/// <summary>
		/// 获取或设置首页文本。
		/// </summary>
		public string FirstText { get; set; }

		/// <summary>
		/// 获取或设置尾页文本。
		/// </summary>
		public string LastText { get; set; }

		/// <summary>
		/// 获取或设置上一页文本。
		/// </summary>
		public string PrevText { get; set; }

		/// <summary>
		/// 获取或设置下一页文本。
		/// </summary>
		public string NextText { get; set; }

		/// <summary>
		/// 页面按钮数目。
		/// </summary>
		public int PageBtnCount { get; set; }

		/// <summary>
		/// 获取或设置页面跳转链接。
		/// </summary>
		public Func<int, string> Url { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.WebPages;

namespace Lightning
{
	// Utilities/helper classes to simplify some infrastructure.
	public static class Utilities
	{
		private static readonly object _configLock = new object();
		private const string RequestDataKey = "__requestData";
		private static readonly Dictionary<string, Configuration> _configSections = new Dictionary<string, Configuration>();

		// Dynamic helper method to get values that may not exist.  Not really an extension method.
		public static T GetValue<T>(dynamic dyn, string key, T defaultValue)
		{
			try
			{
				// Assumes that the dynamic passed in can be cast as an IDictionary<string, object>.
				T value = (T)((IDictionary<string, object>)dyn)[key];
				return value;
			}
			catch (Exception)
			{
				// There was a problem fetching the value.  Return the default value here.
				return defaultValue;
			}
		}

		public static dynamic GetContentBySlug(IEnumerable<dynamic> dyns, string slug)
		{
			return dyns.FirstOrDefault(c => c.Slug == "_footer");
		}

		// Request data is data parsed and processed by the HttpModule, for use in child pages and templates.  The current HttpContext is used to hold request data.
		public static dynamic GetRequestData(this HttpContextBase httpContext)
		{
			return httpContext.Items[RequestDataKey];
		}

		// Used by the HttpModule to set the current request's data.
		public static dynamic SetRequestData(this HttpContextBase httpContext, dynamic value)
		{
			httpContext.Items[RequestDataKey] = value;
			return value;
		}

		// Utility method to create template-relative urls for css, javascript, and image files.
		public static string GetPath(this WebPageBase webPage, string path)
		{
			const string templatePathKey = "__templatePath";

			var basePath = (string)webPage.Context.Items[templatePathKey];

			if (string.IsNullOrWhiteSpace(basePath))
			{
				var template = Path.GetDirectoryName(webPage.VirtualPath) ?? "";
				template = template.Substring(template.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase));

				webPage.Context.Items[templatePathKey] = basePath = VirtualPathUtility.ToAbsolute("~/App_Templates/" + template + "/");
			}

			return basePath + path;
		}

		public static string GetHostKey(this HttpContextBase httpContext)
		{
			var key = httpContext.Request.Url == null ? "" : httpContext.Request.Url.Authority.Replace(":", "_").Replace("..", "");
			return key;
		}

		public static string GetHostDataPath(this HttpContextBase httpContext, string additionalPath = "")
		{
			var path = string.Format("~/App_Data/host_{0}/{1}", httpContext.GetHostKey(), additionalPath ?? "");
			return path;
		}

		// Configuration helper to get values from a name value collection with default fallbacks.
		public static T GetConfigurationValue<T>(this HttpContextBase httpContext, string key, T defaultValue)
		{
			var host = httpContext.GetHostKey();
			Configuration config;

			if (_configSections.ContainsKey(host))
			{
				config = _configSections[host];
			}
			else
			{
				lock (_configLock)
				{
					// two phase locking
					if (_configSections.ContainsKey(host))
					{
						config = _configSections[host];
					}
					else
					{
						config = WebConfigurationManager.OpenWebConfiguration(httpContext.GetHostDataPath("config/"));
						_configSections.Add(host, config);
					}
				}
			}

			try
			{
				return (T)Convert.ChangeType(config.AppSettings.Settings[key].Value, typeof(T));
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
	}
}

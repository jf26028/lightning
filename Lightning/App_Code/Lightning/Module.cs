using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.WebPages;

namespace Lightning
{
	// This HttpModule is the core of the lightning system.  This module is responsible for identifying if the request is to be handled by lightning or if it should let it fall through to asp.net.
	public class Module : IHttpModule
	{
		public static readonly string Version = "1.0.0";

		private readonly object _lock = new object();

		public void Dispose()
		{
		}

		// Initialize the module.  Typically bind to events here, but configuration may be established here, also.
		public void Init(HttpApplication application)
		{
			// We will intercept the asp.net pipeline at the ApplicationPostResolveRequestCache event.  This is the same pattern that the asp.net webpages framework uses, so I assume it is good enough for lightning.
			application.PostResolveRequestCache += onApplicationPostResolveRequestCache;
		}

		// This event handler is fired for each request.  The module fetches the contents, identifies if the url should be handled, and returns the parsed content if necessary.
		private void onApplicationPostResolveRequestCache(object sender, EventArgs e)
		{
			var httpContext = new HttpContextWrapper(((HttpApplication)sender).Context);

			var contents = this.getContents(httpContext);
			var requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2).Split('/')[0];
			var content = contents.FirstOrDefault(c => c.Slug.Equals(requestPath, StringComparison.OrdinalIgnoreCase));
			
			// If content matching the request was found
			if (content != null)
			{
				var template = this.getTemplate(httpContext, content);
				var templateVirtualPath = string.Format("~/App_Templates/{0}/_content.cshtml", template);
				var handler = WebPageHttpHandler.CreateFromVirtualPath(templateVirtualPath);

				if (handler != null)
				{
					// Set the request data
					dynamic requestData = new ExpandoObject();
					requestData.Contents = contents;
					requestData.Content = content;

					// Stash the request data for use by the handlers later.
					Utilities.SetRequestData(httpContext, requestData);

					// Let asp.net know we will handle this request.
					httpContext.RemapHandler(handler);
				}
			}

			// Let asp.net handle this request.
		}

		private List<dynamic> getContents(HttpContextBase httpContext)
		{
			// Fetch all content from the cache.  If it is not in the cache, fetch the data and insert into the cache (using a CacheTimeoutSeconds = 0 will prevent caching).

			var host = httpContext.GetHostKey();
			var cacheKey = "__lightningContent_" + host;

			// todo:  Verify that there is no security issue here using host this way.  aka what if the host == "..\.." or something.

			var contents = (List<dynamic>)httpContext.Cache.Get(cacheKey);

			if (contents == null)
			{
				lock (_lock)
				{
					// two phase locking
					contents = (List<dynamic>)httpContext.Cache.Get(cacheKey);
					if (contents == null)
					{
						var contentVirtualPath = httpContext.GetHostPath("content/");
						var contentPhysicalPath = httpContext.Server.MapPath(contentVirtualPath);

						if (!Directory.Exists(contentPhysicalPath))
						{
							throw new Exception("No content found for host.  Host:  " + host);
						}

						contents = this.getContentProvider().GetContents(contentPhysicalPath);
						int cacheTimeoutSeconds = httpContext.GetConfigurationValue("cacheTimeoutSeconds", 0);

						if (cacheTimeoutSeconds > 0)
						{
							// Add the contents to the cache, using all files in the app_data folder as the cache dependency.  If any app_data changes, the cache will be refreshed.  This may be bad for you if you put other data in the app_data folder.
							httpContext.Cache.Insert(cacheKey, contents, new CacheDependency(Directory.GetDirectories(Path.Combine(contentPhysicalPath, ".."), "*", SearchOption.AllDirectories)), DateTime.UtcNow.AddSeconds(cacheTimeoutSeconds), Cache.NoSlidingExpiration);
						}
					}
				}
			}

			return contents;
		}

		private string getTemplate(HttpContextBase httpContext, dynamic content)
		{
			// If the content has a Template defined, use it.  Otherwise, default to the configured template.

			// todo:  verify that the template exists.
			string template = Utilities.GetValue<string>(content, "Template", httpContext.GetConfigurationValue<string>("template", null) ?? "default");

			return template;
		}

		private IContentProvider getContentProvider()
		{
			// If you would prefer to get the content from a different source, this is where you new up your content provider.
			return new ReadOnlyFileSystemContentProvider();
		}
	}
}

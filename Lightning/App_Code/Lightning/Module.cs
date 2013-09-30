using System;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace Lightning
{
	// This HttpModule is the core of the lightning system.  This module is responsible for identifying if the request is to be handled by lightning or if it should let it fall through to asp.net.
	public class Module : IHttpModule
	{
		// semver of the system.
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

			var contents = this.getContentProvider().GetContents(httpContext);
			var requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2).Split('/')[0];
			var content = contents.FirstOrDefault(c => c.Slug.Equals(requestPath, StringComparison.OrdinalIgnoreCase));

			if (content == null)
			{
				// Content for this request was not found.  Let asp.net handle this request.
				return;
			}

			IHttpHandler handler = null;
			var templates = new[] { Utilities.GetValue<string>(content, "Template", httpContext.GetConfigurationValue<string>("template", null) ?? "default"), "default" };

			// for each possible template, try to create a handler.
			foreach (var template in templates)
			{
				var templateVirtualPath = string.Format("~/App_Templates/{0}/_content.cshtml", template);
				handler = WebPageHttpHandler.CreateFromVirtualPath(templateVirtualPath);

				// If we successfully found a handler, use it.
				if (handler != null)
				{
					break;
				}
			}

			if (handler == null)
			{
				throw new Exception("Content found, but no template available.");
			}

			// Set the request data
			dynamic requestData = new ExpandoObject();
			requestData.Contents = contents;
			requestData.Content = content;

			// Stash the request data for use by the handlers later.
			Utilities.SetRequestData(httpContext, requestData);

			// Let asp.net know we will handle this request.
			httpContext.RemapHandler(handler);
		}

		private IContentProvider getContentProvider()
		{
			// If you would prefer to get the content from a different source, this is where you new up your content provider.
			return new ReadOnlyFileSystemContentProvider();
		}
	}
}

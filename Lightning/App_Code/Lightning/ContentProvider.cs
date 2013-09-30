using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Lightning
{
	public interface IContentProvider
	{
		// Fetch all contents from the content store.
		List<dynamic> GetContents(HttpContextBase httpContext);
	}

	// File system content provider implementation.
	public class ReadOnlyFileSystemContentProvider : IContentProvider
	{
		private static readonly object _lock = new object();

		public List<dynamic> GetContents(HttpContextBase httpContext)
		{
			var hostKey = httpContext.GetHostKey();
			var cacheKey = "__ReadOnlyFileSystemContentProvider_" + hostKey;

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
						// The convention of this content provider is to look in the ~/App_Data/{host}/content folder for the content.
						var contentVirtualPath = httpContext.GetHostPath("content/");
						var contentPhysicalPath = httpContext.Server.MapPath(contentVirtualPath);

						if (!Directory.Exists(contentPhysicalPath))
						{
							throw new Exception("No content found for host.  Host:  " + hostKey);
						}

						contents = this.getContents(contentPhysicalPath);
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

		// Fetch all files in the virtual path (top level only) and parses those files into dynamic objects.
		private List<dynamic> getContents(string physicalPath)
		{
			var contentParser = this.getContentParser();
			var filePaths = Directory.EnumerateFiles(physicalPath, "*.*", SearchOption.TopDirectoryOnly);
			return filePaths.Select(filePath => contentParser.ParseContent(File.ReadLines(filePath).ToArray())).ToList();
		}

		private IContentParser getContentParser()
		{
			// If you would prefer a different content parser, this is where you new up your content parser.
			return new ContentParser();
		}
	}
}

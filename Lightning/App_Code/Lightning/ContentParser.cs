using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Lightning
{
	public interface IContentParser
	{
		dynamic ParseContent(string[] lines);
	}

	// The content parser is responsible for parsing the content from a given content provider.
	public class ContentParser : IContentParser
	{
		// Parse the file, extracting metadata from content.
		public dynamic ParseContent(string[] lines)
		{
			// Content files are parsed into dynamic expando objects to allow for dynamic configuration with conventions for names.
			dynamic contentFile = new ExpandoObject();
			var dictionary = contentFile as IDictionary<string, object>;

			// The logic here:
			//
			// * Each line before a blank line is metadata in the format tag:data.
			// * All lines after that are considered content.
			//     Content is stored in the _content property.
			//     Content is fetched via the GetContent method (as it can be processed by the system).
			for (int counter = 0; counter < lines.Length; counter++)
			{
				string line = lines[counter].Trim();

				// The blank line separates the metadata from the file content.  
				// Before the blank line, everything is metadata.  After the blank line is found, everything else in the file is considered content.
				if (string.IsNullOrWhiteSpace(line.Trim()))
				{
					// The content is composed of all remaining lines.
					contentFile._content = string.Join(Environment.NewLine, lines.Skip(counter + 1).ToArray());
					contentFile._contentProcessed = null;

					// This is an implementation of lazy processing with caching.
					// The content is processed with markdown only when it needs to be, and the result is cached on the content object.
					contentFile.GetContent = new Func<dynamic, string>(content =>
					{
						// If this content has not been processed previously, process it now and cache the output to prevent from processing the same text more than once to get the same output.
						if (content._contentProcessed == null)
						{
							content._contentProcessed = this.processContent(content._content);
						}

						return content._contentProcessed;
					});

					break;
				}
				else
				{
					// Lines starting # are comments.  Do not process comments.
					if (!line.StartsWith("#"))
					{
						string[] metaData = line.Split(":".ToCharArray(), 2);

						// note:  this line prevents config values with empty spaces at the end.  For example, a padded string value.  This may not be what you want.
						dictionary[metaData[0].Trim()] = metaData[1].Trim();
					}
				}
			}

			return contentFile;
		}

		// Process the content of the file.  By default, this is done via a markdown parser.  MarkdownSharp is used in this instance.
		private string processContent(string content)
		{
			// If you have a different processor in mind (or if you write your content in html and dont need a processor) this is the place to change that.
			// Use "return content;" if you do not need a processor (if you write your content in html already).
			return new MarkdownSharp.Markdown().Transform(content);
		}
	}
}
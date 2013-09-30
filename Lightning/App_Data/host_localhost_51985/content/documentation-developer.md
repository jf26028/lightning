Slug:  documentation-developer
Title:  Lightning - Documentation - Developer
Menu:  
Enabled:  true
Position:  0
Author:  Jesse Foster
Description:  Lightning documentation for the developer.
Keywords:  lightning,documentation,page,metadata,templates,configuration,content,module,toolkit

<ol class="breadcrumb">
  <li><a href="/">Home</a></li>
  <li><a href="/documentation">Documentation</a></li>
  <li class="active">Developer Documentation</li>
</ol>

<div class="alert alert-warning">
	<strong>Warning</strong>  Alot of this is (already) out of date, or incomplete.  Updates are coming.
</div>

#Developer Documentation

Lighting consists primarily of 3 core components and templates.  The core componets are the `ContentProvider`, the `HttpModule`, and a few utility/helpers (`Utilities`).

###The HttpModule

The core of Lightning is an asp.net HttpModule that is responsible for responding to content from the content folder.  If the first part of the path matches a slug from the content, the request is intercepted, and handled with the templates in the App_Templates folder with the content passed directly to the Razor files.

Because Lightning only looks at the first segment of the url, it sees http://lightning.simpdev.net/documentation and http://lightning.simpdev.net/documentation/anything-else-can-go/here-and-here/or-here as the same url.  This can be a useful feature for customizing your urls.

The logic for the module follows this pattern:

* Fetch all content (from cache if available).
* From all content, if there is any who has a Slug value that matches the first segment of the url (case insensitive), set the handler to the `<configured template path>\_content.cshtml` while setting the contents and selected content to variables in the request for pickup later in processing.
* If there is no match, it passes the request to asp.net.  Any other html, mvc routes, web pages, etc that are configured to process the request will work as expected.

###The Content Parser

The content parser is responsible for parsing each of the files on the disk into dynamic objects with content.  It is also responsible for applying markdown transformations to the content.

This class implements the `IContentProvider` interface so it can easily be switched out with another provider (possibly from a database).

There is a built in caching mechanism for the markdown processing, also.  This is to prevent the same content from being transformed by the markdown utility more than once.

###Utilites

The Utilites library is very small.  It's primary function is to help dealing with missing values on expando objects, working with cache, and simplifying the communication from the HttpModule to the templates.

###Templates

todo:  more docs here.

<!--
###Extending the Toolkit
-->

<br />

<a href="/documentation" class="btn btn-primary">Back To Documentation</a>

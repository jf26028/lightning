# No slug to handle the url http://lightning.simpdev.net/
Slug:  
Title:  Lightning - Home
Menu:  Home
Enabled:  true
Position:  1

#<img src="/media/host_localhost_51985/lightning-144.png" height="64" style="margin: 0 0 0 -10px" alt="Lightning" title="Lightning" />Lightning
## A micro toolkit to simplify getting your content on your site.

###What is Lightning?

* A simple and flexible way to get your content on your site, without getting in your way.
* An asp.net module to identify your content.
* Flat files to store your content.
* Markdown to parse your content.
* Razor to render your content.

###Features

* Simplicity.
	* The code:  Small, concise and clean.
	* Content:  Flat files.  Publish via ftp, dropbox, file share, xcopy, etc.
	* Deployment:  Deploy from source, ftp, dropbox, file share, xcopy, etc.  Simply as possible.
	* Ui:  Templates use razor to offer full flexibility.
* Speed.
	* Execution:  Less code means less cpu.
	* Caching:  Cache as much processing as possible.  Cache content reading and parsing.  Cache content processing.

###How does Lightning work?

* Lightning is implemented as an asp.net http module.  When a request is made for content, the asp.net pipeline is interrupted, and the content is served.
* Content is stored as markdown files in the website, by default, with extension points to allow for other methods of uploading content to the site.
* Files are parsed once, and cached, so greater performance is achieved.  Cache is invalidated on file change, so changes are displayed instantly.

###To do

* More testing.
* More templates.
* More docs.
* More info.
* More everything.

###License

* Apache 2.0

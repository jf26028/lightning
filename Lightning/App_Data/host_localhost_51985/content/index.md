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

###How does Lightning work?

* Lightning is implemented as an asp.net http module.  When a request is made for content, the asp.net pipeline is interrupted, and the content is served.
* Content is stored as markdown files in the website, by default, with extension points to allow for other methods of uploading content to the site.
* Files are parsed once, and cached, so performance is high.  Cache is invalidated on file change, so changes are displayed instantly.

###Features

* Simplicity.  The code, content, deployment, ui, and publish stories were designed with simplicity as a feature.
* Simple deployment.  XCopy deployment or deploy from source control.
* Simple content management.  Manage content via files and publish content via ftp or dropbox type service.
* Speed.  Near static site speed without sacrificing dynamic capabilities.

###To do

* Testing.
* More templates.
* More docs.
* More info.

###License

* Apache 2.0

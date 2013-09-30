Slug:  documentation-content-author
Title:  Lightning - Documentation - Content Author
Menu:
Enabled:  true
Position:  0
Author:  Jesse Foster
Description:  Lightning documentation for the content author.
Keywords:  lightning,documentation,page,metadata,templates,configuration,content,module,toolkit
Template:  default

<ol class="breadcrumb">
  <li><a href="/">Home</a></li>
  <li><a href="/documentation">Documentation</a></li>
  <li class="active">Content Author Documentation</li>
</ol>

<div class="alert alert-warning">
	<strong>Warning</strong>  Alot of this is (already) out of date, or incomplete.  Updates are coming.
</div>

#Content Author Documentation

###Creating and Publishing Content

Lightning was designed to make it as simple as possible to get content up on the website.

Content is stored in flat files in the App_Data folder for easy deployment.  FTP, dropbox, and/or file copy are the tools used to publish your content.

####Example of how to create a page in Lightning.

* Create a text file
* Copy/paste this text into that file:

		Slug:  hello-world
		Title:  Hello World Page Title
		Menu:  Hello World Menu Title
		Enabled:  true
		Position:  100

		Hello World!

* Save the file as `~/App_Data/content/hello-world.md`, and when you navigate to /hello-world on your site, you should see the hello world page.  ([Hello World on this site](/hello-world "Hello World"))

<div class="alert alert-info">
	It does not matter what you name your file because the configuration will be picked up from the metadata, not the file name.
</div>

###Content Files In Depth

The content files are the heart of the application.  The above example is a simple content file, but is typical of a normal page in Lightning.

####Metadata

Content files start with the metadata in the format of key:value pairs.  That is, a key, followed by a colon (:), then the value.  Lines starting with \# are comments.

<div class="alert alert-info">There is no limit to the number of key:value paris you can have in your content files.  And, order is not important, either.</div>

Metadata is read until a blank line is found.  Everything after the blank line is considered the body.

This format has some conventions:

* Keys and values can only be single line entries.
* Keys and values are trimmed, so trailing and leading spaces are removed.
* Lines that start with # are comments.
* There are required keys in each file:
	* Slug
		* Slug is a required value and is the url of the page.
	    * Example:  the documentation part in the url http://lightning.simpdev.net/documentation-content-author.
	* Title
		* The title of the page.  
		* Example:  <title>Lightning - Documentation - Content Author</title>
	* Menu
		* The text that will show in the menu.  (Dependent of the Template implementation).
		* Example:  Documentation.  Although this key is required, it does not require a value.  This allows you to have pages in the system that are not referenced in the site navigation.
	* Enabled
	    * Enables or disables the page.  This is equivalent of removing the page from the site as the http module will not render disabled pages.
	* Position
	    * The position of the page in the menu.  This is a simple order by, so the numbers do not need to be sequential.
* And, there are optional keys that are supported by the bundled templates.  If you create your own templates, you may want to have your own custom key:values here.
	* Author
		* If populated, this will be included in the metadata of the page.
		* Example:  <meta name="author" content="Jesse Foster" />
	* Description
		* If populated, this will be included in the metadata of the page.
		* Example:  <meta name="description" content="Lightning documentation." />
	* Keywords
		* If populated, this will be included in the metadata of the page.
		* Example:  <meta name="keywords" content="lightning,documentation,page,metadata,templates,configuration,content,module,toolkit" />
	* Template
	    * If populated, this page will be rendered with the specified template.  If it is omitted, the default (configured) template will be used.
	* Url
	    * If populated, the menu will link to the url specified instead of a local page.

Any additional key:value paris can be added here.  There is no limitation on the number you can add.  They can be useful in the template for conditional rendering or passing values from the content to the page.
	
<div class="alert alert-info">Just because a key may be required (Menu for example) does not mean that a value is required.  For Menu, if you leave the value empty, the page will not show in the page navigation menu.  So, the key is required to be in the file, but it does not require a value.</div>

####Body

A blank line is used to identify the end of the metadata and the begining of the body.  Everything after the blank line is considered part of the body.

The body is run through a markdown processor, so html and markdown are valid in the body of the content files.

<div class="alert alert-info">Other processors could be used in place of a markdown, including no processor at all (if you prefer to write your pages in HTML, or possibly having your content generated from an automated system).</div>  This processing happens in the `ContentProvider` class and can easily be changed to suit your needs.

####Summary

The content files can be summarized by metadata (key:value pairs, one per line), a blank line, and the content that will be displayed in the body on the page.

<br />

<a href="/documentation" class="btn btn-primary">Back To Documentation</a>

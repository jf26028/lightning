Slug:  documentation-configuration
Title:  Lightning - Documentation - Configuration
Menu:
Enabled:  true
Position:  0
Author:  Jesse Foster
Description:  Lightning documentation for the configuration.
Keywords:  lightning,documentation,page,metadata,templates,configuration,content,module,toolkit
Template:  default

<ol class="breadcrumb">
  <li><a href="/">Home</a></li>
  <li><a href="/documentation">Documentation</a></li>
  <li class="active">Configuration Documentation</li>
</ol>

#Configuration Documentation

###web.config

The configuration is stored in the web.config as appSetting values, and changes the title of the webiste, where the content is stored, and the template used to render the site.

This configuration is from this site:

	<appSettings>
		<!-- The time files parsed from the hard disk are cached, in seconds. -->
		<add key="cacheTimeoutSeconds" value="3000" />
		
		<!-- The site title. -->
		<add key="siteTitle" value="Lightning" />
		
		<!-- The default template for the site.  Content files can override this value, but this template will be used by default. -->
		<add key="template" value="default" />
		
		<!-- The virtual path to where the content files are stored.  They are kept in the App_Data folder so they are protected.  -->
		<add key="contentVirtualPath" value="~/App_Data/content/" />
	</appSettings>

Some things to note:

* The cache has a dependency on the path in the `contentVirtualPath` value, so content will be fresh and cache will be cleared as new content is uploaded.  And, because of the cache invalidation, this value does not need to be changed for development environments, or rapid content prototyping.
* Setting `cacheTimeoutSeconds` to 0 will disable caching.

<br />

<a href="/documentation" class="btn btn-primary">Back To Documentation</a>

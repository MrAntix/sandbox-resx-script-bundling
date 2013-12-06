sandbox-resx-script-bundling
============================

Bundling Resx files to JavaScript using Microsoft.AspNet.Web.Optimization


example usage
-------------

	  bundles.Add(
		  new ResxScriptBundle("~/bundles/local")
			  .Include<JsResources>("app")
			  .Include<CommonResources>("common")
		  );
      
              
example output (unminified)
---------------------------

English:

	  window.resources=window.resources||{};
	  window.resources.app = {
		"HelloWorld": "Hello World, from JSResources.resx"
	  };
	  window.resources.common = {
		"CompanyName": "Antix Software Limited",
		"English": "English",
		"French": "Français (fallback to english)",
		"German": "Deutsch",
		"Title": "ResxScriptBundle"
	  };

German:

	  window.resources=window.resources||{};
	  window.resources.app = {
		"HelloWorld": "Hallo Welt, von JSResources.resx"
	  };
	  window.resources.common = {
		"CompanyName": "Antix Software Limited",
		"English": "English",
		"French": "Français (fallback auf englisch)",
		"German": "Deutsch",
		"Title": "ResxScriptBundle"
	  };

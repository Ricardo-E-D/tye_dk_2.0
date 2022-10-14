using Umbraco.Core;
using umbraco.editorControls;
using umbraco.interfaces;
using System;

namespace UmbConsole
{
    /// <summary>
    /// Extends the CoreBootManager for use in this Console app.
    /// </summary>
    public class ConsoleBootManager : CoreBootManager
    {
        public ConsoleBootManager(UmbracoApplicationBase umbracoApplication) : base(umbracoApplication)
        {
            //This is only here to ensure references to the assemblies needed for the DataTypesResolver
            //otherwise they won't be loaded into the AppDomain.
            var interfacesAssemblyName = typeof(IDataType).Assembly.FullName;
            //var editorControlsAssemblyName = typeof(uploadField).Assembly.FullName;
        }

        /// <summary>
        /// Can be used to initialize our own Application Events
        /// </summary>
        protected override void InitializeApplicationEventsResolver()
        {
			  try {
				  base.InitializeApplicationEventsResolver();
			  } catch (Exception) { }
        }
		 
        /// <summary>
        /// Can be used to add custom resolvers or overwrite existing resolvers once they are made public
        /// </summary>
        protected override void InitializeResolvers()
        {
            base.InitializeResolvers();
        }
    }
}
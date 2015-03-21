using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Emeraldwalk.Emeraldwalk_LanguageServices.Perl;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Utilities;
using System.Linq;
using System.Collections.Generic;

namespace Emeraldwalk.Emeraldwalk_LanguageServices
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidEmeraldwalk_LanguageServicesPkgString)]
    /// Language service stuff
    [ProvideService(
        typeof(PerlLanguageService),
        ServiceName=PerlLanguageService.SERVICE_NAME)]
    [ProvideLanguageService(
        typeof(PerlLanguageService),
        PerlLanguageService.LANGUAGE_NAME,
        PerlLanguageService.LANGUAGE_RESOURCE_ID
        //CodeSense             = true,
        //RequestStockColors    = false,
        //EnableAsyncCompletion = true,
        //MatchBraces           = true,
        //MatchBracesAtCaret    = true
        //AutoOutlining=true)
        )]
    //[ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideLanguageExtension(typeof(PerlLanguageService), ".pl")]
    public sealed class Emeraldwalk_LanguageServicesPackage : Package
    {
        private IdleHandler _idleHandler { get; set; }

        public Emeraldwalk_LanguageServicesPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            IVsFontAndColorCacheManager mgr = this.GetService(typeof(SVsFontAndColorCacheManager)) as IVsFontAndColorCacheManager;
            mgr.ClearAllCaches();

            //IComponentModel componentModel = (IComponentModel)this.GetService(typeof(SComponentModel));

            //IContentTypeRegistryService contentTypeRegistryService = componentModel.GetService<IContentTypeRegistryService>();
            //IContentType perlContentType = contentTypeRegistryService.GetContentType(PerlLanguageService.LANGUAGE_NAME);
            
            //IFileExtensionRegistryService fileExtensionRegistryService = componentModel.GetService<IFileExtensionRegistryService>();
            //IList<string> exts = fileExtensionRegistryService.GetExtensionsForContentType(perlContentType).ToList();
            //foreach (string ext in exts)
            //{
            //    fileExtensionRegistryService.RemoveFileExtension(ext);
            //}
            //fileExtensionRegistryService.AddFileExtension(".pl", perlContentType);

            //if (perlContentType != null)
            //{
            //    IList<string> exts = fileExtensionRegistryService.GetExtensionsForContentType(perlContentType).ToList();
            //    foreach (string ext in exts)
            //    {
            //        fileExtensionRegistryService.RemoveFileExtension(ext);
            //    }

            //    contentTypeRegistryService.RemoveContentType(Perl.Constants.ContentType);
            //}

            //perlContentType = contentTypeRegistryService.AddContentType(Perl.Constants.ContentType, new List<string> { "code" });
            //fileExtensionRegistryService.AddFileExtension(".emeraldwalk", perlContentType);
            
            //var contentTypes = contentTypeRegistryService.ContentTypes.ToList();

            LanguageService languageService = new PerlLanguageService();
            languageService.SetSite(this);
            
            IServiceContainer serviceContainer = this as IServiceContainer;
            serviceContainer.AddService(typeof(PerlLanguageService), languageService, true);

            IOleComponentManager oleComponentManager = this.GetService(typeof(SOleComponentManager)) as IOleComponentManager;
            this._idleHandler = new IdleHandler(
                oleComponentManager, 
                languageService.OnIdle);
        }

        protected override void Dispose(bool disposing)
        {
            if(this._idleHandler != null && !this._idleHandler.Disposed)
            {
                this._idleHandler.Dispose();
                this._idleHandler = null;
            }

            base.Dispose(disposing);
        }
    }
}

﻿//------------------------------------------------------------------------------
// <copyright file="CommandShowTomatoStatusBarPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using djfoxer.HealthyWithVS.Services;
using djfoxer.HealthyWithVS.Options;

namespace djfoxer.HealthyWithVS
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(CommandShowTomatoStatusBarPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideToolWindow(typeof(djfoxer.HealthyWithVS.ToolWindow.PomodoroToolWindow))]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class CommandShowTomatoStatusBarPackage : AsyncPackage
    {
        /// <summary>
        /// CommandShowTomatoStatusBarPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "03b63e3b-39cd-4c93-98b6-42cf447f55e6";

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandShowTomatoStatusBar"/> class.
        /// </summary>
        public CommandShowTomatoStatusBarPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
            HealthyWithVSSettingsService.Instance.AutostartPomodoroStatusBar = ((OptionPage)GetDialogPage(typeof(OptionPage))).AutostartPomodoroStatusBar;
            HealthyWithVSSettingsService.Instance.WorkTimerSeconds = ((OptionPage)GetDialogPage(typeof(OptionPage))).WorkTimerSeconds;
            HealthyWithVSSettingsService.Instance.WorkoutActive = ((OptionPage)GetDialogPage(typeof(OptionPage))).WorkoutActive;
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override async System.Threading.Tasks.Task InitializeAsync(System.Threading.CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
           
            await base.InitializeAsync(cancellationToken, progress);

            // When initialized asynchronously, we *may* be on a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            // Otherwise, remove the switch to the UI thread if you don't need it.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            CommandShowTomatoStatusBar.Initialize(this);
            djfoxer.HealthyWithVS.ToolWindow.PomodoroToolWindowCommand.Initialize(this);
        }



        #endregion
    }
}

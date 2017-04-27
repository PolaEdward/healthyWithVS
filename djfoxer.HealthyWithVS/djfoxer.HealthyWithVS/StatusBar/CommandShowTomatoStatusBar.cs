﻿//------------------------------------------------------------------------------
// <copyright file="CommandShowTomatoStatusBar.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows.Controls;
using djfoxer.HealthyWithVS.Helpers;
using System.Windows;
using djfoxer.HealthyWithVS.StatusBar;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using djfoxer.HealthyWithVS.Services;
using djfoxer.HealthyWithVS.Options;

namespace djfoxer.HealthyWithVS
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class CommandShowTomatoStatusBar
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("2089436a-ed0c-4bae-b1a3-d16000d5e669");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandShowTomatoStatusBar"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private CommandShowTomatoStatusBar(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;


            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }

            if (HealthyWithVSSettingsService.Instance.AutostartPomodoroStatusBar)
            {
                UIService.Instance.TogglePomodoroTimerStatusBar();
            }

        }

        private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            var commandText = string.Empty;
            if (UIService.Instance.IsPomodoroTimerStatusBarVisible())
            {
                commandText = "Hide Pomodoro Status Bar";
            }
            else
            {
                commandText = "Show Pomodoro Status Bar";
            }
            (sender as OleMenuCommand).Text = commandText;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static CommandShowTomatoStatusBar Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new CommandShowTomatoStatusBar(package);
        }


        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            UIService.Instance.TogglePomodoroTimerStatusBar();
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Andrew Oakley">
//     Copyright (c) 2010 Andrew Oakley
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
//
//     You should have received a copy of the GNU Lesser General Public License
//     along with this program.  If not, see http://www.gnu.org/licenses.
// </copyright>
//-----------------------------------------------------------------------

namespace Shoutcast.Sample.Phone
{
    using System.Windows;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    /// <summary>
    /// This class represents our Windows Phone application.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Field used to avoid double-initialization.
        /// </summary>
        private bool phoneApplicationInitialized = false;

        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += this.Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            this.InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                // Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                // Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        /// <summary>
        /// Gets the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Method that is called when the application is launching (eg, from Start).
        /// This code will not execute when the application is reactivated.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">LaunchingEventArgs associated with this event.</param>
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        /// <summary>
        /// Method that is called when the application is activated (brought to foreground).
        /// This code will not execute when the application is first launched.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">ActivatedEventArgs associated with this event.</param>
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        /// <summary>
        /// Method that is called when the application is deactivated (sent to background).
        /// This code will not execute when the application is closing.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">DeactivatedEventArgs associated with this event.</param>
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        /// <summary>
        /// Method that is called when the application is closing (eg, user hit Back).
        /// This code will not execute when the application is deactivated.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">ClosingEventArgs associated with this event.</param>
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        /// <summary>
        /// Method that is called if a navigation fails.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">NavigationFailedEventArgs associated with this event.</param>
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Method that is called on Unhandled Exceptions.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">ApplicationUnhandledExceptionEventArgs associated with this event.</param>
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        /// <summary>
        /// Initializes the Windows Phone application.  Do not add any additional code to this method!
        /// </summary>
        private void InitializePhoneApplication()
        {
            if (this.phoneApplicationInitialized)
            {
                return;
            }

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            this.RootFrame = new PhoneApplicationFrame();
            this.RootFrame.Navigated += this.CompleteInitializePhoneApplication;

            // Handle navigation failures
            this.RootFrame.NavigationFailed += this.RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            this.phoneApplicationInitialized = true;
        }

        /// <summary>
        /// Finalizes the initialization of the Windows Phone application.  Do not add any additional code to this method!
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">NavigationEventArgs associated with this event.</param>
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != this.RootFrame)
            {
                RootVisual = this.RootFrame;
            }

            // Remove this handler since it is no longer needed
            this.RootFrame.Navigated -= this.CompleteInitializePhoneApplication;
        }

        #endregion
    }
}
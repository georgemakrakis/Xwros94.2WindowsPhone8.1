//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Andrew Oakley">
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
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Microsoft.Phone.Controls;
    using Silverlight.Media;
    using System.Windows.Media.Imaging;
    /// <summary>
    /// This class represents the main page of our Windows Phone application.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "When the page is unloaded, the ShoutcastMediaStreamSource is disposed.")]
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// ShoutcastMediaStreamSource representing a Shoutcast stream.
        /// </summary>
        private ShoutcastMediaStreamSource source;
        //on_off variable for play/pause
        private bool on_off=false;

        /// <summary>
        /// Boolean to stop the status update if an error has occured.
        /// </summary>
        private bool errorOccured;

        /// <summary>
        /// Initializes a new instance of the MainPage class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the media element resource of the page.
        /// </summary>
        private MediaElement MediaPlayer
        {
            get { return this.Resources["mediaPlayer"] as MediaElement; }
        }

        /// <summary>
        /// Event handler called when the buffering progress of the media element has changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">RoutedEventArgs associated with this event.</param>
        private void BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            this.UpdateStatus();
        }

        /// <summary>
        /// Event handler called when the an exception is thrown parsing the streaming media.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">ExceptionRoutedEventArgs associated with this event.</param>
        private void MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.errorOccured = true;
            this.statusTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Error:  {0}", e.ErrorException.Message);
        }

        /// <summary>
        /// Event handler called when the play button is clicked.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">RoutedEventArgs associated with this event.</param>
        private void PlayClick(object sender, RoutedEventArgs e)
        {
            if (on_off == false)
            {
                
                on_off = true;

                this.ResetMediaPlayer();

                play_pause.Background = new ImageBrush
                { ImageSource = new BitmapImage(new Uri("/Assets/appbar.transport.pause.rest.png", UriKind.Relative)) };

                Uri uri = new Uri("http://195.251.162.97:8000/stream.ogg");// MP3
                this.source = new ShoutcastMediaStreamSource(uri);
                this.source.MetadataChanged += this.MetadataChanged;
                this.MediaPlayer.SetSource(this.source);
                this.MediaPlayer.Play();
   
            }
            else if(on_off==true)
            {
                on_off = false;
                
                play_pause.Background = new ImageBrush
                { ImageSource = new BitmapImage(new Uri("/Assets/appbar.transport.play.rest.png", UriKind.Relative)) };
                this.MediaPlayer.Pause();
                //this.MediaPlayer.Stop();
                //this.MediaPlayer.Source = null;
            }
            
        }

        /// <summary>
        /// Event handler called when the metadata of the Shoutcast stream source changes.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">MpegMetadataEventArgs associated with this event.</param>
        private void MetadataChanged(object sender, RoutedEventArgs e)
        {
            this.UpdateStatus();
        }

        /// <summary>
        /// Updates the text block to show the MediaStreamSource's status in the UI.
        /// </summary>
        private void UpdateStatus()
        {
            // If we have an error, we don't want to overwrite the error status.
            if (this.errorOccured)
            {
                MessageBox.Show("A problem occured. Please try again later.", "Error", MessageBoxButton.OK);  
            }

            MediaElementState state = this.MediaPlayer.CurrentState;
            string status = string.Empty;
            switch (state)
            {
                case MediaElementState.Buffering:
                    status = string.Format(CultureInfo.InvariantCulture, "Buffering...{0:0%}", this.MediaPlayer.BufferingProgress);
                    play_pause.IsEnabled = false;
                    break;
                case MediaElementState.Playing:
                    status = string.Format(CultureInfo.InvariantCulture, "Title: {0}", this.source.CurrentMetadata.Title);
                    play_pause.IsEnabled = true;
                    break;
                case MediaElementState.Opening:
                    status = "Opening";
                    //play_pause.IsEnabled = false;
                    break;
                default:
                    status = state.ToString();
                    break;
            }

            this.statusTextBlock.Text = status;
        }

        /// <summary>
        /// Event handler called when the media element state changes.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">RoutedEventArgs associated with this event.</param>
        private void CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            this.UpdateStatus();
        }

        /// <summary>
        /// Resets the media player.
        /// </summary>
        private void ResetMediaPlayer()
        {
            if ((this.MediaPlayer.CurrentState != MediaElementState.Stopped) && (this.MediaPlayer.CurrentState != MediaElementState.Closed))
            {
                this.MediaPlayer.Stop();
                this.MediaPlayer.Source = null;
                this.source.Dispose();
                this.source = null;
            }

            this.errorOccured = false;
        }

        /// <summary>
        /// Event handler called when this page is unloaded.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">RoutedEventArgs associated with this event.</param>
        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            this.ResetMediaPlayer();
        }
    }
}
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#endregion

namespace Hoover.Services
{
    /// <summary>
    /// Windows Phone implementation of the <see cref="NavigationService"/>.
    /// </summary>
    public sealed class NavigationService
    {

        #region Singleton

        /// <summary>
        /// Private constructor.
        /// </summary>
        private NavigationService()
        {
        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static NavigationService()
        {
        }

        private static readonly NavigationService _instance = new NavigationService();
        public static NavigationService Instance { get { return _instance; } }

        #endregion

        /// <summary>
        /// Used in navigation when passing parameters.
        /// </summary>
        private object _navigationData = null;

        /// <summary>
        /// This method retreives the last parameter passed in a navigation.
        /// </summary>
        /// <returns></returns>
        public object GetLastNavigationData()
        {
            object data = _navigationData;
            _navigationData = null;
            return data;
        }

        /// <summary>
        /// This method retreives the last parameter passed in a navigation with explicit type T.
        /// </summary>
        /// <typeparam name="T">Expected type of navigation parameter.</typeparam>
        /// <returns></returns>
        /// <exception cref="Exception">If the parameter with the specified type does not exist.</exception>
        public T GetLastNavigationParameter<T>()
        {
            if (_navigationData is T)
            {
                T data = (T) _navigationData;
                _navigationData = null;
                return data;
            }
            
            throw new Exception();
        }

        /// <summary>
        /// Sets the navigation parameter explicitly with the given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        public void SetNavigationParameter<T>(T parameter)
        {
            _navigationData = parameter;
        }

        /// <summary>
        /// Access to the current frame of the application.
        /// </summary>
        public static Frame Frame
        {
            get { return Application.Current.RootVisual as Frame; }
        }

        /// <summary>
        /// Navigate to the home page.
        /// </summary>
        public void GoHome()
        {
            if (Frame != null)
            {
                while (Frame.CanGoBack) Frame.GoBack();
            }
        }

        /// <summary>
        /// Navigate one page back if can go back.
        /// </summary>
        public void GoBack()
        {
            if (Frame != null && Frame.CanGoBack) Frame.GoBack();
        }

        /// <summary>
        /// Navigate one page forward if can go forward.
        /// </summary>
        public void GoForward()
        {
            if (Frame != null && Frame.CanGoForward) Frame.GoForward();
        }

        /// <summary>
        /// Is any navigation currently in progress.
        /// </summary>
        public bool IsNavigating { get; private set; }

        /// <summary>
        /// Navigate tho the specified page.
        /// </summary>
        /// <param name="pageName">Name of the page. MainPageView, DetailsPageView etc.</param>
        public void Navigate(string pageName)
        {
            Navigate(pageName, null);
        }

        /// <summary>
        /// Navigate to the specified page and include parameter.
        /// </summary>
        /// <param name="pageName">Name of the page. MainPageView.xaml, Views/DetailsPageView.xaml etc.</param>
        /// <param name="parameter">Parameter to include in navigation.</param>
        public void Navigate(string pageName, object parameter)
        {
            IsNavigating = true;

            if (parameter != null)
            {
                _navigationData = parameter;
            }

            Frame.Navigated += FrameOnNavigated;

            Frame.Navigate(new Uri(pageName, UriKind.Relative));
        }

        /// <summary>
        /// Executed when the frame is done navigating to a page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrameOnNavigated(object sender, NavigationEventArgs e)
        {
            Frame.Navigated -= FrameOnNavigated;
            IsNavigating = false;
        }

        public bool CanGoBack
        {
            get { return Frame.CanGoBack; }
        }

        public bool CanGoForward
        {
            get { return Frame.CanGoForward; }
        }
    }

    /// <summary>
    /// All the page paths defined.
    /// </summary>
    public sealed class PageNames
    {
        #region Page Names Used In Navigation

        public static string MainPageName { get { return "/Views/MainPage.xaml"; } }
		public static string TrackingPageName { get { return "/Views/TrackingPage.xaml"; } }
        public static string SettingsPageName { get { return "/Views/SettingsPage.xaml"; } }
        public static string AboutPageName { get { return "/Views/AboutPage.xaml"; } }
        public static string HistoryDetailsPageName { get { return "/Views/HistoryDetailsPage.xaml"; } }

        #endregion
    }
}
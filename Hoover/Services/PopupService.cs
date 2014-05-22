#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hoover.Helpers;
using Hoover.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;

#endregion

namespace Hoover.Services
{
    /// <summary>
    /// This singleton contains the logic for displaying specialized popups.
    /// </summary>
    public sealed class PopupService
    {

        #region Singleton

        /// <summary>
        /// Private constructor.
        /// </summary>
        private PopupService()
        {

        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static PopupService()
        {
        }

        private static readonly PopupService _instance = new PopupService();

        public static PopupService Instance
        {
            get { return _instance; }
        }

        #endregion

        /// <summary>
        /// Displays the weather popup. Returns a <see cref="Task"/> which will be completed after the popup is closed.
        /// </summary>
        public async Task ShowWeatherPopup()
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                //await ShowPopup(new PlayerDetailsView(playerId));
            });
        }

        /// <summary>
        /// Displays popup for session completion. Returns a <see cref="Task"/> which will be completed after the popup is closed.
        /// </summary>
        public async Task ShowSessionCompletedPopup(HistoryItem historyItem)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                //await ShowPopup(new PlayerDetailsView(playerId));
            });
        }

        /// <summary>
        /// Shows the popup
        /// </summary>
        /// <param name="control">The view which will be displayed in the popup.</param>
        /// <param name="closingSource">When not null, this source will be set to close the window.</param>
        /// <returns></returns>
        private async Task ShowPopup(UserControl control, TaskCompletionSource<bool> closingSource = null)
        {
            TaskCompletionSource<bool> waitForClose = closingSource;

            RadModalWindow window = new RadModalWindow();
            window.Background = new SolidColorBrush(Colors.Black)
            {
                Opacity = 0.5
            };
            window.Placement = PlacementMode.CenterCenter;
            window.IsClosedOnOutsideTap = true;
            window.HorizontalAlignment = HorizontalAlignment.Stretch;
            window.VerticalAlignment = VerticalAlignment.Center;

            window.Content = control;

            window.IsAnimationEnabled = true;

            window.OpenAnimation = AnimationsFactory.RadMoveAnimationDefault();
            window.CloseAnimation = AnimationsFactory.RadMoveAnimationRemoveDefault();

            // Hide app bar before opening
            IApplicationBar appBar = null;
            var currentFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (currentFrame != null)
            {
                var currentPage = currentFrame.Content as PhoneApplicationPage;
                if (currentPage != null)
                {
                    appBar = currentPage.ApplicationBar;
                }
            }
            bool beforePopupAppBarState = true;
            if (appBar != null)
            {
                beforePopupAppBarState = appBar.IsVisible;
                appBar.IsVisible = false;
            }

            // Show app bar on closing
            if (window.CloseAnimation == null)
            {
                window.WindowClosed += (sender, args) =>
                {
                    if (appBar != null)
                    {
                        appBar.IsVisible = beforePopupAppBarState;
                    }
                    if (waitForClose != null) waitForClose.TrySetResult(true);
                };
            }
            else
            {
                window.CloseAnimation.Ended += (sender, args) =>
                {
                    if (appBar != null)
                    {
                        appBar.IsVisible = beforePopupAppBarState;
                    }
                    if (waitForClose != null) waitForClose.TrySetResult(true);
                };
            }

            window.IsOpen = true;

            if (waitForClose != null)
            {
                await waitForClose.Task;
                window.IsOpen = false;
            }
        }

    }
}
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;

#endregion

namespace Hoover.Helpers
{
    /// <summary>
    /// Helper for retreiving animations.
    /// </summary>
    public static class AnimationsFactory
    {
        /// <summary>
        /// Returns the <see cref="RadEmptyAnimation"/>.
        /// </summary>
        /// <returns></returns>
        public static RadEmptyAnimation RadEmptyAnimation()
        {
            return new RadEmptyAnimation();
        }

        /// <summary>
        /// Default <see cref="RadMoveAndFadeAnimation"/>.
        /// </summary>
        /// <returns></returns>
        public static RadMoveAndFadeAnimation RadMoveAndFadeAnimationDefault()
        {
            var moveAndFade = new RadMoveAndFadeAnimation();
            moveAndFade.MoveAnimation.StartPoint = new Point(0, -90);
            moveAndFade.MoveAnimation.EndPoint = new Point(0, 0);
            moveAndFade.FadeAnimation.StartOpacity = 0;
            moveAndFade.FadeAnimation.EndOpacity = 1;
            moveAndFade.Easing = new CubicEase();

            return moveAndFade;
        }

        /// <summary>
        /// Default <see cref="RadMoveAndFadeAnimation"/>.
        /// </summary>
        /// <returns></returns>
        public static RadMoveAnimation RadMoveAnimationDefault()
        {
            var move = new RadMoveAnimation();
            move.StartPoint = new Point(0, -90);
            move.EndPoint = new Point(0, 0);
            move.Easing = new CubicEase();

            return move;
        }

        /// <summary>
        /// Default <see cref="RadMoveAnimation"/>
        /// </summary>
        /// <returns></returns>
        public static RadMoveAnimation RadMoveAnimationRemoveDefault()
        {
            var moveAnimation = new RadMoveAnimation();
            moveAnimation.StartPoint = new Point(0, 0);
            moveAnimation.EndPoint = new Point(-500, 0);
            moveAnimation.Duration = TimeSpan.FromMilliseconds(300);
            moveAnimation.Easing = new CubicEase() { EasingMode = EasingMode.EaseOut };

            return moveAnimation;
        }

        /// <summary>
        /// Default <see cref="RadPlaneProjectionAnimation"/>.
        /// </summary>
        /// <returns></returns>
        public static RadPlaneProjectionAnimation RadPlaneProjectionAnimation()
        {
            return new RadPlaneProjectionAnimation
            {
                CenterY = 0.5,
                CenterX = 0,
                StartAngleX = -90,
                EndAngleX = 0,
                Axes = PerspectiveAnimationAxis.X,
                Easing = new CubicEase()
                {
                    EasingMode = EasingMode.EaseOut
                }
            };
        }
    }
}
#region

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Telerik.Windows.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion

namespace Hoover.Behaviors
{
    /// <summary>
    /// This behavior class is a command behavior implementation to be invoked when an item is clicked in a <see cref="ListBox"/>.
    /// </summary>
    public static class ItemClickCommandBehavior
    {
        #region Command Attached Property

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof (ICommand), typeof (ItemClickCommandBehavior),
                                                new PropertyMetadata(null, OnCommandChanged));

        #endregion

        #region Behavior implementation

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox)
            {
                ListBox listBox = d as ListBox;
                listBox.Tap += OnTap;
            }
            else if (d is RadDataBoundListBox)
            {
                RadDataBoundListBox listBox = d as RadDataBoundListBox;
                listBox.Tap += OnTap;
            }
            else return;
        }

        private static void OnTap(object sender, GestureEventArgs e)
        {
            //if (e.Handled) return;

            object commandParameter;
            ICommand cmd;
            if (sender is ListBox)
            {
                ListBox lb = sender as ListBox;
                cmd = lb.GetValue(CommandProperty) as ICommand;
                commandParameter = lb.SelectedItem;
                lb.SelectedItem = null;
            }
            else if (sender is RadDataBoundListBox)
            {
                RadDataBoundListBox lb = sender as RadDataBoundListBox;
                cmd = lb.GetValue(CommandProperty) as ICommand;
                commandParameter = lb.SelectedItem;
                lb.SelectedItem = null;
            }
            else return;

            try
            {
                if (cmd != null && cmd.CanExecute(commandParameter))
                {
                    cmd.Execute(commandParameter);
                }
            }
            catch (Exception)
            {
                // Don't execute command
            }
        }

        #endregion
    }
}
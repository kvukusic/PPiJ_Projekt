#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GART.X3D;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using MathHelper = Microsoft.Xna.Framework.MathHelper;

#endregion

namespace Hoover__n_Recreate.Views
{
    public partial class TestPageView : PhoneApplicationPage
    {
        //private Motion _motion;
        //private PhotoCamera _cam;

        //private List<TextBlock> _textBlocks;
        //private List<GART.X3D.Vector3> _points;
        //private System.Windows.Point _pointOnScreen;

        //private Viewport _viewport;
        //private GART.X3D.Matrix _projection;
        //private GART.X3D.Matrix _view;
        //private GART.X3D.Matrix _attitude;

        //// Constructor
        //public TestPageView()
        //{
        //    InitializeComponent();

        //    // Initialize the list of TextBlock and Vector3 objects.
        //    _textBlocks = new List<TextBlock>();
        //    _points = new List<Vector3>();
        //}

        ///// <summary>
        ///// Helper method that initializes the Viewport and Matrix objects that are used to
        ///// transform points from screen space to world space and back.
        ///// </summary>
        //public void InitializeViewport()
        //{
        //    // Initialize the viewport and matrixes for 3d projection.
        //    _viewport = new Viewport(0, 0, (int) this.ActualWidth, (int) this.ActualHeight);
        //    float aspect = _viewport.AspectRatio;
        //    _projection = GART.X3D.Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 12);
        //    _view = GART.X3D.Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up);
        //}

        ///// <summary>
        ///// Called when a page becomes the active page in a frame.
        ///// </summary>
        ///// <param name="e">An object that contains the event data.</param>
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    // Initialize the camera and set the video brush source.
        //    _cam = new Microsoft.Devices.PhotoCamera();
        //    ViewfinderBrush.SetSource(_cam);

        //    if (!Motion.IsSupported)
        //    {
        //        MessageBox.Show("the Motion API is not supported on this device.");
        //        return;
        //    }

        //    // If the Motion object is null, initialize it and add a CurrentValueChanged
        //    // event handler.
        //    if (_motion == null)
        //    {
        //        _motion = new Motion();
        //        _motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
        //        _motion.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<MotionReading>>(_motion_CurrentValueChanged);
        //    }

        //    // Try to start the Motion API.
        //    try
        //    {
        //        _motion.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("unable to start the Motion API.");
        //    }

        //    // Hook up the event handler for when the user taps the screen.
        //    this.MouseLeftButtonUp += new MouseButtonEventHandler(MainPage_MouseLeftButtonUp);

        //    base.OnNavigatedTo(e);
        //    //ArDisplay.StartServices();
        //}

        ///// <summary>
        ///// Called when a page is no longer the active page in a frame.
        ///// </summary>
        ///// <param name="e">An object that contains the event data.</param>
        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{
        //    // Dispose camera to minimize power consumption and to expedite shutdown.
        //    _cam.Dispose();

        //    base.OnNavigatedFrom(e);
        //    //ArDisplay.StopServices();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void MainPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    // If the Canvas containing the TextBox is visible, ignore
        //    // this event.
        //    if (TextBoxCanvas.Visibility == Visibility.Visible)
        //    {
        //        return;
        //    }

        //    // Save the location where the user touched the screen.
        //    _pointOnScreen = e.GetPosition(LayoutRoot);

        //    // Save the device attitude when the user touched the screen.
        //    _attitude = _motion.CurrentValue.Attitude.RotationMatrix;

        //    // Make the Canvas containing the TextBox visible and
        //    // give the TextBox focus.
        //    TextBoxCanvas.Visibility = Visibility.Visible;
        //    NameTextBox.Focus();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void _motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        //{
        //    // This event arrives on a background thread. Use BeginInvoke
        //    // to call a method on the UI thread.
        //    Dispatcher.BeginInvoke(() => CurrentValueChanged(e.SensorReading));
        //}

        //private void CurrentValueChanged(MotionReading reading)
        //{
        //    // If the viewport width is 0, it needs to be initialized.
        //    if (_viewport.Width == 0)
        //    {
        //        InitializeViewport();
        //    }

        //    // Get the RotationMatrix from the MotionReading.
        //    // Rotate it 90 degrees around the X axis
        //    //   to put it in the XNA Framework coordinate system.
        //    GART.X3D.Matrix attitude = GART.X3D.Matrix.CreateRotationX(MathHelper.PiOver2) * reading.Attitude.RotationMatrix;

        //    // Loop through the points in the list.
        //    for (int i = 0; i < _points.Count; i++)
        //    {
        //        // Create a World matrix for the point.
        //        Matrix world = Matrix.CreateWorld(_points[i], new Vector3(0, 0, 1), new Vector3(0, 1, 0));

        //        // Use Viewport.Project to project the point from 3D space into screen coordinates.
        //        Vector3 projected = _viewport.Project(GART.X3D.Vector3.Zero, _projection, _view, world * attitude);

        //        if (projected.Z > 1 || projected.Z < 0)
        //        {
        //            // If the point is outside of this range, it is behind the camera.
        //            // So hide the TextBlock for this point.
        //            _textBlocks[i].Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            // Otherwise, show the TextBlock.
        //            _textBlocks[i].Visibility = Visibility.Visible;

        //            // Create a TranslateTransform to position the TextBlock.
        //            // Offset by half of the TextBlock's RenderSize to center it on the point.
        //            TranslateTransform tt = new TranslateTransform();
        //            tt.X = projected.X - (_textBlocks[i].RenderSize.Width / 2);
        //            tt.Y = projected.Y - (_textBlocks[i].RenderSize.Height / 2);
        //            _textBlocks[i].RenderTransform = tt;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void NameTextBox_KeyUp(object sender, KeyEventArgs e)
        //{
        //    // If the key is not the Enter key, don't do anything.
        //    if (e.Key != Key.Enter)
        //    {
        //        return;
        //    }

        //    // When the TextBox loses focus. Hide the Canvas containing it.
        //    TextBoxCanvas.Visibility = Visibility.Collapsed;

        //    // If any of the objects we need are not present, exit the event handler.
        //    if (NameTextBox.Text == "" || _pointOnScreen == null || _motion == null)
        //    {
        //        return;
        //    }

        //    // Translate the point before projecting it.
        //    System.Windows.Point p = _pointOnScreen;
        //    p.X = LayoutRoot.RenderSize.Width - p.X;
        //    p.Y = LayoutRoot.RenderSize.Height - p.Y;
        //    p.X *= .5;
        //    p.Y *= .5;

        //    // Use the attitude Matrix saved in the OnMouseLeftButtonUp handler.
        //    // Rotate it 90 degrees around the X axis
        //    // to put it in the XNA Framework coordinate system.
        //    _attitude = Matrix.CreateRotationX(MathHelper.PiOver2) * _attitude;


        //    // Use Viewport.Unproject to translate the point on the screen to 3D space.
        //    Vector3 unprojected = _viewport.Unproject(new Vector3((float) p.X, (float) p.Y, -.9f), _projection, _view, _attitude);
        //    unprojected.Normalize();
        //    unprojected *= -10;

        //    // Call the helper method to add this point.
        //    AddPoint(unprojected, NameTextBox.Text);

        //    // Clear the TextBox.
        //    NameTextBox.Text = "";
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="point"></param>
        ///// <param name="name"></param>
        //private void AddPoint(Vector3 point, string name)
        //{
        //    // Create a new TextBlock. Set the Canvas.ZIndexProperty to make sure
        //    // it appears above the camera rectangle.
        //    TextBlock textblock = new TextBlock();
        //    textblock.Text = name;
        //    textblock.FontSize = 124;
        //    textblock.SetValue(Canvas.ZIndexProperty, 2);
        //    textblock.Visibility = Visibility.Collapsed;

        //    // Add the TextBlock to the LayoutRoot container.
        //    LayoutRoot.Children.Add(textblock);

        //    // Add the TextBlock and the point to the List collections.
        //    _textBlocks.Add(textblock);
        //    _points.Add(point);
        //}
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AllJoynSampleApp.Controls
{
    public sealed partial class ColorPicker : UserControl
    {
        private bool isDragging;

        public ColorPicker()
        {
            this.InitializeComponent();
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            element.CapturePointer(e.Pointer);
            isDragging = true;
        }

        private void Grid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isDragging)
            {
                var element = sender as FrameworkElement;
                var hs = GetHS(e.GetCurrentPoint(element).Position);
                marker.Margin = new Thickness(hs.Hue / 360 * PickerArea.ActualWidth, hs.Saturation * PickerArea.ActualHeight, 0, 0);
            }
        }

        private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                var element = sender as FrameworkElement;
                var hs = GetHS(e.GetCurrentPoint(element).Position);
                Hue = hs.Hue;
                Saturation = hs.Saturation;
                ColorPicked?.Invoke(this, new HS() { Hue = hs.Hue, Saturation = hs.Saturation });
            }
        }

        private HS GetHS(Point p)
        {
            if (p.X < 0) p.X = 0;
            if (p.Y < 0) p.Y = 0;
            if (p.X > PickerArea.ActualWidth) p.X = PickerArea.ActualWidth;
            if (p.Y > PickerArea.ActualHeight) p.Y = PickerArea.ActualHeight;
            double x = p.X / PickerArea.ActualWidth * 360;
            double y = p.Y / PickerArea.ActualHeight;
            return new HS() { Hue = x, Saturation = y };
        }

        private void UpdateMarker()
        {
            double x = Hue % 360 / 360 * PickerArea.ActualWidth;
            double y = Saturation * PickerArea.ActualHeight;
            marker.Margin = new Thickness(x, y, 0, 0);
        }

        public double Hue
        {
            get { return (double)GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }

        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(double), typeof(ColorPicker), new PropertyMetadata(0d, OnHuePropertyChanged));

        private static void OnHuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).UpdateMarker();
        }

        public double Saturation
        {
            get { return (double)GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(ColorPicker), new PropertyMetadata(0d, OnSaturationPropertyChanged));

        private static void OnSaturationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).UpdateMarker();
        }

        public event EventHandler<HS> ColorPicked;

        public struct HS
        {
            public double Hue;
            public double Saturation;
        }
    }
}

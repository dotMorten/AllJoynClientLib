using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace AllJoynSampleApp.Controls
{

    [TemplatePart(Name = "Slider")]
    public abstract class ColorRangePicker : Control
    {
        private Slider slider;
        protected abstract GradientStopCollection GetColorGradient();

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            slider = GetTemplateChild("Slider") as Slider;
            if (slider != null)
            {
                Binding b = new Binding()
                {
                    Path = new PropertyPath(GetValuePropertyName()),
                    Mode = BindingMode.TwoWay,
                    Source  = this
                };
                b.Converter = new BindingConverter(this);
                slider.SetBinding(Slider.ValueProperty, b);
                slider.ThumbToolTipValueConverter = new ThumbTipConverter(this);
            }
            InvalidateColorRangeBrush();
        }

        protected string ThumbTipFormatString { get; set; } = "0";

        private class BindingConverter : IValueConverter
        {
            ColorRangePicker _picker;
            public BindingConverter(ColorRangePicker picker) { _picker = picker; }
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                return _picker.ValueToPercentage((double)value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                return _picker.PercentageToValue((double)value);
            }
        }

        private class ThumbTipConverter : IValueConverter
        {
            ColorRangePicker _picker;
            public ThumbTipConverter(ColorRangePicker picker) { _picker = picker; }
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                return _picker.PercentageToValue((double)value).ToString(_picker.ThumbTipFormatString);
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotSupportedException();
            }
        }

        protected abstract double ValueToPercentage(double value);
        protected abstract double PercentageToValue(double value);

        protected abstract string GetValuePropertyName();
        
        protected void InvalidateColorRangeBrush()
        {
            if (slider != null)
            {
                var brush = new LinearGradientBrush(GetColorGradient(), 0);
                if (Orientation == Orientation.Vertical)
                {
                    brush.StartPoint = new Windows.Foundation.Point(0,1);
                    brush.EndPoint = new Windows.Foundation.Point();
                }
                slider.Background = brush;
            }
        }

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(ColorRangePicker), new PropertyMetadata(null));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(ColorRangePicker), new PropertyMetadata(Orientation.Horizontal, OnOrientationPropertyChanged));

        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ColorRangePicker).InvalidateColorRangeBrush();
        }
    }
}

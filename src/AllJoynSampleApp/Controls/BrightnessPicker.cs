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

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace AllJoynSampleApp.Controls
{    
    public sealed class BrightnessPicker : ColorRangePicker
    {
        public BrightnessPicker()
        {
            this.DefaultStyleKey = typeof(BrightnessPicker);
            ThumbTipFormatString = "0\\%";
        }
        
        protected override GradientStopCollection GetColorGradient()
        {
            return new GradientStopCollection()
                {
                    new GradientStop() { Color = Colors.Black },
                    new GradientStop() { Color = HsvToRgb(Hue, Saturation, 1), Offset = 1 },
                };
        }

        protected override string GetValuePropertyName()
        {
            return nameof(Brightness);
        }

        protected override double ValueToPercentage(double value)
        {
            return value;
        }
        protected override double PercentageToValue(double value)
        {
            return value;
        }

        static Windows.UI.Color HsvToRgb(double hue, double sat, double v)
        {
            hue = hue % 360;
            if (hue < 0) hue += 360;
            double r, g, b;
            if (v <= 0) { r = g = b = 0; }
            else if (sat <= 0) r = g = b = v;
            else
            {
                double hf = hue / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = v * (1 - sat);
                double qv = v * (1 - sat * f);
                double tv = v * (1 - sat * (1 - f));
                switch (i)
                {
                    case 0: r = v; g = tv; b = pv; break;
                    case 1: r = qv; g = v; b = pv; break;
                    case 2: r = pv; g = v; b = tv; break;
                    case 3: r = pv; g = qv; b = v; break;
                    case 4: r = tv; g = pv; b = v; break;
                    case 5: r = v; g = pv; b = qv; break;
                    case 6: r = v; g = tv; b = pv; break;
                    case -1: r = v; g = pv; b = qv; break;
                    default: r = g = b = v; break;
                }
            }
            return Windows.UI.Color.FromArgb(255, 
                (byte)Math.Min(255, (r * 255.0)),
                (byte)Math.Min(255, (g * 255.0)),
                (byte)Math.Min(255, (b * 255.0)));
        }

        public double Brightness
        {
            get { return (double)GetValue(BrightnessProperty); }
            set { SetValue(BrightnessProperty, value); }
        }

        public static readonly DependencyProperty BrightnessProperty =
            DependencyProperty.Register(nameof(Brightness), typeof(double), typeof(BrightnessPicker), new PropertyMetadata(0d));

        public double Saturation
        {
            get { return (double)GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register("Saturation", typeof(double), typeof(BrightnessPicker), new PropertyMetadata(1d, OnHuePropertyChanged));

        public double Hue
        {
            get { return (double)GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }

        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register(nameof(Hue), typeof(double), typeof(BrightnessPicker), new PropertyMetadata(0d, OnHuePropertyChanged));

        private static void OnHuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BrightnessPicker).InvalidateColorRangeBrush();
        }
    }
}

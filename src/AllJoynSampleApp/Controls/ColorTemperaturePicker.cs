using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AllJoynSampleApp.Controls
{
    public sealed class ColorTemperaturePicker : ColorRangePicker
    {
        public ColorTemperaturePicker()
        {
            this.DefaultStyleKey = typeof(ColorTemperaturePicker);
            ThumbTipFormatString = "0⁰K";
        }

        protected override GradientStopCollection GetColorGradient()
        {
            GradientStopCollection coll = new GradientStopCollection();
            for (int i = 0; i <= 5; i++)
            {
                coll.Add(
                    new GradientStop() { Color = TemperatureToColor((MinTemperature + MaxTemperature) * i / 5d + MinTemperature), Offset = i / 5d }
                );
            }
            return coll;
        }
        
        protected override double ValueToPercentage(double value)
        {
            return (value - MinTemperature) / (MaxTemperature - MinTemperature) * 100;
        }
        protected override double PercentageToValue(double value)
        {
            return value / 100 * (MaxTemperature - MinTemperature) + MinTemperature;
        }
        protected override string GetValuePropertyName()
        {
            return nameof(Temperature);
        }

        public double Temperature
        {
            get { return (double)GetValue(TemperatureProperty); }
            set { SetValue(TemperatureProperty, value); }
        }

        public static readonly DependencyProperty TemperatureProperty =
            DependencyProperty.Register(nameof(Temperature), typeof(double), typeof(ColorTemperaturePicker), new PropertyMetadata(5000d));
       
        public double MinTemperature
        {
            get { return (double)GetValue(MinTemperatureProperty); }
            set { SetValue(MinTemperatureProperty, value); }
        }

        public static readonly DependencyProperty MinTemperatureProperty =
            DependencyProperty.Register("MinTemperature", typeof(double), typeof(ColorTemperaturePicker), new PropertyMetadata(2500d));

        public double MaxTemperature
        {
            get { return (double)GetValue(MaxTemperatureProperty); }
            set { SetValue(MaxTemperatureProperty, value); }
        }

        public static readonly DependencyProperty MaxTemperatureProperty =
            DependencyProperty.Register("MaxTemperature", typeof(double), typeof(ColorTemperaturePicker), new PropertyMetadata(9000d));

        private static Color TemperatureToColor(double kelvin)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            //Temperature must fall between 1000 and 40000 degrees
            if (kelvin < 1000) kelvin = 1000;
            if (kelvin > 40000) kelvin = 40000;

            //All calculations require tmpKelvin / 100, so only do the conversion once
            double tmpKelvin = kelvin / 100;

            //Calculate each color in turn

            //First: red
            if (tmpKelvin <= 66)
                r = 255;
            else
            {   //Note: the R-squared value for this approximation is .988
                double tmp = tmpKelvin - 60;
                tmp = 329.698727446 * Math.Pow(tmp, -0.1332047592);
                if (tmp < 0) r = 0;
                else if (tmp > 255) r = 255;
                else
                    r = (byte)tmp;
            }

            //Second: green
            if (tmpKelvin <= 66)
            {
                //Note: the R-squared value for this approximation is .996
                double tmp = tmpKelvin;
                tmp = 99.4708025861 * Math.Log(tmp) - 161.1195681661;
                if (tmp < 0) g = 0;
                else if (tmp > 255) g = 255;
                else
                    g = (byte)tmp;
            }
            else
            {
                //Note: the R-squared value for this approximation is .987
                double tmp = tmpKelvin - 60;
                tmp = 288.1221695283 * Math.Pow(tmp, -0.0755148492);
                if (tmp < 0) g = 0;
                if (tmp > 255) g = 255;
                else g = (byte)tmp;
            }

            //Third: blue
            if (tmpKelvin >= 66)
                b = 255;
            else if (tmpKelvin <= 19)
                b = 0;
            else
            {
                //Note: the R-squared value for this approximation is .998
                double tmp = tmpKelvin - 10;
                tmp = 138.5177312231 * Math.Log(tmp) - 305.0447927307;

                if (tmp < 0) b = 0;
                else if (tmp > 255) b = 255;
                else b = (byte)tmp;
            }
            return Color.FromArgb(255, r, g, b);
        }
    }
}

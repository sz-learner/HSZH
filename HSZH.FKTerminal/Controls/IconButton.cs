using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HSZH.FKTerminal.Controls
{

    public class IconButton : Button
    {
        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }

        public int IconImageSize
        {
            get { return (int)GetValue(IconImageSizeProperty); }
            set { SetValue(IconImageSizeProperty, value); }
        }

        public string IconImage
        {
            get { return (string)GetValue(IconImageProperty); }
            set { SetValue(IconImageProperty, value); }
        }

        public static DependencyProperty IconImageSizeProperty =
            DependencyProperty.Register("IconImageSize", typeof(int), typeof(IconButton),
            new FrameworkPropertyMetadata(32, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IconImageProperty =
           DependencyProperty.Register("IconImage", typeof(string), typeof(IconButton),
           new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue));
            }
            catch (Exception)
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Biller.Controls
{
    public class WatermarkTextBox : TextBox
    {
        static WatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));

            TextProperty.OverrideMetadata(
                typeof(WatermarkTextBox),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(TextPropertyChanged)));
        }

        public WatermarkTextBox()
        {
            this.GotFocus += WatermarkTextBox_GotFocus;
        }

        public static readonly DependencyProperty TextBoxInfoProperty = DependencyProperty.Register(
            "TextBoxInfo",
            typeof(string),
            typeof(WatermarkTextBox),
            new PropertyMetadata(string.Empty));

        public string Watermark
        {
            get { return (string)GetValue(TextBoxInfoProperty); }
            set { SetValue(TextBoxInfoProperty, value); }
        }

        private static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
            "HasText",
            typeof(bool),
            typeof(WatermarkTextBox),
            new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        public bool HasText
        {
            get
            {
                return (bool)GetValue(HasTextProperty);
            }
        }

        private static readonly DependencyProperty SelectTextBeforeSpaceProperty = DependencyProperty.Register(
            "SelectTextBeforeSpace",
            typeof(bool),
            typeof(WatermarkTextBox),
            new PropertyMetadata(false));
        
        public bool SelectTextBeforeSpace
        {
            get { return (bool)GetValue(SelectTextBeforeSpaceProperty); }
            set { SetValue(SelectTextBeforeSpaceProperty, value); }
        }


        static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            WatermarkTextBox itb = (WatermarkTextBox)sender;

            bool actuallyHasText = itb.Text.Length > 0;
            if (actuallyHasText != itb.HasText)
            {
                itb.SetValue(HasTextPropertyKey, actuallyHasText);
            }
        }

        void WatermarkTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            WatermarkTextBox itb = (WatermarkTextBox)sender;

            if (itb.IsFocused && itb.SelectTextBeforeSpace)
            {
                Task.Factory.StartNew(()=>SelectText(itb));
                
            }
        }

        void SelectText(WatermarkTextBox itb)
        {
            Thread.Sleep(100);
            Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        itb.Select(0, itb.Text.IndexOf(" "));
                    }
                    catch (Exception e)
                    {
                        // No space in the string
                    }
                }));
        }
    }
}

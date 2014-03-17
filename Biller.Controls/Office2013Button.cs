using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Biller.Controls
{
    /// <summary>
    /// This button looks like the backstage buttons in the Microsoft Office Products.
    /// </summary>
    public class Office2013Button : Button
    {
        static Office2013Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Office2013Button), new FrameworkPropertyMetadata(typeof(Office2013Button)));
        }

        /// <summary>
        /// URI for displaying an image.
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(Office2013Button), new UIPropertyMetadata(null));

        public static readonly DependencyProperty VectorProperty = DependencyProperty.Register("Vector", typeof(object), typeof(Office2013Button), new UIPropertyMetadata(null));

        public object Vector
        {
            get { return (object)GetValue(VectorProperty); }
            set { SetValue(VectorProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
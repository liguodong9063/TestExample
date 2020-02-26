using System.Windows;
using System.Windows.Input;

namespace TestExample.Utilities
{
    public class CustomKeyBinding: KeyBinding
    {
        public static readonly DependencyProperty CustomGestureProperty = DependencyProperty.Register("CustomGesture", typeof(InputGesture), typeof(CustomKeyBinding), new PropertyMetadata(null, OnCustomGesturePropertyChangedCallback));

        public InputGesture CustomGesture
        {
            get => (InputGesture)GetValue(CustomGestureProperty);
            set => SetValue(CustomGestureProperty, value);
        }

        private static void OnCustomGesturePropertyChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var source = s as CustomKeyBinding;
            if (source == null) return;
            source.Gesture = source.CustomGesture;
        }
    }
}

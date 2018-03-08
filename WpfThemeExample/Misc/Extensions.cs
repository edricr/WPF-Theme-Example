using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WpfThemeExample.Misc
{
    /* Captures mouse wheel events to allow for scrolling when hovering over a
     * ListView inside a ScrollViewer or other similar situation.
     */
    public sealed class IgnoreMouseWheelBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += PreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= PreviewMouseWheel;
            base.OnDetaching();
        }

        void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            var eventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArgs.RoutedEvent = UIElement.MouseWheelEvent;

            AssociatedObject.RaiseEvent(eventArgs);

        }

    }
}

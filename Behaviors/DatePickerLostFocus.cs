using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Quality_Control.Behaviors
{
    public class DatePickerLostFocus : Behavior<DatePicker>
    {
        protected override void OnAttached()
        {
            if (this.AssociatedObject != null)
            {
                base.OnAttached();
                this.AssociatedObject.PreviewKeyDown += AssociatedObject_KeyDown;
            }
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.PreviewKeyDown -= AssociatedObject_KeyDown;
                base.OnDetaching();
            }
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is DatePicker)
            {
                if (e.Key == Key.Enter)
                {
                    //datePicker.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
                    TraversalRequest request = new TraversalRequest(focusDirection);

                    if (elementWithFocus != null)
                    {
                        elementWithFocus.MoveFocus(request);
                    }
                }
            }
        }
    }
}

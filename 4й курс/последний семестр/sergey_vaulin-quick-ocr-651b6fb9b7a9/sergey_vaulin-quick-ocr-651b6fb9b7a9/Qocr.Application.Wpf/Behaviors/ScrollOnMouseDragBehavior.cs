using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Qocr.Application.Wpf.Behaviors
{
    /// <summary>
    /// <see cref="Behavior"/> для <see cref="ScrollViewer"/>.
    /// </summary>
    public class ScrollOnMouseDragBehavior : Behavior<ScrollViewer>
    {
        private Point? _lastMousePositon;

        /// <summary>
        /// Скорость перемещения.
        /// </summary>
        public double Speed { get; set; } = 0.5;

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseMove += AssociatedObjectOnMouseMove;
            AssociatedObject.PreviewMouseUp += AssociatedObjectOnMouseUp;
            AssociatedObject.MouseLeave += AssociatedObjectOnMouseLeave;

            HandControl();
        }

        private void HandControl()
        {
            AssociatedObject.MouseEnter += (s, e) => ProcessHandEvents(Cursors.SizeAll, e);
            AssociatedObject.MouseLeave += (s, e) => ProcessHandEvents(Cursors.Arrow, e);
        }

        private void ProcessHandEvents(Cursor cursor, MouseEventArgs args)
        {
            Mouse.OverrideCursor = cursor;
        }

        private void AssociatedObjectOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            _lastMousePositon = null;
        }

        private void AssociatedObjectOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _lastMousePositon = null;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                if (_lastMousePositon == null)
                {
                    _lastMousePositon = mouseEventArgs.GetPosition(AssociatedObject);
                    return;
                }

                var mousePosition = mouseEventArgs.GetPosition(AssociatedObject);

                if (!(0 < mousePosition.X && mousePosition.X < AssociatedObject.ScrollableWidth) ||
                    !(0 < mousePosition.Y && mousePosition.Y < AssociatedObject.ScrollableHeight))
                {
                    return;
                }

                AssociatedObject.ScrollToVerticalOffset(
                    AssociatedObject.VerticalOffset - Speed * (mousePosition.Y - _lastMousePositon.Value.Y));
                AssociatedObject.ScrollToHorizontalOffset(
                    AssociatedObject.HorizontalOffset - Speed * (mousePosition.X - _lastMousePositon.Value.X));

                _lastMousePositon = mousePosition;
            }
        }
    }
}
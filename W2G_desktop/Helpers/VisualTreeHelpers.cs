using System.Windows;
using System.Windows.Media;

namespace W2G_desktop.Helpers
{
    public static class VisualTreeHelpers
    {
        // Méthode d'extension pour retrouver un parent dans le Visual Tree
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            if (child == null) return null;

            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            if (parentObject is T parent) return parent;

            return FindParent<T>(parentObject);
        }
    }
}
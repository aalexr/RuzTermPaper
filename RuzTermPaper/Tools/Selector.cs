using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper.Tools
{
    public class IfEmptySelector : DataTemplateSelector
    {
        public DataTemplate NotEmptyTemplate { get; set; }

        public DataTemplate EmptyTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            //var control = container as ListView;
            //if (ItemsControl.ItemsControlFromItemContainer(control) is ListView listview && listview.Items.Count == 0)
            //    return EmptyTemplate;

            return NotEmptyTemplate;
        }
    }
}

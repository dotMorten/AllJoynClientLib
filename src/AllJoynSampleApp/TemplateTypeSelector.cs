using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace AllJoynSampleApp
{
    [ContentProperty(Name = nameof(Matches))]
    public class TypeTemplateSelector : DataTemplateSelector
    {
        public TypeTemplateSelector()
        {
            this.Matches = new List<TemplateEntry>();
        }

        public List<TemplateEntry> Matches { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return 
                this.Matches.FirstOrDefault(m => m.TypeName == (item.GetType().FullName))?.Template ?? DefaultTemplate;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }

        public DataTemplate DefaultTemplate { get; set; }
    }

    [ContentProperty(Name = nameof(Template))]
    public class TemplateEntry
    {
        public Type TargetType { get; set; }
        public string TypeName { get; set; }
        public DataTemplate Template { get; set; }
    }
}

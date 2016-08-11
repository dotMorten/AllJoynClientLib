using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            UpdateTypes(Matches);
            return 
                this.Matches.FirstOrDefault(m => IsTypeOf(m.TargetType, item))?.Template ?? DefaultTemplate;
        }

        private static void UpdateTypes(IEnumerable<TemplateEntry> items)
        {
            foreach(var item in items.Where(t=>t.TargetType == null))
            {
                item.TargetType = Type.GetType(item.TypeName, false, false);
                if (item.TargetType == null)
                {
                    item.TargetType = typeof(AllJoynClientLib.Devices.DeviceClient).GetTypeInfo().Assembly.GetType(item.TypeName, false, false);
                }
            }
        }
        private static bool IsTypeOf(Type t, object instance)
        {
            if (t == null || instance == null) return false;
            var t2 = instance.GetType();
            if (t.Equals(t2)) return true;
            return t2.GetTypeInfo().IsSubclassOf(t);
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

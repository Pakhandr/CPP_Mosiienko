using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maui_lab_1.Controls
{
    class BorderedEntry: Entry
    {
        public static readonly BindableProperty PlaceholderProperty =
           BindableProperty.Create(
               propertyName: nameof(Placeholder),
               returnType: typeof(string),
               declaringType: typeof(BorderedEntry),
               defaultValue: string.Empty);

        
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
    }
}

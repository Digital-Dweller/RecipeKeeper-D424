using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Recipe_Keeper.Classes.Utilities
{
    public class TextInputBehavior : Behavior<Entry>
    {

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            string sanitizedText = Regex.Replace(e.NewTextValue, @"[;'\-""\%\*\(\)=\+\[\]\{\}\\/\|<>]", "");
            if (sanitizedText != e.NewTextValue)
            { entry.Dispatcher.Dispatch(() => entry.Text = sanitizedText); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recipe_Keeper.Classes.Utilities
{
    public class UnitInputBehavior : Behavior<Entry>
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
            if (!Regex.IsMatch(e.NewTextValue, @"^(\d+(/(\d*)?)?)?$"))
            {
                entry.Dispatcher.Dispatch(() =>
                {
                    entry.Text = e.OldTextValue ?? string.Empty;
                });
            }
        }
    }
}

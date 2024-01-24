using System;

namespace Logging.Models
{
    public class IgnoreLoggingItems : Attribute
    {
        public Ignore[] IngoredItems { get; set; }

        public IgnoreLoggingItems(Ignore[] ingoredItems) => IngoredItems = ingoredItems;
    }

    public class IncludeLoggingItems : Attribute
    {
        public Include[] IncludedItems { get; set; }

        public IncludeLoggingItems(Include[] includedItems) => IncludedItems = includedItems;
    }
}

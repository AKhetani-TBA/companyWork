using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace ExcelAddIn5.Events
{
    public sealed class BeastEventAggregator: EventAggregator
    {
        private static readonly BeastEventAggregator _instance = new BeastEventAggregator();

        static BeastEventAggregator()
        {
        }

        private BeastEventAggregator()
        {
        }

        public static BeastEventAggregator Instance
        {
            get { return _instance; }
        }    
    }
}

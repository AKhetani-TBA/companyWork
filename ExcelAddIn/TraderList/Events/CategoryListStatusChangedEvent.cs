using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraderList.Events
{
    public class CategoryListStatusChangedEvent
    {
        private CategoryListStatus _status;
        private string _info;


        public CategoryListStatusChangedEvent(CategoryListStatus status, string info)
        {
            _status = status;
            _info = info;
        }

        public CategoryListStatus Status
        {
            get { return _status; }
        }

        public string Info
        {
            get { return _info; }
        }

    }
}

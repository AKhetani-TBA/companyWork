using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelAddIn5.Events
{
    public class CategoryListEvent
    {
        private int _userID;

        public CategoryListEvent(int UserID)
        {
            _userID = UserID;
        }

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }

        }
    }
}

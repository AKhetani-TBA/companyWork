using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beast_RFQ_Addin
{
    public static class Shared
    {
        // RFQ User Role Enum.
        public enum RFQUserRole
        {
            NONE = 0,
            REQUESTOR = 1,
            RESPONDER = 2,
            BOTH = 3
        }

        // Excel Sheet Column Index
        public static int ColumnSIDE = 2;
        public static int ColumnCUSIP = 3;
        public static int ColumnQTY = 4;
        public static int ColumnSTATUS = 5;
        public static int ColumnREQUESTID = 6;
        public static int ColumnTOP1QUOTER = 12;
        public static int ColumnTOP1QUOTEREMAIL = 13;
        public static int ColumnTOP2QUOTER = 15;
        public static int ColumnTOP2QUOTEREMAIL = 16;
        public static int ColumnTOP3QUOTER = 18;
        public static int ColumnTOP3QUOTEREMAIL = 19;
    }
}

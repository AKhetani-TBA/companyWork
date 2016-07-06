using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BASE
{
    public static class Enums
    {
        public enum Roles
        {
            Guest = 0,
            SuperUser = 1,
            ComopanyAdmin = 2,
            NormalUser = 3
        }

        public enum ValidationFlag
        {
            Success = 0,
            AddUserToGroup = 1,
            RemoveUserFromGroup = 0,
            UserNotFound = -2,
            UserLockedOutByHelpDesk = -8,
            PasswordWrongUserLockedOut = -5,
            PasswordWrongLastTime = -4,
            PasswordWrong = -3,
            PasswordMustChange = -6,
            MaxLoginExceeded = -7
        }
        public enum AutoURLApplicationCode
        {
            IRO = 8,
            ISWAP = 9,
            OddLot = 5
        }

        public static class BeastCalcGroups
        {
            public const string Default = "Users";
            public const string CME = "CME_ICAP";
            public const string FinCAD = "FinCad";
            public const string Numerix = "Numerix";
            public const string DummyCompany = "dummy_fi";
            public const string rapidRV = "rapidRV";
        }

        public enum CalcGroupId
        {
            Numerix = 23,
            CME_ICAP = 20
        }

        public enum Vendors
        {
            Cme_Icap = 3,
            Beast = 1,
            NoVendor = -1,
            rapidRV = 1016
        }

        public enum DBServers
        {
            Dev = 1,
            Test = 2,
            Demo = 3,
            Amazon = 4,
            Azure = 5
        }

        public enum BeastWatchdogEvents
        {
            [Description("Started")]
            Started = 41,
            [Description("Restarted")]
            Restarted = 42,
            [Description("Shutdown")]
            Shutdown = 43,
            [Description("Crashed")]
            Crashed = 44,
            [Description("Already Running")]
            AlreadyRunning = 45,
            [Description("Fail To CheckIn")]
            FailToCheckIn = 46,
            [Description("Terminated")]
            Terminated = 47,
            [Description("Watchdog Started")]
            WatchdogStarted = 48,
            [Description("Watchdog Stopped")]
            WatchdogStopped = 49,
            [Description("Watchdog Nothing To Watch")]
            WatchdogNothingToWatch = 50
        }

        public enum BeastApplicationEvents
        {
            SessionServerConnected = 25,
            SessionServerReconnect = 26,
            SessionServerDisconnect = 27,
            IMDMConnection = 28
        }

        public enum BeastAppsCtrlApplications
        {
            Connect = 1,
            Disconnect = 2,
            Reconnect = 3
        }

        public enum BeastWatchdogApplications
        {
            Started = 1,
            Restarted = 2,
            Shutdown = 3,
            AlreadyRunning = 4,
            Crashed = 5,
            FailToCheckIn = 6,
            Terminate = 7,
            NothingToWatch = 8
        }

        public enum Severity
        {
            Error = 81,
            Information = 82,
            Warning = 83
        }
        public enum BeastImageEvents
        {
            Create = 2,
            Open = 3,
            Close = 7,
            Delete = 8,
            Active = 11,
            Crash = 21,
            Restore = 22,
            Shutdown = 71,
            Died = 72
        }

        //public enum BeastImageEvents
        //{
        //    Active = 1,

        //    --Create = 2,
        //    --Close = 3,

        //    --Crash = 4,
        //    --Delete = 5,
        //    --Died = 6,

        //    --Open = 7,

        //    --Restore = 8,
        //    --Shutdown = 9
        //}
    }
}

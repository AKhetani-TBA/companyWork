#region Includes
  using System;
  using System.Text;
#endregion

namespace VCM.EMS.Base
{
   public class LeaveDetails
    {
#region Constructor
        public LeaveDetails()
        {
        this._LeaveId =-1;
        this. _EmpId =-1;
         this._LeaveReason = string.Empty;
         this._isProbable =0;
         this._DayType = string.Empty;
          this._AppliedDate = DateTime.Now;
         this._AppliedBy = -1;
         this._ModifiedDate = DateTime.Now;
         this._ModifiedBy = -1;
         this._FromDate = string.Empty;
         this._ToDate = string.Empty;
         this._isOutOfTown =1;
         this._isAvailOnCall=1;
         this._isSysAvail=1;
         this._isEmergeFromLocAvail=1;
         this._isEmergeToOfcAvail=0;
         this._LastAction = string.Empty;         
         }

#endregion

#region Private Variables

        private System.Int32 _LeaveId;
        private System.Int32 _EmpId;
        private System.String _LeaveReason;
        private System.Int32 _isProbable;
        private System.String _DayType;
        private System.DateTime _AppliedDate;
        private System.Int32 _AppliedBy;
        private System.DateTime _ModifiedDate;
        private System.Int32 _ModifiedBy;
        private System.String _FromDate;
        private System.String _ToDate;
        private System.Int32 _isOutOfTown;
        private System.Int32 _isAvailOnCall;
        private System.Int32 _isSysAvail;
        private System.Int32 _isEmergeFromLocAvail;
        private System.Int32 _isEmergeToOfcAvail;
        private System.String _LastAction;
        private System.Int32 _DepartmentId;
        private System.Int32 _ProductId;
        private System.String _TLApproval;
        private  System.String _TLComments	;
        private System.String _MGRApproval;
        private System.String _MGRComments;	

#endregion

#region Public Properties

          public System.Int32  LeaveId
          {
                get { return  _LeaveId; }
                set { _LeaveId = value; }
          }
        public System.Int32 EmpId
        {
             get { return  _EmpId; }
             set {  _EmpId= value; }
        }
        public System.String  LeaveReason
        {
            get { return  _LeaveReason; }
            set { _LeaveReason = value; }
        }
        public System.Int32 IsProbable
        {
             get { return _isProbable ; }
            set { _isProbable = value; }
        }
        public System.String  DayType
        {
            get { return  _DayType; }
            set { _DayType = value; }
        }
        public System.DateTime  AppliedDate
        {
             get { return  _AppliedDate; }
            set { _AppliedDate = value; }
        }
        public System.Int32 AppliedBy
        {
             get { return  _AppliedBy; }
            set { _AppliedBy = value; }
        }
        public System.DateTime ModifiedDate
        {
             get { return  _ModifiedDate; }
            set { _ModifiedDate = value; }       
        }
        public System.Int32 ModifiedBy
        {
             get { return  _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        public System.String FromDate
        {
             get { return  _FromDate; }
            set { _FromDate = value; }
        }
        public System.String  ToDate
        {
             get { return  _ToDate; }
            set { _ToDate = value; }
        }
        public System.Int32 IsOutOfTown
        {
             get { return  _isOutOfTown; }
            set { _isOutOfTown = value; }
        }
        public System.Int32 IsAvailOnCall
        {
             get { return  _isAvailOnCall ; }
             set {  _isAvailOnCall= value; }
        }
        public System.Int32 IsSysAvail
        {
             get { return  _isSysAvail; }
             set {  _isSysAvail= value; }
        }
        public System.Int32 IsEmergeFromLocAvail
        {
             get { return  _isEmergeFromLocAvail; }
             set {  _isEmergeFromLocAvail = value; }
        }
        public System.Int32 IsEmergeToOfcAvail
        {
             get { return  _isEmergeToOfcAvail; }
             set {  _isEmergeToOfcAvail= value; }
        }
        public System.String  LastAction
        {
             get { return _LastAction ; }
             set {  _LastAction= value; }        
        }

        public System.Int32 DepartmentId
        {
            get { return _DepartmentId; }
            set { _DepartmentId = value; }
        }
        public System.Int32 ProductId
        {
            get { return _ProductId; }
            set { _ProductId = value; }
        }
        public System.String  TlApproval
        {
            get { return _TLApproval; }
            set { _TLApproval = value; }
        }
        public System.String  TlComments
        {
            get { return _TLComments; }
            set { _TLComments = value; }
        }
        public System.String MgrApproval
        {
            get { return _MGRApproval; }
            set { _MGRApproval = value; }
        }
        public System.String MgrComments
        {
            get { return _MGRComments; }
            set { _MGRComments = value; }
        }

 
#endregion



    }
}




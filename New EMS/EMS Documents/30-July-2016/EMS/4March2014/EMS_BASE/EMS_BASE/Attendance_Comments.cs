#region Includes
using System;
using System.Text;
#endregion

namespace VCM.EMS.Base
{

    [Serializable]
    public class Attendance_Comments
    {

        #region Constructor
        public Attendance_Comments()
        {
            this._empId = -1;
            this._dateOfRecord = DateTime.Now;
            this._workDayCategory = -1;
            this._workPlace = -1;
            this._comments = string.Empty;
            this._timeComments = string.Empty;
            this._lunchComments = string.Empty;
            this._cameLate = string.Empty;
            this._earlyleft = string.Empty;
            this._thrnotMain = string.Empty;

            this._dept = -1;
            this._approved = string.Empty;
            this._approvedBy = string.Empty;
            this._approvedDate = System.DateTime.Now;
            this._createdBy = string.Empty;
            this._modifyBy = string.Empty;
            this._LastAction = string.Empty;
        }
        #endregion

        #region Private Variables
        private System.Int32 _empId;
        private System.DateTime _dateOfRecord;
        private System.Int16 _workDayCategory;
        private System.Int16 _workPlace;
        private System.String _comments;
        private System.String _modifyBy;
        private System.String _newCategory;
        private System.DateTime _modifyDate;
        private System.String _timeComments;
        private System.String _lunchComments;
        private System.String _cameLate;
        private System.String _earlyleft;
        private System.String _thrnotMain;
        
        private System.Int32 _dept; 
        private System.String _approved; 
        private System.String _approvedBy;
        private System.DateTime _approvedDate;
        private System.String _createdBy;        
        private System.String _LastAction;

        #endregion

        #region Public Properties
        public System.Int32 EmpId
        {
            get { return _empId; }
            set { _empId = value; }
        }

        public System.DateTime DateOfRecord
        {
            get { return _dateOfRecord; }
            set { _dateOfRecord = value; }
        }
        public System.DateTime ModifyDate
        {
            get { return _modifyDate; }
            set { _modifyDate = value; }
        }

        public System.Int16 WorkDayCategory
        {
            get { return _workDayCategory; }
            set { _workDayCategory = value; }
        }

        public System.Int16 WorkPlace
        {
            get { return _workPlace; }
            set { _workPlace = value; }
        }

        public System.String Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
        public System.String ModifyBy
        {
            get { return _modifyBy; }
            set { _modifyBy = value; }
        }
        public System.String NewCategory
        {
            get { return _newCategory; }
            set { _newCategory = value; }
        }
        public System.String TimeComments
        {
            get { return _timeComments; }
            set { _timeComments = value; }
        }
        public System.String LunchComments
        {
            get { return _lunchComments; }
            set { _lunchComments = value; }
        }
        public System.String CameLate
        {
            get { return _cameLate; }
            set { _cameLate = value; }
        }
        public System.String Earlyleft
        {
            get { return _earlyleft; }
            set { _earlyleft = value; }
        }
        public System.String ThrnotMain
        {
            get { return _thrnotMain; }
            set { _thrnotMain = value; }
        }

        public System.Int32 Dept
        {
            get { return _dept; }
            set { _dept = value; }
        }
        public System.String Approved
        {
            get { return _approved; }
            set { _approved = value; }
        }
        public System.String ApprovedBy
        {
            get { return _approvedBy; }
            set { _approvedBy = value; }
        }
        public System.DateTime ApprovedDate
        {
            get { return _approvedDate; }
            set { _approvedDate = value; }
        }
        public System.String CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }
        public System.String LastAction
        {
            get { return _LastAction; }
            set { _LastAction = value; }
        }
      
        #endregion

    }

}

#region Includes
using System;
using System.Text;
#endregion

namespace VCM.EMS.Base
{

    [Serializable]
    public class SkillDetails
    {

        #region Constructor
        public SkillDetails()
        {
            this._skillId = -1;
            this._empId = -1;
            this._skillName = string.Empty;
            this._lastAction = string.Empty;
            this._createdBy = string.Empty;
            this._modifyBy = string.Empty;
            this._modifyDate = DateTime.Now;
            this._cratedDate = DateTime.Now;
        }
        #endregion

        #region Private Variables
        private System.Int32 _skillId;
        private System.Int32 _empId;
        private System.String _skillName;
        private System.String _lastAction;
        private System.String _createdBy;
        private System.DateTime _cratedDate;
        private System.String _modifyBy;
        private System.DateTime _modifyDate;
       
        #endregion

        #region Public Properties
        public System.Int32 SkillId
        {
            get { return _skillId; }
            set { _skillId = value; }
        }
        public System.Int32 EmpId
        {
            get { return _empId; }
            set { _empId = value; }
        }

        public System.String SkillName
        {
            get { return _skillName; }
            set { _skillName = value; }
        }

        public System.String LastAction
        {
            get { return _lastAction; }
            set { _lastAction = value; }
        }

        public System.String CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public System.String ModifyBy
        {
            get { return _modifyBy; }
            set { _modifyBy = value; }
        }

        public System.DateTime ModifyDate
        {
            get { return _modifyDate; }
            set { _modifyDate = value; }
        }

        public System.DateTime CratedDate
        {
            get { return _cratedDate; }
            set { _cratedDate = value; }
        }

        #endregion

    }
}

using System;
using System.Text;

namespace VCM.EMS.Base
{
    public class CLSLPolicy
    {
        #region Constructor
        public CLSLPolicy()
        {
            this._JoiningMonth = 0;
            this._Leave_Type = string.Empty;
            this._Descriptions = string.Empty;
            this._JAN = 0.0;
            this._FEB = 0.0;
            this._MAR = 0.0;
            this._APR = 0.0;
            this._MAY = 0.0;
            this._JUN = 0.0;
            this._JUL = 0.0;
            this._AUG = 0.0;
            this._SEP = 0.0;
            this._OCT = 0.0;
            this._NOV = 0.0;
            this._DEC = 0.0;
            this._Total_Leave_Applicable = 0.0;
            this._CreatedDate = DateTime.Now;
            this._CreatedBy = string.Empty;


        }

        #endregion

        #region Private Variable

        private System.Int16 _JoiningMonth;
        private System.String _Leave_Type;
        private System.String _Descriptions;
        private System.Double _JAN;
        private System.Double _FEB;
        private System.Double _MAR;
        private System.Double _APR;
        private System.Double _MAY;
        private System.Double _JUN;
        private System.Double _JUL;
        private System.Double _AUG;
        private System.Double _SEP;
        private System.Double _OCT;
        private System.Double _NOV;
        private System.Double _DEC;
        private System.Double _Total_Leave_Applicable;
        private System.DateTime _CreatedDate;
        private System.String _CreatedBy;

        #endregion

        #region Public Properties
        public System.Int16 JoiningMonth
        {
            get { return _JoiningMonth; }
            set { _JoiningMonth = value; }
        }
        public System.String Leave_Type
        {
            get { return _Leave_Type; }
            set { _Leave_Type = value; }
        }
        public System.String Descriptions
        {
            get { return _Descriptions; }
            set { _Descriptions = value; }
        }
        public System.Double JAN
        {
            get { return _JAN; }
            set { _JAN = value; }
        }
        public System.Double FEB
        {
            get { return _FEB; }
            set { _FEB = value; }
        }
        public System.Double MAR
        {
            get { return _MAR; }
            set { _MAR = value; }
        }
        public System.Double APR
        {
            get { return _APR; }
            set { _APR = value; }
        }
        public System.Double MAY
        {
            get { return _MAY; }
            set { _MAY = value; }
        }
        public System.Double JUN
        {
            get { return _JUN; }
            set { _JUN = value; }
        }
        public System.Double JUL
        {
            get { return _JUL; }
            set { _JUL = value; }
        }
        public System.Double AUG
        {
            get { return _AUG; }
            set { _AUG = value; }
        }
        public System.Double SEP
        {
            get { return _SEP; }
            set { _SEP = value; }
        }
        public System.Double OCT
        {
            get { return _OCT; }
            set { _OCT = value; }
        }
        public System.Double NOV
        {
            get { return _NOV; }
            set { _NOV = value; }
        }
        public System.Double DEC
        {
            get { return _DEC; }
            set { _DEC = value; }
        }
        public System.Double Total_Leave_Applicable
        {
            get { return _Total_Leave_Applicable; }
            set { _Total_Leave_Applicable = value; }
        }
        public System.DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public System.String CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        #endregion
    }
}

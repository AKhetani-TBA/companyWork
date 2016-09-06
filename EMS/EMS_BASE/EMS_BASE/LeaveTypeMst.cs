#region Includes
using System;
using System.Text;
#endregion 


namespace VCM.EMS.Base
{
   public  class LeaveTypeMst
    {

        #region LeaveTypeMst

                public LeaveTypeMst() 
              {
                         this._Leave_TypeId=-1;
                         this._Leave_Abbrv = string.Empty;
                         this._Leave_Name = string.Empty;
                         this._Eligibility_Criteria_Start = string.Empty;
                         this._Eligibility_Criteria_End = string.Empty;
                         this._No_Of_Days = -1;
                         this._Max_CarryFwd_Days = -1;
                         this._Create_By = -1;
                         this ._Create_Date= DateTime.Now;
                         this. _Last_Action=string.Empty;
                         this._Wef = DateTime.Now;
                         this._Serial_To_Deduction = -1;

                         this._LeaveId = -1;
                         this._Leave_TypeId = -1;
                         this._Daycount = 1;
                         this._LeaveDate = DateTime.Now;
                         this._CreatedBy = string.Empty;
                    
              }

            #endregion

        #region Private Variables

                        private System.Int32 _Leave_TypeId;
                        private System.String _Leave_Abbrv;
                        private System.String _Leave_Name;
                        private System.String _Eligibility_Criteria_Start;
                        private System.String _Eligibility_Criteria_End;
                        private System.Int16 _No_Of_Days;
                        private System.Int16 _Max_CarryFwd_Days;
                        private System.Int32 _Create_By;
                        private System.DateTime _Create_Date;
                        private System.String _Last_Action;
                        private System.DateTime _Wef;
                        private System.Int16 _Serial_To_Deduction;

                        private System.Int32 _LeaveId;
                        private System.Int32 _LeaveTypeId;
                        private System.Double _Daycount;
                        private System.DateTime _LeaveDate;
                        private System.String _CreatedBy;


        #endregion

        #region Public Properties

                    public  System.Int32  Leave_TypeId
                     {
                           get { return _Leave_TypeId; }
                            set { _Leave_TypeId = value; }
                      }
                    public System.String Leave_Abbrv
                    {
                          get { return _Leave_Abbrv; }
                         set { _Leave_Abbrv = value; }
                    }
                    public System.String Leave_Name
                    {
                            get { return _Leave_Name; }
                            set { _Leave_Name = value; }
                    }
                    public System.String Eligibility_Criteria_Start
                    {
                          get { return _Eligibility_Criteria_Start; }
                          set { _Eligibility_Criteria_Start = value; }
                    }
                    public System.String Eligibility_Criteria_End
                    {
                           get { return _Eligibility_Criteria_End; }
                           set { _Eligibility_Criteria_End = value; }
                    }
                    public System.Int16 No_Of_Days
                    {
                           get { return _No_Of_Days; }
                           set { _No_Of_Days = value; }
                    }
                     public  System.Int16  Max_CarryFwd_Days
                     {
                            get { return _Max_CarryFwd_Days; }
                            set { _Max_CarryFwd_Days = value; }
                      }
                    public System.Int32 Create_By
                    {
                          get { return _Create_By; }
                          set { _Create_By = value; }
                    }
                    public System.DateTime Create_Date
                    {
                          get { return _Create_Date; }
                          set { _Create_Date = value; }
                    }
                    public System.String Last_Action
                    {
                           get { return _Last_Action; }
                           set { _Last_Action = value; }
                    }
                    public System.DateTime Wef
                    {
                        get { return _Wef; }
                        set { _Wef = value; }
                    }
                    public System.Int16 Serial_To_Deduction
                    {
                        get { return _Serial_To_Deduction; }
                        set { _Serial_To_Deduction = value; }
                    }


                    public System.Int32 LeaveId
                    {
                        get { return _LeaveId; }
                        set { _LeaveId = value; }
                    }
                    public System.Int32 LeaveTypeId
                    {
                        get { return _LeaveTypeId; }
                        set { _LeaveTypeId = value; }
                    }
                    public System.Double Daycount
                    {
                        get { return _Daycount; }
                        set { _Daycount = value; }
                    }
                    public System.DateTime LeaveDate
                    {
                        get { return _LeaveDate; }
                        set { _LeaveDate = value; }
                    }
                    public System.String CreatedBy
                    {
                        get { return _CreatedBy; }
                        set { _CreatedBy = value; }
                    }

            #endregion

    }
}




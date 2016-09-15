#region Includes
using System;
using System.Text;
#endregion 

namespace VCM.EMS.Base
{
    public class Holiday
    {
        #region Constructor

        public Holiday() 
        {
         this._HolidayId=-1;
         this ._StartDate= DateTime.Now;
         this._EndDate = DateTime.Now;
         this. _Purpose=string.Empty;
         this._LeaveTypeName = string.Empty;
         this ._Location=string.Empty;
         this ._CreatedDate= DateTime.Now;
         this ._CreatedBy=-1;
         this ._ModifiedDate= DateTime.Now;
         this ._ModifiedBy=-1;
         this. _LastAction=string.Empty;
        }

     #endregion

        #region Private Variables

  private System.Int64 _HolidayId;
  private System.DateTime _StartDate;
  private System.DateTime _EndDate;
  private System.String _Purpose;
  private System.String _LeaveTypeName;
  private System.String _Location	;
  private System.DateTime _CreatedDate	;
  private System.Int64 _CreatedBy;
  private System.DateTime _ModifiedDate;
  private System.Int64 _ModifiedBy;
  private System.String _LastAction;
         
#endregion

        #region Public Properties

  public System.Int64 HolidayId
  {
                get { return  _HolidayId; }
                set { _HolidayId = value; }
  }
  public System.DateTime StartDate
  {
      get { return _StartDate; }
      set { _StartDate = value; }
  }
  public System.DateTime EndDate
  {
                 get { return  _EndDate; }
                set { _EndDate = value; }     
  }
  public System.String Purpose
  {
                get { return  _Purpose; }
                set { _Purpose = value; }
  }
  public System.String LeaveTypeName
  {
                get { return  _LeaveTypeName; }
                set { _LeaveTypeName = value; }
  }
  public System.String Location
  {
                get { return  _Location; }
                set { _Location = value; }
  }
  public System.DateTime CreatedDate	
  {
                get { return  _CreatedDate; }
                set { _CreatedDate= value; }
  }
  public System.Int64 CreatedBy
  {
                get { return  _CreatedBy; }
                set { _CreatedBy = value; }
  }
  public System.DateTime ModifiedDate
  {
                get { return  _ModifiedDate; }
                set { _ModifiedDate = value; }
  }
  public System.Int64 ModifiedBy
  {
                get { return  _ModifiedBy; }
                set { _ModifiedBy = value; }
  }
  public System.String LastAction
  {
                get { return  _LastAction; }
                set { _LastAction = value; }
  }

    #endregion
 
    }

}






    



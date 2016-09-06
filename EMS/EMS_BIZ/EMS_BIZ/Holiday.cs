#region Includes
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
#endregion

namespace VCM.EMS.Biz
{
    [Serializable]
   public class Holiday
    {

        #region Private Declarations
             private VCM.EMS.Dal.Holiday objHolidayDetails;
        #endregion

        #region Constructor
           public Holiday()
           {
             objHolidayDetails = new VCM.EMS.Dal.Holiday();
           }
        #endregion

        #region Public Methods
           
        public int Save_HolidayDetails(VCM.EMS.Base.Holiday objHolidayDetail)
           {
               return objHolidayDetails.Save_HolidayDetails(objHolidayDetail);
           }
        public int Delete_HolidayDetails(System.Int64 holidayId)
           {
               return objHolidayDetails.Delete_HolidayDetails(holidayId);
           }
        public DataSet GetHolidayDetails(System.String location ,System.String leaveTypeName,System.DateTime  startDate,System.DateTime   endDate )
           {
               return objHolidayDetails.GetHolidayDetails(location ,leaveTypeName,startDate,endDate);
           }
        public DataSet GetHolidayDetailsByLocation()
           {
               return objHolidayDetails.GetHolidayDetailsByLocation();
           }
        public DataSet GetHolidayDetailsByPurpose()
           {
               return objHolidayDetails.GetHolidayDetailsByPurpose();
           }
        public DataSet GetLeaveType(System.String location)
           {
               return objHolidayDetails.GetLeaveType(location);
           }
        //public DataSet GetAllHolidayDetails()
        //   {
        //       return objHolidayDetails.GetAllHolidayDetails();
        //   }
        public DataSet GetHolidayDetailsById(System.Int64 holidayId)
           {
               return objHolidayDetails.GetHolidayDetailsById(holidayId);
           }
        #endregion

    }
}


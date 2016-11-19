#region Includes
using System;
using System.Data;
using System.Text;
#endregion

namespace VCM.EMS.Biz
{
    [Serializable]
    public class Attendance_Comments
    {
        #region Private Declarations
        private VCM.EMS.Dal.Attendance_Comments obj_Comments;
        #endregion

        #region Constructor
        public Attendance_Comments()
        {
            obj_Comments = new VCM.EMS.Dal.Attendance_Comments();
        }
        #endregion

        #region Public Methods

        public DataSet Get_CommentData_By_Uid_mth(Int32 UId, int mth, int year)
        {
            return obj_Comments.Get_CommentData_By_Uid_mth(UId, mth, year);
        }
        public DataSet Get_CommentData_By_Datewise(Int32 UId, DateTime dt)
        {
            return obj_Comments.Get_CommentData_By_Datewise(UId, dt);
        }
        public DataSet CheckData(Int32 UId, DateTime recDate)
        {
            return obj_Comments.CheckData(UId, recDate);
        }
        public DataSet CheckApprovedData(int UId, int DeptId, DateTime recDate)
        {
            return obj_Comments.CheckApprovedData(UId,DeptId, recDate);
        }
        public int Save_Comments(VCM.EMS.Base.Attendance_Comments obj_CommentsEntity)
        {
            return obj_Comments.Save_Comments(obj_CommentsEntity);
        }
        public void SaveApprovedComments(VCM.EMS.Base.Attendance_Comments obj_CommentsEntity)
        {
             obj_Comments.SaveApprovedComments(obj_CommentsEntity);
        }
        public string Get_Count_By_Uid_mth(Int64 UId, int mth, int year)
        {
            return obj_Comments.Get_Count_By_Uid_mth(UId,mth,year);
        }

        public int Delete_Comments(System.Int32 userId, DateTime recDt)
        {
            return obj_Comments.Delete_Comments(userId, recDt);
        }

        public int ActivateInactivate_Comments(string strIDs, int modifiedBy, bool isActive)
        {
            return obj_Comments.ActivateInactivate_Comments(strIDs, modifiedBy, isActive);
        }

        public VCM.EMS.Base.Attendance_Comments Get_CommentsByID(System.Int32 luserId)
        {
            return obj_Comments.Get_CommentsByID(luserId);
        }

        public DataSet GetAll(Boolean isActive)
        {
            return obj_Comments.GetAll(isActive);
        }

        public DataSet CheckTLName(string empname , int deptid)
        {
            return obj_Comments.CheckTLName(empname, deptid);
        }

        #endregion
    }
}
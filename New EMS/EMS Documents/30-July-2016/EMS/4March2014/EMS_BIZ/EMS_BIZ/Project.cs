#region Includes
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Project
{
	#region Private Declarations
	private VCM.EMS.Dal.Project objProject;
	#endregion

	#region Constructor
	public Project()
	{
		objProject = new VCM.EMS.Dal.Project();
	}
	#endregion

	#region Public Methods
	public int SaveProject(VCM.EMS.Base.Project objProjectEntity)
	{
		return objProject.SaveProject(objProjectEntity);
	}

	public int DeleteProject(System.Int64 projectId)
	{
		return objProject.DeleteProject(projectId);
	} 

	public int ActivateInactivateProject(string strIDs, int modifiedBy, bool isActive)
	{
		return objProject.ActivateInactivateProject(strIDs, modifiedBy, isActive);
	}

    public DataSet GetProjectByID(int projectId)
	{
		return objProject.GetProjectByID(projectId);
	}

	public List<VCM.EMS.Base.Project> GetAll(Boolean isActive)
	{
		return objProject.GetAll(isActive);
	}
    public DataSet GetAllProjects(int ProjectId)
    { 
        return objProject.GetAllProjects(ProjectId);
    }
    public DataSet GetProjectsDetails(int deptid,int empid)
    {
        return objProject.GetProjectsDetails(deptid,empid);
    }

    public DataSet GetProjectRoleDetails()
    {
        return objProject.GetProjectRoleDetails();
    }

	#endregion

}

}
using System;
using System.Data.SqlClient;

namespace Common.Api.UserManagement
{
	/// <summary>
	/// User logon status class 
	/// </summary>
	[Serializable]
	public class UserLogonStatus
	{
		/// <summary>
		/// user id
		/// </summary>
		public int UserId { get; set; }
		/// <summary>
		/// email
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// logon status
		/// </summary>
		public int LogonStatus { get; set; }


        #region IDataType

        ///// <summary>
        ///// Is this object 
        ///// </summary>
        ///// <param name="uniqueId">object unique data</param>
        ///// <returns></returns>
        //public bool IsThis(object uniqueId)
        //{
        //    return Equals(UniqueId, uniqueId);
        //}

        ///// <summary>
        ///// Type of adapter
        ///// </summary>
        //public Type AdapterType
        //{
        //    get { return typeof(UserLogonStatusAdapter); }
        //}

		/// <summary>
		/// unique id
		/// </summary>
		public object UniqueId
		{
			get { return UserId; }
		}

		#endregion
	}

    ///// <summary>
    ///// User logon status data block adapter
    ///// </summary>
    //[Serializable]
    //public class UserLogonStatusAdapter : DataBlockAdapter
    //{
    //    #region const declaration

    //    private const string F_USER_ID = "UserID";
    //    private const string F_EMAIL = "Email";
    //    private const string F_LOGON_STATUS = "LogonStatus";

    //    #endregion

    //    /// <summary>
    //    /// Read data of specified date type from SqlDataReader
    //    /// </summary>
    //    /// <param name="sdr">SqlDataReader to read data</param>
    //    /// <returns></returns>
    //    public override IDataType GetFromDataRow(SqlDataReader sdr)
    //    {
    //        int idxUserId = sdr.GetOrdinal(F_USER_ID);
    //        int idxEmail = sdr.GetOrdinal(F_EMAIL);
    //        int idxLogonStatus = sdr.GetOrdinal(F_LOGON_STATUS);
    //        return new UserLogonStatus
    //        {
    //            UserId = GetInt32(sdr, idxUserId),
    //            Email = GetString(sdr, idxEmail),
    //            LogonStatus = GetInt32(sdr, idxLogonStatus)
    //        };
    //    }

    //}
}

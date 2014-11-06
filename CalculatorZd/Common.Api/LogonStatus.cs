namespace Common.Api.UserManagement
{
	/// <summary>
	/// Logon status
	/// </summary>
	public enum LogonStatus
	{
		/// <summary>
		/// User with specified login not found
		/// </summary>
		UserNotFound = -1, 

		/// <summary>
		/// User logged on 
		/// </summary>
		Success = 0, 

		/// <summary>
		/// Logon attempts reached maximum
		/// </summary>
		MaxLogonAttemptsReached = 1,

		/// <summary>
		/// User is inactive and cannot be logged on
		/// </summary>
		UserIsInactive = 2,

		/// <summary>
		/// Password is wrong 
		/// </summary>
		WrongPasword = 3,
		
		/// <summary>
		/// Not Acces for login
		/// </summary>
		UserNotAccess = 4,

        UserIsLogon = 5,
        /// <summary>
        /// Email is exist
        /// </summary>
        EmailAllReadyExist = 6
	}
}

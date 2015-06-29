using System;
using System.ComponentModel; 
using System.Runtime.InteropServices; 
using System.Security.Principal;

public enum LogonType
{
    /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on 
    /// by a terminal server, remote shell, or similar process. 
    /// This logon type has the additional expense of caching logon information for disconnected operations; 
    /// therefore, it is inappropriate for some client/server applications, 
    /// such as a mail server. 
    Interactive = 2,
    /// This logon type is intended for high performance servers to authenticate plaintext passwords. 
    /// The LogonUser function does not cache credentials for this logon type. 
    Network = 3,
    /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without 
    /// their direct intervention. This type is also for higher performance servers that process many plaintext 
    /// authentication attempts at a time, such as mail or Web servers. 
    /// The LogonUser function does not cache credentials for this logon type. 
    Batch = 4,
    /// Indicates a service-type logon. The account provided must have the service privilege enabled. 
    Service = 5,
    /// This logon type is for GINA DLLs that log on users who will be interactively using the computer. 
    /// This logon type can generate a unique audit record that shows when the workstation was unlocked. 
    Unlock = 7,
    /// This logon type preserves the name and password in the authentication package, which allows the server to make 
    /// connections to other network servers while impersonating the client. A server can accept plaintext credentials 
    /// from a client, call LogonUser, verify that the user can access the system across the network, and still 
    /// communicate with other servers. 
    /// NOTE: Windows NT: This value is not supported. 
    NetworkCleartText = 8,
    /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections. 
    /// The new logon session has the same local identifier but uses different credentials for other network connections. 
    /// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider. 
    /// NOTE: Windows NT: This value is not supported. 
    NewCredentials = 9,
}
public enum LogonProvider
{
    /// Use the standard logon provider for the system. 
    /// The default security provider is negotiate, unless you pass NULL for the domain name and the user name 
    /// is not in UPN format. In this case, the default provider is NTLM. 
    /// NOTE: Windows 2000/NT: The default security provider is NTLM. 
    Default = 0,
}
public class Impersonation : IDisposable 
{ 
    #region Dll Imports 
    /// Closes an open object handle. 
    /// A handle to an open object. 
    /// True when succeeded; otherwise false . 
    [DllImport("kernel32.dll")] 
    private static extern Boolean CloseHandle(IntPtr hObject);
    /// Attempts to log a user on to the local computer. 
    /// This is the name of the user account to log on to. 
    /// If you use the user principal name (UPN) format, user@DNSdomainname, the 
    /// domain parameter must be null . 
    /// Specifies the name of the domain or server whose 
    /// account database contains the lpszUsername account. If this parameter 
    /// is null , the user name must be specified in UPN format. If this 
    /// parameter is ".", the function validates the account by using only the 
    /// local account database. 
    /// The password 
    /// The logon type 
    /// The logon provides 
    /// The out parameter that will contain the user 
    /// token when method succeeds. 
    /// True when succeeded; otherwise false . 
    [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
    private static extern bool LogonUser( string username, string domain, string password, LogonType logonType, LogonProvider logonProvider, out IntPtr userToken );
    /// Creates a new access token that duplicates one already in existence.
    /// Handle to an access token. 
    /// The impersonation level. 
    /// Reference to the token to duplicate. 
    [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
    private static extern bool DuplicateToken( IntPtr token, int impersonationLevel, ref IntPtr duplication );
    /// The ImpersonateLoggedOnUser function lets the calling thread impersonate the 
    /// security context of a logged-on user. The user is represented by a token handle.
    /// Handle to a primary or impersonation access token that represents a logged-on user.
    /// If the function succeeds, the return value is nonzero. 
    [DllImport("advapi32.dll", SetLastError=true)]
    static extern bool ImpersonateLoggedOnUser( IntPtr userToken ); 
    #endregion 
    #region Private members 
    /// true if disposed; otherwise, false . 
    private bool _disposed; 
    /// Holds the created impersonation context and will be used 
    /// for reverting to previous user. 
    private WindowsImpersonationContext _impersonationContext;
    #endregion 
    #region Ctor & Dtor
    /// Initializes a new instance of the  class and
    /// impersonates with the specified credentials.
    /// his is the name of the user account to log on
    /// to. If you use the user principal name (UPN) format,
    /// user@DNS_domain_name, the lpszDomain parameter must be null .
    /// The name of the domain or server whose account 
    /// database contains the lpszUsername account. If this parameter is null,
    /// the user name must be specified in UPN format. If this parameter is ".",
    /// the function validates the account by using only the local account database.
    /// The plaintext password for the user account. 
    public Impersonation( String username, String domain, String password )
    {
        IntPtr userToken;
        IntPtr userTokenDuplication = IntPtr.Zero;
        // Logon with user and get token. 
        bool loggedOn = LogonUser( username, domain, password, LogonType.Interactive, LogonProvider.Default, out userToken );
        if( loggedOn )
        {
            try 
            {
                // Create a duplication of the usertoken, this is a solution
                // for the known bug that is published under KB article Q319615. 
                if( DuplicateToken( userToken, 2, ref userTokenDuplication ) )
                {
                    // Create windows identity from the token and impersonate the user. 
                    WindowsIdentity identity = new WindowsIdentity( userTokenDuplication );
                    _impersonationContext = identity.Impersonate();
                }
                else
                {
                    // Token duplication failed! 
                    // Use the default ctor overload 
                    // that will use Mashal.GetLastWin32Error(); 
                    // to create the exceptions details. 
                    throw new Win32Exception();
                }
            }
            finally
            {
                // Close usertoken handle duplication when created.
                if( !userTokenDuplication.Equals( IntPtr.Zero ) )
                {
                    // Closes the handle of the user.
                    CloseHandle( userTokenDuplication );
                }
                // Close usertoken handle when created.
                if( !userToken.Equals( IntPtr.Zero ) )
                {
                    // Closes the handle of the user. 
                    CloseHandle( userToken );
                }
            }
        }
        else
        {
            // Logon failed!
            // Use the default ctor overload that
            // will use Mashal.GetLastWin32Error();
            // to create the exceptions details. 
            throw new Win32Exception();
        }
    }
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// is reclaimed by garbage collection.
    ~Impersonation()
    {
        Dispose( false );
    }
    #endregion
    #region Public methods
    /// Reverts to the previous user.
    public void Revert()
    {
        if( _impersonationContext != null )
        {
            // Revert to previour user.
            _impersonationContext.Undo();
            _impersonationContext = null; 
        }
    }
    #endregion 
    #region IDisposable implementation.
    /// Performs application-defined tasks associated with freeing, releasing, or 
    /// resetting unmanaged resources and will revent to the previous user when 
    /// the impersonation still exists. 
    public void Dispose()
    {
        Dispose( true ); 
        GC.SuppressFinalize(this);
    }
    /// Performs application-defined tasks associated with freeing, releasing, or 
    /// resetting unmanaged resources and will revent to the previous user when 
    /// the impersonation still exists. 
    /// Specify true when calling the method directly 
    /// or indirectly by a user’s code; Otherwise false .
    protected virtual void Dispose( bool disposing )
    {
        if( !_disposed )
        {
            Revert();
            _disposed = true; 
        }
    }
    #endregion 
}

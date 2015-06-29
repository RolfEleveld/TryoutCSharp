using System.Web.Security;


namespace Utility
{
    public class UserManager
    {
        public static void GetStatusResult( MembershipCreateStatus status )
        {
            switch ( status )
            {
                case MembershipCreateStatus.DuplicateEmail:
                    //
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    //
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    //
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    //
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    //
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    //
                    break;
                case MembershipCreateStatus.InvalidProviderUserKey:
                    //
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    //
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    //
                    break;
                case MembershipCreateStatus.ProviderError:
                    //
                    break;
                case MembershipCreateStatus.Success:
                    //
                    break;
                case MembershipCreateStatus.UserRejected:
                    //
                    break;
            }
        }
    }
}
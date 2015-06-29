using System;

namespace Utility
{
    public static class HttpExtenders
    {
        public static Uri MashUpWithPathAndQuery( this Uri uri, string pathAndQuery )
        {
            // basic join
            return new Uri( uri, pathAndQuery );
        }
    }
}
using System;
using System.Linq;

namespace Utility
{
    public class Maths
    {
        public int GreatestCommonDenominator( int result )
        {
            //Func<int, int, int> gcd = null;
            //gcd = (x, y) => x%y == 0 ? y : gcd(y, x%y);
            //return Enumerable.Range(1, 20).Aggregate(1, (x, y) => x*(y/gcd(x, y)));
            return 1;
        }
        public long LeastCommonMultiple ( int x, int y)
        {
            //Func<int, int, int> gcd = null;
            //gcd = ( x, y ) => x % y == 0 ? y : gcd( y, x % y );
            //return Enumerable.Range( 1, 20 ).Aggregate( 1, ( x, y ) => x * ( y / gcd( x, y ) ) );
            return 1;
        }
    }
}
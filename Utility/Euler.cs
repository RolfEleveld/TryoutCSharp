#region usings
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Utility
{
    public class Euler
    {
        /// <summary>
        /// Gets the sum of a sequence with a divisor.
        /// </summary>
        /// <param name="fromValue">From value.</param>
        /// <param name="toValue">To value.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns></returns>
        public long GetSum( int fromValue, int toValue, int divisor )
        {
            return Enumerable.Range( fromValue, toValue ).Where( n => n % divisor == 0 ).Sum();
        }

        /// <summary>
        /// Gets the fibonacci sum up to a specified value divisible by divisor.
        /// </summary>
        /// <param name="toValue">To value.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns></returns>
        public long GetFibonacciSum( int toValue, int divisor )
        {
            return FibonacciSequence().TakeWhile( n => n <= toValue ).Where( n => n % divisor == 0 ).Sum();
        }

        /// <summary>
        /// The whole Fibonacci sequence.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> FibonacciSequence()
        {
            int n1 = 0;
            int n2 = 1;

            while ( n1 + n2 < int.MaxValue )
            {
                int n3 = n1 + n2;
                yield return n3;
                n1 = n2;
                n2 = n3;
            }
            // This method will return the indefinate list
            // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns

        /// <summary>
        /// Primes the factors.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public IEnumerable<ulong> PrimeFactors( ulong number )
        {
            Func<ulong, ulong> FindDivisor = n => n == 1UL ? 1UL : CandidateFactors( n ).First( c => n % c == 0 );

            var factors = new HashSet<ulong>();
            while ( number > 1UL )
            {
                ulong divisor = FindDivisor( number );
                factors.Add( divisor );
                number /= divisor;
            }
            return factors;
        }

        /// <summary>
        /// Candidates the factors.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public IEnumerable<ulong> CandidateFactors( ulong n )
        {
            yield return 2UL;
            for ( ulong candidate = 3UL; candidate <= n; candidate += 2UL )
            {
                yield return candidate;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class Equilibrium
    {
        public int Equilibrium(int[] collection)
        {
            // Return P such that 0<=P<len(A)
            // strategy used is to run to the first equilibrium for which sum of lower indices = sum of higher
            // return -1 if not found.
            long sumlower = 0;

            // Get the sum of all values.
            long sumhigher = 0;
            int p = -1;
            if (collection.Length > 0)
            {
                sumhigher = collection.Sum() - collection[0];
                for (int i = 1; i < collection.Length; i++)
                {
                    sumlower += collection[i - 1];
                    sumhigher -= collection[i];
                    if (sumlower == sumhigher)
                    {
                        p = i;
                        break;
                    }
                }
            }
            else
            {
                p = 0;
            }
            return p;
        }
    }
}

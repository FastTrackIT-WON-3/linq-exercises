using System.Collections.Generic;

namespace LinqExercises
{
    public static class NumbersGenerator
    {
        public static IEnumerable<int> Generate()
        {
            int start = 0;
            while (true)
            {
                // Console.WriteLine($"Infinite generator: {start}");
                yield return start;
                if (start < int.MaxValue)
                {
                    start++;
                }
                else
                {
                    start = 0;
                }
            }
        }
    }
}

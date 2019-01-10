using System;

namespace StringsSimilarityAlgorithms
{
    /// <summary>
    /// Class that implements Jaro algorithm for evaluating extent of similarity of 2 strings
    /// </summary>
    /// <remarks>
    /// For the full description visit: <see ref="https://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance"/>
    /// </remarks>
    public class JaroAlgorithm : ISimilarityAlgorithm
    {
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Is thrown when one of arguments is null</exception>
        public double GetSimilarity(string s1, string s2)
        {
            if (s1 == null) throw new ArgumentNullException(nameof(s1));

            if (s2 == null) throw new ArgumentNullException(nameof(s2));

            if (s1.Length == 0 && s2.Length == 0) return 1;

            if (s1.Length == 0 || s2.Length == 0) return 0;

            CheckMatches(s1, s2, out var m, out var t);

            return m == 0 ? 0 : ((double) m / s1.Length + (double) m / s2.Length + (double) (m - t) / m) / 3;
        }

        private static int GetMatchingDistance(string s1, string s2)
        {
            var maxLength = Math.Max(s1.Length, s2.Length);

            return maxLength / 2 - 1;
        }
        
        private static void CheckMatches(string s1, string s2, out int matches, out int transpositions)
        {
            matches = 0;
            transpositions = 0;

            var delta = GetMatchingDistance(s1, s2);
            var matched1 = new bool[s1.Length];
            var matched2 = new bool[s2.Length];

            // calculate matches of any type within the specific distance from the original position
            for (var i = 0; i < s1.Length; i++)
            {
                var start = Math.Max(0, i - delta);
                var end = Math.Min(i + delta + 1, s2.Length);

                for (var j = start; j < end; j++)
                {
                    if (matched2[j]) continue;

                    if (s1[i] != s2[j]) continue;

                    matched1[i] = true;
                    matched2[j] = true;

                    matches++;

                    break;
                }
            }

            if (matches == 0) return;

            // calculate number of transpositions
            var k = 0;
            for (var i = 0; i < s1.Length; i++)
            {
                if (!matched1[i]) continue;

                while (!matched2[k]) k++;

                if (s1[i] != s2[k]) transpositions++;

                k++;
            }

            transpositions /= 2; // due to symmetry
        }
    }
}

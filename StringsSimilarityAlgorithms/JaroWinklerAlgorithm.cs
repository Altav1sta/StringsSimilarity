using System;

namespace StringsSimilarityAlgorithms
{
    /// <summary>
    /// Class that implements Jaro-Winkler algorithm for evaluating extent of similarity of 2 strings
    /// </summary>
    /// <remarks>
    /// For the full description visit: <see ref="https://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance"/> 
    /// </remarks>
    public class JaroWinklerAlgorithm : ISimilarityAlgorithm
    {
        private double _scalingFactor = 0.1;
        private double _threshold = 0.7;

        private readonly JaroAlgorithm _jaroAlgorithm = new JaroAlgorithm();

        /// <summary>
        /// Used for defining how much the score is adjusted upwards for having common prefixes. Range: [0; 0.25]
        /// </summary>
        public double ScalingFactor
        {
            get => _scalingFactor;

            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Value of scaling factor should be a positive number");
                }

                if (value > 0.25)
                {
                    throw new InvalidOperationException("Value of scaling factor should not exceed 0.25");
                }

                _scalingFactor = value;
            }
        }

        /// <summary>
        /// Used for defining starting distance starting from which scaling factor is applied. Range: [0; 1]
        /// </summary>
        public double Threshold
        {
            get => _threshold;

            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Value of threshold should be a positive number");
                }

                if (value > 1)
                {
                    throw new InvalidOperationException("Value of threshold should not exceed 1");
                }

                _threshold = value;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Is thrown when one of arguments is null</exception>
        public double GetSimilarity(string s1, string s2)
        {
            var similarity = _jaroAlgorithm.GetSimilarity(s1, s2);

            if (similarity > _threshold)
            {
                var l = GetLengthOfCommonPrefix(s1, s2);
                l = Math.Min(l, 4);
                similarity = similarity + l * _scalingFactor * (1 - similarity);
            }

            return similarity;
        }

        private static int GetLengthOfCommonPrefix(string s1, string s2)
        {
            var minLength = Math.Min(s1.Length, s2.Length);

            for (var i = 0; i < minLength; i++)
            {
                if (s1[i] != s2[i]) return i;
            }

            return minLength;
        }
    }
}

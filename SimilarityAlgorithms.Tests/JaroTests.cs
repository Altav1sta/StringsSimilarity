using System;
using StringsSimilarityAlgorithms;
using Xunit;

namespace SimilarityAlgorithms.Tests
{
    public class JaroTests
    {
        private readonly ISimilarityAlgorithm _algorithm = new JaroAlgorithm();

        [Fact]
        public void GetSimilarity_Returns1ForEqualStrings()
        {
            var s1 = Guid.NewGuid().ToString();
            var s2 = s1;

            var actual = _algorithm.GetSimilarity(s1, s2);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void GetSimilarity_Returns0ForAbsolutelyDifferentStrings()
        {
            var s1 = new string('a', 10);
            var s2 = new string('b', 10);

            var actual = _algorithm.GetSimilarity(s1, s2);

            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(null, "123")]
        [InlineData("123", null)]
        [InlineData(null, null)]
        public void GetSimilarity_ThrowsExceptionForNulls(string s1, string s2)
        {
            Assert.Throws<ArgumentNullException>(() => _algorithm.GetSimilarity(s1, s2));
        }

        [Theory]
        [InlineData("", "123", 0)]
        [InlineData("123", "", 0)]
        [InlineData("", "", 1)]
        public void GetSimilarity_WorksProperlyWithEmptyStrings(string s1, string s2, double expected)
        {
            var actual = _algorithm.GetSimilarity(s1, s2);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("MARTHA", "MARHTA", (double) 17 / 18)]
        [InlineData("DWAYNE", "DUANE", (double) 37 / 45)]
        [InlineData("DIXON", "DICKSONX", (double) 23 / 30)]
        public void GetSimilarity_WorksProperlyOnWikiExamples(string s1, string s2, double expected)
        {
            var actual = _algorithm.GetSimilarity(s1, s2);

            Assert.Equal(expected, actual, 10);
        }
    }
}

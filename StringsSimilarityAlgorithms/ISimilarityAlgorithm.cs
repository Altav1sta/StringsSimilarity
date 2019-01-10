namespace StringsSimilarityAlgorithms
{
    /// <summary>
    /// Algorithm for evaluating measure of similarity of 2 strings
    /// </summary>
    public interface ISimilarityAlgorithm
    {
        /// <summary>
        /// Gets the extent of similarity between two strings.
        /// Range: [0; 1], where 0 - for absolutely different strings and 1 - for absolutely equal strings
        /// </summary>
        double GetSimilarity(string s1, string s2);
    }
}

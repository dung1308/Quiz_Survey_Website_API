using System.Collections.Generic;
using System.Linq;

public static class StringUtils
{
    public static double JaccardSimilarity(string str1, string str2)
    {
        var set1 = new HashSet<char>(str1);
        var set2 = new HashSet<char>(str2);
        var intersection = set1.Intersect(set2).Count();
        var union = set1.Union(set2).Count();
        return (double)intersection / union;
    }
}

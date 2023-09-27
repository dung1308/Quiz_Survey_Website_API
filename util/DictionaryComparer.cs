namespace survey_quiz_app.util;

public class DictionaryComparer<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
{
    public bool Equals(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
    {
        // Check if count of dictionaries are equal
        if (x.Count != y.Count)
        {
            return false;
        }

        // Check if all key-value pairs are equal
        foreach (KeyValuePair<TKey, TValue> pair in x)
        {
            TValue value;
            if (!y.TryGetValue(pair.Key, out value) || !pair.Value.Equals(value))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(IDictionary<TKey, TValue> obj)
    {
        int hash = 0;
        foreach (KeyValuePair<TKey, TValue> pair in obj)
        {
            int key = pair.Key.GetHashCode();
            int value = pair.Value.GetHashCode();
            hash ^= RotateLeft(key, 5) ^ value;
        }
        return hash;
    }

    private int RotateLeft(int number, int positions)
    {
        uint wrapped = (uint)number << positions;
        return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
    }
}

public static class DictionaryListComparer
{
    public static List<bool> Compare(List<IDictionary<string, object>> list1, List<IDictionary<string, object>> list2)
    {
        List<bool> boolList = new List<bool>();

        foreach (IDictionary<string, object> dict1 in list1)
        {
            bool found = false;

            foreach (IDictionary<string, object> dict2 in list2)
            {
                if (dict1["id"].Equals(dict2["id"]) && Enumerable.SequenceEqual((IEnumerable<string>)dict1["answer"], (IEnumerable<string>)dict2["answer"]))
                {
                    found = true;
                    break;
                }
            }

            boolList.Add(found);
        }

        return boolList;
    }
}
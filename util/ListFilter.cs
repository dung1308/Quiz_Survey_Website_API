namespace survey_quiz_app.util;

public static class ListFilter
{
    public static (List<T>, List<T>) Filter<T>(List<T> list, List<bool> boolList)
    {
        List<T> trueList = new List<T>();
        List<T> falseList = new List<T>();

        for (int i = 0; i < list.Count; i++)
        {
            if (boolList[i])
            {
                trueList.Add(list[i]);
            }
            else
            {
                falseList.Add(list[i]);
            }
        }

        return (trueList, falseList);
    }
}
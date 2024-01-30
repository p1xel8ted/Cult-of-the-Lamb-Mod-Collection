namespace CultOfQoL;

public static class Helpers
{

    public static IEnumerator FilterEnumerator(IEnumerator original, Type[] typesToRemove)
    {
        while (original.MoveNext())
        {
            var current = original.Current;
            if (current != null && !typesToRemove.Contains(current.GetType()))
            {
                yield return current;
            }
        }
    }

    public static string GetGameObjectPath(GameObject obj)
    {
        var path = obj.name;
        var currentParent = obj.transform.parent;

        while (currentParent != null)
        {
            path = currentParent.name + "/" + path;
            currentParent = currentParent.parent;
        }

        return path;
    }
}
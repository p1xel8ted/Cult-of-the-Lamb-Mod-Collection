using UnityEngine;

namespace CultOfQoL;

public static class Helpers
{
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
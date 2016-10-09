using System.IO;
using UnityEngine;

public static class FileLoader
{
    private const string ResourcesStart = "Resources/";
    private const string ResourcesMiddle = "/Resources/";

    public static T Load<T>(string assetPath) where T : Object
    {
        if (assetPath.StartsWith(ResourcesStart) == false && assetPath.Contains(ResourcesMiddle) == false)
        {
            Debug.LogErrorFormat("Unable to load {0}. FileLoader only supports loading through Resources.", assetPath);

            return null;
        }

        string resourcePath;

        if (assetPath.Contains("/Resources/"))
        {
            int index = assetPath.LastIndexOf(ResourcesMiddle);

            resourcePath = assetPath.Remove(0, index + ResourcesMiddle.Length);
        }
        else
        {
            resourcePath = assetPath.Remove(0, ResourcesStart.Length);
        }

        string fileExtension = Path.GetExtension(resourcePath);
        resourcePath = resourcePath.Remove(resourcePath.LastIndexOf(fileExtension), fileExtension.Length);

        T returnValue = Resources.Load<T>(resourcePath);

        return returnValue;
    }
}

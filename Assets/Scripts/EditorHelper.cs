#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class EditorHelper
{
    public static string GetAssetPath(string title, string defaultName)
    {
        return EditorUtility.SaveFilePanelInProject(title, defaultName, "Asset", title, "Asset");
    }
}
#endif

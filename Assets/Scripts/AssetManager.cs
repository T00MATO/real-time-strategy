using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public static class AssetManager
{
    private const string PUBLIC_PATH = "Asset";

    public static string GetPathWithPanel(string title, string defaultName)
    {
        return EditorUtility.SaveFilePanelInProject(title, defaultName, PUBLIC_PATH, title, PUBLIC_PATH);
    }

    public static void CreateWithPanel<T>(string title, string defaultName) where T : ScriptableObject
    {
        var path = GetPathWithPanel(title, defaultName);
        var instance = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(instance, path);
    }
}
#endif

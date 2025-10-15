using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Kamuuucka.Hierarchy.Tool;
/// <summary>
/// Adds colours to the Unity Hierarchy based on the HierarchyDataSO.
/// </summary>
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class HierarchyColor
{
    private static readonly Vector2 Offset = new(50, 1);
    private static HierarchyDataSO _hierarchyData;
    
    static HierarchyColor()
    {
        LoadData();
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    /// <summary>
    /// Find the Hierarchy Data file to load from.
    /// </summary>
    private static void LoadData()
    {
        string[] guids = AssetDatabase.FindAssets("t:HierarchyDataSO");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            _hierarchyData = AssetDatabase.LoadAssetAtPath<HierarchyDataSO>(path);
        }
    }

    /// <summary>
    /// Get instances of objects in the hierarchy window to change their colour.
    /// </summary>
    private static void HandleHierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
    {
        if (_hierarchyData == null) return;

        var obj = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
        if (obj == null) return;

        foreach (var data in _hierarchyData.hierarchyData)
        {
            if (obj.name.StartsWith(data.prefix))
            {
                ChangeHierarchyDisplay(obj, selectionRect, data);
                break;
            }
        }
    }

    /// <summary>
    /// Handle the colour change.
    /// </summary>
    /// <param name="obj">Object that will have the colour changed.</param>
    /// <param name="rect">The rectangle where the name will be.</param>
    /// <param name="data">HierarchyData that will be used to change this colour.</param>
    private static void ChangeHierarchyDisplay(GameObject obj, Rect rect, HierarchyData data)
    {
        string displayName = obj.name;
        displayName = Regex.Replace(displayName, data.prefix, "");
        obj.tag = "EditorOnly";
        
        Rect offsetRect = new Rect(rect.position + Offset, rect.size);
        Rect bgRect = new Rect(rect.x, rect.y, rect.width + 50, rect.height);

        EditorGUI.DrawRect(bgRect, data.backgroundColor);
        EditorGUI.LabelField(offsetRect, displayName, new GUIStyle()
        {
            normal = new GUIStyleState() { textColor = data.textColor },
            fontStyle = FontStyle.Bold
        });
    }
}

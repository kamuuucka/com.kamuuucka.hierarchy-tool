using System;
using UnityEngine;

namespace Kamuuucka.Hierarchy.Tool
{
    [CreateAssetMenu(fileName = "HierarchyData", menuName = "Scriptable Objects / HierarchyData", order = 1)]
    public class HierarchyDataSO : ScriptableObject
    {
        public HierarchyData[] hierarchyData;

    }

    [Serializable]
    public class HierarchyData
    {
        public string prefix = "===";
        public Color backgroundColor = Color.white;
        public Color textColor = Color.black;
    }
}
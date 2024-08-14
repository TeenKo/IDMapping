namespace GameModules.IdMapping.Editor
{
    using UnityEditor;
    using UnityEngine;

    public static class IdGeneratorContextMenu
    {
        [MenuItem("Assets/Create/Id Generator", false, 0)]
        public static void ShowIdGeneratorWindow()
        {
            var window = ScriptableObject.CreateInstance<IdGeneratorWindow>();
            window.titleContent = new GUIContent("Create ID Map");
            window.ShowUtility();
        }
    }
}
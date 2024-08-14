namespace GameModules.IdMapping.Editor
{
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class IdGeneratorWindow : EditorWindow
    {
        private string _className = "NewClass";

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Enter Name for the ID Map", EditorStyles.boldLabel);
            _className = EditorGUILayout.TextField("Name", _className);

            if (!GUILayout.Button("Create Map")) return;
            
            if (string.IsNullOrWhiteSpace(_className))
            {
                Debug.LogError("Class name cannot be empty or only contain whitespace.");
                return;
            }
                
            if (!IsValidClassName(_className))
            {
                Debug.LogError("Class name must contain only letters and no digits or special characters.");
                return;
            }
                
            GenerateScripts(_className);
            Close();
        }
        
        private bool IsValidClassName(string className)
        {
            return Regex.IsMatch(className, @"^[a-zA-Z]+$");
        }

        private void GenerateScripts(string className)
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("No folder selected. Please select a folder in the Project window.");
                return;
            }

            if (!AssetDatabase.IsValidFolder(path))
            {
                path = Path.GetDirectoryName(path);
            }

            var filePath = path.Replace("Assets", "").TrimStart('/');
            var generator = new IdGenerator { Name = className, FilePath = filePath };

            generator.FixedName();
            generator.GenerateContainerClass();
            generator.GenerateDataClass();
            generator.GenerateMapClass();
            generator.GenerateIdClass();

            AssetDatabase.Refresh();
            Debug.Log("ID Map generated successfully.");
        }
    }
}
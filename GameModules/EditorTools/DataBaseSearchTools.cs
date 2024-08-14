namespace GameModules.EditorTools
{
    using System;
    using System.IO;
    using UnityEditor;

    public static class DataBaseSearchTools
    {
        private const string FilterTemplate = "t: {0} {1}";

        public static Object GetAsset(Type type, string filter, string[] folders = null)
        {
            var searchFilter = CreateFilter(type, filter);
            var ids = AssetDatabase.FindAssets(searchFilter);

            foreach (var id in ids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(id);
                if (string.IsNullOrEmpty(assetPath)) continue;

                var fileName = Path.GetFileNameWithoutExtension(assetPath);
                if (!fileName.Equals(filter, StringComparison.OrdinalIgnoreCase))
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
                if (asset != null) return asset;
            }

            foreach (var id in ids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(id);
                if (string.IsNullOrEmpty(assetPath)) continue;

                var asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
                if (asset != null) return asset;
            }

            return null;
        }

        private static string CreateFilter(Type type, string filter) =>
            string.Format(FilterTemplate, type.Name, filter);
    }
}
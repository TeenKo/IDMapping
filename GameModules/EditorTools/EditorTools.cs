namespace GameModules.EditorTools
{
    using System.IO;
    using UnityEngine;

    public static class EditorTools
    {
        private const char MoveDirectorySeparator = '/';
        private const string AssetsFolderName = "Assets";

        public static T GetAsset<T>(string[] folders = null) where T : Object
        {
            var asset = DataBaseSearchTools.GetAsset(typeof(T), string.Empty, folders) as T;
            return asset;
        }
        
        public static string FixDirectoryPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            
            return path.TrimEndPath() + Path.DirectorySeparatorChar;
        }

        private static string TrimEndPath(this string path)
        {
            return string.IsNullOrEmpty(path) ?
                path :
                path.TrimEnd(Path.PathSeparator).TrimEnd(MoveDirectorySeparator).TrimEnd('\\');
        }
    }
}
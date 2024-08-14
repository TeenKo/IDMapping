namespace GameModules.IdMapping.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class IdGenerator
    {
        public string Name;
        public string FilePath;

        public void GenerateContainerClass() => GenerateClass(Name, $@"namespace {CreateNameSpace(FilePath)}
{{
    using System;
    
    [Serializable]
    public class {Name}
    {{
        public string name;
        public int id;
    }}
}}
");

        public void GenerateDataClass() => GenerateClass(Name + "Data", $@"namespace {CreateNameSpace(FilePath)}
{{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;

    [Serializable]
    public class {Name}Data
    {{
        [InlineProperty]
        public List<{Name}> collection = new();
    }}
}}
");

        public void GenerateMapClass() => GenerateClass(Name + "Map", $@"namespace {CreateNameSpace(FilePath)}
{{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Sirenix.OdinInspector;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(menuName = ""Game/Maps/{Name} Map"", fileName = ""{Name} Map"")]
    public class {Name}Map : ScriptableObject
    {{
        [InlineProperty]
        [HideLabel]
        public {Name}Data value = new();

        #region IdGenerator

#if UNITY_EDITOR
        [Button(""Generate Static Properties"")]
        public void GenerateProperties()
        {{
            GenerateStaticProperties(this);
        }}

        public static void GenerateStaticProperties({Name}Map map)
        {{
            var idType = typeof({Name}Id);
            var idTypeName = nameof({Name}Id);
            var scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(map));
            var directoryPath = Path.GetDirectoryName(scriptPath);
            var outputPath = Path.Combine(directoryPath, ""Generated"");
            var outputFileName = $""{{idTypeName}}.Generated.cs"";

            if (!Directory.Exists(outputPath))
            {{
                Directory.CreateDirectory(outputPath);
            }}

            var namespaceName = idType.Namespace;
            var filePath = Path.Combine(outputPath, outputFileName);

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {{
                writer.WriteLine($""namespace {{namespaceName}}"");
                writer.WriteLine(""{{"");
                writer.WriteLine($""    public partial struct {{idTypeName}}"");
                writer.WriteLine(""    {{"");

                var typesField = typeof({Name}Data)
                    .GetField(""collection"", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (typesField != null)
                {{
                    var types = (List<{Name}>)typesField.GetValue(map.value);
                    foreach (var type in types)
                    {{
                        var propertyName = type.name.Replace("" "", """");
                        writer.WriteLine($""        public static {{idTypeName}} {{propertyName}} = new {{idTypeName}} {{{{ value = {{type.id}} }}}};"");
                    }}
                }}

                writer.WriteLine(""    }}"");
                writer.WriteLine(""}}"");
            }}

            AssetDatabase.Refresh();
            Debug.Log(""Partial class with static properties generated successfully."");
        }}
#endif

        #endregion
    }}
}}
");

        public void GenerateIdClass() => GenerateClass(Name + "Id", $@"namespace {CreateNameSpace(FilePath)}
{{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
    using UnityEngine;

#if UNITY_EDITOR
    using GameModules.EditorTools;
#endif

    [Serializable]
    [ValueDropdown(""@{CreateNameSpace(FilePath)}.{Name}Id.Get{Name}Ids()"", IsUniqueList = true, DropdownTitle = ""{Name}"")]
    public partial struct {Name}Id
    {{
        [SerializeField]
        public int value;

        #region static editor data

        private static {Name}Map _map;

        public static IEnumerable<ValueDropdownItem<{Name}Id>> Get{Name}Ids()
        {{
            #if UNITY_EDITOR
            _map ??= EditorTools.GetAsset<{Name}Map>();
            var types = _map;
            if (types == null)
            {{
                yield return new ValueDropdownItem<{Name}Id>()
                {{
                    Text = ""EMPTY"",
                    Value = ({Name}Id)0,
                }};
                yield break;
            }}

            foreach (var type in types.value.collection)
            {{
                yield return new ValueDropdownItem<{Name}Id>()
                {{
                    Text = type.name,
                    Value = ({Name}Id)type.id,
                }};
            }}
            #endif
            yield break;
        }}

        public static string Get{Name}Name({Name}Id id)
        {{
#if UNITY_EDITOR
            var types = Get{Name}Ids();
            var filteredTypes = types.FirstOrDefault(x => x.Value == id);
            var name = filteredTypes.Text;
            return string.IsNullOrEmpty(name) ? string.Empty : name;
#endif
            return string.Empty;
        }}

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {{
            _map = null;
        }}

        #endregion

        public static implicit operator int({Name}Id v) => v.value;
        public static explicit operator {Name}Id(int v) => new {Name}Id {{ value = v }};
        public override string ToString() => value.ToString();
        public override int GetHashCode() => value;

        public {Name}Id FromInt(int data)
        {{
            value = data;
            return this;
        }}

        public override bool Equals(object obj)
        {{
            if (obj is {Name}Id mask) return mask.value == value;
            return false;
        }}
    }}
}}
");
        
        public void FixedName()
        {
            Name = Name.Trim();
            
            if (!string.IsNullOrEmpty(Name))
            {
                Name = char.ToUpper(Name[0]) + Name.Substring(1);
            }
        }

        private void GenerateClass(string className, string scriptContent)
        {
            var path = FixClassPath(FilePath, className);
            File.WriteAllText(path, scriptContent);
            AssetDatabase.Refresh();
            Debug.Log($"{className} class generated successfully.");
        }
        

        private string FixClassPath(string path, string name) =>
            string.IsNullOrEmpty(path) ? path : Path.Combine("Assets", path, $"{name}.cs");

        private string CreateNameSpace(string path) 
        {
            var result = path.Replace("/", ".");
            result = result.Replace(" ", string.Empty);

            return result;
        }
    }
}
namespace GameModules.IdMapping.Demo.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EditorTools;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    [ValueDropdown("@GameModules.IdMapping.Demo.Data.ShapeId.GetFigureIds()", IsUniqueList = true, DropdownTitle = "Shape")]
    public partial struct ShapeId
    {
        [SerializeField]
        public int value;

        #region static editor data

        private static ShapeMap _map;

        public static IEnumerable<ValueDropdownItem<ShapeId>> GetFigureIds()
        {
            #if UNITY_EDITOR
            _map ??= EditorTools.GetAsset<ShapeMap>();
            var types = _map;
            if (types == null)
            {
                yield return new ValueDropdownItem<ShapeId>()
                {
                    Text = "EMPTY",
                    Value = (ShapeId)0,
                };
                yield break;
            }

            foreach (var type in types.value.collection)
            {
                yield return new ValueDropdownItem<ShapeId>()
                {
                    Text = type.name,
                    Value = (ShapeId)type.id,
                };
            }
            #endif
            yield break;
        }

        public static string GetFigureName(ShapeId id)
        {
#if UNITY_EDITOR
            var types = GetFigureIds();
            var filteredTypes = types.FirstOrDefault(x => x.Value == id);
            var name = filteredTypes.Text;
            return string.IsNullOrEmpty(name) ? string.Empty : name;
#endif
            return string.Empty;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            _map = null;
        }

        #endregion

        public static implicit operator int(ShapeId v) => v.value;
        public static explicit operator ShapeId(int v) => new ShapeId { value = v };
        public override string ToString() => value.ToString();
        public override int GetHashCode() => value;

        public ShapeId FromInt(int data)
        {
            value = data;
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj is ShapeId mask) return mask.value == value;
            return false;
        }
    }
}

namespace GameModules.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using UnityEngine;
    using Random = System.Random;

    public static class GameTools
    { 
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            var rnd = new Random();
            return source.OrderBy(item => rnd.Next());
        }
        
        public static bool IsInRange(in Vector3 source, in Vector3 target, in float range)
        {
            return (source - target).sqrMagnitude <= range * range;
        }

        public static bool IsInRange(in Vector3 source, in Vector3 target, in float range, out float sqrDistance)
        {
            sqrDistance = (source - target).sqrMagnitude;
            return sqrDistance <= range * range;
        }
        
        public static Vector3 ClampToFlatArea(Vector3 source, Bounds area)
        {
            var clampedX = Mathf.Clamp(source.x, area.min.x, area.max.x);
            var clampedZ = Mathf.Clamp(source.z, area.min.z, area.max.z);

            return new Vector3(clampedX, source.y, clampedZ);
        }

        public static bool IsInFlatRange(in Vector3 source, in Vector3 target, in float range)
        {
            return GetFlatSqrDistance(source, target) <= range * range;
        }

        public static bool IsInFlatArea(in Vector3 source, in Bounds area)
        {
            return source.x > area.min.x && source.x < area.max.x &&
                   source.z > area.min.z && source.z < area.max.z;
        }

        public static bool IsInArea(in Vector3 source, in Bounds area)
        {
            return source.x > area.min.x && source.x < area.max.x &&
                   source.y > area.min.y && source.y < area.max.y &&
                   source.z > area.min.z && source.z < area.max.z;
        }

        public static float GetFlatSqrDistance(in Vector3 source, in Vector3 target)
        {
            return new Vector3(source.x - target.x, 0, source.z - target.z).sqrMagnitude;
        }

        public static Vector3 GetOffsetPosition(in Vector3 source, in Vector3 target, in float offset)
        {
            var direction = target - source;
            return source + direction.normalized * (direction.magnitude - offset);
        }

        public static Quaternion LookAtTarget(in Vector3 sourcePosition, in Vector3 targetPosition)
        {
            var direction = Vector3.Scale(sourcePosition - targetPosition, new Vector3(1, 0, 1));

            return Quaternion.LookRotation(direction, Vector3.up);
        }

        public static int GetLayerIndexFromLayerMask(in LayerMask layerMask)
        {
            var layerMaskValue = layerMask.value;
            var layerLog = layerMaskValue <= 0 ? 0 : Mathf.Log(layerMaskValue, 2);
            var layerLogInt = (int)layerLog;
            var layerLogMod = layerLogInt > 0 ? (int)(layerLog % layerLogInt) : 0;
            var layerIndex = layerLogMod == 0 ? layerLogInt : 0;

            return layerIndex;
        }

        public static int GetMinIndexOfEnum<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<int>().Min();
        }

        public static int GetMaxIndexOfEnum<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<int>().Max();
        }

        public static string RGBAToStringFromColor(Color color)
        {
            var colorRGBA = $"{color.r}f; {color.g}f; {color.b}f; {color.a}f";
            colorRGBA = colorRGBA.Replace(',', '.');
            colorRGBA = colorRGBA.Replace(';', ',');
            return colorRGBA;
        }
        
        public static bool GetFirstComponent<T>([NotNull] Transform transform, out T component) where T : Component
        {
            if (transform.TryGetComponent(out T comp))
            {
                component = comp;
                return true;
            }

            if (transform.Cast<Transform>().Any(child => GetFirstComponent(child, out comp)))
            {
                component = comp;
                return true;
            }

            component = null;
            return false;
        }

        public static void FindComponentsInParent<T>([NotNull] Transform transform, ref T[] components)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out T component))
                {
                    Array.Resize(ref components, components.Length + 1);
                    components[components.Length - 1] = component;
                }

                if (child.childCount > 0)
                {
                    FindComponentsInParent(child, ref components);
                }
            }
        }

        public static void FindComponentsInParent<T>([NotNull] Transform transform, ref List<T> components)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out T component))
                {
                    components.Add(component);
                }

                if (child.childCount > 0)
                {
                    FindComponentsInParent(child, ref components);
                }
            }
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }

        public static T[] GetRandomElements<T>(T[] collection, int count)
        {
            var collectionLength = collection.Length;
            if (count < 1) count = 1;
            if (collectionLength < count) count = collection.Length;
            var availableIndices = new List<int>();
            for (var i = 0; i < collectionLength; i++) availableIndices.Add(i);
            var elements = new T[count];
            for (var i = 0; i < count; i++)
            {
                var index = UnityEngine.Random.Range(0, availableIndices.Count);
                elements[i] = collection[availableIndices[index]];
                availableIndices.RemoveAt(index);
            }

            return elements;
        }

        public static Bounds GetBoundsOfPoints(in IEnumerable<Vector3> points)
        {
            var min = Vector3.positiveInfinity;
            var max = Vector3.negativeInfinity;
            foreach (var point in points)
            {
                if (point.x < min.x) min.x = point.x;
                if (point.y < min.y) min.y = point.y;
                if (point.z < min.z) min.z = point.z;
                if (point.x > max.x) max.x = point.x;
                if (point.y > max.y) max.y = point.y;
                if (point.z > max.z) max.z = point.z;
            }

            var center = (min + max) / 2;
            return new Bounds(center, max - min);
        }

        public static Bounds GetBoundsOfPoints(in List<Vector3> points)
        {
            var min = Vector3.positiveInfinity;
            var max = Vector3.negativeInfinity;
            foreach (var point in points)
            {
                if (point.x < min.x) min.x = point.x;
                if (point.y < min.y) min.y = point.y;
                if (point.z < min.z) min.z = point.z;
                if (point.x > max.x) max.x = point.x;
                if (point.y > max.y) max.y = point.y;
                if (point.z > max.z) max.z = point.z;
            }

            var center = (min + max) / 2;
            return new Bounds(center, max - min);
        }

        public static Vector3 GetRandomPointInsideBounds(Bounds Bounds)
        {
            Vector3 extents = Bounds.size / 2f;

            Vector3 point = new Vector3(
                UnityEngine.Random.Range(-extents.x, extents.x),
                UnityEngine.Random.Range(-extents.y, extents.y),
                UnityEngine.Random.Range(-extents.z, extents.z)
            ) + Bounds.center;

            return point;
        }


        public static class Bezier
        {
            /// <summary>Calculate and return position on bezier curve throw t-time</summary>
            /// <param name="p0">source position</param>
            /// <param name="p1">tangent position</param>
            /// <param name="p2">target position</param>
            /// <param name="t">time clamped to 0...1</param>
            /// <returns></returns>
            public static Vector3 QuadraticBezierCurvePoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
            {
                t = Mathf.Clamp01(t);
                // B(t) = (1 - t)^2 * P0 + 2 * (1 - t) * t * P1 + t^2 * P2
                var oneMinusT = 1 - t;
                var doubleT = t * t;
                return oneMinusT * oneMinusT * p0 + 2 * oneMinusT * t * p1 + doubleT * p2;
            }

            public static Vector3 QuadraticBezierCurvePoint(in Vector3 originPoint, in Vector3 targetPoint,
                in float handleRatio, in float t)
            {
                var distance = targetPoint - originPoint;
                var middlePoint = originPoint + distance * handleRatio;
                middlePoint[1] = distance.sqrMagnitude / 50;
                return QuadraticBezierCurvePoint(originPoint, middlePoint, targetPoint, t);
            }

            /// <summary>
            /// Create and out bezier points
            /// </summary>
            /// <param name="p0">source position</param>
            /// <param name="p1">tangent position</param>
            /// <param name="p2">target position</param>
            /// <param name="numPoints">points number</param>
            /// <param name="points">points array</param>
            public static void QuadraticBezierCurve(in Vector3 p0, in Vector3 p1, in Vector3 p2, int numPoints,
                out Vector3[] points)
            {
                numPoints = Mathf.Max(4, numPoints);
                points = new Vector3[numPoints + 1];
                for (var i = 0; i <= numPoints; i++)
                {
                    var t = (float)i / numPoints;
                    points[i] = QuadraticBezierCurvePoint(p0, p1, p2, t);
                }
            }
        }
    }
}
namespace GameModules.IdMapping.Demo.Logic
{
    using Data;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public class ShapeController : MonoBehaviour
    {
        [BoxGroup("General")]
        public SpriteRenderer spriteRenderer;
        [BoxGroup("General")]
        public TextMeshPro text;
        
        [BoxGroup("Data")]
        public ShapeMap shapeMap;
        
        private ShapeId _currentShapeId;
        private ShapeData _data;

        [ResponsiveButtonGroup("First")]
        public void SetRandom()
        {
            var collection = _data.collection;
            var randomIndex = Random.Range(0, collection.Count);
            var randomElement = collection[randomIndex];

            _currentShapeId = (ShapeId)randomElement.id;
        }

        [ResponsiveButtonGroup("Second")]
        public void Circle()
        {
            _currentShapeId = ShapeId.Circle;
        }
        
        [ResponsiveButtonGroup("Second")]
        public void Rectangle()
        {
            _currentShapeId = ShapeId.Rectangle;
        }
        
        [ResponsiveButtonGroup("Second")]
        public void Square()
        {
            _currentShapeId = ShapeId.Square;
        }
        
        [ResponsiveButtonGroup("Second")]
        public void Triangle()
        {
            _currentShapeId = ShapeId.Triangle;
        }

        private void Start()
        {
            _data = shapeMap.value;
            SetRandom();
        }

        private void Update()
        {
            UpdateShapeData();
        }

        private void UpdateShapeData()
        {
            var shape = _data.collection[_currentShapeId];

            spriteRenderer.sprite = shape.sprite;
            spriteRenderer.color = shape.color;

            text.text = shape.text;
            text.color = shape.colorText;
        }
    }
}
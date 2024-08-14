namespace GameModules.IdMapping.Demo.Data
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;

    [Serializable]
    public class ShapeData
    {
        [InlineProperty]
        public List<Shape> collection = new();
    }
}

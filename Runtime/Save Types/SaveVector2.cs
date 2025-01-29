using UnityEngine;

namespace Floofinator.SimpleSave
{
    [System.Serializable]
    public struct SaveVector2
    {
        public float x,y;
        public SaveVector2(Vector2 value)
        {
            x = value.x;
            y = value.y;
        }
        public readonly Vector2 Extract()
        {
            return new Vector2(x,y);
        }
        public static implicit operator SaveVector2(Vector2 value)
        {
            return new SaveVector2(value);
        }
        public static implicit operator Vector2(SaveVector2 value)
        {
            return value.Extract();
        }
    }
}
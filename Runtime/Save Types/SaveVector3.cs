using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    [System.Serializable]
    public struct SaveVector3
    {
        public float x,y,z;
        public SaveVector3(Vector3 value)
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
        readonly Vector3 Extract()
        {
            return new Vector3(x,y,z);
        }
        public static implicit operator SaveVector3(Vector3 value)
        {
            return new SaveVector3(value);
        }
        public static implicit operator Vector3(SaveVector3 value)
        {
            return value.Extract();
        }
    }
}
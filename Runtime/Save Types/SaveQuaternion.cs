using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    [System.Serializable]
    public struct SaveQuaternion
    {
        public float x,y,z,w;
        public SaveQuaternion(Quaternion value)
        {
            x = value.x;
            y = value.y;
            z = value.z;
            w = value.w;
        }
        public readonly Quaternion Extract()
        {
            return new Quaternion(x,y,z,w);
        }
        public static implicit operator SaveQuaternion(Quaternion value)
        {
            return new SaveQuaternion(value);
        }
        public static implicit operator Quaternion(SaveQuaternion value)
        {
            return value.Extract();
        }
    }
}

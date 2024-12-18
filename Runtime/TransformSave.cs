using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public class TransformSave : IdentifiedBehaviour, ISaveable
    {
        public virtual object Save()
        {
            TransformData data = new()
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale
            };
            return data;
        }
        public virtual void Load(object saveData)
        {
            TransformData data = (TransformData)saveData;
            transform.SetPositionAndRotation(data.Position, data.Rotation);
            transform.localScale = transform.localScale;
        }
        public virtual Type GetSaveType()
        {
            return typeof(TransformData);
        }
        [Serializable]
        public struct TransformData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Scale;
        }
    }
}
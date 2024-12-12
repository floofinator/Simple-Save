using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace Floofinator.SimpleSave
{
    public class TransformSave : IdentifiedBehaviour, ISaveable
    {
        public object Save()
        {
            TransformData data = new()
            {
                Position = transform.position,
                Rotation = transform.eulerAngles,
                Scale = transform.localScale
            };
            return data;
        }
        public void Load(object saveData)
        {
            TransformData data = (TransformData)saveData;
            transform.position = data.Position;
            transform.eulerAngles = data.Rotation;
            transform.localScale = transform.localScale;
        }
        public Type GetSaveType()
        {
            return typeof(TransformData);
        }
        [Serializable]
        public struct TransformData
        {
            public Vector3 Position;
            public Vector3 Rotation;
            public Vector3 Scale;
        }
    }
}
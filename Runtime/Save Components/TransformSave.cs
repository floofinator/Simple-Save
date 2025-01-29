using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public class TransformSave : IdentifiedBehaviour, ISaveable
    {
        [SerializeField] bool local = false;
        public virtual object Save()
        {
            TransformData data = new()
            {
                Position = local?transform.localPosition:transform.position,
                Rotation = local?transform.localRotation:transform.rotation,
                Scale = transform.localScale
            };
            return data;
        }
        public virtual void Load(object saveData)
        {
            TransformData data = (TransformData)saveData;
            if (local) transform.SetLocalPositionAndRotation(data.Position, data.Rotation);
            else transform.SetPositionAndRotation(data.Position, data.Rotation);
            transform.localScale = transform.localScale;

            if (TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.position = transform.position;
                rigidbody.rotation = transform.rotation;
            }
        }
        public virtual System.Type GetSaveType()
        {
            return typeof(TransformData);
        }
        [System.Serializable]
        public struct TransformData
        {
            public SaveVector3 Position;
            public SaveQuaternion Rotation;
            public SaveVector3 Scale;
        }
    }
}
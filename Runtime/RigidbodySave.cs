using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Floofinator.SimpleSave
{
    public class RigidbodySave : IdentifiedBehaviour, ISaveable
    {
        Rigidbody rb;
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }
        public System.Type GetSaveType()
        {
            return typeof(RigidbodyData);
        }
        public object Save()
        {
            return new RigidbodyData()
            {
                Position = rb.position,
                Rotation = rb.rotation.eulerAngles,
                Scale = transform.localScale,
                Velocity = rb.velocity,
                AngularVelocity = rb.angularVelocity
            };
        }
        public void Load(object saveData)
        {
            SetState((RigidbodyData)saveData);
        }
        void SetState(RigidbodyData data)
        {
            transform.SetPositionAndRotation(rb.position = data.Position, rb.rotation = Quaternion.Euler(data.Rotation));
            transform.localScale = data.Scale;

            if (rb.isKinematic) return;

            rb.velocity = data.Velocity;
            rb.angularVelocity = data.AngularVelocity;
        }
        [System.Serializable]
        public struct RigidbodyData
        {
            public Vector3 Position;
            public Vector3 Rotation;
            public Vector3 Scale;
            public Vector3 Velocity;
            public Vector3 AngularVelocity;
        }
    }
}
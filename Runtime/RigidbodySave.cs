using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public class VelocitySave : IdentifiedBehaviour, ISaveable
    {
        Rigidbody rb;
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }
        public System.Type GetSaveType()
        {
            return typeof(VelocityData);
        }
        public object Save()
        {
            return new VelocityData()
            {
                Velocity = rb.velocity,
                AngularVelocity = rb.angularVelocity
            };
        }
        public void Load(object saveData)
        {
            VelocityData data = (VelocityData)saveData;

            if (rb.isKinematic) return;

            rb.velocity = data.Velocity;
            rb.angularVelocity = data.AngularVelocity;
        }
        [System.Serializable]
        public struct VelocityData
        {
            public Vector3 Velocity;
            public Vector3 AngularVelocity;
        }
    }
}
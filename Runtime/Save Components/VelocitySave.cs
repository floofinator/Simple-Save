using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public class VelocitySave : IdentifiedBehaviour, ISaveable
    {
        Rigidbody rb;
        void Awake()
        {
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
            public SaveVector3 Velocity;
            public SaveVector3 AngularVelocity;
        }
    }
}
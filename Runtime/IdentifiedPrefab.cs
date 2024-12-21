using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public class IdentifiedPrefab : IdentifiedBehaviour
    {
        [SerializeField] string resourcePath;
        public string ResourcePath => resourcePath;
        protected override void Start()
        {
            GenerateID();

            base.Start();
        }
    }
}

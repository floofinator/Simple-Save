using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    /// <summary>
    /// Used to identify unique instances of objects.
    /// </summary>
    public class IdentifiedObject : IdentifiedBehaviour
    {
        public override void IdentifyParent()
        {
            if (transform.parent == null) return;
            //if this is a prefab id and we want to see if there is one in parent
            ParentObject = transform.parent.GetComponentInParent<IdentifiedObject>();
        }
    }
}

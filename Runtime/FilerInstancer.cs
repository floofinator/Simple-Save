using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public class FilerInstancer : MonoBehaviour
    {
        [SerializeField] FilerType type = FilerType.JSON;
        [SerializeField] bool replace = true;
        [SerializeField] string saveRoot = "";
        enum FilerType 
        {
            JSON, BINARY, XML
        }
        private void Awake()
        {
            if (Filer.Instance != null && !replace) return;

            switch (type)
            {
                case FilerType.JSON:
                    Filer.Instance = new JsonFiler();
                    break;
                case FilerType.BINARY:
                    Filer.Instance = new BinaryFiler();
                    break;
                case FilerType.XML:
                    Filer.Instance = new XMLFiler();
                    break;
            }

            Filer.Instance.SetRoot(saveRoot);
        }
    }
}

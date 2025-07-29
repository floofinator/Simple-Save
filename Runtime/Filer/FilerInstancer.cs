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
            if (SceneFiler.Filer != null && !replace) return;

            switch (type)
            {
                case FilerType.JSON:
                    SceneFiler.Filer = new JsonFiler();
                    break;
                case FilerType.BINARY:
                    SceneFiler.Filer = new BinaryFiler();
                    break;
                case FilerType.XML:
                    SceneFiler.Filer= new XMLFiler();
                    break;
            }

            SceneFiler.Filer.Root = saveRoot;
        }
    }
}

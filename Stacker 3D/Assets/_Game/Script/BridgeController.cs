using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private bool isBuilt;

    public bool IsBuilt => isBuilt;

    public void SetBuilt()
    {
        boxCollider.enabled = false;

        meshRenderer.enabled = true;

        isBuilt = true;
    }

}

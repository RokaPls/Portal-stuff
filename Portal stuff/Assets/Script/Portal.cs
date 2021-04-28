using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    
    public bool FirstPortal = true;

    //portalmanager

    [SerializeField]
    private string hitTag;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != hitTag)
            return;

        //portal to other object
    }
}

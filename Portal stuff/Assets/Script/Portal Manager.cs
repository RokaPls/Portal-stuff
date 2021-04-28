using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PortalManager : MonoBehaviour
{
    [SerializeField]
    private Portal firstPortal;

    [SerializeField]
    private Portal secondPortal;

    //this is the camera transform
    [SerializeField]
    private Transform playerCam;

    private float magnitude;
    private Vector3 velocity;

    //checks if raycast hit
    private RaycastHit hit;

    //uses a mask for the raycast
    [SerializeField]
    private LayerMask rayMask;

    //makes a list to store information about the portals
    private List<objectPortal> currentObjects = new List<objectPortal>();

    public void PortalObject(bool isFirstPortal,GameObject currentObj)
    {
        objectPortal objectToCheck = GetObjectPortalByGameObject(currentObj);
        
        //i honestly dont know what this does
        if(currentObjects.Contains(objectToCheck))
        {
            if (isFirstPortal && objectToCheck.currentPortal == firstPortal)
                return;

            if (!isFirstPortal && objectToCheck.currentPortal == secondPortal)
                return;

            currentObjects.Remove(objectToCheck);
        }

        //this uses rigidbody to use portal mechanics
        Rigidbody tempRigid = currentObj.GetComponent<Rigidbody>();

        Vector3 pos = Vector3.zero;
        Vector3 rot = Vector3.zero;

        magnitude = tempRigid.velocity.magnitude;

        objectPortal newObject = new objectPortal();

        Portal newPortal = firstPortal;

        if (!isFirstPortal)
            newPortal = secondPortal;

        pos = newPortal.transform.position;

        if (!newPortal.gameObject.activeSelf)
            return;

        rot = newPortal.transform.rotation.eulerAngles;
        velocity = newPortal.transform.forward;

        newObject.currentPortal = newPortal;
        newObject.currentGameObject = currentObj;

        currentObjects.Add(newObject);

        tempRigid.velocity = Vector3.zero;

        if (currentObj.layer == 8)
            pos += newPortal.transform.forward * 1.8f;

        currentObj.transform.position = pos;

        if(currentObj.layer == 8)
        {
            //i am like 99% sure that this is the code that causes every problem in your code good luck fixing it dumbas*
            //currentObj.GetComponent<PlayerMovment>().mouseLook
        }
        else
        {
            currentObj.transform.rotation = Quaternion.Euler(rot);
        }
    }

    //this is the code to place a portal
    public void SetPortal(Portal currentPortal)
    {
        if(Physics.Raycast(playerCam.position, playerCam.forward, out hit, 500, rayMask))
        {
            //checks if you hit another portal so it doesnt place the portal
            if (hit.transform.gameObject.layer == 17)
                return;

            //this does math that i dont understand but it somehow sees you placed a portal
            currentPortal.gameObject.SetActive(true);

            currentPortal.transform.position = hit.point;

            currentPortal.transform.rotation = Quaternion.LookRotation(hit.normal);

        }
    }

    //this removes the portal 
    public IEnumerator RemoveObject(objectPortal removeObjectPortal)
    {
        yield return new WaitForSeconds(0.1f);

        if (currentObjects.Contains(removeObjectPortal))
            yield break;

        currentObjects.Remove(removeObjectPortal);
    }

    public objectPortal GetObjectPortalByGameObject(GameObject objectToCheck)
    {
        for (int i = 0; i < currentObjects.Count; i++)
        {
            if (currentObjects[i].currentGameObject == objectToCheck)
                return currentObjects[i];
        }

        return new objectPortal();
    }

}
[System.Serializable]
public struct objectPortal
{
    public Portal currentPortal;
    public GameObject currentGameObject;
}

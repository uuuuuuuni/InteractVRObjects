using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPointer : MonoBehaviour
{
    public GameObject myObject;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);*/
        //transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

        /*if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider != null)
            {
                if (myObject != hit.collider.gameObject)
                {
                    myObject = hit.collider.gameObject;
                    Debug.Log("On VR Raycast Enter");
                    gameObject.GetComponent<LineRenderer>().materials[0].color = Color.red;
                }
            }
        }*/
        RaycastHit hit = new RaycastHit();
        //Debug.Log(transform.forward);
        Ray ray = new Ray(transform.position, transform.position + transform.forward*1000);
        //Debug.DrawRay(transform.position, transform.forward*1000, Color.green);
        if (Physics.Raycast(ray, out hit, 1000, -1))
        {
            //Debug.Log("COLISION");
            //Debug.DrawLine(ray.origin, hit.point, Color.yellow);
            //smokeMover.transform.position += curVec.normalized * Time.deltaTime * moverSpeed;
        }
        /*if (!Physics.Raycast(ray, out hit, 1, -1) && hit.transform != myObject.transform)
        {
            Debug.Log("NO COLLISON");
            Debug.DrawLine(ray.origin, hit.point, Color.yellow);
            //smokeMover.transform.position += curVec.normalized * Time.deltaTime * moverSpeed;
        }
        else
        {
            Debug.Log("COLLISON");
        }*/
    }
}

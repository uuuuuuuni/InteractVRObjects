using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Painter : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 lastPosition;
    private GameObject lastSphere;
    private float radius;
    private GameObject currentObject;
    private Stack<GameObject> objectStack;
    private Stack<GameObject> undone;
    public GameObject indexFinger;

    void Start()
    {
        radius = 0.01F;
        lastSphere = null;
        objectStack = new Stack<GameObject>();
        undone = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Si pulsamos el gatillo
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            // Creamos un nuevo objeto
            currentObject = new GameObject();
            Instantiate(currentObject);
            objectStack.Push(currentObject);
            undone.Clear();
        }

        // Mientras el gatillo esté pulsado
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            // Pintamos la esfera si la posición del mando está lo suficientemente alejada de la esfera anterior
            //Vector3 position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Vector3 position = indexFinger.transform.position;
            float distance = (position - lastPosition).magnitude;
            if (distance > radius / 5)
            {
                Paint(position);
            }
        }
        // Si soltamos el gatillo
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && lastSphere!=null)
        {
            // Rompemos el enlace entre las esferas para el próximo objeto
            lastSphere = null;
        }

        // El botón B es para deshacer el último objeto
        if (OVRInput.GetDown(OVRInput.Button.Two) && objectStack.Count != 0)
        {
            GameObject deleted = objectStack.Pop();
            deleted.SetActive(false);
            undone.Push(deleted);
        }

        // El botón A es para rehacer el último objeto deshecho
        if (OVRInput.GetDown(OVRInput.Button.One) && undone.Count!=0)
        {
            GameObject recovered = undone.Pop();
            recovered.SetActive(true);
            objectStack.Push(recovered);
        }
    }

    private void Paint(Vector3 position)
    {
        // Creamos la nueva esfera
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(radius, radius, radius);
        sphere.transform.SetParent(currentObject.transform);
        sphere.transform.position = position;
        if (lastSphere != null) {
            // Acoplamos la esfera al objeto actual
            AcoplarEsfera(sphere, currentObject);
        }
        // Actualizamos la posición de la última esfera
        lastPosition = position;
        // Actualizamos el objeto asignado a la última esfera
        lastSphere = sphere.gameObject;
    }

    private void AcoplarEsfera(GameObject sphere, GameObject myObject)
    {
        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        capsule.transform.localScale = new Vector3(radius, radius, radius);
        capsule.transform.SetParent(myObject.transform);
        Vector3 start = lastSphere.transform.position;
        Vector3 end = sphere.transform.position;
        capsule.transform.position = (end - start) / 2 + start;

        Vector3 v3T = capsule.transform.localScale;
        v3T.y = (end - start).magnitude;
        capsule.transform.localScale = v3T;

        capsule.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);
    }
}

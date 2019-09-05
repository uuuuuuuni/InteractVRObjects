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
    private GameObject vistaPrevia;
    public Material materialVistaPrevia;

    void Start()
    {
        radius = 0.01F;
        lastSphere = null;
        objectStack = new Stack<GameObject>();
        undone = new Stack<GameObject>();
        vistaPrevia = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        vistaPrevia.transform.localScale = new Vector3(radius, radius, radius);
        vistaPrevia.GetComponent<Renderer>().material = materialVistaPrevia;
        vistaPrevia.transform.position = indexFinger.transform.position;
        vistaPrevia.transform.SetParent(indexFinger.transform);
    }

    // Update is called once per frame
    void Update()
    {
        // Si pulsamos el gatillo
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            // Creamos un nuevo objeto
            currentObject = new GameObject();
            //Instantiate(currentObject);
            objectStack.Push(currentObject);
            undone.Clear();
        }

        // Mientras el gatillo esté pulsado
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)==1)
        {
            // Pintamos la esfera si la posición del mando está lo suficientemente alejada de la esfera anterior
            //Vector3 position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Vector3 position = indexFinger.transform.position;
            float distance = (position - lastPosition).magnitude;
            if (distance > radius / 10)
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


        /////////////////////////////////////////////////////////////////////
        ///             Aumentar o disminuir el trazo (Unai)
        ///////////////////////////////////////////////////////////////////// 

        // En el caso de que movamos el Joystick derecho hacia la izquierda
        // disminuiremos el radio de las esferas que estan siendo creadas y 
        // que conforman el trazo
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
        {
            if (radius - 0.00000001f >= 0)
            {
                radius = radius / 1.01f;
                vistaPrevia.transform.localScale = new Vector3(radius, radius, radius);
            }

        }

        // Por otro lado, en el caso de mover el Joystick derecho hacia la
        // derecha, el radio de las esferas aumentará
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        {
            radius = radius * 1.01f;
            vistaPrevia.transform.localScale = new Vector3(radius, radius, radius);
        }

  
    }

    /****************************************************************************
     * Método VistaPreviaEsfera
     ****************************************************************************
     * Método encargado de darnos una vista previa del grosor del trazo de nuestro
     * "pincel".
     * 
     * Recibe por parámetro la posición del dedo de nuestra mano.
     ****************************************************************************/ 
    private void VistaPreviaEsfera(Vector3 position)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1);
        Color color = sphere.GetComponent<MeshRenderer>().material.color;
        color.a = 0.3f;
        sphere.GetComponent<MeshRenderer>().material.color = color;
        sphere.transform.localScale = new Vector3(radius, radius, radius);
        sphere.transform.position = position;
        //StartCoroutine(DestruirVistaPrevia(sphere));
    }

    /*****************************************************************************
     * Método DestruirVistaPrevia
     *****************************************************************************
     * Método encargado de eliminar la vista previa del trazo pasado unos escasos 
     * segundos.
     *****************************************************************************/
    IEnumerator DestruirVistaPrevia(GameObject vistaPrevia)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(vistaPrevia);
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
/*
// Comprobamos que estamos clickando
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            objetoActual = new GameObject();
Instantiate(objetoActual);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            // Observamos la posición del ratón y la convertimos a coordenadas del mundo virtual
            Vector3 position = Input.mousePosition;
// Habría que ajustar la distancia de la cámara
position.z = 10.0f;
            position = Camera.main.ScreenToWorldPoint(position);
            // Pintamos la esfera si está lo suficientemente alejada de la esfera anterior
            float distance = (position - lastPosition).magnitude;
            if (distance > radius/10)
            {
                Paint();
// Actualizamos la posición de la última esfera
lastPosition = position;
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            lastSphere = null;
        }

        // Comprobamos que estamos clickando
        */
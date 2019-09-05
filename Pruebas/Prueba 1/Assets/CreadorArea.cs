using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProBuilder2.Common;
using ProBuilder2.MeshOperations;


public class CreadorArea : MonoBehaviour
{

    pb_Object objetoPrueba;
    public float m_RadiusMin = 1.5f;
    public float m_RadiusMax = 2f;
    public float m_Height = 1f;
    public bool m_FlipNormals = false;

    // Start is called before the first frame update
    void Start(){
        var go = new GameObject();
        objetoPrueba = go.gameObject.AddComponent<pb_Object>();

        //InvokeRepeating("Rebuild", 0f, .1f);
        Vector3[] points = new Vector3[5];

        for (int i = 0, c = points.Length; i < c; i++) {
            float angle = Mathf.Deg2Rad * ((i / (float)c) * 360f);
            points[i] = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * Random.Range(m_RadiusMin, m_RadiusMax);
        }

        // CreateShapeFromPolygon is an extension method that sets the pb_Object mesh data with vertices and faces
        // generated from a polygon path.
        objetoPrueba.CreateShapeFromPolygon(points, m_Height, m_FlipNormals);
    }

    /*void Rebuild() {
        // Create a circle of points with randomized distance from origin.
        Vector3[] points = new Vector3[32];

        for (int i = 0, c = points.Length; i < c; i++) {
            float angle = Mathf.Deg2Rad * ((i / (float)c) * 360f);
            points[i] = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * Random.Range(m_RadiusMin, m_RadiusMax);
        }

        // CreateShapeFromPolygon is an extension method that sets the pb_Object mesh data with vertices and faces
        // generated from a polygon path.
        objetoPrueba.CreateShapeFromPolygon(points, m_Height, m_FlipNormals);
    }*/

    
}

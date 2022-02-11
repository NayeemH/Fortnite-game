using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafacing : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        Camera m_Camera = Camera.main; //this line will called for In scene main camera tagged 
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
                             m_Camera.transform.rotation * Vector3.up); //function for the camera facing
    }
}

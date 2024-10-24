using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movimiento : MonoBehaviour
{
    Transform transformObj;
    Vector3 vec = new Vector3(1.0f,0,0);
    // Start is called before the first frame update
    void Start()
    {
        transformObj = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            transformObj.position += vec;
        }
        
     
    }
}

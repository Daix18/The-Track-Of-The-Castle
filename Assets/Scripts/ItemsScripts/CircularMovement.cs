using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    float timeCounter = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        timeCounter += Time.deltaTime;
        float x = Mathf.Cos(timeCounter);
        float y = Mathf.Sin(timeCounter);
        float z = 15;

        transform.position = new Vector3(x, y, z); 

    }

    public void MoverDerecha()
    {
        timeCounter += Time.deltaTime;
        float x = Mathf.Cos(timeCounter);
        float y = Mathf.Sin(timeCounter);
        float z = 15;

        transform.position = new Vector3(x, y, z);
    }

       


        
    
}

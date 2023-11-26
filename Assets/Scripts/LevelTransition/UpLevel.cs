using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpLevel : MonoBehaviour
{
    //Donde hacemos el Tp
    public Transform upLevel;

    private void OnTriggerEnter(Collider other)
    {       
        if (other.CompareTag("Player"))
        {
            Debug.Log("Se ha hecho el Tp");
            other.GetComponent<CharacterController>().enabled = false;           
            other.transform.position = upLevel.transform.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}

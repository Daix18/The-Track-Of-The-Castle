using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class DownLevel : MonoBehaviour
{
    //Donde hacemos el Tp
    public Transform downLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Se ha hecho el Tp");
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = downLevel.transform.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}

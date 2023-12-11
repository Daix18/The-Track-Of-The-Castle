using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateGear : MonoBehaviour
{
    public static RotateGear THIS;

    public static event Action<string, int> RotatedGear = delegate { };

    private bool coroutineAllowed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        THIS = this;
    }


    private void OnMouseDown()
    {
        if (coroutineAllowed && MysteryBoxController.THIS.controlPuzzle == true) 
        {
            StartCoroutine(RotateWheel());
        }
    }

    //Lo que ocurre en la rotación de un engranaje.
    IEnumerator RotateWheel() 
    {
        coroutineAllowed = false;

        yield return new WaitForSeconds(1);
    }
}

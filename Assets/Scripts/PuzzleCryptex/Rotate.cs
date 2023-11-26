using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public static Rotate THIS;

    public static event Action<string, int> Rotated = delegate { };

    private bool coroutineAllowed;

    private int numberShown;

    // Start is called before the first frame update
    void Start()
    {       
        coroutineAllowed = true;
        numberShown = 0;
    }

    private void Awake()
    {
        THIS = this;
    }

    private void OnMouseDown()
    {
        if (coroutineAllowed && CryptexController.THIS.controlPuzzle == true)
        {
            StartCoroutine(RotateWheel());
        }
    }

    private IEnumerator RotateWheel()
    {
        coroutineAllowed = false;

        for (int i = 0; i <= 11 ; i++)
        {
            transform.Rotate(-3f, 0f, 0f);
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;

        numberShown += 1;
        if (numberShown > 9)
        {
            numberShown = 0;
        }

        Rotated(name,numberShown);        
    }
}

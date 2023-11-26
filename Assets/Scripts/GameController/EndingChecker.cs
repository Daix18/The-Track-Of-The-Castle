using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingChecker : MonoBehaviour
{
    public static EndingChecker THIS;

    public bool endingChecked;

    private void Awake()
    {
        THIS = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        endingChecked = true;
    }
    private void OnTriggerExit(Collider other)
    {
        endingChecked = false;
    }
}

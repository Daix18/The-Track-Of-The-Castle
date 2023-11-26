using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ElevatorController.THIS.insideElevator = true;
    }

    private void OnTriggerExit(Collider other)
    {
        ElevatorController.THIS.insideElevator = false;
    }
}

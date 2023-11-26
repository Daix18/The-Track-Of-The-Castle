using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ElevatorTriggerController : MonoBehaviour
{
    /// <summary>
    /// Evento a los que se suscribirán otros objetos del juego
    /// </summary>
    public ElevatorEvent enter, exit;

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    [System.Serializable]
    public class ElevatorEvent : UnityEvent { }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public static ElevatorController THIS;

    /// <summary>
    /// Animator
    /// </summary>
    Animator animator;

    /// <summary>
    /// Player
    /// </summary>
    public GameObject player;

    public BoxCollider upLevel;
    public BoxCollider downLevel;

    public bool insideElevator;

    public enum DefaultState { Up, Down, SecretDown }
    public DefaultState defaultState;

    DefaultState currentState;

    /// <summary>
    /// Timepo de espera para cambiar de estado
    /// </summary>
    float waitTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        THIS = this;
    }

    /// <summary>
    /// Se sube a la parte de arriba
    /// </summary>
    public void Up()
    {
        currentState = DefaultState.Up;

        if (defaultState == DefaultState.Up)
            waitTime = 2;
        else
            waitTime = 0;

        Invoke("ChangeState", waitTime);
    }

    /// <summary>
    /// Se baja a la planta baja
    /// </summary>
    public void Down()
    {
        currentState = DefaultState.Down;

        if (defaultState == DefaultState.Down)
            waitTime = 2;
        else
            waitTime = 0;

        Invoke("ChangeState", waitTime);
    }

    /// <summary>
    /// Se baja a la planta secreta
    /// </summary>
    public void SecretDown()
    {
        currentState = DefaultState.SecretDown;

        Invoke("Change Down", waitTime);
    }

    public void CheckPlayer()
    {
        if (insideElevator == true)
        {
            //Hace que el player sea hijo del ascensor
            player.transform.SetParent(transform);

            //Se desactiva el CharacterController del player
            player.GetComponent<CharacterController>().enabled = false;
        }
        else
        {
            //Hacer que el player ya no sea hijo del ascensor
            player.transform.SetParent(null);

            //Se activa el CharacterController del player
            player.GetComponent<CharacterController>().enabled = true;
        }     
    }

    public void UnlockPlayer()
    {
        //Hacer que el player ya no sea hijo del ascensor
        player.transform.SetParent(null);

        //Se activa el CharacterController del player
        player.GetComponent<CharacterController>().enabled = true;
    }

    /// <summary>
    /// LLamada por Up y Down
    /// </summary>
    void ChangeState()
    {
        animator.SetTrigger("Change State");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBoxController : MonoBehaviour
{
    public static MysteryBoxController THIS;

    public Camera playerCamera;

    // El punto alrededor del cual orbitará la cámara
    public Transform puntoFocal; 

    public bool insideMysteryBox;

    public bool controlPuzzle;

    //Velocidad a la que rota la cámara
    public float velocidadRotacion = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        THIS = this;
    }

    // Update is called once per frame
    void Update()
    {
        //La cámara va a rotar cuando se hada clic izquierdo
        if (Input.GetMouseButton(0)) 
        {
            // Obtener el movimiento del ratón
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Rotar la cámara en función del movimiento del ratón
            playerCamera.transform.RotateAround(puntoFocal.position, Vector3.up, mouseX * velocidadRotacion);
            playerCamera.transform.RotateAround(puntoFocal.position, Vector3.right, mouseY * velocidadRotacion);         
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameController.THIS.insidePuzzle = true;
        insideMysteryBox = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameController.THIS.insidePuzzle = false;
        insideMysteryBox = false;
    }
}

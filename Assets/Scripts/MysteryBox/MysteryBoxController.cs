using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBoxController : MonoBehaviour
{
    public static MysteryBoxController THIS;

    public Camera playerCamera;

    public GameObject MystryBoxCameraPosition;

    // El punto alrededor del cual orbitará la cámara
    public Transform puntoFocal; 

    public bool insideMysteryBox;

    public bool controlPuzzle;

    public bool YReached = false;

    //Velocidad a la que rota la cámara
    public float velocidadRotacion = 2.0f;

    public float MaxlimitadorPosicion;

    public float MinlimitadorPosición;  

    public float speed = 2.0f;

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
        if (controlPuzzle == true)
        {
            //La cámara va a rotar cuando se hada clic izquierdo
            if (Input.GetMouseButton(0)) 
            {
                // Obtener el movimiento del ratón
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                //Se limita la posición de la cámara en el máximo
                Vector3 maxlimitedY = playerCamera.transform.position;
                maxlimitedY.y = Mathf.Clamp(maxlimitedY.y, 0, MaxlimitadorPosicion);
                playerCamera.transform.position = maxlimitedY;      

                //Descomentar cuando implementamos el puzzle en la escena principal
                //Vector3 minlimitedY = playerCamera.transform.position;
                //minlimitedY.y = Mathf.Clamp(minlimitedY.y, 0, MinlimitadorPosición);
                //playerCamera.transform.position = minlimitedY;
                
                //Cuando llegamos a la posición limite hacemos que no rote
                if (maxlimitedY.y == MaxlimitadorPosicion)
                {                  
                    YReached = true;                   
                    velocidadRotacion = 0;
                }                
                // Rotar la cámara en función del movimiento del ratón
                playerCamera.transform.RotateAround(puntoFocal.position, Vector3.up, mouseX * velocidadRotacion);
                playerCamera.transform.RotateAround(puntoFocal.position, Vector3.right, mouseY * velocidadRotacion);
            }
            //Cuando hemos llegado al máximo volvemos a la posición inicial del puzzle
            if (YReached == true)
            {
                //Hacemos la transición a la posición incial y cuando termine, reseteamos la rotación de la cámara como antes.
                playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position,MystryBoxCameraPosition.transform.position , speed * Time.deltaTime);

                //Esto hace que mire al puzzle
                playerCamera.transform.LookAt(puntoFocal.position);
                if (Vector3.Distance(playerCamera.transform.position,MystryBoxCameraPosition.transform.position ) < 0.01f)
                {
                    playerCamera.transform.rotation =  new Quaternion(0,0,0,0);
                    Debug.Log("Se resetea la sensibilidad del ratón");
                    velocidadRotacion = 2.0f;
                    YReached = false;
                }
            }
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

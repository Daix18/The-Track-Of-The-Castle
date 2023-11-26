using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FirstPersonController : MonoBehaviour
{
    public static FirstPersonController THIS;

    /// <summary>
    /// Velocidad de movimiento
    /// </summary>
    public float movementSpeed = 5.0f;

    /// <summary>
    /// Sensiblidad del ratón
    /// </summary>
    public float mouseSensitivity = 3.0f;

    public float originalSensitivity;

    /// <summary>
    /// 
    /// </summary>
    float verticalRotation = 0.0f;

    /// <summary>
    /// 
    /// </summary>
    public float upDownRange = 60.0f;

    /// <summary>
    /// Velocidad cuando corre
    /// </summary>
    public float runSpeed = 10.0f;

    /// <summary>
    /// Objeto de referencia del puzzle
    /// </summary>
    //public GameObject puzzleReference;   

    private void Start()
    {
        //Oculta el cursor
        UnityEngine.Cursor.visible = false;

        //Bloquea el cursor para que el usuario no pueda hacer clic fuera de la ventana del juego
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        //La sensibilidad se guarda nada más empezar el juego.
        originalSensitivity = mouseSensitivity;
    }

    private void Awake()
    {
          THIS = this;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Metodo de movimiento
    /// </summary>
    private void HandleMovement()
    {                      
            //Mover al jugador hacia delante/atrás y hacia los lados
            float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
            float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

            //Comprobamos si el jugador está corriendo
            if (Input.GetKey(KeyCode.LeftShift))
            {
                forwardSpeed *= 2.0f;
                sideSpeed *= 2.0f;
            }

            Vector3 speed = new Vector3(sideSpeed, 0, forwardSpeed);
            speed = transform.rotation * speed;
            CharacterController cc = GetComponent<CharacterController>();
            cc.SimpleMove(speed);        

            //Rota la cámara
            RotateCamera();
    }
    
    public void RotateCamera()
    {
        //Rota la cámara horizontalmente
        transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);

        //Rotar la cámara verticalmente
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}


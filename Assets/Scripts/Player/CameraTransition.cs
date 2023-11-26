using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public static CameraTransition THIS;

    //Posición orignal
    public Transform originalPosition;

    //Jugador
    public GameObject player;

    //Velocidad
    public float speed = 1.0f;

    //Dentro del puzzle
    public bool insidePuzzle = false;

    //Rotación de la cámara en el puzzle del Giroscopio
    public Vector3 GyroCameraPositon;

    //Rotación de la cámara en el puzzle del Ajedrez
    public Vector3 ChessCameraPosition;

    //Rotación de la cámara en puzzle del Cryptex
    public Vector3 CryptexCameraPosition;

    //Rotación de la cámara en el puzzle del Mapa
    public Vector3 MapaCameraPosition;

    //Lista de puzzles
    public List<Transform> puzzleTargets;

    //Haciendo transición
    bool transitioning = false;

    //Transición de vuelta
    bool backtransitioning = false;


    private void Awake()
    {
        THIS = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartTransition();
        }
        if (insidePuzzle == true)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                backtransitioning = true;
            }
        }

        if (transitioning)
        {
            if (GyroScopeController.THIS.insideGyro == true)
            {
                //Usa la función Lerp para mover la cámara gradualmente hacia la posición del objeto vacío
                transform.position = Vector3.Lerp(transform.position, puzzleTargets[0].position, speed * Time.deltaTime);

                //La distancia entre la cámara y el objeto vacío es menor a una tolerancia pequeña
                if (Vector3.Distance(transform.position, puzzleTargets[0].position) < 0.01f)
                {
                    EnterPuzzle();
                    Debug.Log("Ha terminado la transición");
                    Debug.Log("Ha entrado en el puzzle del Giroscopio");
                    Quaternion gyroRotation = Quaternion.Euler(GyroCameraPositon);
                    transform.rotation = gyroRotation;            
                }
            }

            if (CryptexController.THIS.insideCryptex == true)
            {
                //Usa la función Lerp para mover la cámara gradualmente hacia la posición del objeto vacío
                transform.position = Vector3.Lerp(transform.position, puzzleTargets[1].position, speed * Time.deltaTime);

                //La distancia entre la cámara y el objeto vacío es menor a una tolerancia pequeña
                if (Vector3.Distance(transform.position, puzzleTargets[1].position) < 0.01f)
                {
                    EnterPuzzle();
                    Debug.Log("Ha terminado la transición");
                    Debug.Log("Ha entrado en el puzzle del Giroscopio");
                    Quaternion cryptexRotation = Quaternion.Euler(CryptexCameraPosition);
                    transform.rotation = cryptexRotation;
                }
            }

            if (MapaController.THIS.insideMapa == true)
            {
                //Usa la función Lerp para mover la cámara gradualmente hacia la posición del objeto vacío
                transform.position = Vector3.Lerp(transform.position, puzzleTargets[2].position, speed * Time.deltaTime);

                //La distancia entre la cámara y el objeto vacío es menor a una tolerancia pequeña
                if (Vector3.Distance(transform.position, puzzleTargets[2].position) < 0.01f)
                {
                    EnterPuzzle();
                    Debug.Log("Ha terminado la transición");
                    Debug.Log("Ha entrado en el puzzle del Mapa");
                    Quaternion cryptexRotation = Quaternion.Euler(MapaCameraPosition);
                    transform.rotation = cryptexRotation;
                    Camera mainCamera = Camera.main;
                    int currentCullingMask = mainCamera.cullingMask;
                    int newLayerMask = currentCullingMask | (1 << LayerMask.NameToLayer("Hover"));
                    mainCamera.cullingMask = newLayerMask;
                }
            }            
        }

        if (backtransitioning)
        {
            //Usa la función lerp para mover la cámara gradualmente hacia su posición original
            transform.position = Vector3.Lerp(transform.position, originalPosition.position, speed * Time.deltaTime);            

            //La distancia entre la cámara y su posición original es menor a una tolerancia pequeña
            if (Vector3.Distance(transform.position, originalPosition.position) < 0.01f)
            {
                ExitPuzzle();

                //Activamos movimiento del jugador
                player.GetComponent<FirstPersonController>().enabled = true;
                player.GetComponent<CharacterController>().enabled = true;              

                //Desactivamos la Layer "Hover"
                Camera mainCamera = Camera.main;
                int currentCullingMask = mainCamera.cullingMask;
                int layerIndexToDisable = LayerMask.NameToLayer("Hover");
                int newLayerMask = currentCullingMask & ~(1 << layerIndexToDisable);
                mainCamera.cullingMask = newLayerMask;                
                                              
                //foreach (Rotate rotateScript in CryptexController.THIS.rotateScript)
                //{
                //        rotateScript.enabled = false;
                //}                
            }
        }
    }

    public void StartTransition()
    {

        if (insidePuzzle == true)
        {
            transitioning = true;

            //Desactivamos el movimiento del jugador
            player.GetComponent<FirstPersonController>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;
        }
    } 
    
    void EnterPuzzle()
    {
        transitioning = false;

        transform.SetParent(null);

        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;

        insidePuzzle = true;
    }

    void ExitPuzzle()
    {
        backtransitioning = false;

        //transform.SetParent(null);
        transform.SetParent(player.transform);

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;

        insidePuzzle = false;
    }
}
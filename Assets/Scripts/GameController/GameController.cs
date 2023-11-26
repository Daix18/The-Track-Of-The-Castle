using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController THIS;

    [Header("Camera Transition")]
     
    //Cámara del jugador
    public Camera playerCamera;

    //Posición orignal
    public Transform originalPosition;

    //Jugador
    public GameObject player;

    //Velocidad
    public float speed = 1.0f;

    [Header("Puzzles")]

    //Dentro del puzzle
    public bool insidePuzzle = false;

    public bool firstKeyObtained;

    public bool secondKeyObtained;

    public bool thirdKeyObtained;

    public CapsuleCollider playerCollider;

    //Rotación de la cámara en el puzzle del Giroscopio
    public Vector3 GyroCameraPositon;

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
    [Header("Panels")]

    public bool endingChecked;

    public bool gamePaused;

    public GameObject endPanel;

    public GameObject menuPanel;

    public GameObject controlPanel;

    public GameObject llavesPanel;

    [Header("GameSettings")]

    public Transform startPosition;

    public List<GameObject> notas;


    // Start is called before the first frame update
    private void Start()
    {
        //player.transform.position = startPosition.position;
        //player.transform.rotation = Quaternion.Euler(new Vector2(0, 90));
    }
    void Awake()
    {      
        THIS = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (insidePuzzle) 
            {
                StartTransition();
            }
            else
            {
                backtransitioning = true;
            }
        }
        //if (insidePuzzle == true)
        //{
        //    if (Input.GetKeyDown(KeyCode.V))
        //    {
        //    }
        //}

        if (EndingChecker.THIS.endingChecked == true)
        {
            if (thirdKeyObtained == true && secondKeyObtained == true && firstKeyObtained == true)
            {
                Debug.Log("Se ha acabado el juego!");
                endPanel.SetActive(true);
                llavesPanel.SetActive(false);
                player.GetComponent<FirstPersonController>().enabled = false;
                player.GetComponent<CharacterController>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
            else
            {
                endPanel.SetActive(false);
                llavesPanel.SetActive(true);
                player.GetComponent<FirstPersonController>().enabled = true;
                player.GetComponent<CharacterController>().enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;                

            }

        }

        if (transitioning)
        {
            if (GyroScopeController.THIS.insideGyro == true)
            {
                //Usa la función Lerp para mover la cámara gradualmente hacia la posición del objeto vacío
                playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, puzzleTargets[0].position, speed * Time.deltaTime);

                //La distancia entre la cámara y el objeto vacío es menor a una tolerancia pequeña
                if (Vector3.Distance(playerCamera.transform.position, puzzleTargets[0].position) < 0.01f)
                {
                    EnterPuzzle();
                    Debug.Log("Ha terminado la transición");
                    Debug.Log("Ha entrado en el puzzle del Giroscopio");
                    GyroScopeController.THIS.controlPuzzle = true;
                    playerCollider.enabled = false;
                    Quaternion gyroRotation = Quaternion.Euler(GyroCameraPositon);
                    playerCamera.transform.rotation = gyroRotation;
                }
            }

            if (CryptexController.THIS.insideCryptex == true)
            {
                //Usa la función Lerp para mover la cámara gradualmente hacia la posición del objeto vacío
                playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, puzzleTargets[1].position, speed * Time.deltaTime);

                //La distancia entre la cámara y el objeto vacío es menor a una tolerancia pequeña
                if (Vector3.Distance(playerCamera.transform.position, puzzleTargets[1].position) < 0.01f)
                {
                    EnterPuzzle();
                    Debug.Log("Ha terminado la transición");
                    Debug.Log("Ha entrado en el puzzle del Giroscopio");
                    CryptexController.THIS.controlPuzzle = true;
                    Quaternion cryptexRotation = Quaternion.Euler(CryptexCameraPosition);
                    playerCamera.transform.rotation = cryptexRotation;
                }
            }

            if (MapaController.THIS.insideMapa == true)
            {
                //Usa la función Lerp para mover la cámara gradualmente hacia la posición del objeto vacío
                playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, puzzleTargets[2].position, speed * Time.deltaTime);

                //La distancia entre la cámara y el objeto vacío es menor a una tolerancia pequeña
                if (Vector3.Distance(playerCamera.transform.position, puzzleTargets[2].position) < 0.01f)
                {
                    EnterPuzzle();
                    Debug.Log("Ha terminado la transición");
                    Debug.Log("Ha entrado en el puzzle del Mapa");
                    MapaController.THIS.controlPuzzle = true;
                    Quaternion cryptexRotation = Quaternion.Euler(MapaCameraPosition);
                    playerCamera.transform.rotation = cryptexRotation;
                    //Camera mainCamera = Camera.main;
                    //int currentCullingMask = mainCamera.cullingMask;
                    //int newLayerMask = currentCullingMask | (1 << LayerMask.NameToLayer("Hover"));
                    //mainCamera.cullingMask = newLayerMask;
                }
            }
        }

        if (backtransitioning)
        {
            //Usa la función lerp para mover la cámara gradualmente hacia su posición original
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, originalPosition.position, speed * Time.deltaTime);

            //La distancia entre la cámara y su posición original es menor a una tolerancia pequeña
            if (Vector3.Distance(playerCamera.transform.position, originalPosition.position) < 0.01f)
            {
                ExitPuzzle();

                //Activamos movimiento del jugador
                player.GetComponent<FirstPersonController>().enabled = true;
                player.GetComponent<CharacterController>().enabled = true;
                playerCollider.enabled = true;

                //Ocultar el cursor
                Cursor.visible = false;

                //Bloquea el cursor 
                Cursor.lockState = CursorLockMode.Locked;

                //Desactivamos la Layer "Hover"
                //Camera mainCamera = Camera.main;
                //int currentCullingMask = mainCamera.cullingMask;
                //int layerIndexToDisable = LayerMask.NameToLayer("Hover");
                //int newLayerMask = currentCullingMask & ~(1 << layerIndexToDisable);
                //mainCamera.cullingMask = newLayerMask;

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

        playerCamera.transform.SetParent(null);

        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;

        insidePuzzle = false;
    }

    void ExitPuzzle()
    {
        GyroScopeController.THIS.controlPuzzle = false;

        CryptexController.THIS.controlPuzzle = false;

        MapaController.THIS.controlPuzzle = false;

        backtransitioning = false;

        //transform.SetParent(null);
        playerCamera.transform.SetParent(player.transform);

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;

        insidePuzzle = false;
    }

    public void PauseGame()
    {
        Debug.Log("Se ha pausado el juego");

        Debug.Log("Sensibilidad del jugador " + FirstPersonController.THIS.mouseSensitivity);

        //Se activa el menu de pausa
        menuPanel.SetActive(true);

        gamePaused = true;

        //Oculta el cursor
        Cursor.visible = true;

        // Bloquea el cursor para que el usuario no pueda hacer clic fuera de la ventana del juego
        Cursor.lockState = CursorLockMode.None;

        //Desactivamos el movimiento del jugador
        //player.GetComponent<FirstPersonController>().enabled = false;
        FirstPersonController.THIS.mouseSensitivity = 0;
        player.GetComponent<CharacterController>().enabled = false;

        Time.timeScale = 0;
    }

    /// <summary>
    /// Continua el juego 
    /// </summary>
    public void ResumeGame()
    {
        Debug.Log("Se ha continuado el juego");

        Debug.Log("Sensibilidad del jugador " + FirstPersonController.THIS.mouseSensitivity);

        gamePaused = false;

        if (CryptexController.THIS.insideCryptex == true || GyroScopeController.THIS.insideGyro == true || MapaController.THIS.insideMapa == true)
        {
            //Se muestra el cursor
            Cursor.visible = true;

            //Desbloquea el cursor
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            //Ocultar el cursor
            Cursor.visible = false;

            //Bloquea el cursor 
            Cursor.lockState = CursorLockMode.Locked;
        }


        //Desactivamos el movimiento del jugador
        //player.GetComponent<FirstPersonController>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        FirstPersonController.THIS.mouseSensitivity = 3;

        //Se desactiva el menu de pausa
        menuPanel.SetActive(false);

        Time.timeScale = 1;
    }

    /// <summary>
    /// Vulve al menu principal
    /// </summary>
    public void GoMainMenu()
    {
        //Se carga el menu principal
        SceneManager.LoadScene(0);

        Time.timeScale = 1; 
    }

    public void Controls()
    {
        controlPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void ExitControls()
    {
        controlPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

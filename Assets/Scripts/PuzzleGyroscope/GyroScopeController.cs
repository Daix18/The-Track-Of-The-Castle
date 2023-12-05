using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroScopeController : MonoBehaviour
{
    public static GyroScopeController THIS;

    // La posición deseada del objeto
    public Vector3 correctPosition;

    public bool isRotating;

    public bool insideGyro;

    public bool controlPuzzle;


    public Image thirdKey;

    public List<GameObject> buttons; // Lista que almacenará los botones

    public List<GameObject> piecesToRotate; // Lisra de piezas que se rotan.

    public int[] result, correctCombination;

    private bool coroutineAllowed;

    // Start is called before the first frame update
    void Start()
    {
        result = new int[] { 0, 0, 0 };
        correctCombination = new int[] { 4, 2, 3};
        coroutineAllowed = true;       
    }

    private void Awake()
    {
        THIS = this;
    }

    public void AddToResult(int buttonIndex)
    {
        if (buttonIndex >= 0 && buttonIndex < result.Length)
        {
            result[buttonIndex]++;
            CheckCombination();
            // Verificar si se alcanzó el límite de 9
            if (result[buttonIndex] > 4)
            {
                // Reiniciar el valor a 0
                result[buttonIndex] = 0;
            }
        }
    }    

    private void CheckCombination()
    {
        // Comprobar si se ha alcanzado la combinación correcta
        if (result[0] == 4 && result[1] == 2 && result[2] == 3)
        {
            Debug.Log("¡Combinación correcta alcanzada!");
            
            thirdKey.gameObject.SetActive(true);

            GameController.THIS.thirdKeyObtained = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameController.THIS.insidePuzzle = true;
        insideGyro = true;
    }
    private void OnTriggerExit(Collider other)
    {
        GameController.THIS.insidePuzzle = false;
        insideGyro = false;
    }
}

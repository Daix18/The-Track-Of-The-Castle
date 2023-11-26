using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CryptexController : MonoBehaviour
{
    public static CryptexController THIS;

    //public Rotate[] rotateScript;

    public int[] result, correctCombination;

    public Image secondKey;

    public bool controlPuzzle;

    public bool insideCryptex = false;
    
    // Start is called before the first frame update
    void Start()
    {
        result = new int[] { 0, 0, 0, 0 };
        correctCombination = new int[] { 3, 7, 9, 6 };
        Rotate.Rotated += CheckResults;
        //rotateScript = GetComponentsInChildren<Rotate>();
    }

    private void Awake()
    {
        THIS = this;
    }

    void CheckResults (string wheelName, int number)
    {
        switch (wheelName)
        {

            case "Anilla1":
                result[0] = number;
                break;

            case "Anilla2":
                result[1] = number; 
                break;

            case "Anilla3":
                result[2] = number; 
                break;
            case "Anilla4":
                result[3] = number;
                break;
        }
        if (result[0] == correctCombination[0] && result[1] == correctCombination[1] && result[2] == correctCombination[2] && result[3] == correctCombination[3])
        {
            Debug.Log("Se ha abierto!");
            secondKey.gameObject.SetActive(true);
            GameController.THIS.secondKeyObtained = true;
        }
    }

    private void OnDestroy()
    {
        Rotate.Rotated -= CheckResults;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameController.THIS.insidePuzzle = true;
        insideCryptex = true;
    }
    private void OnTriggerExit(Collider other)
    {
        GameController.THIS.insidePuzzle = false;
        insideCryptex = false;
    }
}

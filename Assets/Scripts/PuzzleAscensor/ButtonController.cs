using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ElevatorController;

public class ButtonController : MonoBehaviour
{
    /// <summary>
    /// Tiempo que se mantiene pulsado el botón
    /// </summary>
    //public float tiempoEspera = 5f;

    /// <summary>
    /// 
    /// </summary>
    //private float tiempoPresionado = 0f;aaaaaaaa

    /// <summary>
    /// Si se presiona el botón
    /// </summary> 
    /// 
    Material originalMaterial;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
    }
    void OnMouseDown()
    {
        if (GameController.THIS.insidePuzzle == false)
        {
            if (THIS.insideElevator == true)
            {
                THIS.CheckPlayer();
                if (THIS.defaultState == DefaultState.Up)
                {                    
                    THIS.Down();
                }
                else if (THIS.defaultState == DefaultState.Down)
                {                    
                    THIS.Up();
                }
            }
            else
            {
                if (THIS.insideElevator == false)
                {
                    THIS.CheckPlayer();
                    if (THIS.defaultState == DefaultState.Up)
                    {
                        THIS.Down();
                    }
                    else if (THIS.defaultState == DefaultState.Down)
                    {
                        THIS.Up();
                    }                   
                }
            }
        }

        if (GameController.THIS.insidePuzzle == true)
        {
            if (!GyroScopeController.THIS.isRotating)
            {               
                switch (gameObject.name)
                {
                    case "GyroButton1":
                        if (GyroScopeController.THIS.buttons.Count > 0)
                        {
                            Debug.Log("Se ha pulsado el botón1");
                            RotateGyro.THIS.StartRotate(GyroScopeController.THIS.piecesToRotate[0], 1);
                            GyroScopeController.THIS.AddToResult(0);
                        }
                        break;
                    case "GyroButton2":
                        if (GyroScopeController.THIS.buttons.Count > 1)
                        {
                            Debug.Log("Se ha pulsado el botón2");
                            RotateGyro.THIS.StartRotate(GyroScopeController.THIS.piecesToRotate[1], 2);
                            GyroScopeController.THIS.AddToResult(1);
                        }
                        break;
                    case "GyroButton3":
                        if (GyroScopeController.THIS.buttons.Count > 2)
                        {
                            Debug.Log("Se ha pulsado el botón3");
                            RotateGyro.THIS.StartRotate(GyroScopeController.THIS.piecesToRotate[2], 3);
                            GyroScopeController.THIS.AddToResult(2);
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}

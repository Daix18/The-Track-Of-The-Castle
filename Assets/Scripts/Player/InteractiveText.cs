using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class InteractiveText : MonoBehaviour
{
    public TextMeshProUGUI textKeys;

    private bool textShown = false; // Variable para controlar si el texto ya se ha mostrado
    public float maxRaycastDistance = 10f;
    public float maxDisplayDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            // Comprobar si el objeto colisionado es el que queremos mostrar
            if (hit.distance <= maxDisplayDistance && hit.collider.gameObject == gameObject && !textShown)
            {
                // Mostrar el texto
                CanvasGroup canvasGroup = textKeys.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 1f;
                textKeys.gameObject.SetActive(true);
           
                if (canvasGroup.alpha == 1f)
                {                    
                    textShown = true; // Establecer la bandera de texto mostrado como verdadera

                    textKeys.text = "Presiona la [E] para cojer objetos";                    
                }
                
            }
            if (PickupController.THIS.isPickedUp == true)
            {
                Debug.Log("Ha entrado en el if");
                textKeys.text = "Mantén la [R] para rotar el objeto\n" +
                    "Vuelve a presionar la [E] para soltar el objeto\n" + 
                    "Usa el click izquierdo para lanzar las piezas";
            }

            else
            { 
                //Ocultar el texto
                CanvasGroup canvasGroup = textKeys.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0f;
                textKeys.gameObject.SetActive(false);
            }
        }        
        // Visualizar el Raycast en el editor
        Debug.DrawRay(ray.origin, ray.direction * maxRaycastDistance, Color.red);
    }
    private void OnDrawGizmos()
    {
        // Visualizar el Raycast en el editor incluso cuando no se está ejecutando
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * maxRaycastDistance);
    }
}

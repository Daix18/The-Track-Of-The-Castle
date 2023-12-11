using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PiezasMapa : MonoBehaviour
{
    public static PiezasMapa THIS;

    public int xInicial;
    public int yInicial;
    public int index;

    private Vector3 initialPosition;
    private Collider currentCollider;
    public bool isBeingDragged;
    public bool isDraggingZ;
    public bool isCorrectlyPlaced;
    bool isRaycastActive = false;

    public float minX_Z = -1;
    public float maxX_Z = 1;

    public LayerMask mesa;


    private void Awake()
    {
        THIS = this;
    }

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (MapaController.THIS.controlPuzzle == true)
        {
            if (isBeingDragged)
            {
                //targetPosition.z = Mathf.Clamp(targetPosition.z, minX_Z, maxX_Z);
                //targetPosition.x = Mathf.Clamp(targetPosition.x, minX_Z, maxX_Z);
                transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), Time.deltaTime * 10f);
            
                RaycastHit hit;
                Ray ray = new Ray(transform.position, Vector3.down);

                if (Physics.Raycast(ray, out hit))
                {
                    currentCollider = hit.collider;
                }
                else
                {
                    currentCollider = null;
                }

                // Visualizar el raycast en la escena
                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
            
            }
        }
    }
       
    public void OnMouseDown()
    {
        isBeingDragged = true;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDraggingZ = Mathf.Abs(mousePosition.z - transform.position.z) < Mathf.Abs(mousePosition.x - transform.position.x);
    }

    public void OnMouseUp()
    {
        isBeingDragged = false;

        // Verificar si se ha identificado un collider de tipo Trigger
        if (currentCollider != null && currentCollider.isTrigger)
        {
            // Realizar la interpolación de posición hacia el collider identificado
            StartCoroutine(MoveToTile(currentCollider.transform.position, 0.5f));
            Debug.Log("Ha hecho una interpolación al collider" + currentCollider);

            // Obtener el componente PiezasIndexTrigger del collider
            PiezasIndexTrigger triggerComponent = currentCollider.GetComponent<PiezasIndexTrigger>();

            if (triggerComponent != null)
            {
                Debug.Log("Index del trigger: " + triggerComponent.index);
                Debug.Log("Index de la pieza: " + index);

                if (triggerComponent.index == index)
                {
                    isCorrectlyPlaced = true;
                    Debug.Log("Pieza colocada correctamente en el sitio");
                }
                else
                {
                    isCorrectlyPlaced = false;
                    Debug.Log("La pieza no está en el sitio correcto");
                }
            }
        }
        else
        {
            StartCoroutine(MoveToTile(initialPosition, 0.5f));
            isCorrectlyPlaced = false;

        }

        currentCollider = null;
    }

    private Vector3 GetTargetPosition()
    {
        Plane plane = new Plane(Vector3.forward, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        
        if (Physics.Raycast(ray,out RaycastHit hit, 10 , mesa))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y += 0.1f;
            return targetPosition;
        }
        return transform.position;
    }

    private IEnumerator MoveToTile(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}


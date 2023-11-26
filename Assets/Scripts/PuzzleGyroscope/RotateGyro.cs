using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGyro : MonoBehaviour
{
    public static RotateGyro THIS;

    public static event Action<string, int> Rotated = delegate { };

    public float rotationTime = 0.5f; // El tiempo que tardará la rotación en completarse

    private bool coroutineAllowed;

    private int[] result, correctCombination;

    private Quaternion initialRotation; // La rotación actual del objeto antes de la rotación
    private Quaternion targetRotation; // La rotación final que se desea alcanzar

    // Start is called before the first frame update
    void Start()
    {
        coroutineAllowed = true;
    }

    private void Awake()
    {
        THIS = this;
    }
    public void StartRotate(GameObject pieceToRotate, int direction)
    {
        if (GyroScopeController.THIS.controlPuzzle == true)
        {
            if (coroutineAllowed)
            {
                StartCoroutine(RotatePiece(pieceToRotate, direction));
            }

        }
    }

    private IEnumerator RotatePiece(GameObject pieceToRotate, int direction)
    {
        coroutineAllowed = false;

        GyroScopeController.THIS.isRotating = true;
        initialRotation = pieceToRotate.transform.rotation; // Almacenar la rotación actual
        targetRotation = initialRotation * Quaternion.Euler(-36f * direction, 0f, 0f); // Calcular la rotación final

        float elapsedTime = 0f;
        while (elapsedTime < rotationTime)
        {
            // Interpolar suavemente entre la rotación inicial y la rotación final
            float t = elapsedTime / rotationTime;   
            pieceToRotate.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Esperar al siguiente frame antes de continuar
        }

        pieceToRotate.transform.rotation = targetRotation; // Asegurarse de que la pieza esté en la rotación final exacta
        coroutineAllowed = true;

        // Llamar al evento de rotación completada
        Rotated.Invoke(pieceToRotate.name, direction);

        GyroScopeController.THIS.isRotating = false;

    }
}

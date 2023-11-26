using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiezasIndexTrigger : MonoBehaviour
{
    public static PiezasIndexTrigger THIS;

    public int index;

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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la pieza está siendo arrastrada
        PiezasMapa pieza = other.GetComponent<PiezasMapa>();
        if (pieza != null && pieza.isBeingDragged)
        {
            // Acciones a realizar cuando la pieza entra en el trigger
            Debug.Log("Pieza en Trigger " + index);
            // Realiza las acciones adicionales que necesites con el collider y el índice correspondiente          
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    //Referencia al script
    public static PickupController THIS;

    //Referncia al jugador
    public GameObject player;    

    //Referencia al holdPos
    public Transform holdPos;
    
    //Fuerza a la que lanzamos el objeto
    public float throwForce = 500f; 

    //Rango para cojer un objeto
    public float pickUpRange = 5f;

    //Velocidad a la que se coje un objeto   
    public float pickupSpeed = 1f;

    //Comprueba si se ha cojido
    public bool isPickedUp;
    
    private float rotationSensitivity = 1f;
    private GameObject heldObj; 
    private Rigidbody heldObjRb; 
    private bool canDrop = true; 
    private int LayerNumber; 
  
    //
    FirstPersonController mouseLookScript;
    void Start()
    {
        mouseLookScript = player.GetComponent<FirstPersonController>();
        LayerNumber = LayerMask.NameToLayer("HoldLayer");                                                                                                                          
    }

    void Awake()
    {
        THIS = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
            {                               
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {                    
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        PickUpObject(hit.transform.gameObject);
                    }

                }
            }
            else
            {
                if (canDrop == true)
                {
                    StopClipping(); 
                    DropObject();
                }
            }
        }
        //Si estamos sujetando un objeto
        if (heldObj != null) 
        {
            isPickedUp = true;
            MoveObject(); 
            RotateObject();            
            if(GameController.THIS.gamePaused == true) 
            {
                mouseLookScript.mouseSensitivity = 0; 
            }          
            //Si se presiona click izquierdo, se lanza el objeto
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) 
            {
                StopClipping();
                ThrowObject();                
            }
        }
        else
        {
            isPickedUp = false;
        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) 
        {            
            heldObj = pickUpObj; 
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); 
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; 
            heldObj.layer = LayerNumber;             
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {      
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; 
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; 
        heldObj = null; 
    }
    void MoveObject()
    {
        heldObj.transform.position = Vector3.Lerp(heldObj.transform.position, holdPos.transform.position, pickupSpeed * Time.deltaTime);
        if (Vector3.Distance(heldObj.transform.position, holdPos.transform.position) < 0.01f)
        {            
            heldObj.transform.position = holdPos.transform.position;
        }        
    }
    void RotateObject()
    {
        //Si se presiona la R rotamos el objeto
        if (Input.GetKey(KeyCode.R))
        {
            canDrop = false;            
            mouseLookScript.mouseSensitivity = 0f;
            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;            
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {           
            mouseLookScript.mouseSensitivity = 3f;            
            canDrop = true;
        }
    }

    void ThrowObject()
    {        
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping() 
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); 
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        
        if (hits.Length > 1)
        {
             
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);  
            
        }
    }
}


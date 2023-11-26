using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public static NoteController THIS;

    public Vector3 originalPosition;

    private void Awake()
    {
        THIS = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }
}

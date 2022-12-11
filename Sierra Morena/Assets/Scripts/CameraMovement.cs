using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class CameraMovement : MonoBehaviour
{

    private float sensitivity = 300.0f;

    private GameObject character;

    public Transform orientation;

    private float xRotation;

    private float yRotation;

    public Transform cameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        yRotation += mouseX;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(xRotation,yRotation,0);

        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        character.transform.Rotate(Vector3.up, mouseX );
        


    }
}

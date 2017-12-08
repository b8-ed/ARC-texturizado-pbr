using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scr_Camara : MonoBehaviour {
    //USUARIO CON LAS TECLAS O CON EL MOUSE
    public enum ModoCamara {INMOVIL,USUARIO_MOVIENDO , ORBITAR }

    [Header("Velocidades")]
    public float zoomSpeed = 100.0f;
    public float rotationSpeed = 50.0f;

    [Header("Velocidad Mouse")]
    public float mouseRotationMultiplier = 0.2f;
    public float mouseZoomMultiplier = 5.0f;

    [Header("Limites")]
    public float minZoomDistance = 0.0f;
    public float maxZoomDistance = 100.0f;

    public bool correctZoomingOutRatio = true;
    /// <summary>
    /// objeto que debe cambiar
    /// </summary>
    public GameObject objectToFollow;
    public float goToSpeed = 0.1f;
    private Vector3 cameraTarget;

    private float currentCameraDistance;
    private Vector3 goingToCameraTarget = Vector3.zero;
    private bool doingAutoMovement = false;
    private Vector3 lastMousePos;


    public float minOchenta=10;
    public ModoCamara Modo= ModoCamara.INMOVIL;
    [Range(-0.5f,0.5f)]
    public float Lado=0;

    void Start()
    {
        Modo = ModoCamara.INMOVIL;
        Cursor.visible = true;
        //currentCameraDistance = minZoomDistance + ((maxZoomDistance - minZoomDistance) / 2.0f);
        currentCameraDistance = minZoomDistance;
        lastMousePos = Vector3.zero;
    }

    public void CalculateBounds()
    {
        if (objectToFollow.GetComponentInChildren<MeshRenderer>().bounds.extents.z > objectToFollow.GetComponentInChildren<MeshRenderer>().bounds.extents.x)
        {
            minZoomDistance = objectToFollow.GetComponentInChildren<MeshRenderer>().bounds.extents.z + 5.0f;
        }
        else
        {
            minZoomDistance = objectToFollow.GetComponentInChildren<MeshRenderer>().bounds.extents.x + 5.0f;
        }
    }

    void Update()
    {
        UpdateRotation();
        UpdateZooming();
        UpdatePosition();
        UpdateAutoMovement();
        lastMousePos = Input.mousePosition;
    }
    //ROTATION WORKING WITH KEYBOARD
    private void UpdateRotation()
    {
        float deltaAngleH = 0.0f;//el movimiento alrededor del objeto, en x 
                                 // float deltaAngleV = 0.0f;//el movimiento alrededor del objeto, en y

        /*if (Input.GetKey(KeyCode.A) && freeMode.isOn == true)
        {
            deltaAngleH = 1.0f;
        }
        if (Input.GetKey(KeyCode.D) && freeMode.isOn == true)
        {
            deltaAngleH = -1.0f;
        }
        */        
        if (  Modo == ModoCamara.USUARIO_MOVIENDO   )//para hacer el movimiento con el mouse
        {
            if(Input.GetMouseButton(0))
            {
                Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);
                //print(deltaMousePos);  si el delta.x es menor a 0 es a la izquierda el movimiento
                deltaAngleH += deltaMousePos.x * mouseRotationMultiplier;//

            }
            else if(Input.GetAxis("Horizontal")!=0)
            {
                deltaAngleH += (Input.GetAxis("Horizontal")*-5.0f) * mouseRotationMultiplier;
            }

         //   deltaAngleV -= deltaMousePos.y * mouseRotationMultiplier; //para girar hacia arriba
        }else if(Modo == ModoCamara.ORBITAR)
        {
            Lado = Mathf.Clamp(Lado, -0.5f, 0.5f);
            deltaAngleH += Lado;
        }
        else if(Modo == ModoCamara.INMOVIL)
        {

        }

        SetLocalEulerAngles(transform,
            Mathf.Min(minOchenta, Mathf.Max(5.0f, transform.localEulerAngles.x * Time.deltaTime * rotationSpeed)),
            transform.localEulerAngles.y + deltaAngleH * Time.deltaTime * rotationSpeed
        );
    }
            // Mathf.Min(80.0f, Mathf.Max(5.0f, transform.localEulerAngles.x + deltaAngleV * Time.deltaTime * rotationSpeed)),
              

    //ZOOMING WORKING WITH KEYBOARD
    private void UpdateZooming()
    {
        float deltaZoom = 0.0f;
        /* if (Input.GetKey(KeyCode.S) && freeMode.isOn == true)
         {
             deltaZoom = 1.0f;
         }
         if (Input.GetKey(KeyCode.W)&& freeMode.isOn == true)
         {
             deltaZoom = -1.0f;
         }*/

        //Scroll con mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        deltaZoom -= scroll * mouseZoomMultiplier;

        float zoomedOutRatio = correctZoomingOutRatio ? (currentCameraDistance - minZoomDistance) / (maxZoomDistance - minZoomDistance) : 0.0f;
        currentCameraDistance = Mathf.Max(minZoomDistance, Mathf.Min(maxZoomDistance, currentCameraDistance + deltaZoom * Time.deltaTime * zoomSpeed * (zoomedOutRatio * 2.0f + 1.0f)));
    }

    private void UpdatePosition()
    {
        if (objectToFollow != null)
        {
            cameraTarget = Vector3.Lerp(cameraTarget, objectToFollow.transform.position, goToSpeed);
        }

        transform.position = cameraTarget;
        transform.Translate(Vector3.back * currentCameraDistance);
    }

    private void UpdateAutoMovement()
    {
        if (doingAutoMovement)//se acerca solo
        {
            cameraTarget = Vector3.Lerp(cameraTarget, goingToCameraTarget, goToSpeed);
            if (Vector3.Distance(goingToCameraTarget, cameraTarget) < 1.0f)
            {
                doingAutoMovement = false;
            }
        }
    }

    void SetLocalEulerAngles(Transform _transform, float? x = null, float? y = null, float? z = null)
    {
        Vector3 vector = new Vector3();
        if (x != null) { vector.x = x.Value; } else { vector.x = _transform.localEulerAngles.x; }
        if (y != null) { vector.y = y.Value; } else { vector.y = _transform.localEulerAngles.y; }
        if (z != null) { vector.z = z.Value; } else { vector.z = _transform.localEulerAngles.z; }
        _transform.localEulerAngles = vector;
    }

}

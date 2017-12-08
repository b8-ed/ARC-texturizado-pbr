//Code by Luis Bazan
//Github user: luisquid11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CameraMovement : MonoBehaviour {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;

    [Header("Velocidades")]
    public float panSpeed = 15.0f;
    public float zoomSpeed = 100.0f;
    public float rotationSpeed = 50.0f;

    [Header("Velocidad Mouse")]
    public float mousePanMultiplier = 0.1f;
    public float mouseRotationMultiplier = 0.2f;
    public float mouseZoomMultiplier = 5.0f;

    [Header("Limites")]
    public float minZoomDistance = 20.0f;
    public float maxZoomDistance = 200.0f;
    [Header("Ajustes")]
    public float smoothingFactor = 0.1f;

    public bool correctZoomingOutRatio = true;
    public bool smoothing = true;

    //public int screenEdgeSize = 10;

    public GameObject objectToFollow;
    public float goToSpeed = 0.1f;
    private Vector3 cameraTarget;

    // private fields
    private float currentCameraDistance;
    float offsetcurrentCameraDistance;
    private Vector3 lastMousePos;
    private Vector3 lastPanSpeed = Vector3.zero;
    private Vector3 goingToCameraTarget = Vector3.zero;
    private bool doingAutoMovement = false;

    //private bool lastframeBool = false;

    Vector3 initialPos;
    Quaternion initialRot;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = true;
        currentCameraDistance = minZoomDistance + ((maxZoomDistance - minZoomDistance) / 2.0f);
        lastMousePos = Vector3.zero;
    }

    void toggleCambio(bool toggleValue)
    {
        if (!toggleValue)
        {
            gameObject.transform.LookAt(GameObject.Find("base").transform);
            gameObject.transform.position = initialPos;
            gameObject.transform.rotation = initialRot;
            GoTo(Vector3.zero);
            OnResetPressed();
            lastMousePos = Vector3.zero;
        }
    }


    // Update is called once per frame
    void Update()
    {
        UpdatePanning();
        UpdateRotation();
        UpdateZooming();
        UpdatePosition();
        UpdateAutoMovement();
        lastMousePos = Input.mousePosition;    
    }

    public void OnResetPressed()
    {
        currentCameraDistance = minZoomDistance + ((maxZoomDistance - minZoomDistance) / 2.0f);

        UpdateZooming();
        UpdatePosition();
        UpdateAutoMovement();
    }

    public void GoTo(Vector3 position)
    {
        doingAutoMovement = true;
        goingToCameraTarget = position;
        objectToFollow = null;
    }

    public void Follow(GameObject gameObjectToFollow)
    {
        objectToFollow = gameObjectToFollow;
    }

    #region private functions

    private void UpdatePanning()
    {
        Vector3 moveVector = new Vector3(0, 0, 0);
        /*if (Input.GetKey(KeyCode.A) && freeMode.isOn == true)
        {
            moveVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.S) && freeMode.isOn == true)
        {
            moveVector.z -= 1;
        }
        if (Input.GetKey(KeyCode.D) && freeMode.isOn == true)
        {
            moveVector.x += 1;
        }
        if (Input.GetKey(KeyCode.W) && freeMode.isOn == true)
        {
            moveVector.z += 1;
        }*/

        /*if (Input.mousePosition.x < screenEdgeSize)
        {
            //X negativo
        }
        else if (Input.mousePosition.x > Screen.width - screenEdgeSize)
        {
            //X positivo
        }
        if (Input.mousePosition.y < screenEdgeSize)
        {
            //Z negativo
        }
        else if (Input.mousePosition.y > Screen.height - screenEdgeSize)
        {
            //Z Positivo
        }*/

        //Padding con clic central
        if (Input.GetMouseButton(2)) //Clic central
        {
            Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);
            moveVector += new Vector3(-deltaMousePos.x, 0, -deltaMousePos.y) * mousePanMultiplier;
        }

        if (moveVector != Vector3.zero)
        {
            objectToFollow = null;
            doingAutoMovement = false;
        }

        var effectivePanSpeed = moveVector;
        if (smoothing)
        {
            effectivePanSpeed = Vector3.Lerp(lastPanSpeed, moveVector, smoothingFactor);
            lastPanSpeed = effectivePanSpeed;
        }

        var oldXRotation = transform.localEulerAngles.x;

        // Set the local X rotation to 0;
        SetLocalEulerAngles(transform, 0.0f);

        cameraTarget = cameraTarget + transform.TransformDirection(effectivePanSpeed) * panSpeed * Time.deltaTime;

        // Set the old x rotation.
        SetLocalEulerAngles(transform, oldXRotation);
    }

    //ROTATION WORKING WITH KEYBOARD
    private void UpdateRotation()
    {
        float deltaAngleH = 0.0f;
        float deltaAngleV = 0.0f;

        /*if (Input.GetKey(KeyCode.A) && freeMode.isOn == true)
        {
            deltaAngleH = 1.0f;
        }
        if (Input.GetKey(KeyCode.D) && freeMode.isOn == true)
        {
            deltaAngleH = -1.0f;
        }
        */
        if (Input.GetMouseButton(1))
        {
            Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);
            deltaAngleH += deltaMousePos.x * mouseRotationMultiplier;
            deltaAngleV -= deltaMousePos.y * mouseRotationMultiplier;
        }

        SetLocalEulerAngles(transform,
            Mathf.Min(80.0f, Mathf.Max(5.0f, transform.localEulerAngles.x + deltaAngleV * Time.deltaTime * rotationSpeed)),
            transform.localEulerAngles.y + deltaAngleH * Time.deltaTime * rotationSpeed
        );
    }

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
        if (doingAutoMovement)
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
    #endregion
}


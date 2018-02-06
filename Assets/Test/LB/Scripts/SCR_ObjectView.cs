using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ObjectView : MonoBehaviour
{

    public float TurnX;
    public float TurnY;
    public float TurnZ = 90.0f;

    public float MoveX;
    public float MoveY;
    public float MoveZ;

    public bool World = true;
    bool subiendo = true;
    Vector3 startPos;
    Vector3 endPos;

    bool isAnimOn = false;

    public Transform targetObject;

    private float rotValue = 0.0f;

    private void Start()
    {
        rotValue = TurnZ;
    }

    public void Init(Transform _target)
    {
        //isAnimOn = false;
        targetObject = _target;
        startPos = targetObject.position;
        endPos = new Vector3(targetObject.position.x + MoveX, targetObject.position.y + MoveY, targetObject.position.z + MoveZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject == null)
            return;
        if(isAnimOn)
        {
            if (World == true)
            {
                targetObject.Rotate(TurnX * Time.deltaTime, TurnY * Time.deltaTime, TurnZ * Time.deltaTime, Space.World);
                //transform.Translate(MoveX * Time.deltaTime, MoveY * Time.deltaTime, MoveZ * Time.deltaTime, Space.World);

                if (subiendo == true)
                {
                    targetObject.position = Vector3.Lerp(startPos, endPos, Time.time);
                    if (targetObject.position == endPos)
                    {
                        subiendo = false;
                    }
                }

                else
                {
                    targetObject.position = Vector3.Lerp(endPos, startPos, Time.time);
                    if (targetObject.position == startPos)
                    {
                        subiendo = true;
                    }
                }

            }
            else
            {
                targetObject.Rotate(TurnX * Time.deltaTime, TurnY * Time.deltaTime, TurnZ * Time.deltaTime, Space.Self);
                targetObject.Translate(MoveX * Time.deltaTime, MoveY * Time.deltaTime, MoveZ * Time.deltaTime, Space.Self);
            }
        }        
    }

    public void ToggleAnimation(UnityEngine.UI.Text btnText)
    {
        isAnimOn = !isAnimOn;
        if (isAnimOn)
            btnText.text = "Animación activa";
        else
            btnText.text = "Animación desactivada";
        Debug.Log("Animation " + isAnimOn);
    }

    public void RotateRight()
    {
        TurnZ = -rotValue;
    }

    public void RotateLeft()
    {
        TurnZ = rotValue;
    }


}

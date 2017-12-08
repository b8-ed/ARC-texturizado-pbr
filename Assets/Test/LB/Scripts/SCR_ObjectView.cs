using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ObjectView : MonoBehaviour
{

    public float TurnX;
    public float TurnY = 90.0f;
    public float TurnZ;

    public float MoveX;
    public float MoveY;
    public float MoveZ;

    public bool World;
    bool subiendo = true;
    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(transform.position.x + MoveX, transform.position.y + MoveY, transform.position.z + MoveZ);
    }

    // Update is called once per frame
    void Update()
    {

        if (World == true)
        {
            transform.Rotate(TurnX * Time.deltaTime, TurnY * Time.deltaTime, TurnZ * Time.deltaTime, Space.World);
            //transform.Translate(MoveX * Time.deltaTime, MoveY * Time.deltaTime, MoveZ * Time.deltaTime, Space.World);

            if (subiendo == true)
            {
                transform.position = Vector3.Lerp(startPos, endPos, Time.time);
                if (transform.position == endPos)
                {
                    subiendo = false;
                }
            }

            else
            {
                transform.position = Vector3.Lerp(endPos, startPos, Time.time);
                if (transform.position == startPos)
                {
                    subiendo = true;
                }
            }

        }
        else
        {
            transform.Rotate(TurnX * Time.deltaTime, TurnY * Time.deltaTime, TurnZ * Time.deltaTime, Space.Self);
            transform.Translate(MoveX * Time.deltaTime, MoveY * Time.deltaTime, MoveZ * Time.deltaTime, Space.Self);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iluminacion : MonoBehaviour {

    public Light BackLight;
    public Light KeyLight;
    public Light FillLight;

    public float f_spotAngle;


    public float RadioAlCentro;

    public float f_AlturaSet;

    void Start ()
    {
        Triangular();
            CambiarLucesADireccional();

    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
            CambiarLucesADireccional();
        if (Input.GetKeyDown(KeyCode.S))
            CambiarLucesAPoint();

        SeguirObjeto();
    }


    void Triangular()
    {
        KeyLight.gameObject.transform.position = new Vector3(RadioAlCentro, f_AlturaSet, -RadioAlCentro / 2);
        BackLight.gameObject.transform.position = new Vector3(0, f_AlturaSet, RadioAlCentro);
        FillLight.gameObject.transform.position = new Vector3(-RadioAlCentro, f_AlturaSet, -RadioAlCentro / 2);

    }

    public void CambiarLucesADireccional()
    {
        BackLight.type = LightType.Spot;
        KeyLight.type = LightType.Spot;
        FillLight.type = LightType.Spot;
    }

    public void CambiarLucesAPoint()
    {
        BackLight.type = LightType.Point;
        KeyLight.type = LightType.Point;
        FillLight.type = LightType.Point;
    }

    void SeguirObjeto()
    {
        BackLight.transform.LookAt(Vector3.zero);
        KeyLight.transform.LookAt(Vector3.zero);
        FillLight.transform.LookAt(Vector3.zero);

        BackLight.spotAngle = f_spotAngle;
        KeyLight.spotAngle = f_spotAngle;
        FillLight.spotAngle = f_spotAngle;

        Triangular();
    }

}

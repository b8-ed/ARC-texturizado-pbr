using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Iluminacion : MonoBehaviour {

    public Light BackLight;
    public Light KeyLight;
    public Light FillLight;

    public float f_spotAngle;
    public float f_range;

    public float RadioAlCentro;

    public float f_AlturaSet;
    public float rotation;

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
        if (Input.GetKeyDown(KeyCode.Q))
            ApagarKeyLight();
        if (Input.GetKeyDown(KeyCode.W))
            ApagarBackLight();
        if (Input.GetKeyDown(KeyCode.E))
            ApagarFillLightt();

        SeguirObjeto();
    }

  
    //Apaga la luz key
    public void ApagarKeyLight()
    {
        if (KeyLight.gameObject.activeSelf)
            KeyLight.gameObject.SetActive(false);
        else
            KeyLight.gameObject.SetActive(true);

    }
    //Apaga la luz Back
    public void ApagarBackLight()
    {
        if (BackLight.gameObject.activeSelf)
            BackLight.gameObject.SetActive(false);
        else
            BackLight.gameObject.SetActive(true);
    }
    //Apaga la luz Fill
    public void ApagarFillLightt()
    {
        if (FillLight.gameObject.activeSelf)
            FillLight.gameObject.SetActive(false);
        else
            FillLight.gameObject.SetActive(true);
    }
    //actualiza y pone las luces en orden
    public void Triangular()
    {

        gameObject.transform.Rotate(new Vector3(0, 1, 0), rotation * Time.deltaTime);

        GameObject tmp = gameObject;
        GameObject t0 = KeyLight.gameObject;
        GameObject t1 = BackLight.gameObject;
        GameObject t2 = FillLight.gameObject;


        KeyLight.gameObject.transform.localPosition = new Vector3(RadioAlCentro, f_AlturaSet, -RadioAlCentro / 2);
        BackLight.gameObject.transform.localPosition = new Vector3(0, f_AlturaSet, RadioAlCentro);
        FillLight.gameObject.transform.localPosition = new Vector3(-RadioAlCentro, f_AlturaSet, -RadioAlCentro / 2);

        KeyLight.transform.rotation = t0.transform.rotation;
        BackLight.transform.rotation = t1.transform.rotation;
        FillLight.transform.rotation = t2.transform.rotation;
        gameObject.transform.rotation = tmp.transform.rotation;
    }
    //cambiar las luces a modo direccional
    public void CambiarLucesADireccional()
    {
        BackLight.type = LightType.Spot;
        KeyLight.type = LightType.Spot;
        FillLight.type = LightType.Spot;
    }
    //Cambiar las luces a modo de punto
    public void CambiarLucesAPoint()
    {
        BackLight.type = LightType.Point;
        KeyLight.type = LightType.Point;
        FillLight.type = LightType.Point;
    }
    //Ve al centro / puede cambiar a objeto
    void SeguirObjeto()
    {
        BackLight.transform.LookAt(Vector3.zero);
        KeyLight.transform.LookAt(Vector3.zero);
        FillLight.transform.LookAt(Vector3.zero);

        BackLight.spotAngle = f_spotAngle;
        KeyLight.spotAngle = f_spotAngle;
        FillLight.spotAngle = f_spotAngle;

        BackLight.range = f_range;
        KeyLight.range = f_range;
        FillLight.range = f_range;

        Triangular();
    }

}

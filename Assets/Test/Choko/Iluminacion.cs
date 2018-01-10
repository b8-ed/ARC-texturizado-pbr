using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Iluminacion : MonoBehaviour {

    public Light BackLight;
    public Light KeyLight;
    public Light FillLight;

    public float intencidad;

    public float f_spotAngle;
    public float f_range;

    public float RadioAlCentro;

    public float f_AlturaSet;
    public float rotation;


    //Variables de color de luces
    public Color32[] ColorBackLight;
    public Color32[] ColorKeyLight;
    public Color32[] ColorFillLight;

    //

    public Color32 ColorLibreBackLight;
    public Color32 ColorLibreKeyLight;
    public Color32 ColorLibreFillLight;

    bool cambioColor;

    void Start()
    {
        Triangular();
        CambiarLucesADireccional();

        ColorBackLight = new Color32[3];
        ColorKeyLight = new Color32[3];
        ColorFillLight = new Color32[3];


        //3 sets de colores en triadas
        //https://color.adobe.com/

        ColorBackLight[0] = new Color32(25, 255, 99, 255);
        ColorBackLight[1] = new Color32(0, 237, 255, 255);
        ColorBackLight[2] = new Color32(18, 37, 178, 255);

        ColorKeyLight[0] = new Color32(92, 9, 178, 255);
        ColorKeyLight[1] = new Color32(178, 9, 110, 255);
        ColorKeyLight[2] = new Color32(255, 70, 0, 255);

        ColorFillLight[0] = new Color32(255, 152, 0, 255);
        ColorFillLight[1] = new Color32(255, 221, 0, 255);
        ColorFillLight[2] = new Color32(75, 178, 9, 255);

        ColorLibreBackLight = Color.white;
        ColorLibreFillLight = Color.white;
        ColorLibreKeyLight = Color.white;

        cambioColor = false;

    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))//cambia las luces a direccional
            CambiarLucesADireccional();
        if (Input.GetKeyDown(KeyCode.S))//cambia las luces a point
            CambiarLucesAPoint();
        if (Input.GetKeyDown(KeyCode.Q)) //apaga la key light
            ApagarKeyLight();
        if (Input.GetKeyDown(KeyCode.W)) //apaga la back light
            ApagarBackLight();
        if (Input.GetKeyDown(KeyCode.E)) // apaga la fill light
            ApagarFillLightt();

        if (Input.GetKeyDown(KeyCode.U))
            TriadaAlphaDeColoresLuces();
        if (Input.GetKeyDown(KeyCode.I))
            TriadaBetaDeColoresLuces();
        if (Input.GetKeyDown(KeyCode.O))
            TriadaGamaDeColoresLuces();
        if (Input.GetKeyDown(KeyCode.P))
            SeleccionDeColorLibre();

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

        if(cambioColor)
            CambioDeColorLibre();
    }

    //activar Triada de color 1
    void TriadaAlphaDeColoresLuces()
    {
        cambioColor = false;

        BackLight.color = ColorBackLight[0];
        FillLight.color = ColorFillLight[0];
        KeyLight.color = ColorKeyLight[0];
    }
    //activar Triada de color 2
    void TriadaBetaDeColoresLuces()
    {
        cambioColor = false;

        BackLight.color = ColorBackLight[1];
        FillLight.color = ColorFillLight[1];
        KeyLight.color = ColorKeyLight[1];
    }
    //activar Triada de color 3
    void TriadaGamaDeColoresLuces()
    {
        cambioColor = false;

        BackLight.color = ColorBackLight[2];
        FillLight.color = ColorFillLight[2];
        KeyLight.color = ColorKeyLight[2];

    }

    //activar Seleccion de Color Libre
    void SeleccionDeColorLibre()
    {
        cambioColor = true;
    }

    void CambioDeColorLibre()
    {
        BackLight.color = ColorLibreBackLight;
        FillLight.color = ColorLibreFillLight;
        KeyLight.color = ColorLibreKeyLight;
    }
}

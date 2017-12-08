using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargaDeObjetos : MonoBehaviour {

    public GameObject[] modelos3D;

    public GameObject[] instanciaModelos3D;
    public int indiceObjetoActivo;

    Scr_Camara scrCam;

    void Start ()
    {
        scrCam = FindObjectOfType<Scr_Camara>();
        CargarObjetos();
        instanciaModelos3D = new GameObject[modelos3D.Length];
        CrearObjetos();
        ActivarObjetoActual();
	}
    
    void ActivarObjetoActual()
    {
        instanciaModelos3D[indiceObjetoActivo].gameObject.SetActive(true);
        scrCam.objectToFollow = instanciaModelos3D[indiceObjetoActivo].gameObject;
        scrCam.CalculateBounds();
    }
    void DesactivarObjetoActual()
    {
        instanciaModelos3D[indiceObjetoActivo].gameObject.SetActive(false);
    }

    void CargarObjetos()
    {
        modelos3D = Resources.LoadAll<GameObject>("Modelos");
    }
    void CrearObjetos()
    {
        print(modelos3D.Length);

        for (int i = 0; i < modelos3D.Length; i++)
        {
            instanciaModelos3D[i] = Instantiate(modelos3D[i], Vector3.zero, Quaternion.identity);
            instanciaModelos3D[i].AddComponent<SCR_ObjectView>();
            instanciaModelos3D[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Mandar 1 como parametro = [adelante / ++]
    /// ::
    /// Mandar -1 como paramero = [retrocedar / --]
    /// </summary>
    public void CambiarObjetoActual(int _metodo)
    {
        //Adelanta la seleccion del Objeto Actual
        if(_metodo == 1)
        {
            DesactivarObjetoActual();
            if (indiceObjetoActivo < instanciaModelos3D.Length)
                indiceObjetoActivo++;
            else
                indiceObjetoActivo = 0;
            ActivarObjetoActual();
        }
        //Regresa en la seleccion del Objeto Actual
        else if(_metodo == -1)
        {
            DesactivarObjetoActual();
            if (indiceObjetoActivo > 0)
                indiceObjetoActivo--;
            else
                indiceObjetoActivo = instanciaModelos3D.Length - 1;
            ActivarObjetoActual();
        }
    }
}

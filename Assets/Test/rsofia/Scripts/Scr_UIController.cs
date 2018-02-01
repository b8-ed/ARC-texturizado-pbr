//ruth sofia brown
//git rsofia
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Este script va en el canvas
//Es el encargado de prender y apagar las interfaces
// segun la que este prendida
public class Scr_UIController : MonoBehaviour
{
    public GameObject interfazPrincipal;
    public GameObject[] otrasInterfaces;

    private void Start()
    {
        //por default todos los menus salen apagados
        TurnOffAllMenus();
    }

    private void Update()
    {
        //El menu principal sale al presionar P
        if(Input.GetKeyDown(KeyCode.P))
        {
            TurnOnMainUI();
        }
        //La interfaz hace toggle con H
        else if(Input.GetKeyDown(KeyCode.H))
        {
            ToggleMainUI();
        }
        //Apagar los menus con ESC
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(interfazPrincipal.activeSelf)
            {
                TurnOffMainMenu();
            }
            else
            {
                TurnOnMainUI();
            }
        }

        if(interfazPrincipal.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                AbrirInterfaz(0);
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                AbrirInterfaz(1);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                AbrirInterfaz(2);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                AbrirInterfaz(3);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                AbrirInterfaz(4);
            }
        }
    }

    //Funcion que prende unicamente la interfaz principal
    public void TurnOnMainUI()
    {
        TurnOffOtherMenus();
        foreach(GameObject obj in otrasInterfaces)
        {
            obj.SetActive(false);
        }
        interfazPrincipal.SetActive(true);
    }

    //Funcion que apaga todas las interfaces
    private void TurnOffAllMenus()
    {
        TurnOffOtherMenus();
        TurnOffMainMenu();
    }

    //Apaga unicamene la interfaz principal
    //A las otras interfaces no les hace nada
    private void TurnOffMainMenu()
    {
        interfazPrincipal.SetActive(false);
    }

    //Funcion que apaga todos los otros menus (pero no hace nada con el principal)
    private void TurnOffOtherMenus()
    {
        foreach (GameObject obj in otrasInterfaces)
        {
            obj.SetActive(false);
        }
    }

    //Prender y apaga la interfaz principal
    private void ToggleMainUI()
    {
        if (interfazPrincipal.activeSelf)
            TurnOffAllMenus();
        else
            TurnOnMainUI();
    }

    public void AbrirInterfaz(int index)
    {
        TurnOffMainMenu();
        for(int i = 0; i < otrasInterfaces.Length; i++)
        {
            if (index != i)
                otrasInterfaces[i].SetActive(false);
            else
                otrasInterfaces[i].SetActive(true);
        }
    }
}

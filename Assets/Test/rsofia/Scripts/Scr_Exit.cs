using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Exit : MonoBehaviour {

    public GameObject sureQuit;

    private void Start()
    {
        sureQuit.SetActive(false);
    }

    public void SureQuit()
    {
        sureQuit.SetActive(true);
    }

    public void CancelQuit()
    {
        sureQuit.SetActive(false);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}

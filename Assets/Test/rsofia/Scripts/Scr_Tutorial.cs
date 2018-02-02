//ruth sofia brown
//git rsofia
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scr_Tutorial : MonoBehaviour
{
    public GameObject tutorial;

    private void Start()
    {
        tutorial.SetActive(false);
    }

    public void TurnOnTutorial()
    {
        tutorial.SetActive(true);
    }
}

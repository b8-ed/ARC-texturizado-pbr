using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SCR_Creditos : MonoBehaviour
{
    public Text Credit_txt;

    int num_progra, num_arte;
    StreamReader reader;
    string creditos;
    string nombre, modelo, proceso, hdr;

    private void Start()
    {
        creditos = "";
        reader = new StreamReader("Creditos.cts");
        hdr = reader.ReadLine();
        creditos += hdr + "\n\n";
        num_progra = int.Parse(reader.ReadLine());
        num_arte = int.Parse(reader.ReadLine());

        creditos += "Programacion:";
        for (int i = 0; i < num_progra; i++)
        {
            nombre = reader.ReadLine();
            creditos += "\n\t" + nombre + "\n\t\t";
            int num_acciones = int.Parse(reader.ReadLine());
            for (int j = 0; j < num_acciones; j++)
            {
                modelo = reader.ReadLine();
                creditos += modelo + "\n\t\t";
            }
        }
        creditos += "\nArte:";
        for (int i = 0; i < num_arte; i++)
        {
            nombre = reader.ReadLine();
            creditos += "\n\t" + nombre + "\n\t";
            int num_acciones = int.Parse(reader.ReadLine());
            for (int j = 0; j < num_acciones; j++)
            {
                modelo = reader.ReadLine();
                proceso = reader.ReadLine();
                creditos += modelo + ": " + proceso + "\n\t";
            }
        }
        Credit_txt.text = creditos;
    }
}
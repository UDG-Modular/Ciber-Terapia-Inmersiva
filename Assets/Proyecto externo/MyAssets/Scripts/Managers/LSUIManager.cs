using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LSUIManager : MonoBehaviour
{
    //Crear instancia de la clase
    public static LSUIManager instance;
    //Texto para mostrar el nombre del nivel
    //public TextMeshProUGUI lNameText;
    //Panel para mostrar el text
    //public GameObject lNamePanel;
    //public GameObject imagePanel;
    //Texto de las monedas
    //public TextMeshProUGUI coinsText;

    void Awake()
    {
        instance = this;
    }


    void Update()
    {
        
    }
}

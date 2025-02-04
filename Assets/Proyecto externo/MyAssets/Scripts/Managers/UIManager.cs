using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject pauseScreen, optionsScreen;

    //Variables para cargar el canvas de mainMenu y levelselect
    public string mainMenu, levelSelect;

    void Awake()
    {
        Instance = this;
    }

    //Manega el pausado y despausado del juego
    public void Resume()
    {
       // GameManager.Instance.PauseUnpause();
    }
    //Activa pantalla de opciones
    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }
    //Desactiva pantalla de opciones
    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }
    //Pantalla de selección de nivel
    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
        //Evita que cuando cambia la escena se pause
        Time.timeScale = 1f;
    }
    //Pantalla del menú principal
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
        //Evita que cuando cambia la escena se pause
        Time.timeScale = 1f;
    }
}

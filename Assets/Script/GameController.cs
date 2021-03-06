﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStates
{
    init,
    card1,
    card2,
    secretRoomOpen
}

public class GameController : MonoBehaviour {

    public Image card1Image;
    public Image card2Image;

    public Text aKey;
    public Animator panelPause;
	public GameObject optionsPanel;
	public RMF_RadialMenu radialMenu;
	public Teleport teleport;
	public Zoom zoom;

    public GameObject player;

	public GameObject focusButton;

    public GameStates gameState;
    
    ObjectAnalizer analizer;

    [System.NonSerialized]
    public int idObjAnalize = -1;

    public static GameController controller;

    public bool paused;

    void Awake()
    {
        controller = this;
    }

    // Use this for initialization
    void Start () {
        analizer = FindObjectOfType<ObjectAnalizer>();
        gameState = GameStates.init;
		Physics.gravity = new Vector3 (0,-10, 0);

     }
	
	// Update is called once per frame
	void Update () {

		foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
			if(Input.GetKey(vKey)){
				print (vKey);

			}
		}


        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown("joystick button 3")) && idObjAnalize != -1)
        {
            analizer.ShowObject(idObjAnalize);
            player.SetActive(false);
        }

            //Pause
            if ((Input.GetKeyUp(KeyCode.Escape)|| Input.GetKeyDown("joystick button 7")) && analizer.active == false)
        {
			SwitchPause ();
        }

		//God MOde
         if (Input.GetKeyDown(KeyCode.F10))
        {
			if (player.tag == "God") {
				player.tag = "Player";
			} else {
				player.tag = "God";
			}

        }

		zoom.enabled = false;
		teleport.enabled = false;
		//Habilidad seleccionada: ZOOM
		if (radialMenu.index == 1) {
			zoom.enabled = true;
		}

		//Habilidad: Teleport
		if (radialMenu.index == 2) {
			teleport.enabled = true;
		}


		//Game States
		if (gameState == GameStates.card1)
        {
            card1Image.enabled = true;
        }
        if (gameState == GameStates.card2)
        {
            card2Image.enabled = true;
        }
        if (card2Image.enabled && card1Image.enabled)
        {
            gameState = GameStates.secretRoomOpen;
        }
    }

	public void SwitchPause (){
		
		paused = !paused;

		Time.timeScale = paused ? 0 : 1;

		//Fades del animator
		panelPause.SetBool("Pause", paused);

		if (!paused)			
			optionsPanel.SetActive (false);

		//Control de cursor visible/invisible
		Cursor.visible = paused;
		if (paused) {
			Cursor.lockState = CursorLockMode.None;
			//Mandamos el foco al primer botón del menú(por el mando)
			FindObjectOfType<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (focusButton);
            

		} else {
			FindObjectOfType<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
			Cursor.lockState = CursorLockMode.Locked;
		}

	}
}

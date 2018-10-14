using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button m_StartButton;
    public Button m_Exit;

    private void Awake()
    {
        m_StartButton.onClick.AddListener(delegate 
        {
            GameMode.m_GameMode.m_GameLogic.SetState(GameState.GS_Game);
            enabled = false;
        });

        //m_Exit
    }

    void Start ()
    {
	}
	
	void Update ()
    {
	}
}

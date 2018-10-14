using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScoreMenuType
{
    SMT_Win,
    SMT_Lose
}

public class ScoreMenu : MonoBehaviour
{
    public Text m_MessageText;
    public Text m_ScoreText;

    public Button m_RestartButton;
    public Button m_MainMenuButton;

    public void SetupWindow(ScoreMenuType InType, int InScore)
    {
        m_MessageText.text = InType == ScoreMenuType.SMT_Lose ? "Looozer but I can understand yours slide skillzzz" : "CONGRATZ DUDE YOU ARE WINNER!11";
        m_ScoreText.text = "Your score:" + InScore;
    }

    private void Awake()
    {
        m_RestartButton.onClick.AddListener(delegate
        {
            GameMode.m_GameMode.m_GameLogic.SetState(GameState.GS_Game);
        });

        m_MainMenuButton.onClick.AddListener(delegate
        {
            GameMode.m_GameMode.m_GameLogic.SetState(GameState.GS_Menu);
        });

    }

    void Start ()
    {

	}
	
	void Update ()
    {
		
	}
}

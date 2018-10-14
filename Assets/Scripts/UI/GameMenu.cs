using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Text m_ScoreText;
    public Text m_HealthText;
    public Text m_TimeText;

    void Start ()
    {
	}

    private void Update()
    {
        if(GameMode.m_GameMode != null && GameMode.m_GameMode.m_GameLogic.m_Player != null)
        {
            m_HealthText.text = "Health: " + GameMode.m_GameMode.m_GameLogic.m_Player.m_PlayerLogic.m_Health;
            m_ScoreText.text = "Score: " + GameMode.m_GameMode.m_GameLogic.m_Player.m_PlayerLogic.m_Score;
            m_TimeText.text = "Time elapsed: " + GameMode.m_GameMode.m_GameLogic.m_SessionTime;
        }
    }
}

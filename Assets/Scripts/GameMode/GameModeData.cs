using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData //It must be a ScriptableObject
{
    [SerializeField]
    public GameObject m_GameMenu;

    [SerializeField]
    public GameObject m_MainMenu;

    [SerializeField]
    public GameObject m_ScoreMenu;

    [SerializeField]
    public GameObject m_PrefabCollectible;

    [SerializeField]
    public GameObject m_PrefabPlayer;

    [SerializeField]
    public float m_MaxCollectibleBoxSpawnRadius = 30.0f;

    [SerializeField]
    public float m_MaxSessionTime = 120.0f;

    [SerializeField]
    public PlayerData m_PlayerData = new PlayerData(100, 100, 400);

    public GameData()
    {

    }
};
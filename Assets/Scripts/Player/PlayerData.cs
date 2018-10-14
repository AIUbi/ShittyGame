using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    [SerializeField]
    public int m_MaxHealth;

    [SerializeField]
    public int m_MaxScore;

    [SerializeField]
    public float m_JumpForce;

    public PlayerData(int InMaxHealth, int InMaxScore, float InJumpForce)
    {
        m_MaxHealth = InMaxHealth;
        m_MaxScore = InMaxScore;
        m_JumpForce = InJumpForce;
    }
}
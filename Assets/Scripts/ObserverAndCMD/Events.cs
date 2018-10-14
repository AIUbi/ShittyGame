using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateChangedEvent : IEvent
{
    public GameState m_State;

    public GameStateChangedEvent(GameState InState)
    {
        m_State = InState;
    }
}

public class PlayerMoveEvent : IEvent
{
    public float m_Right;
    public float m_Forward;
    public float m_LookAt;
    public bool m_Jump;

    public PlayerMoveEvent(float InHorizontal, float InVertical, float InLookAt, bool InJump)
    {
        m_Right = InHorizontal;
        m_Forward = InVertical;
        m_LookAt = InLookAt;
        m_Jump = InJump;
    }
}

public class PlayerPickupEvent : IEvent
{
    public CollectibleData m_Data;
}

public class PlayerCollideEvent : IEvent
{
    public Collision m_Collision;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : IEventListener
{
    private float Rotation;

    PlayerData m_PlayerData;

    public int m_Health { get; private set; }
    public int m_Score { get; private set; }
    public Rigidbody m_PlayerBody { get; private set; }

    public PlayerLogic(PlayerData InData, Rigidbody InPlayerBody)
    {
        m_PlayerData = InData;
        m_PlayerBody = InPlayerBody;
        m_Health = m_PlayerData.m_MaxHealth;
        m_Score = 0;

        GameMode.m_GameMode.m_EventSystem.Subscribe("OnPlayerMove", this);
        GameMode.m_GameMode.m_EventSystem.Subscribe("OnPlayerPickup", this);
    }

    public void TakeDamage(int InDamage)
    {
        m_Health = Mathf.Clamp(m_Health - Mathf.Abs(InDamage), 0, m_Health);
    }

    public void TakeHealth(int InHealth)
    {
        m_Health = Mathf.Clamp(m_Health + Mathf.Abs(InHealth), m_Health, m_PlayerData.m_MaxHealth);
    }

    public void TakeScore(int InScore)
    {
        m_Score = Mathf.Clamp(m_Score + Mathf.Abs(InScore), m_Score, m_PlayerData.m_MaxScore);
    }

    public bool IsAlive()
    {
        return m_Health > 0;
    }

    public void Receive(IEvent InEvent)
    {
        PlayerMoveEvent MoveEvent = InEvent as PlayerMoveEvent;

        if (MoveEvent != null)
        {
            ApplyMovement(MoveEvent);
        }

        PlayerPickupEvent PickupEvent = InEvent as PlayerPickupEvent;

        if(PickupEvent != null)
        {
            ApplyPickup(PickupEvent.m_Data);
        }
    }

    private void ApplyMovement(PlayerMoveEvent InMoveEvent)
    {
        m_PlayerBody.AddForce(m_PlayerBody.transform.forward * InMoveEvent.m_Forward + m_PlayerBody.transform.right * InMoveEvent.m_Right, ForceMode.VelocityChange);

        RaycastHit Hit;
        Physics.Raycast(m_PlayerBody.position, Vector3.down, out Hit);

        if (InMoveEvent.m_Jump && Hit.distance <= 1.0f)
        {
            m_PlayerBody.AddForce(m_PlayerBody.transform.up * m_PlayerData.m_JumpForce);
        }

        Rotation += InMoveEvent.m_LookAt*4.0f;

        m_PlayerBody.rotation = Quaternion.Euler(0, Rotation, 0);
    }

    private void ApplyPickup(CollectibleData InData)
    {
        switch(InData.m_Type)
        {
            case CollectibleType.CT_Damage:

                TakeDamage(InData.m_Value);

                break;
            case CollectibleType.CT_Health:

                TakeHealth(InData.m_Value);

                break;
            case CollectibleType.CT_Score:

                TakeScore(InData.m_Value);

                break;
        }
    }
}

public class Player : MonoBehaviour
{
    PlayerCollideEvent m_PlayerCollideEvent = new PlayerCollideEvent();

    public PlayerLogic m_PlayerLogic { get; private set; }
    public PlayerInput m_PlayerInput { get; private set; }
    public Camera m_PlayerCamera { get; private set; }

    private void Awake()
    {
        enabled = false;

        m_PlayerLogic = new PlayerLogic(GameMode.m_GameMode.m_GameData.m_PlayerData, gameObject.GetComponent<Rigidbody>());
        m_PlayerInput = gameObject.AddComponent<PlayerInput>();

        m_PlayerCamera = GetComponentInChildren<Camera>();
    }

    private void OnCollisionEnter(Collision InCollision) //I solve it in Player because it called only once per frame if I'll implement it in Collectibles I will have overhead.
    {
        if (InCollision.gameObject.GetComponent<Collectible>() != null)
        {
            m_PlayerCollideEvent.m_Collision = InCollision;

            GameMode.m_GameMode.m_EventSystem.Broadcast("OnPlayerCollide", m_PlayerCollideEvent);
        }
    }
}

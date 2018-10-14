using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMoveEvent MoveEvent = new PlayerMoveEvent(0, 0, 0, false);

    public void FixedUpdate()
    { 
        MoveEvent.m_Right = Input.GetAxis("Horizontal");
        MoveEvent.m_Forward = Input.GetAxis("Vertical");
        MoveEvent.m_Jump = !Mathf.Approximately(Input.GetAxis("Jump"), 0);
        MoveEvent.m_LookAt = Input.GetAxis("Mouse X");

        GameMode.m_GameMode.m_EventSystem.Broadcast("OnPlayerMove", MoveEvent);
    }
}

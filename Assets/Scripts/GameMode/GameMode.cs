using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum GameState
{
    GS_None,
    GS_Menu,
    GS_Game,
    GS_Score
}

public class GameModeLogic : IEventListener //GameModeModel inherited from controller receiver.
{
    private PlayerPickupEvent m_PlayerPickupEvent = new PlayerPickupEvent();

    public Canvas m_HUDCanvas { get; private set; }
    public GameObject m_MainMenu { get; private set; }
    public GameObject m_ScoreBoard { get; private set; }
    public GameObject m_GameMenu { get; private set; }

    public GameState m_GameState { get; private set; }
    public Player m_Player { get; private set; }
    public List<GameObject> m_Collectibles { get; private set; }

    public float m_SessionTime { get; private set; }

    public GameModeLogic()
    {
        m_Collectibles = new List<GameObject>();

        m_HUDCanvas = GameMode.m_GameMode.GetComponentInChildren<Canvas>();

        GameMode.m_GameMode.m_EventSystem.Subscribe("OnPlayerCollide", this);
        GameMode.m_GameMode.m_EventSystem.Subscribe("OnGameStateChanged", this);
        GameMode.m_GameMode.m_EventSystem.Subscribe("OnPlayerPickup", this);
    }

    public void Receive(IEvent InEvent)
    {
        PlayerCollideEvent CollideEvent = InEvent as PlayerCollideEvent;

        if (CollideEvent != null)
        {
            Collectible CollectibleObject = CollideEvent.m_Collision.collider.GetComponent<Collectible>();

            if (CollectibleObject != null)
            {
                m_PlayerPickupEvent.m_Data = CollectibleObject.m_CollectibleLogic.m_CollectibleData;

                GameMode.m_GameMode.m_EventSystem.Broadcast("OnPlayerPickup", m_PlayerPickupEvent);

                GameObject.Destroy(CollideEvent.m_Collision.collider.gameObject);
            }
        }

        PlayerPickupEvent PickupEvent = InEvent as PlayerPickupEvent;

        if(PickupEvent != null)
        {
            if(!m_Player.m_PlayerLogic.IsAlive() || m_Player.m_PlayerLogic.m_Score >= GameMode.m_GameMode.m_GameData.m_PlayerData.m_MaxScore)
            {
                SetState(GameState.GS_Score);
            }
        }
    }

    public void SetState(GameState InState) //I want here a state-machine by bitflags with complex UX system BUT...
    {
        if (InState != m_GameState)
        {
            m_GameState = InState;

            switch (m_GameState)
            {
                case GameState.GS_Menu:

                    ToggleMenu(true);
                    ToggleGameMenu(false);
                    ToggleScoreboard(false);

                    SwitchCameras(false, true);

                    break;
                case GameState.GS_Game:

                    ToggleMenu(false);
                    ToggleScoreboard(false);
                    ToggleGameMenu(true);

                    StartGame();
                    SwitchCameras(true, false);

                    break;
                case GameState.GS_Score:

                    ToggleMenu(false);
                    ToggleScoreboard(true);
                    ToggleGameMenu(false);

                    SwitchCameras(false, true);

                    break;
            }

            GameMode.m_GameMode.m_EventSystem.Broadcast("OnStateChanged", new GameStateChangedEvent(InState));
        }
    }

    public void UpdateSessionTime(float InDelta)
    {
        m_SessionTime = Mathf.Clamp(m_SessionTime - InDelta, 0, m_SessionTime);

        if(m_SessionTime <= 0f)
        {
            SetState(GameState.GS_Score);
        }
    }

    private void DestroyGameObjects()
    {
        if (m_Player != null)
        {
            GameObject.Destroy(m_Player.gameObject);
        }

        foreach (GameObject TempCollectible in m_Collectibles)
        {
            GameObject.Destroy(TempCollectible.gameObject);
        }

        m_Collectibles.Clear();
    }

    private void StartGame() //Actually I just can reload this scene, it may be easier to control but..
    {
        DestroyGameObjects();

        m_SessionTime = GameMode.m_GameMode.m_GameData.m_MaxSessionTime;
        m_Player = GameObject.Instantiate(GameMode.m_GameMode.m_GameData.m_PrefabPlayer).GetComponent<Player>();

        float BoxSize = GameMode.m_GameMode.m_GameData.m_MaxCollectibleBoxSpawnRadius;

        for(int I = 0; I < 100; ++I)
        {
            Vector3 RandomPosition = new Vector3(Random.Range(-BoxSize, BoxSize), 0, Random.Range(-BoxSize, BoxSize));

            GameObject TempCollectible = GameObject.Instantiate(GameMode.m_GameMode.m_GameData.m_PrefabCollectible, RandomPosition, Quaternion.identity);
            m_Collectibles.Add(TempCollectible);
        }
    }

    private void ToggleScoreboard(bool InToggle)
    {
        if (InToggle)
        {
            m_ScoreBoard = GameObject.Instantiate(GameMode.m_GameMode.m_GameData.m_ScoreMenu);
            
            m_ScoreBoard.transform.SetParent(m_HUDCanvas.transform, false);

            ScoreMenu TempScoreMenu = m_ScoreBoard.GetComponentInChildren<ScoreMenu>();
            TempScoreMenu.SetupWindow(!m_Player.m_PlayerLogic.IsAlive() ? ScoreMenuType.SMT_Lose : ScoreMenuType.SMT_Win, m_Player.m_PlayerLogic.m_Score);

            DestroyGameObjects();
        }
        else if (m_ScoreBoard != null)
        {
            GameObject.Destroy(m_ScoreBoard);
        }
    }

    private void ToggleMenu(bool InToggle)
    {
        if (InToggle)
        {
            m_MainMenu = GameObject.Instantiate(GameMode.m_GameMode.m_GameData.m_MainMenu);
            m_MainMenu.transform.SetParent(m_HUDCanvas.transform, false);
        }
        else if(m_MainMenu != null)
        {
            GameObject.Destroy(m_MainMenu);
        }
    }

    private void ToggleGameMenu(bool InToggle)
    {
        if (InToggle)
        {
            m_GameMenu = GameObject.Instantiate(GameMode.m_GameMode.m_GameData.m_GameMenu);

            m_GameMenu.transform.SetParent(m_HUDCanvas.transform, false);
        }
        else if (m_GameMenu != null)
        {
            GameObject.Destroy(m_GameMenu);
        }
    }

    public void SwitchCameras(bool InCharacterCamera, bool InGameModeCamera)
    {
        GameMode.m_GameMode.m_GameModeCamera.enabled = InGameModeCamera;

        if (GameMode.m_GameMode.m_GameLogic.m_Player != null)
        {
            GameMode.m_GameMode.m_GameLogic.m_Player.m_PlayerCamera.enabled = InCharacterCamera;
        }
    }
}

public class GameMode : MonoBehaviour
{
    [SerializeField]
    public GameData m_GameData = new GameData();

    public GameModeLogic m_GameLogic { get; private set; }

    public EventSystem m_EventSystem;

    public static GameMode m_GameMode;

    public Camera m_GameModeCamera { get; private set; }

    private void Awake()
    {
        m_GameMode = this;

        m_EventSystem = new EventSystem();

        m_GameLogic = new GameModeLogic();

        m_GameModeCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        m_GameLogic.SetState(GameState.GS_Menu);
    }

    void Update()
    {
        if (m_GameLogic.m_GameState == GameState.GS_Game)
        {
            m_GameLogic.UpdateSessionTime(Time.deltaTime);
        }
    }
}

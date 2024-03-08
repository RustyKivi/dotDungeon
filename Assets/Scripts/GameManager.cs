using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Dictionary<ulong, GameObject> playerInfo = new Dictionary<ulong, GameObject>();

    [Header("Networking")]
    public bool connected;
    public bool inGame;
    public bool isHost;
    public ulong myClientId;

    [Header("World Objects")]
    public GameObject lobbyDoor;
    [SerializeField] private GameObject multiMenu;
    [SerializeField] private GameObject multiLobby;
    public GameObject hostObject;
    [Header("GUI")]
    [SerializeField] private GameObject playerFieldBox;
    [SerializeField] private GameObject playerCardPrefab;
    [SerializeField] private GameObject startButton;
    
    [Header("Player Objects")]
    public CameraController playerCamera;
    public GameObject myObject;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    

    public void StartGame()
    {
        playerCamera.camInit(true);
    }

    public void Init()
    {
        playerCamera.target = myObject.transform;
    }

    public void HostCreated()
    {
        //multiMenu.SetActive(false);
        //multiLobby.SetActive(true);
        isHost = true;
        connected = true;
        hostObject.SetActive(false);
    }

    public void ConnectedAsClient()
    {
        //multiMenu.SetActive(false);
        //multiLobby.SetActive(true);
        isHost = false;
        connected = true;
        hostObject.SetActive(false);
    }

    public void Disconnected()
    {
        playerInfo.Clear();
        GameObject[] playercards = GameObject.FindGameObjectsWithTag("PlayerCard");
        foreach(GameObject card in playercards)
        {
            Destroy(card);
        }

        multiMenu.SetActive(true);
        multiLobby.SetActive(false);
        isHost = false;
        connected = false;
    }

    public void AddPlayerToDictionary(ulong _cliendId, string _steamName, ulong _steamId)
    {
        if (!playerInfo.ContainsKey(_cliendId))
        {
            PlayerInfo _pi = Instantiate(playerCardPrefab, playerFieldBox.transform).GetComponent<PlayerInfo>();
            _pi.steamId = _steamId;
            _pi.steamName = _steamName;
            playerInfo.Add(_cliendId, _pi.gameObject);
        }
    }

    public void UpdateClients()
    {
        foreach(KeyValuePair<ulong,GameObject> _player in playerInfo)
        {
            ulong _steamId = _player.Value.GetComponent<PlayerInfo>().steamId;
            string _steamName = _player.Value.GetComponent<PlayerInfo>().steamName;
            ulong _clientId = _player.Key;

            NetworkTransmission.instance.UpdateClientsPlayerInfoClientRPC(_steamId, _steamName, _clientId);

        }
    }

    public void RemovePlayerFromDictionary(ulong _steamId)
    {
        GameObject _value = null;
        ulong _key = 100;
        foreach(KeyValuePair<ulong,GameObject> _player in playerInfo)
        {
            if(_player.Value.GetComponent<PlayerInfo>().steamId == _steamId)
            {
                _value = _player.Value;
                _key = _player.Key;
            }
        }
        if(_key != 100)
        {
            playerInfo.Remove(_key);
        }
        if(_value!= null)
        {
            Destroy(_value);
        }
    }

    public void ReadyButton(bool _ready)
    {
        NetworkTransmission.instance.IsTheClientReadyServerRPC(_ready, myClientId);
    }

    public bool CheckIfPlayersAreReady()
    {
        bool _ready = false;

        foreach(KeyValuePair<ulong,GameObject> _player in playerInfo)
        {
            if (!_player.Value.GetComponent<PlayerInfo>().isReady)
            {
                startButton.SetActive(false);
                return false;
            }
            else
            {
                startButton.SetActive(true);
                _ready = true;
            }
        }

        return _ready;
    }

    public void Quit()
    {
        Application.Quit();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            playerFieldBox.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            playerFieldBox.SetActive(false);
        }
    }
}

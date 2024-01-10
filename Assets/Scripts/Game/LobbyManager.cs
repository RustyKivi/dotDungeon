using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    [Header("Menu")]
    [SerializeField] private GameObject multiMenu;
    [SerializeField] private GameObject multiLobby;
    [Space]
    [SerializeField] private GameObject playerFieldBox, playerCardPrefab;
    [SerializeField] private GameObject readyButton, NotreadyButton, startButton;
    [Space]
    public TMP_Text lobbyOwner;
    public TMP_Text modeDisplay;
    public Image modeBanner;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate instance of GameConsole. Destroying the new one.");
            Destroy(gameObject);
        }
    }
    //Network Handling
    public void HostCreated()
    {
        multiMenu.SetActive(false);
        multiLobby.SetActive(true);
        GameManager.instance.isHost = true;
        GameManager.instance.connected = true;
    }

    public void ConnectedAsClient()
    {
        multiMenu.SetActive(false);
        multiLobby.SetActive(true);
        GameManager.instance.isHost = false;
        GameManager.instance.connected = true;
    }

    public void Disconnected()
    {
        GameManager.instance.playerInfo.Clear();
        GameObject[] playercards = GameObject.FindGameObjectsWithTag("PlayerCard");
        foreach(GameObject card in playercards)
        {
            Destroy(card);
        }

        multiMenu.SetActive(true);
        multiLobby.SetActive(false);
        GameManager.instance.isHost = false;
        GameManager.instance.connected = false;
    }

    public void AddPlayerToDictionary(ulong _cliendId, string _steamName, ulong _steamId, string _class, int _plrLevel, int _phyLevel,int _mgcLevel, int _defLevel)
    {
        if (!GameManager.instance.playerInfo.ContainsKey(_cliendId))
        {
            PlayerInfo _pi = Instantiate(playerCardPrefab, playerFieldBox.transform).GetComponent<PlayerInfo>();
            _pi.steamId = _steamId;
            _pi.steamName = _steamName;
            _pi.level = _plrLevel;
            _pi.role = _class;
            _pi.level_phy = _phyLevel;
            _pi.level_mgc = _mgcLevel;
            _pi.level_def = _defLevel;
            GameManager.instance.playerInfo.Add(_cliendId, _pi.gameObject);
        }
    }

    public void UpdateClients()
    {
        foreach(KeyValuePair<ulong,GameObject> _player in GameManager.instance.playerInfo)
        {
            ulong _steamId = _player.Value.GetComponent<PlayerInfo>().steamId;
            string _steamName = _player.Value.GetComponent<PlayerInfo>().steamName;
            ulong _clientId = _player.Key;
            string _class = _player.Value.GetComponent<PlayerInfo>().role;
            int _plrLevel = _player.Value.GetComponent<PlayerInfo>().level;
            int _phyLevel = _player.Value.GetComponent<PlayerInfo>().level_phy;
            int _mgcLevel = _player.Value.GetComponent<PlayerInfo>().level_mgc;
            int _defLevel = _player.Value.GetComponent<PlayerInfo>().level_def;

            NetworkTransmission.instance.UpdateClientsPlayerInfoClientRPC(_steamId, _steamName, _clientId, _class,_plrLevel,_phyLevel,_mgcLevel,_defLevel);

        }
    }

    public void RemovePlayerFromDictionary(ulong _steamId)
    {
        GameObject _value = null;
        ulong _key = 100;
        foreach(KeyValuePair<ulong,GameObject> _player in GameManager.instance.playerInfo)
        {
            if(_player.Value.GetComponent<PlayerInfo>().steamId == _steamId)
            {
                _value = _player.Value;
                _key = _player.Key;
            }
        }
        if(_key != 100)
        {
            GameManager.instance.playerInfo.Remove(_key);
        }
        if(_value!= null)
        {
            Destroy(_value);
        }
    }

    public void ReadyButton(bool _ready)
    {
        NetworkTransmission.instance.IsTheClientReadyServerRPC(_ready, GameManager.instance.myClientId);
    }

    public bool CheckIfPlayersAreReady()
    {
        bool _ready = false;

        foreach(KeyValuePair<ulong,GameObject> _player in GameManager.instance.playerInfo)
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
}

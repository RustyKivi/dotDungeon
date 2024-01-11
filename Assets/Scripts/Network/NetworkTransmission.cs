using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkTransmission : NetworkBehaviour
{
    public static NetworkTransmission instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddMeToDictionaryServerRPC(ulong _steamId,string _steamName, ulong _clientId, string _class, int _plrLevel, int _phyLevel,int _mgcLevel, int _defLevel)
    {
        GameConsole.instance.Output($"{_clientId} {_steamName} has joined");
        LobbyManager.instance.AddPlayerToDictionary(_clientId, _steamName, _steamId, _class,_plrLevel,_phyLevel,_mgcLevel,_defLevel);
        LobbyManager.instance.UpdateClients();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveMeFromDictionaryServerRPC(ulong _steamId)
    {
        RemovePlayerFromDictionaryClientRPC(_steamId);
    }

    [ClientRpc]
    private void RemovePlayerFromDictionaryClientRPC(ulong _steamId)
    {
        Debug.Log("removing client");
        LobbyManager.instance.RemovePlayerFromDictionary(_steamId);
    }

    [ClientRpc]
    public void UpdateClientsPlayerInfoClientRPC(ulong _steamId,string _steamName, ulong _clientId, string _class, int _plrLevel, int _phyLevel,int _mgcLevel, int _defLevel)
    {
        LobbyManager.instance.AddPlayerToDictionary(_clientId, _steamName, _steamId, _class,_plrLevel,_phyLevel,_mgcLevel,_defLevel);
    }

    [ServerRpc(RequireOwnership = false)]
    public void IsTheClientReadyServerRPC(bool _ready, ulong _clientId)
    {
        AClientMightBeReadyClientRPC(_ready, _clientId);
    }

    [ClientRpc]
    private void AClientMightBeReadyClientRPC(bool _ready, ulong _clientId)
    {
        foreach(KeyValuePair<ulong,GameObject> player in GameManager.instance.playerInfo)
        {
            if(player.Key == _clientId)
            {
                player.Value.GetComponent<PlayerInfo>().isReady = _ready;
                player.Value.GetComponent<PlayerInfo>().readyImage.SetActive(_ready);
                if (NetworkManager.Singleton.IsHost)
                {
                    Debug.Log(LobbyManager.instance.CheckIfPlayersAreReady());
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = true)]
    public void UpdateSelectedModeServerRPC(int _gamemode)
    {
        UpdateSelectedModeClientRPC(_gamemode);
        Debug.Log("ServerRPC update" + _gamemode);
    }

    [ClientRpc]
    private void UpdateSelectedModeClientRPC(int _gamemode)
    {
        GameManager.instance.SwitchGameMode(_gamemode);
        Debug.Log("ClientRPC update" + _gamemode);
    }
    
    [ServerRpc(RequireOwnership = true)]
    public void StartGameServerRPC()
    {
        StartGameClientRPC();
    }

    [ClientRpc]
    private void StartGameClientRPC()
    {
        GameManager.instance.loadScreen.SetActive(true);
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerServerRPC(ulong _steamId, string _role)
    {
        UpdatePlayerClientRPC(_steamId,_role);
    }

    [ClientRpc]
    private void UpdatePlayerClientRPC(ulong _steamId, string _role)
    {
        foreach(KeyValuePair<ulong,GameObject> _player in GameManager.instance.playerInfo)
        {
            if(_player.Value.GetComponent<PlayerInfo>().steamId == _steamId)
            {
                PlayerInfo playerData = _player.Value.GetComponent<PlayerInfo>();
                playerData.role = _role;

                // GUI update
                playerData.UpdateGUI();
            }
        }
    }
}
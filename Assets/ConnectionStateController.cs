using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class ConnectionStateController : NetworkBehaviour
{
    public static ConnectionStateController instance;

    public enum GameStates { Menu, Game, Win, Lose }
    public GameStates gameState;

    private int _ConnectedPlayers;

    [Header("Events")]
    public Action<GameStates> onGameStateChenged;

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
    }

    private void NetworkManager_OnServerStarted()
    {
        if (!IsServer) return;

        _ConnectedPlayers++;
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback; ;
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        _ConnectedPlayers++;

        if (_ConnectedPlayers >= 2) GameStartClientRpc();
    }

    [ClientRpc]
    private void GameStartClientRpc()
    {
        gameState = GameStates.Game;
        onGameStateChenged?.Invoke(gameState);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback; 
    }
}

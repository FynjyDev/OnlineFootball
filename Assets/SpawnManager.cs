using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private Transform ballSpawnPos;
    [SerializeField] private GameObject ballPrefab;

    private void Start()
    {
        ConnectionStateController.instance.onGameStateChenged += GameStateChangeCallback;
    }

    private void GameStateChangeCallback(ConnectionStateController.GameStates _gameState)
    {
        switch (_gameState)
        {
            case ConnectionStateController.GameStates.Game: SpawnBall(); break;
        }
    }

    public void SpawnBall()
    {
        if (!IsServer) return;

        GameObject newBall = Instantiate(ballPrefab, ballSpawnPos.position, Quaternion.identity, transform);
        newBall.GetComponent<NetworkObject>().Spawn();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ConnectionStateController.instance.onGameStateChenged -= GameStateChangeCallback;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class PlayerMarker : NetworkBehaviour
{
    [SerializeField] private Image marker;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer && IsOwner)
            SetMarkerColorServerRpc(Color.red);
    }

    [ServerRpc]
    private void SetMarkerColorServerRpc(Color _color)
    {
        SetMarkerColorClientRpc(_color);
    }

    [ClientRpc]
    private void SetMarkerColorClientRpc(Color _color)
    {
        marker.color = _color;
    }
}

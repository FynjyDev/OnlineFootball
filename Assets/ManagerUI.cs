using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    public enum PanelTypes { connection, waiting, game }

    [Header("PANELS")]
    [SerializeField] private List<Panel> panels;

    private void Start()
    {
        OpenPanel(PanelTypes.connection, true);
        ConnectionStateController.instance.onGameStateChenged += GameStateChangeCallback;
    }

    private void GameStateChangeCallback(ConnectionStateController.GameStates _gameState)
    {
        switch (_gameState)
        {
            case ConnectionStateController.GameStates.Game:OpenPanel(PanelTypes.game, true); break;
        }
    }

    public void OpenPanel(PanelTypes _panelType, bool _closeAnother = false)
    {
        if (_closeAnother) CloseAllPanels();

        GetPanelByType(_panelType).gameObject.SetActive(true);
    }

    public void CloseAllPanels()
    {
        for (int i = 0; i < panels.Count; i++) panels[i].gameObject.SetActive(false);
    }

    private Panel GetPanelByType(PanelTypes _panelType)
    {
        Panel _panel = new Panel();
        for (int i = 0; i < panels.Count; i++) if (panels[i].panelType == _panelType) _panel = panels[i];

        return _panel;
    }

    public void HostButtonCallback()
    {
        NetworkManager.Singleton.StartHost();
        OpenPanel(PanelTypes.waiting, true);
    }

    public void ClientButtonCallback()
    {
        NetworkManager.Singleton.StartClient();
        OpenPanel(PanelTypes.waiting, true);
    }

    private void OnDestroy()
    {
        ConnectionStateController.instance.onGameStateChenged -= GameStateChangeCallback;
    }
}

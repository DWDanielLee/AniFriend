using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class StartManager : MonoBehaviourPunCallbacks {
    static StartManager instance;

    [SerializeField] string gameVersion = "1.0";
    [Space]
    [SerializeField] Button button_Play;
    [SerializeField] Text text_network;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    void Start() {
        if (button_Play != null) {
            button_Play.interactable = false;
        }

        if (text_network != null) {
            text_network.text = "Offline : Connecting...";
        }

        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        if (text_network != null) {
            text_network.text = "Online";
        }

        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }

        if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();
        }

        if (button_Play != null) {
            button_Play.interactable = true;
        }
    }

    public override void OnConnectedToMaster() {
        if (button_Play != null) { 
            button_Play.interactable = true;
        }

        if (text_network != null) {
            text_network.text = "Online";
        }
    }

    public override void OnDisconnected(DisconnectCause cause) {
        if (button_Play != null) {
            button_Play.interactable = false;
        }

        if (text_network != null) {
            text_network.text = "Offline : Connecting...";
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    public void BtnPlay() {
        if (!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene("2Select");
        }
    } 
}

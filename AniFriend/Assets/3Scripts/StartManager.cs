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

        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }

        if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();
        }

        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.Disconnect();
        }

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        if (text_network != null) {
            text_network.text = "마스터 서버에 연결중...";
        }
    }

    public override void OnConnectedToMaster() {
        if (button_Play != null) { 
            button_Play.interactable = true;
        }

        if (text_network != null) {
            text_network.text = "연결성공";
        }
    }

    public override void OnDisconnected(DisconnectCause cause) {
        if (button_Play != null) {
            button_Play.interactable = false;
        }

        if (text_network != null) {
            text_network.text = "연결실패: 다시 연결중...";
        }

        PhotonNetwork.ConnectUsingSettings();
    }
}

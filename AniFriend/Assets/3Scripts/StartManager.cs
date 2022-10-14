using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public sealed class StartManager : MonoBehaviourPunCallbacks {
    static StartManager instance;

    [SerializeField] string gameVersion = "1.0";

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    void Start() {
        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }

        if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();
        }
    }

    public override void OnConnectedToMaster() {
        
    }

    public override void OnDisconnected(DisconnectCause cause) {
        PhotonNetwork.ConnectUsingSettings();
    }
}

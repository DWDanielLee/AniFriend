using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LobbyManager : MonoBehaviourPunCallbacks {
    static LobbyManager instance;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    void Start() {
        if (!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene("Start");
            return;
        }

        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }

        if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {

    }

    public override void OnDisconnected(DisconnectCause cause) {
        SceneManager.LoadScene("Start");
    }
}

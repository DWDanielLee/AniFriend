using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public sealed class PlayManager : MonoBehaviourPunCallbacks {
    static PlayManager instance;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    void Start() {
        if (!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene("Start");
            return;
        }

        if (!PhotonNetwork.InLobby) {
            SceneManager.LoadScene("Lobby");
            return;
        }

        if (!PhotonNetwork.InRoom) {
            
        }
    }

    public override void OnJoinedRoom() {
        
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause) {
        SceneManager.LoadScene("Start");
    }
}

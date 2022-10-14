using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public sealed class SettingManager : MonoBehaviourPunCallbacks {
    static SettingManager instance;
    
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

        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause) {
        SceneManager.LoadScene("Start");
    }
}

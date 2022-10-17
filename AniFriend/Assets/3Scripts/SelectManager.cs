using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public sealed class SelectManager : MonoBehaviourPunCallbacks { 
    static SelectManager instance;

    [SerializeField] GameObject[] characters;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    void Start() {
        if (!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene("1Start");
            return;
        }

        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }

        if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();
        }
    }

    public override void OnDisconnected(DisconnectCause cause) {
        SceneManager.LoadScene("1Start");
    }

    public void BtnNext() {
        if (characters == null) return;

        for (var i = 0; i < characters.Length - 1; i++) {
            if (characters[i] == null) continue;
            if (characters[i].activeInHierarchy) {
                characters[i].SetActive(false);
                characters[i + 1].SetActive(true);
                break;
            }
        }
    }

    public void BtnPrev() {
        if (characters == null) return;

        for (var i = characters.Length - 1; i >= 1; i--) {
            if (characters[i].activeInHierarchy) {
                if (characters[i] == null) continue;
                characters[i - 1].SetActive(true);
                characters[i].SetActive(false);
                break;
            }
        }
    }

    public void BtnSelectCharacter() {
        if (characters == null) return;

        for (var i = 0; i < characters.Length; i++) {
            if (characters[i] == null) continue;
            if (characters[i].activeInHierarchy) {
                ExitGames.Client.Photon.Hashtable properties = 
                    PhotonNetwork.LocalPlayer.CustomProperties;
                if (properties.ContainsKey("Character")) {
                    properties["Character"] = characters[i].name;
                } else {
                    properties.Add("Character", characters[i].name);
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
                break;
            }
        }

        SceneManager.LoadScene("3Lobby");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;

public sealed class SettingManager : MonoBehaviourPunCallbacks {
    static SettingManager instance;

    [SerializeField] InputField roomName;
    [SerializeField] Text status;
    [SerializeField] Text pop;
    [SerializeField] Button next;
    [SerializeField] GameObject loading;

    List<RoomInfo> roomList;

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

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
        => this.roomList = roomList;

    public override void OnDisconnected(DisconnectCause cause) 
        => SceneManager.LoadScene("1Start");


    public void Overlap() {
        if (roomName == null) return;

        if (next != null) {
            next.interactable = false;
        }

        if (roomName.text.Length < 2) {
            if (status != null) {
                status.text = "방이름은 최소한 2글자 이상이어야 합니다.";
            }
            return;
        }

        if (roomName.text.Length > 15) {
            if (status != null) {
                status.text = "방이름은 15글자 이하이어야 합니다.";
            }
            return;
        }

        foreach (var info in roomList) {
            if (roomName.text == info.Name) {
                if (status != null) {
                    status.text = "이미 존재하는 방이름입니다.";
                }
                return;
            }
        }

        if (status != null) {
            status.text = "사용 가능한 방이름 입니다.";
        }

        if (next != null) {
            next.interactable = true;
        }
    }

    public void BtnNext() {
        if (roomName == null || pop == null) return;
        var properties = PhotonNetwork.LocalPlayer.CustomProperties;
        if (properties.ContainsKey("Title")) {
            properties["Title"] = roomName.text;
        } else {
            properties.Add("Title", roomName.text);
        }
        if (properties.ContainsKey("Host")) {
            properties["Host"] = true;
        } else {
            properties.Add("Host", true);
        }
        if (properties.ContainsKey("Population")) {
            properties["Population"] = Convert.ToInt32(pop.text);
        } else {
            properties.Add("Population", Convert.ToInt32(pop.text));
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        if (loading != null) loading.SetActive(true);
        StartCoroutine(LoadScene());
    }

    public void BtnPop(int num) {
        if (pop != null) {
            pop.text = num.ToString();
        }
    }

    public void BtnBack() => SceneManager.LoadScene("3Lobby");

    IEnumerator LoadScene() {
        var asyncOption = SceneManager.LoadSceneAsync("5Play");
        asyncOption.allowSceneActivation = false;
        yield return new WaitForSeconds(3f);
        asyncOption.allowSceneActivation = true;
    }
}

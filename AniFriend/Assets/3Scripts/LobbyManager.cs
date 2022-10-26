using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LobbyManager : MonoBehaviourPunCallbacks {
    public static LobbyManager Instance { get; private set; }

    [SerializeField] GameObject prefab_room;
    [SerializeField] RectTransform content;
    [SerializeField] GameObject loading;
    [SerializeField] float spacing = 15f;

    List<Tuple<GameObject, RoomInfo>> rooms = new List<Tuple<GameObject, RoomInfo>>();

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    void Start() {
        if (!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene("1Start");
            return;
        }

        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }

        StartCoroutine("Renew");
    }

    IEnumerator Renew() {
        while (true) {
            if (PhotonNetwork.InLobby) {
                PhotonNetwork.LeaveLobby();
            }
            PhotonNetwork.JoinLobby();
            yield return new WaitForSeconds(5f);
        }
    }

    void OnDestroy() => StopCoroutine("Renew");

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        if (prefab_room == null || content == null) return;
 
        if (rooms.Count == roomList.Count) {
            var infos = from room in rooms
                        select room.Item2;

            foreach (var info in infos) {
                foreach (var room in roomList) {
                    if (info.Name == room.Name && info.PlayerCount == room.PlayerCount) 
                        goto Continue;
                }
                goto IsDifferent;
            Continue: continue;
            }
        } else { goto IsDifferent; }

        return;

    IsDifferent:
        while (rooms.Count > 0) {
            var obj = rooms[0].Item1;
            rooms.RemoveAt(0);
            Destroy(obj);
        }

        foreach (var info in roomList) {
            if (prefab_room == null) continue;

            var obj = Instantiate(prefab_room, content);
            if (obj == null) return;

            var room = obj.GetComponent<Room>();
            if (room == null) { Destroy(obj); return; }

            room.Init(info.Name, info.PlayerCount, info.MaxPlayers);
            rooms.Add(new Tuple<GameObject, RoomInfo>(obj, info));
        }

        var (width, height) = (content.sizeDelta.x, 0f);

        var rect = prefab_room.GetComponent<RectTransform>();
        if (rect != null) {
            height += rect.rect.height * rooms.Count;
        }

        var padding = content.GetComponent<VerticalLayoutGroup>();
        if (padding != null) {
            height += padding.padding.top + padding.padding.bottom;
        }

        height += spacing * (rooms.Count > 0 ? rooms.Count - 1 : 0);

        content.sizeDelta = new Vector2(width, height);
    }

    public override void OnDisconnected(DisconnectCause cause)
        => SceneManager.LoadScene("1Start");

    public void NextScene(string title) {
        if (title == null) return;

        var properties = PhotonNetwork.LocalPlayer.CustomProperties;
        if (properties.ContainsKey("Title")) {
            properties["Title"] = title;
        } else {
            properties.Add("Title", title);
        }
        if (properties.ContainsKey("Host")) {
            properties["Host"] = false;
        } else {
            properties.Add("Host", false);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        if (loading != null) loading.SetActive(true);
        StartCoroutine(LoadScene());
    }

    public void BtnBack() => SceneManager.LoadScene("2Select");

    public void BtnCreate() => SceneManager.LoadScene("4Setting");

    IEnumerator LoadScene() {
        var asyncOption = SceneManager.LoadSceneAsync("5Play");
        asyncOption.allowSceneActivation = false;
        yield return new WaitForSeconds(3f);
        asyncOption.allowSceneActivation = true;
    }
}

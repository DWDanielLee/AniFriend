using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LobbyManager : MonoBehaviourPunCallbacks {
    public static LobbyManager Instance { get; private set; }

    [SerializeField] GameObject prefab_room;
    [SerializeField] RectTransform content;
    [SerializeField] GameObject loading;

    Queue<GameObject> roomQueue = new Queue<GameObject>();

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
                if (roomQueue.Count != PhotonNetwork.CountOfRooms) {
                    PhotonNetwork.LeaveLobby();
                    PhotonNetwork.JoinLobby();
                }
            } else {
                PhotonNetwork.JoinLobby();
            }
            yield return new WaitForSeconds(5f);
        }
    }

    void OnDestroy() => StopCoroutine("Renew");

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        if (prefab_room == null || content == null) return;

        while (roomQueue.Count > 0) {
            Destroy(roomQueue.Dequeue());
        }

        foreach (var info in roomList) {
            if (prefab_room == null) continue;

            var obj = Instantiate(prefab_room, content);
            if (obj == null) return;

            var room = obj.GetComponent<Room>();
            if (room == null) { Destroy(obj); return; }

            room.Init(info.Name, info.PlayerCount, info.MaxPlayers);
            roomQueue.Enqueue(obj);
        }

        var (width, height) = (content.sizeDelta.x, 0f);
        var rect = prefab_room.GetComponent<RectTransform>();
        if (rect != null) {
            height += rect.rect.height * roomQueue.Count;
        }
        var padding = content.GetComponent<VerticalLayoutGroup>();
        if (padding != null) {
            height += padding.padding.top + padding.padding.bottom;
        }
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

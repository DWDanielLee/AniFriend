using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class PlayManager : MonoBehaviourPunCallbacks {
    static PlayManager instance;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    void Start() {
        if (!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene("1Start");
            return;
        }

        if (!PhotonNetwork.InLobby) {
            SceneManager.LoadScene("3Lobby");
            return;
        }

        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }

        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Title")) {
            SceneManager.LoadScene("3Lobby");
            return;
        }
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Character")) {
            SceneManager.LoadScene("3Lobby");
            return;
        }
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Host")) {
            SceneManager.LoadScene("3Lobby");
            return;
        }

        var title = (string)PhotonNetwork.LocalPlayer.CustomProperties["Title"];

        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Host"]) {
            if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Population")) {
                SceneManager.LoadScene("3Lobby");
                return;
            }
            var pop = (int)PhotonNetwork.LocalPlayer.CustomProperties["Population"];
            PhotonNetwork.CreateRoom(title, new RoomOptions() { MaxPlayers = (byte)pop, PublishUserId = true });
            return;
        }

        PhotonNetwork.JoinRoom(title);
    }

    public override void OnJoinedRoom() {
        var character = (string)PhotonNetwork.LocalPlayer.CustomProperties["Character"];
        PhotonNetwork.Instantiate(character, Vector3.zero, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient) {
            if (Chatting.Instance != null) {
                var message = "방이 성공적으로 생성되었습니다.";
                Chatting.Instance.SystemMessage(message);
            }
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        SceneManager.LoadScene("3Lobby");
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        SceneManager.LoadScene("3Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause) {
        SceneManager.LoadScene("1Start");
    }

    void OnDestroy() {
        if (Chatting.Instance == null) return;
        var nickName = PhotonNetwork.LocalPlayer.NickName == null 
            || PhotonNetwork.LocalPlayer.NickName == ""
            ? PhotonNetwork.LocalPlayer.UserId : PhotonNetwork.LocalPlayer.NickName;
        var message = $"{nickName}님이 퇴장하셨습니다.";
        Chatting.Instance.SystemMessage(message);
    }
}

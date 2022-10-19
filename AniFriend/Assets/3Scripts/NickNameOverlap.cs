using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public sealed class NickNameOverlap : MonoBehaviourPunCallbacks {
    [SerializeField] Canvas init;
    [SerializeField] InputField inputField;
    [SerializeField] Text overlap;
    [SerializeField] Button button;
    
    public void OnOverlap() {
        if (inputField == null || overlap == null) return;

        if (button != null) {
            button.interactable = false;
        }

        if (inputField.text.Length < 2) {
            overlap.text = "닉네임은 2글자 이상이어야 합니다.";
            return;
        }

        if (inputField.text.Length > 15) {
            overlap.text = "닉네임은 15자 이하이어야 합니다.";
            return;
        }

        var otherPlayers = PhotonNetwork.PlayerListOthers;
        foreach (var player in otherPlayers) {
            if (inputField.text == player.NickName) {
                overlap.text = "닉네임이 이미 존재합니다.";
                return;
            }
        }

        if (button != null) {
            button.interactable = true;
        }

        overlap.text = "사용 가능한 닉네임입니다.";
    }

    public void Btn_Complete() {
        if (init == null) return;

        if (inputField != null) {
            PhotonNetwork.LocalPlayer.NickName = inputField.text;
        }

        if (Chatting.Instance != null) {
            var nickName = PhotonNetwork.LocalPlayer.NickName == null 
                || PhotonNetwork.LocalPlayer.NickName == ""
                ? PhotonNetwork.LocalPlayer.UserId : PhotonNetwork.LocalPlayer.NickName;
            var message = string.Format($"{nickName}님이 입장하셨습니다.");
            Chatting.Instance.SystemMessage(message);
        }

        init.enabled = false;
    }
}

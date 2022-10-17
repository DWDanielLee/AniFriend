using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Room : MonoBehaviour {
    [HideInInspector] public Text title, population;
    int curPop, maxPop;

    public void Init(string title, int curPop, int maxPop) {
        if (this.title != null) {
            this.title.text = title;
        }

        (this.curPop, this.maxPop) = (curPop, maxPop);

        if (population != null) { 
            population.text = curPop + " / " + maxPop;
        }
    }

    public void BtnEnter() {
        if (title == null) return;
        if (curPop >= maxPop) return;
        if (LobbyManager.Instance != null) { 
            LobbyManager.Instance.NextScene(title.text); 
        } 
    }
}

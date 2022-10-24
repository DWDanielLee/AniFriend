using UnityEngine;

public sealed class BgmManager : MonoBehaviour {
    void Start() {
        var bgm = FindObjectsOfType<BgmManager>();
        if (bgm.Length > 1) Destroy(gameObject);
    }
}

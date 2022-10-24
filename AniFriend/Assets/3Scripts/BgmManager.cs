using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class BgmManager : MonoBehaviour {
    void Start() {
        var bgm = FindObjectsOfType<BgmManager>();
        if (bgm.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    void Update() {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(4))
        {
            Destroy(gameObject);
        }
    }
}

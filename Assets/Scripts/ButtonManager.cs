using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    public void NewGameBtn(string mainGameScreen) {
        SceneManager.LoadScene(mainGameScreen);
    }
}

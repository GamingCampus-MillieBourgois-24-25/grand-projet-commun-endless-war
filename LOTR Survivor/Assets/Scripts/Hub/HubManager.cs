using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    //~~ Start Game ~~\\
    [SerializeField] private Button startButton;
    [SerializeField] private string nextScene;
    //~~ Power-Up ~~\\


    //~~ Character Selection ~~\\
    [SerializeField] private Image imageCharacter;
    [SerializeField] private Button nextCharacter;
    [SerializeField] private Button previousCharacter;
    // Start is called before the first frame update
    void Start()
    {
            if (startButton == null)
            {
                Debug.LogError("Start Button n'est pas assigné dans l'inspecteur !");
                return;
            }

            startButton.onClick.AddListener(GameScene);
        }

    void GameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestMobile");
    }
}

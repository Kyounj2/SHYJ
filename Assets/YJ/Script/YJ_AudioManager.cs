using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 이 아이는 오디오매니저!
public class YJ_AudioManager : MonoBehaviour
{
    // 메인카메라를 가져올것
    Camera main;

    // 오디오
    AudioSource audio;

    // 브금목록
    [SerializeField]
    [Header("BGM")]
    public AudioClip connec_LobbyBGM;
    public AudioClip readyBGM;
    public AudioClip gameBGM;
    public AudioClip killerBGM;
    public AudioClip playerBGM;

    int winner = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        main = Camera.main;
        audio = main.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "ConnectionScene" ||
            SceneManager.GetActiveScene().name == "LobbyScene")
        {
            if(audio.clip != connec_LobbyBGM)
            {
                audio.clip = connec_LobbyBGM;
                audio.Play();
            }
        }

       if (SceneManager.GetActiveScene().name == "ReadyScene" && main == null)
        {
            main = Camera.main;
            audio = main.GetComponent<AudioSource>();
            audio.clip = readyBGM;
            audio.Play();
        }

       if (SceneManager.GetActiveScene().name == "GameScene" && main == null)
        {
            main = Camera.main;
            audio = main.GetComponent<AudioSource>();
            audio.clip = gameBGM;
            audio.Play();
        }

        if (SceneManager.GetActiveScene().name == "EndingScene" && main == null)
        {
            winner = GameObject.Find("UsersData").GetComponent<UsersData>().winner;
            main = Camera.main;
            audio = main.GetComponent<AudioSource>();
            if(winner == 1)
            {
                audio.clip = killerBGM;
                audio.Play();
            }
            else if(winner == 2)
            {
                audio.clip = playerBGM;
                audio.Play();
            }
        }
    }
}

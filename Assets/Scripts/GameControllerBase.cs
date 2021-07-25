using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameControllerBase : MonoBehaviour
{
    public Camera _mainCamera;
    public GameObject _platformContainer;
    [SerializeField]
    protected CharacterController player1;
    public CharacterController _playerOne
    {
        get { return player1; }
        set { player1 = value; }
    }
    public Color _playerOneBackground;
    public TextMeshPro _playerOneVictoryText;

    [SerializeField]
    protected CharacterController player2;
    public CharacterController _playerTwo
    {
        get { return player2; }
        set { player2 = value; }
    }
    public Color _playerTwoBackground;
    public TextMeshPro _playerTwoVictoryText;
    public AudioSource _audioPlayer;
    public AudioClip _gameOverSound;

    public GameObject[] playerSpawns = new GameObject[2];

    public GameObject pauseMenu;
    protected bool paused = false;

    // Used to initialize the script
    protected virtual void Start()
    {
        UnPauseGame();

        _playerOneBackground = ColorManager.player1Background;
        _playerTwoBackground = ColorManager.player2Background;

        _playerOne._animationController._mySpriteRenderer.color = ColorManager.player1Color;
        _playerTwo._animationController._mySpriteRenderer.color = ColorManager.player2Color;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetStage();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                PauseGame();
            }
            else
            {
                UnPauseGame();
            }
        }
    }


    public virtual void KillPlayer(PlayerIndex player)
    {
        if (player == PlayerIndex.One)
        {
            _playerTwoVictoryText.DOFade(1f, 2f);
            _playerOne.NonMeleeEndgameEffect(false);
            _playerTwo.NonMeleeEndgameEffect(true);
        }
        else if (player == PlayerIndex.Two)
        {
            _playerOneVictoryText.DOFade(1f, 2f);
            _playerOne.NonMeleeEndgameEffect(true);
            _playerTwo.NonMeleeEndgameEffect(false);
        }
        _platformContainer.SetActive(false);
        _mainCamera.DOColor(Color.black, 1f).SetEase(Ease.Linear);
        _audioPlayer.PlayOneShot(_gameOverSound);

    }

    public virtual void MeleeKillPlayer(PlayerIndex player)
    {
        if (player == PlayerIndex.One)
        {
            _playerTwo.ShowEndgameEffect(winner: false, _playerOne.transform.position);
            _playerOne.ShowEndgameEffect(winner: true, _playerTwo.transform.position);
            _playerOneVictoryText.DOFade(1f, 2f);
        }
        else
        {
            _playerOne.ShowEndgameEffect(winner: false, _playerTwo.transform.position);
            _playerTwo.ShowEndgameEffect(winner: true, _playerOne.transform.position);
            _playerTwoVictoryText.DOFade(1f, 2f);
        }
        _platformContainer.SetActive(false);
        _mainCamera.DOColor(Color.black, 1f).SetEase(Ease.Linear);
        _audioPlayer.PlayOneShot(_gameOverSound);
    }

    private Tween currentTimeTween;
    public virtual void PauseGame()
    {
        //Time.timeScale = 0;
        currentTimeTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 0.2f).SetEase(Ease.OutQuint);
        pauseMenu.SetActive(true);
        paused = true;
    }

    public virtual void UnPauseGame()
    {
        if (currentTimeTween != null)
        {
            currentTimeTween.Kill();
            currentTimeTween = null;
        }
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        paused = false;
    }

    public virtual void Cleanup()
    {
        _playerOneVictoryText.DOKill();
        _playerTwoVictoryText.DOKill();
        _playerOneVictoryText.alpha = 0;
        _playerTwoVictoryText.alpha = 0;
        _mainCamera.DOKill();

        _playerOne.transform.position = playerSpawns[0].transform.position;
        _playerTwo.transform.position = playerSpawns[1].transform.position;

        _platformContainer.SetActive(true);
    }

    public virtual void ResetStage()
    {
        UnPauseGame();

        Cleanup();

        _playerOne.Cleanup();
        _playerTwo.Cleanup();

        _playerOne._animationController.Cleanup();
        _playerTwo._animationController.Cleanup();

    }
}

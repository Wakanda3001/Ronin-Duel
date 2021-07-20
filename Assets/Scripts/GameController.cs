using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEditor.Animations;
using UnityEditor;

public enum PlayerIndex { One, Two }

public interface IGameController
{
    public CharacterController _playerOne { get; set; }
    public CharacterController _playerTwo { get; set; }
    public void KillPlayer(PlayerIndex player);
    public void MeleeKillPlayer(PlayerIndex player);
}
public class GameController : MonoBehaviour, IGameController
{

    public Camera _mainCamera;
    public GameObject _platformContainer;
    [SerializeField]
    private CharacterController player1;
    public CharacterController _playerOne{
        get { return player1; }
        set { player1 = value; }
    }
    public Color _playerOneBackground;
    public TextMeshPro _playerOneVictoryText;

    [SerializeField]
    private CharacterController player2;
    public CharacterController _playerTwo { 
        get { return player2; }
        set { player2 = value; }
    }
    public Color _playerTwoBackground;
    public TextMeshPro _playerTwoVictoryText;
    public AudioSource _audioPlayer;
    public AudioClip _switchSound1;
    public AudioClip _switchSound2;
    public AudioClip _gameOverSound;
    public PlayerIndex _currentPlayer = PlayerIndex.One;
    public float _timePerActivePlayer = 2f; // In seconds

    public GameObject spawnpointParent; //Gameobject spawnpoints are children of
    public GameObject[] playerSpawns = new GameObject[2];
    private GameObject oldPos;
    public GameObject switcher;
    public GameObject arrow;

    public GameObject pauseMenu;
    private bool paused  = false;
    public Vector3 player1PauseVelocity;
    public Vector3 player2PauseVelocity;
    
    private List<GameObject> spawnpoints = new List<GameObject>();

    // Used to initialize the script
    void Start()
    {
        _playerOneBackground = ColorManager.player1Background;
        _playerTwoBackground = ColorManager.player2Background;

        _playerOne._animationController._mySpriteRenderer.color = ColorManager.player1Color;
        _playerTwo._animationController._mySpriteRenderer.color = ColorManager.player2Color;

        foreach(Transform child in spawnpointParent.transform)
        {
            spawnpoints.Add(child.gameObject);
        }

        SpawnSwitcher(PlayerIndex.Two);

    }

    // Update is called once per frame
    void Update()
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

    public void SpawnSwitcher(PlayerIndex activator)
    {
        if(activator == _currentPlayer)
        {
            return;
        }
        GameObject newPos = spawnpoints[Random.Range(0, spawnpoints.Count)];
        while (newPos == oldPos)
        {
            newPos = spawnpoints[Random.Range(0, spawnpoints.Count)];
        }
        oldPos = newPos;

        switcher.transform.position = newPos.transform.position;

        SwitchActivePlayer();
        
    }


    public void KillPlayer(PlayerIndex player)
    {
        if(player == PlayerIndex.One)
        {
            _playerTwoVictoryText.DOFade(1f, 2f);
            _playerOne.NonMeleeEndgameEffect(false);
            _playerTwo.NonMeleeEndgameEffect(true);
        }
        else if(player == PlayerIndex.Two)
        {
            _playerOneVictoryText.DOFade(1f, 2f);
            _playerOne.NonMeleeEndgameEffect(true);
            _playerTwo.NonMeleeEndgameEffect(false);
        }
        _platformContainer.SetActive(false);
        _mainCamera.DOColor(Color.black, 1f).SetEase(Ease.Linear);
        _audioPlayer.PlayOneShot(_gameOverSound);

    }

    public void MeleeKillPlayer(PlayerIndex player)
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

    public void OnPlayersTouched()
    {
        if (_currentPlayer == PlayerIndex.One)
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

    void SwitchActivePlayer()
    {

        //switcher.GetComponent<SpriteRenderer>().color = _currentPlayer == PlayerIndex.One ? Color.blue : Color.red ; <= better & cooler
        if (_currentPlayer == PlayerIndex.One)
        {
            switcher.GetComponent<SpriteRenderer>().color = ColorManager.player1Color;
            arrow.GetComponent<SpriteRenderer>().color = ColorManager.player1Color;
            switcher.GetComponent<Renderer>().sharedMaterial.SetColor("_GlowColor", Color.blue);
            _mainCamera.backgroundColor = _playerTwoBackground;
            _currentPlayer = PlayerIndex.Two;
            _audioPlayer.Stop();
            _audioPlayer.PlayOneShot(_switchSound2);
        }
        else
        {
            switcher.GetComponent<SpriteRenderer>().color = ColorManager.player2Color;
            arrow.GetComponent<SpriteRenderer>().color = ColorManager.player2Color;
            switcher.GetComponent<Renderer>().sharedMaterial.SetColor("_GlowColor", Color.red);
            _mainCamera.backgroundColor = _playerOneBackground;
            _currentPlayer = PlayerIndex.One;
            _audioPlayer.Stop();
            _audioPlayer.PlayOneShot(_switchSound1);
        }
    }

    public void PauseGame()
    {
        player1PauseVelocity = _playerOne._myRigidbody2D.velocity;
        player2PauseVelocity = _playerTwo._myRigidbody2D.velocity;
        _playerOne.DeactivateCharacter();
        _playerTwo.DeactivateCharacter();
        pauseMenu.SetActive(true);
        paused = true;
    }

    public void UnPauseGame()
    {
        _playerOne.enabled = true;
        _playerTwo.enabled = true;
        _playerOne._myRigidbody2D.isKinematic = false;
        _playerTwo._myRigidbody2D.isKinematic = false;
        pauseMenu.SetActive(false);
        paused = false;
        _playerOne._myRigidbody2D.velocity = player1PauseVelocity;
        _playerTwo._myRigidbody2D.velocity = player2PauseVelocity;
    }

    public void Cleanup()
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

    public void ResetStage()
    {
        UnPauseGame();

        Cleanup();

        _playerOne.Cleanup();
        _playerTwo.Cleanup();

        _playerOne._animationController.Cleanup();
        _playerTwo._animationController.Cleanup();

        
        if(_currentPlayer == PlayerIndex.One)
        {
            SpawnSwitcher(PlayerIndex.Two);
        }
        else
        {
            SpawnSwitcher(PlayerIndex.One);
        }

    }
}

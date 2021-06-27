using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEditor.Animations;
using UnityEditor;

public enum PlayerIndex { One, Two }
public class GameController : MonoBehaviour
{

    public Camera _mainCamera;
    public GameObject _platformContainer;
    public CharacterController _playerOne;
    public Color _playerOneBackground;
    public TextMeshPro _playerOneVictoryText;
    public CharacterController _playerTwo;
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
    
    private float _gameTimer;
    private bool _gameOngoing;
    private List<GameObject> spawnpoints = new List<GameObject>();

    private Vector3 playerScale = new Vector3(5f, 4.25f, 1f); 

    // Used to initialize the script
    void Start()
    {
        _gameTimer = _timePerActivePlayer;
        _gameOngoing = true;


        foreach(Transform child in spawnpointParent.transform)
        {
            spawnpoints.Add(child.gameObject);
        }

        SpawnSwitcher(PlayerIndex.Two);

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (_gameOngoing)
        {
            _gameTimer -= Time.deltaTime;
            if (_gameTimer <= 0)
            {
                SwitchActivePlayer();
                _gameTimer = _timePerActivePlayer;
            }
        } */

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetStage();
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
            _playerOneVictoryText.DOFade(1f, 2f);
            _playerOne.NonMeleeEndgameEffect(false);
            _playerTwo.NonMeleeEndgameEffect(true);
        }
        else if(player == PlayerIndex.Two)
        {
            _playerTwoVictoryText.DOFade(1f, 2f);
            _playerOne.NonMeleeEndgameEffect(true);
            _playerTwo.NonMeleeEndgameEffect(false);
        }
        _platformContainer.SetActive(false);
        _mainCamera.DOColor(Color.black, 1f).SetEase(Ease.Linear);
        _audioPlayer.PlayOneShot(_gameOverSound);

        _gameOngoing = false;
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

        _gameOngoing = false;
    }

    void SwitchActivePlayer()
    {

        //switcher.GetComponent<SpriteRenderer>().color = _currentPlayer == PlayerIndex.One ? Color.blue : Color.red ; <= better & cooler
        if (_currentPlayer == PlayerIndex.One)
        {
            switcher.GetComponent<SpriteRenderer>().color = Color.blue;
            arrow.GetComponent<SpriteRenderer>().color = Color.blue;
            switcher.GetComponent<Renderer>().sharedMaterial.SetColor("Glow Color", Color.blue);
            _mainCamera.backgroundColor = _playerTwoBackground;
            _currentPlayer = PlayerIndex.Two;
            _audioPlayer.Stop();
            _audioPlayer.PlayOneShot(_switchSound2);
        }
        else
        {
            switcher.GetComponent<SpriteRenderer>().color = Color.red;
            arrow.GetComponent<SpriteRenderer>().color = Color.red;
            switcher.GetComponent<Renderer>().sharedMaterial.SetColor("Glow Color", Color.red);
            _mainCamera.backgroundColor = _playerOneBackground;
            _currentPlayer = PlayerIndex.One;
            _audioPlayer.Stop();
            _audioPlayer.PlayOneShot(_switchSound1);
        }
    }

    void ResetStage()
    {
        _playerOneVictoryText.DOKill();
        _playerTwoVictoryText.DOKill();
        _playerOneVictoryText.alpha = 0;
        _playerTwoVictoryText.alpha = 0;
        _mainCamera.DOKill();
        _gameOngoing = true;

        _playerOne.enabled = true;
        _playerTwo.enabled = true;
        _playerOne._myRigidbody2D.isKinematic = false;
        _playerTwo._myRigidbody2D.isKinematic = false;

        AnimationController playerOneAnimator = _playerOne._animationController;
        AnimationController playerTwoAnimator = _playerTwo._animationController;
        playerOneAnimator.playerFade.Kill();
        playerTwoAnimator.playerFade.Kill();
        playerOneAnimator._attackTrail.SetActive(false);
        playerTwoAnimator._attackTrail.SetActive(false);
        playerOneAnimator._mySpriteRenderer.color = playerOneAnimator.startColor;
        playerTwoAnimator._mySpriteRenderer.color = playerTwoAnimator.startColor;
        playerOneAnimator._mySpriteRenderer.gameObject.transform.localScale = playerScale;
        playerTwoAnimator._mySpriteRenderer.gameObject.transform.localScale = playerScale;

        _playerOne.transform.position = playerSpawns[0].transform.position;
        _playerTwo.transform.position = playerSpawns[1].transform.position;

        _platformContainer.SetActive(true);
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

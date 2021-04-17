using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public enum PlayerIndex { One, Two }

    public Camera _mainCamera;
    public GameObject _platformContainer;
    public CharacterController _playerOne;
    public Color _playerOneBackground;
    public CharacterController _playerTwo;
    public Color _playerTwoBackground;
    public PlayerIndex _currentPlayer = PlayerIndex.One;
    public float _timePerActivePlayer = 2f; // In seconds
    
    private float _gameTimer;
    private bool _gameOngoing;

    // Used to initialize the script
    void Start()
    {
        _gameTimer = _timePerActivePlayer;
        _gameOngoing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameOngoing)
        {
            _gameTimer -= Time.deltaTime;
            if (_gameTimer <= 0)
            {
                SwitchActivePlayer();
                _gameTimer = _timePerActivePlayer;
            }
        }
    }

    public void OnPlayersTouched()
    {
        _gameOngoing = false;
        if (_currentPlayer == PlayerIndex.One)
        {
            _playerTwo.ShowEndgameEffect(winner: false, _playerOne.transform.position);
            _playerOne.ShowEndgameEffect(winner: true, _playerTwo.transform.position);
        }
        else
        {
            _playerOne.ShowEndgameEffect(winner: false, _playerTwo.transform.position);
            _playerTwo.ShowEndgameEffect(winner: true, _playerOne.transform.position);
        }
        _platformContainer.SetActive(false);
        _mainCamera.DOColor(Color.black, 1f).SetEase(Ease.Linear);
    }

    void SwitchActivePlayer()
    {
        if (_currentPlayer == PlayerIndex.One)
        {
            _mainCamera.backgroundColor = _playerTwoBackground;
            _currentPlayer = PlayerIndex.Two;
        }
        else
        {
            _mainCamera.backgroundColor = _playerOneBackground;
            _currentPlayer = PlayerIndex.One;
        }
    }
}

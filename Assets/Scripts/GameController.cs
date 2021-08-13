using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEditor.Animations;
using UnityEditor;

public enum PlayerIndex { One, Two }

public class GameController : GameControllerBase
{

    public AudioClip _switchSound1;
    public AudioClip _switchSound2;
    public PlayerIndex _currentPlayer = PlayerIndex.One;

    public GameObject spawnpointParent; //Gameobject spawnpoints are children of
    private GameObject oldPos;
    public GameObject switcher;
    public GameObject arrow;
    
    private List<GameObject> spawnpoints = new List<GameObject>();

    // Used to initialize the script
    protected override void Start()
    {
        base.Start();

        foreach(Transform child in spawnpointParent.transform)
        {
            spawnpoints.Add(child.gameObject);
        }

        SpawnSwitcher(PlayerIndex.Two);

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

    Color ReduceToHue(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return Color.HSVToRGB(H, 1f, 1f);
    }

    void SwitchActivePlayer()
    {

        //switcher.GetComponent<SpriteRenderer>().color = _currentPlayer == PlayerIndex.One ? Color.blue : Color.red ; <= better & cooler
        if (_currentPlayer == PlayerIndex.One)
        {
            switcher.GetComponent<SpriteRenderer>().color = ReduceToHue(ColorManager.player1Color);
            arrow.GetComponent<SpriteRenderer>().color = ColorManager.player1Color;
            switcher.GetComponent<Renderer>().sharedMaterial.SetColor("_GlowColor", ReduceToHue(ColorManager.player1Color));
            switcher.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", ReduceToHue(ColorManager.player1Color));
            _mainCamera.backgroundColor = _playerTwoBackground;
            _currentPlayer = PlayerIndex.Two;
            _audioPlayer.Stop();
            _audioPlayer.PlayOneShot(_switchSound2);
        }
        else
        {
            switcher.GetComponent<SpriteRenderer>().color = ReduceToHue(ColorManager.player2Color);
            arrow.GetComponent<SpriteRenderer>().color = ColorManager.player2Color;
            switcher.GetComponent<Renderer>().sharedMaterial.SetColor("_GlowColor", ReduceToHue(ColorManager.player2Color));
            switcher.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", ReduceToHue(ColorManager.player2Color));
            _mainCamera.backgroundColor = _playerOneBackground;
            _currentPlayer = PlayerIndex.One;
            _audioPlayer.Stop();
            _audioPlayer.PlayOneShot(_switchSound1);
        }
    }

    public override void ResetStage()
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

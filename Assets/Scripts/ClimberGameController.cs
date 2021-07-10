using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ClimberGameController : MonoBehaviour, IGameController
{
    public List<GameObject> sets1_1;
    public List<GameObject> sets1_2;
    public List<GameObject> sets1_3;
    public List<GameObject> sets1_4;
    public List<GameObject> sets2_1;
    public List<GameObject> sets2_2;
    public List<GameObject> sets2_3;
    public List<GameObject> sets2_4;
    public List<GameObject> sets3_1;
    public List<GameObject> sets3_2;
    public List<GameObject> sets3_3;
    public List<GameObject> sets3_4;
    public List<GameObject> sets4_1;
    public List<GameObject> sets4_2;
    public List<GameObject> sets4_3;
    public List<GameObject> sets4_4;

    public List<GameObject>[,] allSets = new List<GameObject>[4, 4];

    public float distanceBetween = 12f;
    
    public float player1x = 6f;
    public float player2x = -6f;
    public float currentDistance = 10f;
    public int startingPos = 1;
    private int currentPos = 1;

    public float loadDistance = 30f;
    private float highestPoint = 0f;

    [SerializeField]
    private CharacterController player1;
    public CharacterController _playerOne
    {
        get { return player1; }
        set { player1 = value; }
    }
    [SerializeField]
    private CharacterController player2;
    public CharacterController _playerTwo
    {
        get { return player2; }
        set { player2 = value; }
    }
    public GameObject deathwall;
    public float wallDelay = 20f;

    public TextMeshPro _playerOneVictoryText;
    public TextMeshPro _playerTwoVictoryText;
    public GameObject _platformContainer;
    public AudioSource _audioPlayer;
    public AudioClip _switchSound1;
    public AudioClip _switchSound2;
    public AudioClip _gameOverSound;

    private void Start()
    {
        allSets[0, 0] = sets1_1;
        allSets[0, 1] = sets1_2;
        allSets[0, 2] = sets1_3;
        allSets[0, 3] = sets1_4;
        allSets[1, 0] = sets2_1;
        allSets[1, 1] = sets2_2;
        allSets[1, 2] = sets2_3;
        allSets[1, 3] = sets2_4;
        allSets[2, 0] = sets3_1;
        allSets[2, 1] = sets3_2;
        allSets[2, 2] = sets3_3;
        allSets[2, 3] = sets3_4;
        allSets[3, 0] = sets4_1;
        allSets[3, 1] = sets4_2;
        allSets[3, 2] = sets4_3;
        allSets[3, 3] = sets4_4;

        _playerOne._animationController._mySpriteRenderer.color = ColorManager.player1Color;
        _playerTwo._animationController._mySpriteRenderer.color = ColorManager.player2Color;

    }

    public void UpdateOnCameraPos()
    {
        if (Camera.main.transform.position.y + loadDistance > currentDistance)
        {
            loadNextChunk();
        }
        float higherPlayer = Mathf.Max(_playerOne.transform.position.y, _playerTwo.transform.position.y);
        highestPoint = Mathf.Max(highestPoint, higherPlayer);

        deathwall.transform.position = new Vector3(deathwall.transform.position.x, highestPoint - wallDelay, deathwall.transform.position.z);

    }
    public int[] generatePath(int pos)
    {
        int rand = Random.Range(1, 5);
        int coinFlip = Random.Range(1, 3);
        int nextPos = pos;

        /* Alternate setup that starts new path adjacent to where the last one ended, requires different design for the chunks
        if(pos == 1)
        {
            nextPos = 2;
        }
        else if (pos == 2)
        {
            if(coinFlip == 1)
            {
                nextPos = 1;
            }
            else
            {
                nextPos = 3;
            }
        }
        else if (pos == 3)
        {
            if (coinFlip == 1)
            {
                nextPos = 2;
            }
            else
            {
                nextPos = 4;
            }
        }
        else
        {
            nextPos = 3;
        } */ 

        return new int[] { nextPos, rand };
    }
    public void loadNextChunk()
    {
        float y = currentDistance + distanceBetween;
        int[] path = generatePath(currentPos);
        List<GameObject> set = allSets[path[0] - 1, path[1] - 1];
        int randList = Random.Range(0, set.Count);
        GameObject chunk = set[randList];
        Instantiate(chunk, new Vector3(player1x, y), Quaternion.identity, _platformContainer.transform);
        GameObject player2Tower = Instantiate(chunk, new Vector3(player2x, y), Quaternion.identity, _platformContainer.transform);
        player2Tower.transform.localScale = new Vector3(-player2Tower.transform.localScale.x, player2Tower.transform.localScale.y, player2Tower.transform.localScale.z);
        currentPos = path[1];
        currentDistance+=distanceBetween;
    }

    private void Update()
    {
        UpdateOnCameraPos();
        if (Input.GetKeyDown(KeyCode.G))
        {
            loadNextChunk();
        }
    }

    public void KillPlayer(PlayerIndex player)
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
        Camera.main.DOColor(Color.black, 1f).SetEase(Ease.Linear);
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
        Camera.main.DOColor(Color.black, 1f).SetEase(Ease.Linear);
        _audioPlayer.PlayOneShot(_gameOverSound);
    }
}

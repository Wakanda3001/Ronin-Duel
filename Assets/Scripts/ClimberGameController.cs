using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class ClimberGameController : GameControllerBase
{

    [Serializable]
    public class Set
    {
        public GameObject _gameObject;
        public int start;
        public int end;
    }
    public List<Set> sets;


    [SerializeField]
    private GameObject generatedPlatforms;

    public float distanceBetween = 12f;
    
    public float player1x = 6f;
    public float player2x = -6f;
    public float currentDistance = -2f;
    private int stackHeight = 0;
    public int powerupFrequency = 3;
    public int startingPos = 1;
    private int currentPos = 1;

    public float loadDistance = 30f;
    private float highestPoint = 0f;

    public float midline = 0f;

    public GameObject deathwall;
    public float wallDelay = 20f;
    public float wallSmoothing = 1f;

    public GameObject powerup;
    public GameObject player1Background;
    public GameObject player2Background;
    public float powerupLength = 5f;
    bool isPlayerPowered = false;
    PlayerIndex poweredPlayer = PlayerIndex.One;


    public void UpdateOnCameraPos()
    {
        if (Camera.main.transform.position.y + loadDistance > currentDistance)
        {
            loadNextChunk();
        }
        float higherPlayer = Mathf.Max(_playerOne.transform.position.y, _playerTwo.transform.position.y);
        highestPoint = Mathf.Max(highestPoint, higherPlayer);

        float newY = Mathf.Lerp(deathwall.transform.position.y, highestPoint - wallDelay, wallSmoothing * Time.deltaTime);

        deathwall.transform.position = new Vector3(deathwall.transform.position.x, newY, deathwall.transform.position.z);

    }

    protected override void Update()
    {
        base.Update();
        UpdateOnCameraPos();
    }
    public int[] generatePath(int pos)
    {
        int rand = UnityEngine.Random.Range(1, 5);
        int coinFlip = UnityEngine.Random.Range(1, 3);
        int nextPos = pos;

        return new int[] { nextPos, rand };
    }

    Color ReduceToHue(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return Color.HSVToRGB(H, 1f, 1f);
    }

    public void loadNextChunk()
    {
        float y = currentDistance + distanceBetween;
        int[] path = generatePath(currentPos);
        List<GameObject> fittingSets = new List<GameObject>();
        foreach(Set localSet in sets)
        {
            if(localSet.start == path[0] && localSet.end == path[1])
            {
                fittingSets.Add(localSet._gameObject);
            }
        }
        int randList = UnityEngine.Random.Range(0, fittingSets.Count);
        GameObject chunk = fittingSets[randList];
        GameObject player1Tower = Instantiate(chunk, new Vector3(player1x, y), Quaternion.identity, generatedPlatforms.transform);
        GameObject player2Tower = Instantiate(chunk, new Vector3(player2x, y), Quaternion.identity, generatedPlatforms.transform);
        player2Tower.transform.localScale = new Vector3(-player2Tower.transform.localScale.x, player2Tower.transform.localScale.y, player2Tower.transform.localScale.z);
        if(stackHeight % powerupFrequency == 0)
        {
            GameObject player1Spawnpoint = powerup;
            GameObject player2Spawnpoint = powerup;
            foreach (Transform element in player1Tower.transform)
            {
                if (element.name == "spawnpoint")
                {
                    player1Spawnpoint = element.gameObject;
                }
            }
            foreach (Transform element in player2Tower.transform)
            {
                if (element.name == "spawnpoint")
                {
                    player2Spawnpoint = element.gameObject;
                }
            }
            GameObject p1Powerup = Instantiate(powerup, player2Spawnpoint.transform);
            p1Powerup.GetComponent<SpriteRenderer>().color = ReduceToHue(ColorManager.player1Color);
            p1Powerup.GetComponent<Renderer>().material.SetColor("_GlowColor", ReduceToHue(ColorManager.player1Color));
            p1Powerup.GetComponent<Renderer>().material.SetColor("_Color", ReduceToHue(ColorManager.player1Color));
            GameObject p2Powerup = Instantiate(powerup, player1Spawnpoint.transform);
            p2Powerup.GetComponent<SpriteRenderer>().color = ReduceToHue(ColorManager.player2Color);
            p2Powerup.GetComponent<Renderer>().material.SetColor("_GlowColor", ReduceToHue(ColorManager.player2Color));
            p2Powerup.GetComponent<Renderer>().material.SetColor("_Color", ReduceToHue(ColorManager.player2Color));
        }
        currentPos = path[1];
        currentDistance+=distanceBetween;
        stackHeight++;
    }
    public override void KillPlayer(PlayerIndex player)
    {
        base.KillPlayer(player);
        foreach (Transform platform in generatedPlatforms.transform)
        {
            Destroy(platform.gameObject);
        }
    }
    public override void MeleeKillPlayer(PlayerIndex player)
    {
        base.MeleeKillPlayer(player);
        foreach (Transform platform in generatedPlatforms.transform)
        {
            Destroy(platform.gameObject);
        }
    }

    public override void Cleanup()
    {
        base.Cleanup();

        foreach(Transform platform in generatedPlatforms.transform)
        {
            Destroy(platform.gameObject);
        }

        currentDistance = -2f;
        currentPos = 1;
        highestPoint = 0f;
        stackHeight = 0;
        isPlayerPowered = false;
        player1Background.GetComponent<BindToColor>().enabled = true;
        player2Background.GetComponent<BindToColor>().enabled = true;
    }

    public void OnPlayersTouched()
    {
        if (isPlayerPowered)
        {
            if(poweredPlayer == PlayerIndex.One)
            {
                MeleeKillPlayer(PlayerIndex.One);
            }
            else
            {
                MeleeKillPlayer(PlayerIndex.Two);
            }
        }
        else
        {
            if (player1.transform.position.x > midline && player2.transform.position.x > midline)
            {
                MeleeKillPlayer(PlayerIndex.One);
            }
            else if (player1.transform.position.x < midline && player2.transform.position.x < midline)
            {
                MeleeKillPlayer(PlayerIndex.Two);
            }
        }
    }

    List<Coroutine> PowerupEndCoroutines = new List<Coroutine>();
    public void PowerUpEffect(PlayerIndex player)
    {
        player1Background.GetComponent<BindToColor>().enabled = false;
        player2Background.GetComponent<BindToColor>().enabled = false;
        foreach (Coroutine coroutine in PowerupEndCoroutines)
        {
            StopCoroutine(coroutine);
        }
        isPlayerPowered = true;
        if(player == PlayerIndex.One)
        {
            player2Background.GetComponent<SpriteRenderer>().color = ColorManager.player1Background;
            player1Background.GetComponent<SpriteRenderer>().color = ColorManager.player1Background;
            poweredPlayer = PlayerIndex.One;
        }
        else
        {
            player2Background.GetComponent<SpriteRenderer>().color = ColorManager.player2Background;
            player1Background.GetComponent<SpriteRenderer>().color = ColorManager.player2Background;
            poweredPlayer = PlayerIndex.Two;
        }
        PowerupEndCoroutines.Add(StartCoroutine(EndPowerup()));
        
    }

    IEnumerator EndPowerup()
    {
        yield return new WaitForSeconds(powerupLength);
        player1Background.GetComponent<SpriteRenderer>().color = ColorManager.player1Background;
        player2Background.GetComponent<SpriteRenderer>().color = ColorManager.player2Background;
        isPlayerPowered = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationController : MonoBehaviour
{
    public SpriteRenderer _mySpriteRenderer;
    public Sprite _idleSprite;
    public Sprite _walkSprite;
    public Sprite _jumpSprite;
    public Sprite _fallSprite;
    public GameObject _attackTrail;
    
    private Tween _bobAnimation;
    public Sequence playerFade;

    private enum State { Idle, Walking, Jumping, StillLanding, MovingLanding }
    private State _state;

    private bool _landingComplete;

    public Color startColor;

    // Used to initialize the script
    void Start()
    {
        startColor = _mySpriteRenderer.color;
        EnterIdleState();
    }

    public void KillAnimations(bool winner, Vector2 trailFacing = default)
    {
        _bobAnimation.Kill();
        _mySpriteRenderer.sprite = _fallSprite;

        if (!winner)
        {
            _attackTrail.transform.right = trailFacing;
            _attackTrail.SetActive(true);
            playerFade = DOTween.Sequence();
            playerFade.Append(_mySpriteRenderer.transform.DOScaleY(0f, 3f).SetEase(Ease.InQuad));
            playerFade.Join(_mySpriteRenderer.DOColor(Color.white, 1f).SetEase(Ease.Linear));
            playerFade.Insert(1f, _mySpriteRenderer.DOColor(Color.clear, 1f).SetEase(Ease.Linear));
        }
    }

    public void NonMeleeKill(bool winner)
    {
        _bobAnimation.Kill();
        _mySpriteRenderer.sprite = _fallSprite;

        if (!winner)
        {
            playerFade = DOTween.Sequence();
            playerFade.Append(_mySpriteRenderer.transform.DOScaleY(0f, 3f).SetEase(Ease.InQuad));
            playerFade.Join(_mySpriteRenderer.DOColor(Color.white, 1f).SetEase(Ease.Linear));
            playerFade.Insert(1f, _mySpriteRenderer.DOColor(Color.clear, 1f).SetEase(Ease.Linear));
        }
    }

    // Called every frame with information from the CharacterController
    public void UpdateState(bool grounded, Vector2 velocity)
    {
        if (!grounded)
        {
            if (_state != State.Jumping) // Enter Jumping if in the air)
            {
                EnterJumpingState();
            }
            if (velocity.y >= 0)
            {
                _mySpriteRenderer.sprite = _jumpSprite;
            }
            else
            {
                _mySpriteRenderer.sprite = _fallSprite;
            }
        }
        else if (grounded && _state == State.Jumping) // Exit Jumping if on the ground, pick appropriate Landing state
        {
            if (Mathf.Abs(velocity.x) > .1f)
            {
                EnterMovingLandingState();
            }
            else
            {
                EnterStillLandingState();
            }
        }

        if (Mathf.Abs(velocity.x) > .1f) // If moving, enter Walk visuals, though details depend on whether the character is still landing
        {
            if (_state == State.Idle)
            {
                EnterWalkingState();
            }
            else if (_state == State.MovingLanding && _landingComplete)
            {
                _landingComplete = false;
                EnterWalkingState();
            }
            else if (_state == State.StillLanding)
            {
                _mySpriteRenderer.sprite = _walkSprite;
                _state = State.MovingLanding;
            }
        }
        else // No horizontal motion: enter Idle visuals
        {
            if (_state == State.Walking)
            {
                EnterIdleState();
            }
            else if (_state == State.StillLanding && _landingComplete)
            {
                _landingComplete = false;
                EnterIdleState();
            }
            else if (_state == State.MovingLanding)
            {
                _mySpriteRenderer.sprite = _idleSprite;
                _state = State.StillLanding;
            }
        }
    }

    private void EnterIdleState()
    {
        _mySpriteRenderer.sprite = _idleSprite;
        transform.localScale = Vector3.one;
        _bobAnimation.Kill();
        // These are programmatic animations called "Tweens". This one is set to scale the character down to .9, looping back and forth, with a nice rounded animation curve
        _bobAnimation = transform.DOScaleY(.9f, .75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);

        _state = State.Idle;
    }

    private void EnterWalkingState()
    {
        _mySpriteRenderer.sprite = _walkSprite;
        transform.localScale = Vector3.one;
        _bobAnimation.Kill(); // The previous animation should be stopped before we create a new one
        _bobAnimation = transform.DOScaleY(.9f, .25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);

        _state = State.Walking;
    }
    
    private void EnterJumpingState()
    {
        _mySpriteRenderer.sprite = _jumpSprite;
        transform.localScale = Vector3.one;
        _bobAnimation.Kill();
        _bobAnimation = transform.DOScaleY(.9f, .25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);

        _state = State.Jumping;
    }

    private void EnterStillLandingState()
    {
        _mySpriteRenderer.sprite = _idleSprite;
        transform.localScale = Vector3.one;
        _bobAnimation.Kill();
        _bobAnimation = transform.DOScaleY(.6f, .12f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine);
        _bobAnimation.OnComplete(() => _landingComplete = true);

        _state = State.StillLanding;
    }

    private void EnterMovingLandingState()
    {
        _mySpriteRenderer.sprite = _walkSprite;
        transform.localScale = Vector3.one;
        _bobAnimation.Kill();
        _bobAnimation = transform.DOScaleY(.6f, .12f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine);
        _bobAnimation.OnComplete(() => _landingComplete = true);

        _state = State.MovingLanding;
    }
}

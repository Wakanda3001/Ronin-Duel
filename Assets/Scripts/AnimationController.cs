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
    
    private Tween _bobAnimation;

    private enum State { Idle, Walking, Jumping, StillLanding, MovingLanding }
    private State _state;

    private bool _landingComplete;

    // Used to initialize the script
    void Start()
    {
        EnterIdleState();
    }

    public void UpdateState(bool grounded, float horizontalV)
    {
        if (!grounded && _state != State.Jumping)
        {
            EnterJumpingState();
        }
        else if (grounded && _state == State.Jumping)
        {
            if (Mathf.Abs(horizontalV) > .1f)
            {
                EnterMovingLandingState();
            }
            else
            {
                EnterStillLandingState();
            }
        }

        if (Mathf.Abs(horizontalV) > .1f)
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
        else
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
        _bobAnimation = transform.DOScaleY(.9f, .75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);

        _state = State.Idle;
    }

    private void EnterWalkingState()
    {
        _mySpriteRenderer.sprite = _walkSprite;
        transform.localScale = Vector3.one;
        _bobAnimation.Kill();
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

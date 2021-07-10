using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    public AnimationController _animationController;
    public float _runSpeed = 40f;
    public float _jumpForce = 400f;                          // Amount of instantaneous force added when the player jumps.
    public float _movementSmoothing = 0.5f;  // How much to smooth out the movement
    public bool _airControl = false;                         // Whether or not a player can steer while jumping;
    public LayerMask _whatIsGround;                          // A set of Physics Layers determining what is ground to the character
    public Transform _groundCheck;                           // A position marking where to check if the player is grounded.
    public float _topOfScreen = 10f;                         // Y-position of top of screen to spawn character if it falls
    public float _bottomOfScreen = -10f;                     // Y-position of bottom of screen to detect that the character has fallen
    public float _victoryTravelDist = 2f;
    public bool _facingRight = true;                         // For determining which way the player is currently facing.

    private const float WALL_RADIUS = 0.3f;
    private bool _sliding = false;
    private enum SlideDirection {Right, Left};
    private SlideDirection slideDirection;
    public float wallJumpDist = 10f;
    [SerializeField]
    private int lastGrounded = 0;

    private const float GROUNDED_RADIUS = .03f; // Radius of the overlap circle to determine if grounded
    private bool _grounded;            // Whether or not the player is grounded.
    public Rigidbody2D _myRigidbody2D;
    public Vector3 _velocity = Vector3.zero;

    public float speed = 10f;
    public Vector2 smoothedVelocity = Vector2.zero;
    public Vector2 xVelocity = Vector2.zero;
    public Vector2 bounceVelocity = Vector2.zero;
    public Vector2 jumpVelocity = Vector2.zero;

    public bool bouncing = false;


    // Used to initialize the script
    void Start()
    {
        _myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ShowEndgameEffect(bool winner, Vector2 otherCharacterPosition)
    {
        Vector2 toOtherCharacter = (otherCharacterPosition - (Vector2)transform.position).normalized;
        if (winner)
        {
            transform.DOMove((Vector2)transform.position + toOtherCharacter * _victoryTravelDist, .2f);
            _animationController.KillAnimations(true);
        }
        else
        {
            _animationController.KillAnimations(false, -toOtherCharacter);
        }
        DeactivateCharacter();
    }

    public void NonMeleeEndgameEffect(bool winner)
    {
        _animationController.NonMeleeKill(winner);
        DeactivateCharacter();
    }

    public void DeactivateCharacter()
    {
        _myRigidbody2D.velocity = Vector2.zero;
        _myRigidbody2D.isKinematic = true;
        enabled = false;
    }

    /// <summary>
    /// Must be called in FixedUpdate()
    /// </summary>
    public void Move(float move, bool jump)
    {
        if (enabled)
        {
            CheckIfGrounded();
            ApplyVerticalMovement(jump);
            ApplyHorizontalMovement(move * _runSpeed);
            _animationController.UpdateState(_grounded, _myRigidbody2D.velocity);

            //_myRigidbody2D.velocity = new Vector2(smoothedVelocity.x, _myRigidbody2D.velocity.y) + bounceVelocity;
            if (bounceVelocity != Vector2.zero)
            {
                _myRigidbody2D.velocity = bounceVelocity;
            }
            _myRigidbody2D.AddForce(xVelocity + jumpVelocity, ForceMode2D.Impulse);

            jumpVelocity = Vector2.zero;
            bounceVelocity = Vector2.zero;

        }
    }

    void CheckIfGrounded()
    {
        if (lastGrounded > 0)
        {
            lastGrounded--;
        }
        // The player is grounded if a circlecast at the groundcheck position hits anything designated as ground
        bool prevSliding = false;
        //prevSliding variable records which way character was facing when entering wall, allows for turning away from wall slightly before jump
        if (_sliding)
        {
            prevSliding = true;
        }
        _grounded = false;
        _sliding = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, GROUNDED_RADIUS, _whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _grounded = true;
                bouncing = false;
                lastGrounded = 5;
                break;
            }
        }
        if (!_grounded)
        {
            colliders = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - 0.2f, transform.position.y - 0.1f), new Vector2(transform.position.x + 0.2f, transform.position.y + 0.1f), _whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    _sliding = true;
                    if (!prevSliding)
                    {
                        if (_facingRight)
                        {
                            slideDirection = SlideDirection.Right;
                        }
                        else
                        {
                            slideDirection = SlideDirection.Left;
                        }
                    }
                    break;
                }
            }
        }
    }

    void ApplyHorizontalMovement(float move)
    {
        // Only control the player if grounded or airControl is turned on
        if (_grounded || _airControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, 0);
            // And then smoothing it out and applying it to the character
            if (!bouncing)
            {
                smoothedVelocity = Vector3.SmoothDamp(_myRigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);
                xVelocity = new Vector2(-1 * _myRigidbody2D.velocity.x + smoothedVelocity.x, 0);
            }
            else
            {
                Vector2 bounceMove = targetVelocity;
                if(_myRigidbody2D.velocity.x + targetVelocity.x < 4 && _myRigidbody2D.velocity.x + targetVelocity.x > -4)
                {
                    xVelocity = targetVelocity;
                }
                else
                {
                    xVelocity = Vector2.zero;
                }
            }


            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !_facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && _facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
    }

    void ApplyVerticalMovement(bool jump)
    {

        // If the player should jump...
        if ((_grounded || lastGrounded > 1) && jump)
        {
            // Add a vertical force to the player.
            _grounded = false;
            jumpVelocity = new Vector2(0f, 15f); //I changed the AddForce because it could get ridiculous with wall jumps. Could be worked around, but for now rb.velocity works fine.
            //_myRigidbody2D.AddForce(new Vector2(0f, 800f));
        }

        else if (_sliding && jump)
        {
            int slideNum = 1;
            if (slideDirection == SlideDirection.Right)
            {
                slideNum = -1;
            }
            _myRigidbody2D.velocity = new Vector2(wallJumpDist * slideNum, 15f);
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }
    
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCD = 0.25f; 
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;
    
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;
    private bool canMove = true;

    public bool infrontOfInteractable = false;
    public GameObject interactable; 

    protected override void Awake() 
    { 
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        playerControls.Movement.Interact.performed += _ => Interact(); // remove later when functioning system implemented?

        startingMoveSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    public void SetCanMove(bool bCanMove)
    {
        canMove = bCanMove;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls?.Disable(); // ? nach playerControls macht das es nur ausgeführt wird wenn es die Instance gibt, sonst wird es übersprungen :)
    }

    // good for player inputs
    private void Update()
    {
        PlayerInput();
    }

    // good for physics
    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider; 
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>(); 

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead || !canMove )
        {
            return;
        }
        
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if(mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
            facingLeft = false; 
        }
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true; 
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed; 
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    // remove or adjust when functioning system implemented
    private void Interact()
    {
        if (infrontOfInteractable && interactable)
        {
            interactable.GetComponent<Interactable>().Interact();
        }
    }
}

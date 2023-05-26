using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmallMarioScript : MonoBehaviour
{
    private Animator smallMarioAnimator;
    private Rigidbody2D myRigidbody2D;
    private GameObject mainCamera;
    private Vector3 startPosition;
    private Vector3 cameraStartPosition;

    private bool right = false;
    private bool left = false;
    private bool jump = false;
    private bool down = false;
    private bool dieFlag = false;
    private bool onSomthingFleg = false;
    private bool firstUpPlace = true;
    private bool canDieFlag = true;

    private float curTime = 1;
    private float saveGravity;
    public float marioSpeed = MARIO_DEF_SPEED;
    private float TouchTime;

    // Titles
    private GameObject livesTitle;
    private string livesString;
    private int curLives;
    [SerializeField] private ManegeTitlesScript titles;

    // Kill Mushroom
    private MushroomEnemyScript mushroomEnemy;

    //Jump
    private int onePress = 0;
    private bool isCloud = false;
    private float jumpVol = JUMP_VOL;
    public float normalJump = NORMAL_JUMP;
    public float cloudJump = CLOUD_JUMP;

    //GroundCheck
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool isTouchingGround;

    //BigMario

    #region BigMario

    [Header("Big Mario")] [SerializeField] private Animator rendererAnimator;
    private Animator BigMarioAnimator;
    private SpriteRenderer BigMarioSprite;
    [SerializeField] private SpriteRenderer smallSprite;
    private CapsuleCollider2D CapCollider;
    private bool bigMarioFlag = false;

    private float bigMarioOffSet = BIG_MARIO_OFF_SET;
    private float bigMarioXCollider = BIG_MARIO_X_COLLIDER;
    private float bigMarioYCollider = BIG_MARIO_Y_COLLIDER;

    private float normalOffSet = NORMAL_OFF_SET;
    private float normalMarioXCollider = NORMAL_MARIO_X_COLLIDER;
    private float normalMarioYCollider = NORMAL_MARIO_Y_COLLIDER;

    #endregion

    // FireHit
    private float lastFireHit = 0;

    //Audio clips
    private AudioSource marioAudioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip growSound;
    [SerializeField] private AudioClip dieSound;

    // Luigi
    private bool isLuigi = false;
    [Header("Luigi")] [SerializeField] private Animator luigiAnimator;
    [SerializeField] private SpriteRenderer luigiSpriteRenderer;
    [SerializeField] private Animator bigLuigiAnimator;
    [SerializeField] private SpriteRenderer bigLuigiSprite;
    private float luigiJump = 0;
    private float luigiVol = 0;

    // Brick explode
    [Header("Explode")] [SerializeField] private GameObject brickExplode;

    //Constants

    #region Constants

    private const float GAME_RIGHT_BOUND = 24.3f;
    private const float CAM_UP_RIGHT_BOUND = 24f;
    private const float CAM_UPPER_BOUND = 20f;
    private const int CAM_Z_POS = -15;
    private const float PLAYER_NEW_Y = 0.4f;
    private const float CAM_NEW_Y = 1.2f;
    private const float JUMP_GRAVITY = 0.4f;
    private const float EPSILON = 0.1f;
    private const float DELTA = 0.1f;
    private const float CAM_X_DIFF = 2.04f;
    private const float Y_DIFF = 1.3f;
    private const int FLIP = 180;
    private const int WAIT_TIME = 2;
    private const int UPGRADE_MUSHROOM_SCORE = 1000;
    private const int KILL_FORCE = 50;
    private const float LUIGI_JUMP = 1.5f;
    private const float LUIGI_VOL = 0.5f;
    private const float MARIO_DEF_SPEED = 1.2f;
    private const float JUMP_VOL = 2.6f;
    private const float NORMAL_JUMP = 2.6f;
    private const float CLOUD_JUMP = 3.1f;
    private const float BIG_MARIO_OFF_SET = 0.07f;
    private const float BIG_MARIO_X_COLLIDER = 0.13f;
    private const float BIG_MARIO_Y_COLLIDER = 0.3f;
    private const float NORMAL_OFF_SET = 0f;
    private const float NORMAL_MARIO_X_COLLIDER = 0.12f;
    private const float NORMAL_MARIO_Y_COLLIDER = 0.14f;

    #endregion

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        myRigidbody2D = GetComponent<Rigidbody2D>();
        saveGravity = myRigidbody2D.gravityScale;

        startPosition = myRigidbody2D.position;
        cameraStartPosition = mainCamera.transform.position;
        smallMarioAnimator = rendererAnimator;
        myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        CapCollider = gameObject.GetComponent<CapsuleCollider2D>();
        marioAudioSource = gameObject.GetComponent<AudioSource>();
        BigMarioAnimator = gameObject.GetComponent<Animator>();
        BigMarioSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void TurnToBigMario()
    {
        DisableAnimators();
        DisableSprites();
        luigiJump = 0;
        luigiVol = 0;
        isLuigi = false;
        smallMarioAnimator = BigMarioAnimator;
        smallMarioAnimator.enabled = true;
        BigMarioSprite.enabled = true;
        CapCollider.offset = new Vector2(0, bigMarioOffSet);
        CapCollider.size = new Vector2(bigMarioXCollider, bigMarioYCollider);

        if (!bigMarioFlag)
        {
            bigMarioFlag = true;
            smallMarioAnimator.SetTrigger("Grow");
            marioAudioSource.clip = growSound;
            marioAudioSource.Play(0);
        }
    }

    private void TurnToSmallMario()
    {
        DisableAnimators();
        DisableSprites();
        luigiJump = 0;
        luigiVol = 0;
        isLuigi = false;
        bigMarioFlag = false;
        smallMarioAnimator = rendererAnimator;
        smallMarioAnimator.enabled = true;
        smallSprite.enabled = true;
        CapCollider.offset = new Vector2(0, normalOffSet);
        CapCollider.size = new Vector2(normalMarioXCollider, normalMarioYCollider);
    }

    private void ChangeToLuigi()
    {
        isLuigi = true;
        luigiJump = LUIGI_JUMP;
        luigiVol = LUIGI_VOL;
        DisableAnimators();
        DisableSprites();
        bigMarioFlag = false;
        smallMarioAnimator = luigiAnimator;
        smallMarioAnimator.enabled = true;
        luigiSpriteRenderer.enabled = true;
        CapCollider.offset = new Vector2(0, normalOffSet);
        CapCollider.size = new Vector2(normalMarioXCollider, normalMarioYCollider);
    }

    private void TurnToBigLuigi()
    {
        DisableAnimators();
        DisableSprites();
        luigiJump = LUIGI_JUMP;
        luigiVol = LUIGI_VOL;
        smallMarioAnimator = bigLuigiAnimator;
        smallMarioAnimator.enabled = true;
        bigLuigiSprite.enabled = true;
        CapCollider.offset = new Vector2(0, bigMarioOffSet);
        CapCollider.size = new Vector2(bigMarioXCollider, bigMarioYCollider);
        isLuigi = true;

        if (!bigMarioFlag)
        {
            bigMarioFlag = true;
            smallMarioAnimator.SetTrigger("Grow");
            marioAudioSource.clip = growSound;
            marioAudioSource.Play(0);
        }
    }

    private void DisableAnimators()
    {
        luigiAnimator.enabled = false;
        bigLuigiAnimator.enabled = false;
        smallMarioAnimator.enabled = false;
        BigMarioAnimator.enabled = false;
    }

    private void DisableSprites()
    {
        luigiSpriteRenderer.enabled = false;
        bigLuigiSprite.enabled = false;
        BigMarioSprite.enabled = false;
        smallSprite.enabled = false;
    }

    public bool IsLuigi()
    {
        return isLuigi;
    }

    public bool IsBig()
    {
        return bigMarioFlag;
    }

    public void MakeSmallPublic()
    {
        if (isLuigi)
        {
            ChangeToLuigi();
        }
        else
        {
            TurnToSmallMario();
        }
    }

    private void ChangeToMario()
    {
        DisableAnimators();
        DisableSprites();
        isLuigi = false;
        luigiJump = 0;
        TurnToSmallMario();
    }

    private IEnumerator FlagWait()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        canDieFlag = true;
    }

    private void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (mainCamera.transform.position.x < transform.position.x && transform.position.x < GAME_RIGHT_BOUND)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y,
                mainCamera.transform.position.z);
        }

        if (dieFlag == false)
        {
            left = Input.GetKey("left") || Input.GetKey("a");
            right = Input.GetKey("right") || Input.GetKey("d");
            jump = Input.GetKey("up") || Input.GetKey("w") || Input.GetKey("space");
            down = Input.GetKey("down") || Input.GetKey("s");
        }

        // handle jump time
        if (Input.GetKey("up") || Input.GetKey("w") || Input.GetKey("space"))
        {
            myRigidbody2D.gravityScale = JUMP_GRAVITY;
        }
        else
        {
            myRigidbody2D.gravityScale = saveGravity;
        }

        // Handle right left 
        if (Input.GetKey("right") || Input.GetKey("left"))
        {
            if (curTime < marioSpeed)
            {
                curTime += Time.deltaTime;
            }
        }
        else
        {
            curTime = marioSpeed;
        }

        // Walk animation
        if (Mathf.Abs(myRigidbody2D.velocity.x) > EPSILON && Mathf.Abs(myRigidbody2D.velocity.y) < EPSILON)
        {
            smallMarioAnimator.SetBool("Walk", true);
        }
        else
        {
            smallMarioAnimator.SetBool("Walk", false);
        }

        if (Input.GetKeyDown("up") || Input.GetKeyDown("space") || Input.GetKeyDown("w"))
        {
            onePress = 1;
        }

        myRigidbody2D.transform.rotation = new Quaternion(0, myRigidbody2D.transform.rotation.y, 0, 0);

        //Luigi
        if (Input.GetKeyDown("c") && !dieFlag)
        {
            if (isLuigi)
            {
                if (bigMarioFlag)
                {
                    TurnToBigMario();
                }
                else
                {
                    ChangeToMario();
                }
            }
            else
            {
                if (bigMarioFlag)
                {
                    TurnToBigLuigi();
                }
                else
                {
                    ChangeToLuigi();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.x < mainCamera.transform.position.x - CAM_X_DIFF)
        {
            if (mainCamera.transform.position.x < GAME_RIGHT_BOUND)
            {
                myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
                myRigidbody2D.transform.position = new Vector2(mainCamera.transform.position.x - CAM_X_DIFF,
                    myRigidbody2D.transform.position.y);
            }
        }

        if (mainCamera.transform.position.x > CAM_UP_RIGHT_BOUND)
        {
            if (myRigidbody2D.transform.position.y > mainCamera.transform.position.y &
                myRigidbody2D.transform.position.y < CAM_UPPER_BOUND)
            {
                mainCamera.transform.position =
                    new Vector3(mainCamera.transform.position.x, myRigidbody2D.transform.position.y,
                        mainCamera.transform.position.z);
            }

            // set new respawn location
            if (firstUpPlace)
            {
                firstUpPlace = false;
                startPosition = new Vector3(GAME_RIGHT_BOUND, PLAYER_NEW_Y, 0);
                cameraStartPosition = new Vector3(GAME_RIGHT_BOUND, CAM_NEW_Y, CAM_Z_POS);
            }

            if (myRigidbody2D.transform.position.y < mainCamera.transform.position.y - Y_DIFF && !dieFlag)
            {
                MarioDie();
            }
        }

        if (left && transform.position.x > mainCamera.transform.position.x - CAM_X_DIFF && !down)
        {
            myRigidbody2D.velocity = new Vector2(-1 * curTime - luigiVol, myRigidbody2D.velocity.y);
            myRigidbody2D.transform.rotation = new Quaternion(0f, -FLIP, 0f, 0);
        }

        if (right && !down)
        {
            myRigidbody2D.velocity = new Vector2(curTime + +luigiVol, myRigidbody2D.velocity.y);
            myRigidbody2D.transform.rotation = new Quaternion(0f, 0, 0f, 0);
        }

        if (!right && !left)
        {
            myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
        }

        if (jump && onePress == 1 && isTouchingGround && !down)
        {
            marioAudioSource.clip = jumpSound;
            marioAudioSource.Play(0);
            if (isCloud)
            {
                jumpVol = cloudJump + luigiJump;
            }
            else
            {
                jumpVol = normalJump + luigiJump;
            }

            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpVol);
            smallMarioAnimator.SetBool("Walk", false);
            smallMarioAnimator.SetBool("Jump", true);
            onePress = 0;
        }

        // Animator normal state
        if (myRigidbody2D.velocity.y == 0 || (myRigidbody2D.velocity.y < DELTA && onSomthingFleg))
        {
            smallMarioAnimator.SetBool("Jump", false);
        }

        if (down)
        {
            if (bigMarioFlag)
            {
                smallMarioAnimator.SetBool("Crunch", true);
            }
        }
        else
        {
            smallMarioAnimator.SetBool("Crunch", false);
        }

        if (transform.position.y < -3 && !dieFlag)
        {
            MarioDie();
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.tag == "Cloud")
        {
            isCloud = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "MushroomEnemy")
        {
            mushroomEnemy = col.gameObject.GetComponent<MushroomEnemyScript>();
            if (myRigidbody2D.position.y > col.transform.position.y + DELTA)
            {
                if (!dieFlag)
                {
                    myRigidbody2D.AddForce(new Vector2(0, KILL_FORCE));
                    mushroomEnemy.MushroomKilledByMario();
                }
            }
            else if (!bigMarioFlag && canDieFlag)
            {
                MarioDie();
            }

            else
            {
                canDieFlag = false;
                StartCoroutine(FlagWait());
                if (isLuigi)
                {
                    ChangeToLuigi();
                }
                else
                {
                    TurnToSmallMario();
                }
            }
        }

        if (col.transform.tag == "Bird")
        {
            if (myRigidbody2D.position.y < col.transform.position.y)
            {
                if (!bigMarioFlag && canDieFlag)
                {
                    MarioDie();
                }
                else
                {
                    canDieFlag = false;
                    StartCoroutine(FlagWait());
                    if (isLuigi)
                    {
                        ChangeToLuigi();
                    }
                    else
                    {
                        TurnToSmallMario();
                    }
                }
            }
            else
            {
                myRigidbody2D.AddForce(new Vector2(0, KILL_FORCE));
            }
        }

        if (col.transform.CompareTag("Dragon"))
        {
            MarioDie();
        }

        if (col.transform.CompareTag("UpgradeMushroom"))
        {
            if (isLuigi)
            {
                TurnToBigLuigi();
            }
            else
            {
                TurnToBigMario();
            }

            Destroy(col.gameObject);
            titles.AddScore(UPGRADE_MUSHROOM_SCORE);
        }

        if (col.transform.CompareTag("NormalBrick") && bigMarioFlag)
        {
            if (transform.position.y + DELTA + DELTA < col.transform.position.y)
            {
                Instantiate(brickExplode, new Vector3(col.transform.position.x, col.transform.position.y, -1),
                    Quaternion.identity);
                Destroy(col.gameObject);
            }
        }

        if (col.transform.position.y < myRigidbody2D.position.y)
        {
            onSomthingFleg = true;
        }

        if (col.transform.CompareTag("Fire"))
        {
            lastFireHit = Time.time;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        onSomthingFleg = false;
        isCloud = false;
    }

    public float LastFireHit()
    {
        return lastFireHit;
    }

    public void MarioDie()
    {
        if (!dieFlag)
        {
            //die sound
            marioAudioSource.clip = dieSound;
            marioAudioSource.Play(0);

            dieFlag = true;
            myRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX; // No movement if die
            StartCoroutine(MarioWaitTillDie());

            livesTitle = GameObject.FindGameObjectsWithTag("LivesTitle")[0];
            livesString = livesTitle.GetComponent<TextMeshProUGUI>().text.Split(" ")[1];
            curLives = int.Parse(livesString);

            curLives -= 1;
            livesString = curLives.ToString();
            livesTitle.GetComponent<TextMeshProUGUI>().text = "LIVES \n" + livesString;
            titles.PauseTime();
            if (curLives == 0)
            {
                StartCoroutine(OpenGameOver());
            }
        }
    }

    private IEnumerator MarioWaitTillDie()
    {
        smallMarioAnimator.SetBool("Die", true);
        yield return new WaitForSeconds(WAIT_TIME);
        smallMarioAnimator.SetBool("Die", false);
        mainCamera.transform.position = cameraStartPosition;
        myRigidbody2D.transform.position = startPosition;
        dieFlag = false;
        // No movement if die, put movement back.
        myRigidbody2D.constraints = RigidbodyConstraints2D.None;
        myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Change size
        if (bigMarioFlag)
        {
            if (isLuigi)
            {
                ChangeToLuigi();
            }
            else
            {
                ChangeToMario();
            }
        }

        titles.ResetTime();
    }

    private IEnumerator OpenGameOver()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("EndGameDead");
        SceneManager.UnloadSceneAsync("Game");
    }
}
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure; //Custom Xinput Wrapper because Unity's sucks
using UnityEngine;

public class MainPlayerController : MonoBehaviour {

    PlayerIndex playerIndex;
    bool playerIndexSet = false;
    public GamePadState state;
    public GamePadState prevState;
    public float speed;
    private Rigidbody rb;
    public string horizontalMove;
    public string verticalMove;
    public string action;
    public int playerNum;
    public bool isActive;
    public new SkinnedMeshRenderer renderer;

    public bool[] isClose;

    public LineRenderer lightning;


    private float timeLimit = 3.0f;

    Coroutine coFlash;

    public bool slipping;

    Animator anim;

    public Vector3 startPosition;

    void Awake()
    {
        //lightning = transform.Find("SimpleLightningBoltPrefab" + playerNum).gameObject.GetComponent<LineRenderer>();
        //lightning.enabled = false;
        isClose = new bool[] { false, false, false, false };
        slipping = false;
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    void Start()
    {
        PlayerIndex testPlayerIndex = (PlayerIndex)(playerNum - 1);  //Grab Player controller;
        GamePadState testState = GamePad.GetState(testPlayerIndex);
        if (testState.IsConnected)
        {
            Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
            playerIndex = testPlayerIndex;
            playerIndexSet = true;
        }

        //isClose = new bool[] { false, false, false, false };
        rb = GetComponent<Rigidbody>();
        isActive = true;
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        
    }

    void FixedUpdate()
    {
        //anim.Play("Idle");

        if (transform.position.y < -20)
        {
            transform.position = startPosition;
        }

        // Get input movement directions
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        float xinputHorizontal = state.ThumbSticks.Left.X;
        float xinputVertical = state.ThumbSticks.Left.Y;

        // Set movement velocity
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 xinputMovement = new Vector3(xinputHorizontal, 0.0f, xinputVertical);

        
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
            anim.Play("Walk");
        }
        else
        {
            anim.Play("Idle");
        }

        //if (Input.GetButtonDown(action) && isActive)
        //{

        //    rewardPlayer(10);
        //}

        if (!isActive)
        {
            if (timeLimit < 2.9)
            {
                lightning.enabled = false;
            }
            if (timeLimit <= 0.0)
            {
                isActive = true;
                if (coFlash != null)
                {
                    StopCoroutine(coFlash);
                }
                renderer.enabled = true; // make sure renderer is on
                timeLimit = 3.0f;
            }
            else
            {
                timeLimit -= Time.deltaTime;    
            }
        }
        
        
        if (xinputHorizontal != 0 || xinputVertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(xinputMovement, Vector3.up);
            //anim.Play("Walk");
        }
        Vector3 xMove = xinputMovement.normalized * speed;
        Vector3 Move = movement.normalized * speed;

        if (xMove == Vector3.zero)
        {
            //anim.Play("Idle");
        }

        if (!slipping)
        {
            rb.velocity = new Vector3(Move.x, rb.velocity.y, Move.z);

        }

        




    }

    // Update is called once per frame
    void Update () {
        prevState = state;
        state = GamePad.GetState(playerIndex);

        if (GetComponent<MainPlayerController>().prevState.Buttons.RightShoulder == ButtonState.Released && GetComponent<MainPlayerController>().state.Buttons.RightShoulder == ButtonState.Pressed)
        {
            GetComponent<AudioSource>().Play();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ScoreBehavior.currentBlob = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ScoreBehavior.currentBlob = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ScoreBehavior.currentBlob = 3;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
            {
                //the ray collided with something, you can interact
                // with the hit object now by using hit.collider.gameObject
                rb.AddForce(Vector3.up * 75.0f, ForceMode.Impulse);
            }
            else
            {
                //nothing was below your gameObject within 10m.
            }
            if (rb.velocity.y == 0)
            {
                //rb.AddForce(Vector3.up * 50.0f, ForceMode.Impulse);
            }
        }
    }

    public void setCheckpoint()
    {

        startPosition = transform.position;

    }

    public void rewardPlayer(int rewardNumber)
    {
        int plusMinus = Random.Range(1, 6);

        for (int i = 0; i < rewardNumber + plusMinus; i++)
        {
            GameObject coin = Instantiate(Resources.Load("coin", typeof(GameObject)), transform.position + (Vector3.up * 20) + new Vector3(0, i, 0), Random.rotation) as GameObject;
        }
    }

    public void punishPlayer()
    {
        //startFlash();
        //isActive = false;

        //int plusMinus = Random.Range(1, 6);

        //if (ScoreBehavior.PlayerScores[playerNum - 1] > 7 + plusMinus)
        //{

        //    for (int i = 0; i < 7 + plusMinus; i++)
        //    {
        //        GameObject coin = Instantiate(Resources.Load("coin", typeof(GameObject)), transform.position + Vector3.up, Random.rotation) as GameObject;
        //    }
        //    ScoreBehavior.PlayerScores[playerNum - 1] = ScoreBehavior.PlayerScores[playerNum - 1] - 10;
        //}
        //else
        //{

        //    for (int i = 0; i < ScoreBehavior.PlayerScores[playerNum - 1]; i++)
        //    {
        //        GameObject coin = Instantiate(Resources.Load("coin", typeof(GameObject)), transform.position + Vector3.up, Random.rotation) as GameObject;
        //    }
        //    ScoreBehavior.PlayerScores[playerNum - 1] = 0;
        //}

        //rb.AddExplosionForce(50f, transform.position, 5.0f, 5.0f);
    }

    public void beAbsorbed()
    {
        
        punishPlayer();

        transform.Translate(Vector3.down * 10f, Space.World);
        rb.isKinematic = true;

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            isClose[other.GetComponent<MainPlayerController>().playerNum - 1] = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Player")
        {
            isClose[other.GetComponent<MainPlayerController>().playerNum - 1] = false;
        }
    }

    /**
     * startFlash() creates and instantiates a coroutine called Flash under coFlash 
     */
    public void startFlash()
    {
        coFlash = StartCoroutine(Flash(5f, 0.1f));
    }

    /**
     * Flash is a coroutine which causes the model renderer to turn on and off repeatedly to give of the
     * flashing frames look when a player is hit
     */
    IEnumerator Flash(float time, float intervalTime)
    {
        float elapsedTime = 0f;
        int index = 0;
        while (elapsedTime < time)
        {
            renderer.enabled = ((index % 2) == 0);//colors[index % 2];

            elapsedTime += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
    }
}

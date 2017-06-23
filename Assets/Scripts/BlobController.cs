using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlobController : MonoBehaviour {

    public float Distance;
    private GameObject player;
    private Vector3 target;
    private Transform[] targets;
    private float lookAtDistance;
    private float chaseRange = 10.0f;
    private float moveSpeed;
    private int rotationSpeed = 10;
    private Rigidbody rb;
    //Animator anim;
    public bool isMoving;
    public bool isKnockback;

    public Texture[] textures;

   // private EnemyHealth enemyHealth;

    Vector3 dir;
    float timeLimit = 2.2f; // 10 seconds.

    public new SkinnedMeshRenderer renderer;
    private Color[] colors = { Color.white, new Color(1.0f, 0.4f, 0.7f, 1.0f)};
    public Color chosenColor;
    public int chosenIdx; // green, yellow, blue, white, pink
    private Color[] blobColors = { new Color(0.53f, 1.0f, 0.3f, 1.0f), new Color(1.0f, 1.0f, 0.5f, 1.0f), new Color(0.3f, 0.79f, 1.0f, 1.0f), Color.white};
    Coroutine coFlash;
    float savedTime;
    float timeLeft;
    private ParticleSystem ps;

    //public Material mat;

    private Transform currentTarget;

    public bool following;
    public int blobNumber;
    private Image marker;

    void Awake()
    {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        marker = GetComponentInChildren<Image>();
        ps = GetComponent<ParticleSystem>();
        chosenIdx = Random.Range(0, blobColors.Length);
        chosenColor = blobColors[chosenIdx];
        chosenColor = Color.green;
        if (blobNumber == 2)
        {
            chosenColor = new Color(0.3F, 0.1F, 0.6F, 0.5F);
        }
        renderer.material.color = chosenColor;
        colors[0] = chosenColor;
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.FindGameObjectWithTag("ground").GetComponent<BoxCollider>());

        switch (chosenIdx)
        {
           
            default:
                moveSpeed = 5.0f;
                lookAtDistance = 10.0f;
                
                break;
        }
        //Debug.Log(colors[0]);
    }
 
     void Start () 
     {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform.position;
        following = true;

        rb = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        savedTime = Time.time;
        timeLeft = 3.0f;

       
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        //target = GameObject.Find("Player").transform;
    }
 
     void Update ()
     {

       
        if (transform.position.y < -10)
        {
            transform.position = player.GetComponent<MainPlayerController>().startPosition;
        }

        greenLogic();
        


         

     }



     // Turn to face the player.
     void lookAt()
     {
         // Rotate to look at player.
         Quaternion rotation = Quaternion.LookRotation(target - transform.position);
         rotation.z = 0;
         rotation.x = 0;

         transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*rotationSpeed);
         Distance = Vector3.Distance(target, transform.position);
        //transform.LookAt(Target); alternate way to track player replaces both lines above.
    }


    void wander()
    {
        
        if (Time.time - savedTime <= 2.2 && transform.position.y < 30)
        {
            //transform.rotation = Random.rotation;
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        else if (Time.time - savedTime >= 2.2 && Time.time - savedTime <= 5)
        {
            
        }
        else if (Time.time - savedTime > 5)
        {
            Vector3 randomLook = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            transform.rotation = Quaternion.LookRotation(randomLook.normalized, Vector3.up);
            savedTime = Time.time;
        }
    }


    private void greenLogic()
    {
        
        if (ScoreBehavior.currentBlob == blobNumber)
        {
            marker.enabled = true;
            if (Input.GetButtonDown("Fire1"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        target = hit.point;
                        following = false;
                        //cube.transform.position = targetPosition;
                    }
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                target = player.transform.position;
                following = true;
            }
        }
        else
        {
            marker.enabled = false;

        }

        //foreach (Transform target in targets)
        //{
        //    if (Vector3.Distance(target.position, transform.position) < min && target.position.y > -5)
        //    {
        //        currentTarget = target;
        //        min = Vector3.Distance(target.position, transform.position);

        //    }

        //    float angle = 30;
        //    if (Vector3.Angle(target.transform.forward, transform.position - target.position) < angle && (transform.position - target.position).magnitude < 15f)
        //    {
        //        numFacing++;
        //    }

        //}

        //if (numFacing > 1)
        //{
        //    if (transform.localScale.x > 0.5)
        //    {
        //        transform.localScale -= new Vector3(0.006F, 0.006F, 0.006F);
        //        rb.mass -= 0.1f;
        //        moveSpeed = 1.0f;
        //        renderer.material.color = colors[1];
        //    }
        //}
        //else
        //{
        //    transform.localScale += new Vector3(0.005F, 0.005F, 0.005F);
        //    rb.mass += 0.15f;
        //    moveSpeed = 4.0f;
        //    renderer.material.color = chosenColor;
        //}


        // AI begins tracking player.
        //if (Distance < lookAtDistance)
        //{
        lookAt();
        if (following)
        {
            target = player.transform.position;
            follow();
        }
        else
        {
            moveTo();
        }
        //}


        // Attack! Chase the player until/if player leaves attack range.
        

        
    }


    void follow()
    {
        if (Distance > 2.0f)
        {

            Vector3 temp = transform.forward;
            temp.y = 0;
            transform.position += temp * moveSpeed * Time.deltaTime;
            
        }
    }

    void moveTo()
    {
        if (Distance > 0.5f) { 
            Vector3 temp = transform.forward;
            temp.y = 0;
            transform.position += temp * moveSpeed * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }

    public void die()
    {

        ps.Emit(100);
        transform.position = new Vector3(0, -10, 0);
        Destroy(gameObject, 3f);

    }


}

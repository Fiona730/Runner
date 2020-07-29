using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public bool isAlive;
    public bool inGround;
    public float speed;
    public float jumpForce;
    public AudioClip jumpClip;
    public ParticleSystem[] particle = new ParticleSystem[5];
    //RunNormal, RunFast, RunSlow, Jump, Die
    public int coinCount;
    
    public Vector3 posSaved;
    public Quaternion rotSaved;
    public float gameTimeSaved;
    public int coinCountSaved;
    public GameObject[] coinList; // recover coins from the last checkpoint
    public int deadTimes;
    public float gameTime;
    public float percentage; // percentage of path passed

    Rigidbody rigid;
    Renderer render;
    Animator animator;
    PlayerHUD hud;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        render = GetComponentInChildren<Renderer>();
        animator = GetComponentInChildren<Animator>();
        hud = FindObjectOfType<PlayerHUD>();

        render.material.color = Color.yellow;
        SetParticleSystem(0);
        //particle[4].Stop();

        isAlive = false;
        gameObject.SetActive(false);
        coinCount = 0;

        deadTimes = 0;
        gameTime = 0;
        coinCountSaved = 0;
        coinList = new GameObject[0];
        posSaved = transform.position;
        rotSaved = transform.rotation;
        gameTimeSaved = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (animator)
        {
            animator.SetBool("InGround", inGround);
        }

        if (!isAlive) SetParticleSystem(-1);

        /*if (isAlive && !inGround) {
            SetParticleSystem(3);
        }*/

    }

    public void Move(float multiplier)
    {
        if (!isAlive) return;

        float angle = transform.rotation.eulerAngles.y;
        rigid.velocity = new Vector3(Mathf.Sin(2*Mathf.PI*angle/360f), 0, Mathf.Cos(2*Mathf.PI*angle/360f)) * speed * multiplier;

        //
        //rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void Jump()
    {
        if (!isAlive) return;
        float angle = transform.rotation.eulerAngles.y;
        rigid.velocity = new Vector3(Mathf.Sin(2*Mathf.PI*angle/360f), 0, Mathf.Cos(2*Mathf.PI*angle/360f)) * speed * 1.3f;
        //rigid.angularVelocity = Vector3.zero; 
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        AudioSource.PlayClipAtPoint(jumpClip, transform.position);
    }

    /*public void RayCastDown()
    {
        Ray playerRay = new Ray(transform.position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit))
        {
            //Debug.DrawLine(playerRay.origin, playerHit.point, Color.green, 10);
            //Debug.Log("1:"+playerHit.collider.gameObject.name);
            Ray secondRay= new Ray(playerHit.point, playerRay.direction);
            RaycastHit hexHit;
            if (Physics.Raycast(secondRay, out hexHit))
            {
                //Debug.DrawLine(secondRay.origin, hexHit.point, Color.red, 10);
                //Debug.Log("2:"+hexHit.collider.gameObject.name);
                
            }
        }
    }*/

    public Collider HexagonCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.3f);
        //Debug.Log(colliders.Length);
        int tmp = -1;
        float distance;
        float max_distance = 30;
        for (int i = 0; i < colliders.Length; i++)
        {
            //Debug.Log(colliders[i].gameObject.name);
            string s = colliders[i].gameObject.name.Substring(0, colliders[i].gameObject.name.Length-7);
            if (((IList)GenerateScene.hex_name).Contains(s))
            {
                distance = (colliders[i].transform.position - transform.position).magnitude;
                if(distance < max_distance)
                {
                    max_distance = distance;
                    tmp = i;
                }
                // return colliders[i];
            }
        }
        if(tmp>=0) return colliders[tmp];
        return null;
    }

    /*public Vector3 GetHexTurnPosition(Collider collider)
    {
        Transform currentHex = collider.transform;
        Vector3 dis = currentHex.position - transform.position;
        dis.y = 0;
        Vector3 turnPosition;

        if (Vector3.Dot(dis, transform.forward)>=0)
        {
            // Turn at the current hexagon
            turnPosition = currentHex.position;
            turnPosition.y = transform.position.y;
            return turnPosition;
        }

        // Vector3.Dot(dis, transform.forward)<0
        // Turn at the next hexagon
        turnPosition = currentHex.position + transform.forward * 1.1f;
        turnPosition.y = transform.position.y;
        return turnPosition;
    }*/

    public Vector3 GetHexTurnPosition(Collider collider)
    {
        Vector3 currentHex = collider.transform.position;
        Vector3 turnPosition = currentHex + transform.forward * Scene.xDistanceHex;
        turnPosition.y = transform.position.y;
        return turnPosition;
    }

    public void SetParticleSystem(int num)
    {
        if (num>=0 && num<particle.Length-1)
            particle[num].gameObject.SetActive(true);
        for (int i=0; i<particle.Length-1; i++)
        {
            if (i!=num) particle[i].gameObject.SetActive(false);
        }
    }

    public void KeepParticleSystem(int num)
    {
        for (int i=0; i<particle.Length; i++)
        {
            if (particle[i].gameObject.activeSelf)
                particle[i].gameObject.SetActive(true);
        }
    }

    public void AddCoin()
    {
        coinCount += 1;
    }

    public void recoverTransform()
    {
        GetComponentInChildren<Renderer>().enabled=true;
        SetParticleSystem(0);

        coinCount = coinCountSaved;
        transform.position = posSaved;
        transform.rotation = rotSaved;
        gameTime = gameTimeSaved;
        isAlive = true;
        for (int i=0; i<coinList.Length; i++)
        {
            coinList[i].SetActive(true);
        }
        coinList = new GameObject[0];
    }

    public void Die()
    {
        isAlive = false;
        deadTimes += 1;
        particle[4].Play();
        GetComponentInChildren<Renderer>().enabled=false;
        hud.GameEnd();
    }

    public void GameComplete()
    {
        isAlive = false;
        gameObject.SetActive(false);
        hud.GameComplete();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}

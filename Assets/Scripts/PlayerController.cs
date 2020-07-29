using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCharacter character;
    public float speedChangeTime = 1f;
    Vector3 turnPosition;
    bool waitToTurn;
    bool countJump;
    float timeJump;
    float speedMultiplier;
    float timeSpeed;
    bool turn_left, turn_right;

    public void Initialize()
    {
        waitToTurn = false;
        countJump = false;
        timeJump = 0;
        speedMultiplier = 1;
        timeSpeed = 0;
    }

    void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
        Initialize();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!character.isAlive)
        {
            return;
        }

        character.gameTime += Time.deltaTime;

        Collider collider = character.HexagonCheck();
        if (collider==null) character.inGround = false;
        else character.inGround = true;

        if (collider==null && character.transform.position.y<0.5 && !countJump)
            //Debug.Log("Get null in collision detection");
            character.Die();

        // Turning Logic
        turn_left = Input.GetKeyDown(KeyCode.LeftArrow);
        turn_right = Input.GetKeyDown(KeyCode.RightArrow);
        if (turn_left||turn_right)
        {
            if(character.inGround)
            {
                float angle = turn_left? 300:60;
                character.transform.rotation = Quaternion.Euler(0, angle, 0) * character.transform.rotation;
                turnPosition = character.GetHexTurnPosition(collider);
                waitToTurn = true;
            }
            //else Debug.Log("Get null in collision detection");
        }

        // Count Jump
        if (countJump)
        {
            timeJump += Time.deltaTime;
            if (timeJump >= 1f)
            {
                timeJump = 0;
                countJump = false;
                // character.SetParticleSystem(0);
            }
        }

        if (timeSpeed>0)
        {
            timeSpeed += Time.deltaTime;
            if (timeSpeed >= speedChangeTime)
            {
                timeSpeed = 0;
                speedMultiplier = 1;
                character.SetParticleSystem(0);
            }
        }
        

        if(!waitToTurn)
        {
            if(character.inGround) //如果转向，先不考虑脚下石块的功能
            {
                //Debug.Log("Reach Here "+collider.gameObject.name);
                switch(collider.gameObject.name)
                {
                    case "grass(Clone)":
                    case "stone(Clone)":
                    case "sand(Clone)":
                    case "water(Clone)":
                        character.Move(1*speedMultiplier);
                        //character.SetParticleSystem(0);
                        break;
                    case "speedUp(Clone)":
                        character.Move(2);
                        speedMultiplier = 2f;
                        timeSpeed = Time.deltaTime;
                        character.SetParticleSystem(1);
                        break;
                    case "speedDown(Clone)":
                        character.Move(0.5f);
                        speedMultiplier = 0.5f;
                        timeSpeed = Time.deltaTime;
                        character.SetParticleSystem(2);
                        break;
                    case "jump(Clone)":
                        if(!countJump)
                        {
                            character.Jump();
                            countJump = true;
                            speedMultiplier = 1;
                            timeSpeed = 0;
                            character.SetParticleSystem(3);
                        }
                        break;
                    case "checkpoint(Clone)":
                        //character.particle[4].Play();
                        character.Move(1);
                        character.SetParticleSystem(0);
                        break;
                    case "start(Clone)":
                        character.Move(1);
                        character.SetParticleSystem(0);
                        break;
                    default:
                        break;
                }
            }
        }
        else {
            character.transform.position = Vector3.MoveTowards(character.transform.position, turnPosition, character.speed*Time.deltaTime);
            if(character.transform.position == turnPosition) waitToTurn = false;
        }
        //Debug.Log("Pos1: "+character.transform.position);
        //Debug.Log("Pos2: "+turnPosition);
    }

}

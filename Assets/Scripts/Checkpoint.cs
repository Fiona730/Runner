using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{    
    public int num; // serial number
    public int total;
    public ParticleSystem particle;

    public AudioClip checkPointSound;

    /*private void Awake()
    {
        Scene.GetCheckpointPosValue();
        for (int i=0; i<Scene.checkpointPos.Length; i++)
        {
            if (Mathf.Abs(transform.position.x-Scene.checkpointPosAxis[i].axis_x)<0.0001 && Mathf.Abs(transform.position.z-Scene.checkpointPosAxis[i].axis_z)<0.0001)
            {
                num = i;
                break;
            }
        }
    }*/

    void Awake()
    {
        //particle = (GameObject)Resources.Load("ParticlePrefabs/FX_Dust_Prefab_01");
        particle.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Reach Checkpoint: "+num);
        var player = other.GetComponent<PlayerCharacter>();

        if (player)
        {
            player.coinCountSaved = player.coinCount;
            player.posSaved = player.transform.position + Scene.xDistanceHex*player.transform.forward*2;
            player.rotSaved = player.transform.rotation;
            player.gameTimeSaved = player.gameTime;
            player.coinList = new GameObject[0];
            player.percentage = (num+1)/(float)(total+1);
            AudioSource.PlayClipAtPoint(checkPointSound, transform.position);
        }
    }
}

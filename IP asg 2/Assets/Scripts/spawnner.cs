/**
Author: Bryan Gregory Soh

Name of Class: spawner of the note

Description of Class: This class helps spawn the note infronnt of the player everytime a note is taken out.

Date Created: 17/02/2022
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnner : MonoBehaviour
{

    public GameObject NotePrefeb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void intCheck()
    {
        //spawns the prefeb
        Instantiate(NotePrefeb,transform.position,transform.rotation);
    }
}

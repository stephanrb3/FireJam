﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    [SerializeField] public float m_MaxSpeed;                    // The fastest the player can travel.
	[SerializeField] public float m_Accel;
	[SerializeField] public float m_Decel;

    public float pushup;

    public GameManager GM;

    public float bulletSpeed;
    public float bulletFreq;
    private float bulletRate;
    public bool isWater;
    public Waterspurt spurt;
    MapTile[,] grid;

    private float lastShotTime;

    private Vector2 lastMove;

    private void Start() {
        grid = GameObject.Find("Map").GetComponent<Map>().grid;
        lastShotTime = Time.time;
        lastMove = new Vector2(0,1);
        bulletRate = 1f/bulletFreq;
    }

    void FixedUpdate(){
        Shoot();
    }


    public void MoveW(Vector2 move){
        if (isWater){
            transform.rotation = Quaternion.LookRotation(Vector3.forward, move);
            transform.Translate(move * m_MaxSpeed * Time.deltaTime, Space.World);
            lastMove = move;
            Constrain();
        }
    }


    public void MoveF(Vector2 move) {
        if(!isWater) {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, move);
            transform.Translate(move * m_MaxSpeed * Time.deltaTime, Space.World);
            Constrain();
            FireSpawner fspawn = this.GetComponent<FireSpawner>();
            fspawn.CmdFireSpew();
        }
    }

    void Constrain(){
        if(transform.position.x <= -0.4f){
            transform.SetPositionAndRotation( new Vector3(-0.4f, transform.position.y, transform.position.z), transform.rotation);
        }
        if(transform.position.y <= -0.4f){
            transform.SetPositionAndRotation( new Vector3(transform.position.x, -0.4f, transform.position.z), transform.rotation);
        }
        if(transform.position.x >=15.4f){
            transform.SetPositionAndRotation( new Vector3(15.4f, transform.position.y, transform.position.z), transform.rotation);
        }
        if(transform.position.y >=15.4f){
            transform.SetPositionAndRotation( new Vector3(transform.position.x, 15.4f, transform.position.z), transform.rotation);
        }
    }

    public void Shoot(){
        float currTime = Time.time;
        if (currTime-lastShotTime> bulletRate && isWater){
            Waterspurt sput = Instantiate(spurt);
            sput.Init(transform.position, lastMove, grid);
            sput.transform.position = sput.transform.position + pushup * new Vector3( lastMove.x, lastMove.y, 0);
            lastShotTime = currTime;
        }
    }

}	

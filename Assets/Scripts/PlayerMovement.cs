﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float gravityForce;
    [SerializeField] float jumpForce;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;

    PlayerAttack playerAttack;
    PlayerStats playerStat;
    private int playerIndex;

    private bool isGrouded;


    Vector3 direction = new Vector3(0, 0, 0);

    Planet planet = null;

    Rigidbody rigidBody;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerStat = GetComponent<PlayerStats>();
        planet = FindObjectOfType<Planet>();
        isGrouded = true;
        playerIndex = playerStat.playerIndex;
    }

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("HorizontalP" + playerIndex.ToString());
        float verticalMovement = Input.GetAxis("VerticalP" + playerIndex.ToString());
        Vector3 direction = (planet.transform.position - transform.position).normalized;

        transform.rotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation; //mantiene il player orientato verso la superfice

        rigidBody.AddForce(direction * gravityForce, ForceMode.Acceleration);

        rigidBody.AddTorque(transform.up * horizontalMovement * rotationSpeed, ForceMode.VelocityChange);

        rigidBody.AddForce(transform.forward * verticalMovement * movementSpeed, ForceMode.Impulse);

    }

    private void Update()
    {
        if (Input.GetButtonDown("JumpP" + playerIndex) && isGrouded)
        {
            rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrouded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Planet>())
        {
            isGrouded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gear"))
        {
            onPicked(other);
        }
        if (other.CompareTag("Sniper"))
        {
            onPicked(other);
        }
        if (other.CompareTag("Shotgun"))
        {
            onPicked(other);
        }
        if (other.CompareTag("SMG"))
        {
            onPicked(other);
        }
    }
    void onPicked(Collider other)
    {
        
        if (other.CompareTag("Gear"))
        {
            playerStat.activeGear(true);
        }
        else
        {
            int i = 0;
            if (other.CompareTag("Sniper"))
            {
                i = 3;
                Debug.Log("Sniper");
                playerAttack.currentBullet = playerAttack.bullets[3];
            }
            if (other.CompareTag("Shotgun"))
            {
                i = 1;
                Debug.Log("Shotgun");
                playerAttack.currentBullet = playerAttack.bullets[1];
            }
            if (other.CompareTag("SMG"))
            {
                i = 2;
                Debug.Log("SMG");
                playerAttack.currentBullet = playerAttack.bullets[2];
            }
            playerAttack.currentAmmo = playerAttack.currentBullet.startAmmo;
            playerStat.UpdateWeaponsHUD();
            playerStat.UpdateAmmoRemain(playerAttack.currentAmmo);
        }
        Destroy(other.gameObject);
    }
}

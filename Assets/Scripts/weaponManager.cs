﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponManager : MonoBehaviour
{
    public Transform RightHand, LeftHand;
     Vector3 originalRHPosition, originalLHPosition;
    public GameObject shotgun, molotovChild, molotovPrefab, knife;
    public Transform shotgunLH, shotgunRH;
    public float throwDelay;

    public enum weapon { shotgun, molotov, knife }
    public weapon equippedWeapon;
    bool wasHoldingShotGunLastFrame = false;
    bool isThrowingMolotov;

    public int shotgunAmmo, numberOfMolotovs;

    void Start()
    {
        originalLHPosition = LeftHand.position;
        originalRHPosition = RightHand.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (equippedWeapon == weapon.shotgun)
        {
            wasHoldingShotGunLastFrame = true;
            shotgun.SetActive(true);
            molotovChild.SetActive(false);
            knife.SetActive(false);
            RightHand.transform.position = shotgunRH.position;
            LeftHand.transform.position = shotgunLH.position;

            if (Input.GetKeyDown(KeyCode.Mouse0) && shotgunAmmo != 0)
            {
                shotgun.GetComponent<shotgunScript>().shoot();
            }


        }
        else if (wasHoldingShotGunLastFrame)
        {
            wasHoldingShotGunLastFrame = false;
            shotgun.SetActive(false);
            RightHand.transform.position = originalRHPosition;
            LeftHand.transform.position = originalLHPosition;
        }

        if (equippedWeapon == weapon.molotov)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isThrowingMolotov && numberOfMolotovs != 0)
            {
                isThrowingMolotov = true;
                StartCoroutine("throwMolotov");
                GetComponent<Animator>().SetTrigger("throw");
                numberOfMolotovs--;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && numberOfMolotovs != 0)
            {
                GetComponent<Animator>().SetTrigger("drink");
                numberOfMolotovs--;
            }
            molotovChild.SetActive(numberOfMolotovs != 0);
            knife.SetActive(false);
        }

        if (equippedWeapon == weapon.knife)
        {
            shotgun.SetActive(false);
            molotovChild.SetActive(false);
            knife.SetActive(true);


            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GetComponent<Animator>().SetTrigger("knife");
            }
        }
    }


    public IEnumerator throwMolotov()
    {
        yield return new WaitForSeconds(throwDelay);
        isThrowingMolotov = false;
        Instantiate(molotovPrefab, RightHand.position, Quaternion.identity);
    }


}
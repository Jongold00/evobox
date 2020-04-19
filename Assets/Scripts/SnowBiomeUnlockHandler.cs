using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowBiomeUnlockHandler : MonoBehaviour
{
    public GameObject info_snow;
    public GameObject Unlock_snow;
    public GameObject info_mountain;
    public GameObject Unlock_mountain;
    public GameObject snowBiomeArrow_fromMountain;
    public GameObject snowBiomeArrow_fromMain;
    public GameObject mainBiomeArrow;
    public GameObject mountainBiomeArrow;
    public Transform mainCameraPos;
    public Transform snowCameraPos;
    public Transform mountainCameraPos;
    public GameObject snowBiomeDividers;
    public GameObject mountainBiomeDividers;

    public int currentBiome = 1;


    private bool snowUnlocked = false;
    private bool mountainUnlocked = false;


    // Update is called once per frame
    void Update()
    {
        if (!snowUnlocked)
        {
            if (GameObject.FindGameObjectsWithTag("Wolf").Length >= 5
                && GameObject.FindGameObjectsWithTag("Deer").Length >= 5
                && GameObject.FindGameObjectsWithTag("Bear").Length >= 3
                && currentBiome == 1)
            {
                info_snow.SetActive(false);
                Unlock_snow.SetActive(true);
            }
            else
            {
                info_snow.SetActive(true);
                Unlock_snow.SetActive(false);
            }
        }
        if (!mountainUnlocked)
        {
            if (GameObject.FindGameObjectsWithTag("Bear").Length >= 10 
                && currentBiome == 2)
            {
                info_mountain.SetActive(false);
                Unlock_mountain.SetActive(true);
            }
            else
            {
                info_mountain.SetActive(true);
                Unlock_mountain.SetActive(false);
            }
        }
    }

    public void onUnlockSnowClick()
    {
        snowUnlocked = true; 
        snowBiomeDividers.SetActive(false);
        Unlock_snow.SetActive(false);
        snowBiomeArrow_fromMain.SetActive(true);
    }

    public void onUnlockMountainClick()
    {
        mountainUnlocked = true; 
        mountainBiomeDividers.SetActive(false);
        Unlock_mountain.SetActive(false);
        mountainBiomeArrow.SetActive(true);
    }

    public void onMainBiomeArrowClick()
    {
        currentBiome = 1;
        snowBiomeArrow_fromMain.SetActive(true);
        mainBiomeArrow.SetActive(false);
        mountainBiomeArrow.SetActive(false);
        Camera.main.transform.position = mainCameraPos.position;
        Camera.main.transform.rotation = mainCameraPos.rotation;
    }

    public void onSnowBiomeArrowClick_FromMain()
    {
        currentBiome = 2;
        snowBiomeArrow_fromMain.SetActive(false);
        if (mountainUnlocked)
        {
            mountainBiomeArrow.SetActive(true);
        }
        mainBiomeArrow.SetActive(true);
        // TODO make this lerp nicely
        Camera.main.transform.position = snowCameraPos.position;
        Camera.main.transform.rotation = snowCameraPos.rotation; 
    }

    public void onSnowBiomeArrowClick_fromMountain()
    {
        currentBiome = 2;
        snowBiomeArrow_fromMountain.SetActive(false);
        mountainBiomeArrow.SetActive(true);
        mainBiomeArrow.SetActive(true);
        // TODO make this lerp nicely
        Camera.main.transform.position = snowCameraPos.position;
        Camera.main.transform.rotation = snowCameraPos.rotation;
    }



    public void onMountainBiomeArrowClick()
    {
        currentBiome = 3;
        snowBiomeArrow_fromMountain.SetActive(true);
        mainBiomeArrow.SetActive(false);
        mountainBiomeArrow.SetActive(false);
        Camera.main.transform.position = mountainCameraPos.position;
        Camera.main.transform.rotation = mountainCameraPos.rotation;
    }
}

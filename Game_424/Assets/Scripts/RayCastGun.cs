using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class RayCastGun : MonoBehaviour
{
    public GameObject TemperatureBar;
    public GameObject BatteryBar;
    public GameObject EnergyBar;
    public int selectedGun = 0;

    // Public variables for customization
    public Camera playerCamera;
    public Transform laserOrigin;
    public float laserDuration = 0.1f;
    public float fireRate = 0.25f;
    public float x = 0.5f, y = 0.5f, z = 0.5f;
    public Material hitMaterial;
    public TextMeshProUGUI hitText;

    // Weapon Battery System
    public enum BatteryCapacity { Capacity5000, Capacity10000, Capacity15000 }
    public BatteryCapacity batteryCapacity = BatteryCapacity.Capacity5000;
    private float currentBattery;

    // Weapon Fire System
    private bool isFiring = true;
    private float loadedEnergy;

    public int totalShots = 0;
    public int hitShots = 0;
    public int totalDamage = 0;
    public int enemiesKilled = 0;
    public float gameDuration = 0f;


    // Weapon Shooting System
    public enum WeaponRange { Range10, Range15, Range20 }
    public WeaponRange weaponRange = WeaponRange.Range10;


    // Weapon Cooling System
    public float minTemperature = 20f;
    public float maxTemperature = 70f;
    private float currentTemperature = 20f;
    public float coolingDuration = 5f;
    float initialLaserWidth;
    LineRenderer laserLine;
    float fireTimer = 0f;

    public float averageHitDistance = 0f;


    private void Awake()
    {
        currentBattery = GetBatteryCapacityValue(batteryCapacity);
        laserLine = GetComponent<LineRenderer>();
        initialLaserWidth = 0.01f;

        BatteryBar = GameObject.FindWithTag("BatteryBar");
        TemperatureBar = GameObject.FindWithTag("TemparatureBar");
        EnergyBar = GameObject.FindWithTag("EnergyBar");
        hitText = GameObject.Find("HitText").GetComponent<TextMeshProUGUI>();
        hitText.text = "";

    }

    private void Update()
    {
        fireTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && fireTimer > fireRate)
        {
            fireTimer = 0f;

            if (!isFiring && currentTemperature <= minTemperature)
            {
                isFiring = true;
                StartCoroutine(EnergyLoading());
            }
        }
        if (Input.GetButtonDown("Reload") )
        {
            BatteryBar.GetComponent<Slider>().value = 1;
            currentBattery = GetBatteryCapacityValue(batteryCapacity);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
            if (currentTemperature == minTemperature)
            {
                Shoot();
                totalShots++;
            }
        }
        coolingDuration = Mathf.Pow(2, currentTemperature / 10) / Mathf.Pow(2, currentTemperature / 20);

        if (currentTemperature > minTemperature && !isFiring)
        {
            currentTemperature = Mathf.Max(currentTemperature - (Time.deltaTime * (50f / coolingDuration)), minTemperature);
        }
        TemperatureBar.GetComponent<Slider>().value = (currentTemperature - minTemperature) / (maxTemperature - minTemperature);

        gameDuration = GetGameDuration();
    }

    IEnumerator EnergyLoading()
    {
        float elapsedTime = 0f;
        float startWidth = 0.01f;
        float targetWidth = 0.5f;
        float energyLoadingDuration = 5f;

        while (isFiring)
        {
            elapsedTime += Time.deltaTime;
            loadedEnergy = Mathf.Pow(2, elapsedTime) * 100;
            if (currentBattery != 0 && loadedEnergy > currentBattery)
            {
                loadedEnergy = currentBattery;
            }
            else if (currentBattery == 0)
            {
                loadedEnergy = 0;
                break;
            }
            float normalizedEnergy = Mathf.Log(loadedEnergy / 100) / Mathf.Log(2f) / 5;
            EnergyBar.GetComponent<Slider>().value = normalizedEnergy;

            float width = Mathf.Lerp(startWidth, targetWidth, elapsedTime / energyLoadingDuration);
            laserLine.startWidth = width;
            laserLine.endWidth = width;

            if (energyLoadingDuration > 0 && elapsedTime >= energyLoadingDuration)
            {
                laserLine.startWidth = 0.5f;
                laserLine.endWidth = 0.5f;

                elapsedTime = 0f;
                EnergyBar.GetComponent<Slider>().value = 0;

                laserLine.startWidth = initialLaserWidth;
                laserLine.endWidth = initialLaserWidth;

                // Reset the laser width and height after energy loading is complete
                isFiring = false;
                yield return null;
            }
            else
            {
                yield return null;
            }
        }

        if (loadedEnergy < currentBattery)
            Shoot();

        elapsedTime = 0f;
        EnergyBar.GetComponent<Slider>().value = 0;

        // The coroutine will only reach this point if !isFiring
        laserLine.startWidth = initialLaserWidth;
        laserLine.endWidth = initialLaserWidth;

        // Reset the laser width and height after energy loading is complete
        isFiring = false;
    }

    void Shoot()
    {
        float usedEnergy = loadedEnergy;
        // Check if there is enough battery and temperature for shooting
        if (usedEnergy < currentBattery)
        {

            currentBattery -= usedEnergy;
            BatteryBar.GetComponent<Slider>().value = currentBattery / GetBatteryCapacityValue(batteryCapacity);
            currentTemperature += usedEnergy / 10f;
            currentTemperature = Mathf.Min(currentTemperature, maxTemperature);

            Fire();
        }
        else
        {
            hitText.text = "Not enough battery";

        }


    }

    void Fire()
    {
        if (currentBattery > loadedEnergy)
        {
            float range = GetWeaponRangeValue(weaponRange);

            Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(x, y, z));

            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, laserOrigin.forward, out hit, range))
            {
                hitShots++;

                EnemyController controller = hit.collider.GetComponent<EnemyController>();
                float distance_to_target = Vector3.Distance(rayOrigin, hit.point);

                averageHitDistance = (averageHitDistance * (hitShots - 1) + distance_to_target) / hitShots;

                float firePower = CalculateFirePower(loadedEnergy, distance_to_target);
                hitText.text = "Hit";

                laserLine.SetPosition(0, laserOrigin.position);
                laserLine.SetPosition(1, hit.point);


                //Debug.Log(firePower);
                // Deal damage to the target based on the calculated fire power

                if (firePower > 0f)
                {

                    hitShots++;
                    totalDamage += (int)firePower;

                    if (controller != null)
                    {
                        if (controller.TakeDamage(firePower))
                        {
                            enemiesKilled++;
                        }



                    }
                }

                loadedEnergy = 0f;
            }
            else
            {
                laserLine.SetPosition(0, laserOrigin.position);
                laserLine.SetPosition(1, rayOrigin + (laserOrigin.forward * range));
            }

            StartCoroutine(RenderLaser());
        }
    }





    IEnumerator RenderLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        hitText.text = "";
        laserLine.enabled = false;
    }



    private float CalculateFirePower(float loadedEnergy, float distance_to_target)
    {

        return loadedEnergy - (loadedEnergy * (Mathf.Log(distance_to_target) / Mathf.Log(20f)));
    }







    float GetBatteryCapacityValue(BatteryCapacity capacity)
    {
        switch (capacity)
        {
            case BatteryCapacity.Capacity5000:
                return 5000f;
            case BatteryCapacity.Capacity10000:
                return 10000f;
            case BatteryCapacity.Capacity15000:
                return 15000f;
            default:
                return 5000f;
        }
    }

    // Helper method to get the range value based on the chosen enum
    float GetWeaponRangeValue(WeaponRange range)
    {
        switch (range)
        {
            case WeaponRange.Range10:
                return 10f;
            case WeaponRange.Range15:
                return 15f;
            case WeaponRange.Range20:
                return 20f;
            default:
                return 10f;
        }
    }

    private float GetGameDuration()
    {
        return Time.time;
    }
}

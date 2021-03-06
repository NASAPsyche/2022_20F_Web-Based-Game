using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;

public class GammaGenerator : MonoBehaviour
{
    public GammaRayController mainController;
    public GameObject sunRayMaster;
    public GameObject asteroidObj;    

    private Timer ejectTimer;

    // Start is called before the first frame update
    void Start()
    {
        ejectTimer = Timer.Register(Random.Range(Constants.Spectrometer.Ray.ejectMinTime, Constants.Spectrometer.Ray.ejectMaxTime), this.EjectRay, useRealTime: false);
        asteroidObj = LevelController.mainAsteroid;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EjectRay()
    {
        if(asteroidObj == null)
            asteroidObj = LevelController.mainAsteroid;

        Timer.Cancel(ejectTimer);

        Vector3 vectVari = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
        Vector2 direction = ((asteroidObj.transform.position + vectVari) - this.transform.position).normalized;
        GameObject sunRayTemp = Instantiate(sunRayMaster);
        sunRayTemp.transform.parent = mainController.raysList.transform;

        sunRayTemp.SetActive(true);
        sunRayTemp.transform.position = this.transform.position;
        sunRayTemp.GetComponent<Rigidbody2D>().velocity = direction * Constants.Spectrometer.Ray.ejectForce;

        ejectTimer = Timer.Register(Random.Range(Constants.Spectrometer.Ray.ejectMinTime, Constants.Spectrometer.Ray.ejectMaxTime), this.EjectRay, useRealTime: false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class OrbitalGravity : MonoBehaviour
{
    public GameObject body;
    public bool massFactor;
    [ConditionalField("massFactor")]
    public static int factor = 1;
    private Vector3 displacement;
    private Vector2 normalUnit;
    private float radius;
    private double massByMass;
    [ReadOnly]
    public Vector2 force;

    public bool boostInitVelocity;
    [ConditionalField(nameof(boostInitVelocity))]
    public float boostPercentage = 1.0f;

    //public void Awake()
    //{
    //    displacement = this.transform.position - body.transform.position;
    //    normalUnit = new Vector2(displacement.x, displacement.y).normalized;
    //    radius = displacement.magnitude;

    //    Physics2D.gravity = normalUnit * (gravConst * (massByMass / Mathf.Pow(radius, 2)));
    //}

    // Start is called before the first frame update
    void Start()
    {    
        if(massFactor)
            massByMass = factor * body.GetComponent<Rigidbody2D>().mass * this.GetComponent<Rigidbody2D>().mass;
        else
            massByMass = body.GetComponent<Rigidbody2D>().mass * this.GetComponent<Rigidbody2D>().mass;

        if (boostInitVelocity)
            BoostVelocity();
    }


    // Update is called once per frame
    public void FixedUpdate()
    {
        displacement = this.transform.position - body.transform.position;
        normalUnit = new Vector2(displacement.x, displacement.y).normalized;
        radius = displacement.magnitude;
        force = -normalUnit * (Constants.Space.gravityConstant * (float)(massByMass / Mathf.Pow(radius, 2)));
        this.GetComponent<Rigidbody2D>().AddForce(force);
    }

    public void BoostVelocity()
    {
        Vector3 initDisplace = body.transform.position - this.transform.position;
        float initRadius = initDisplace.magnitude;
        float initVel = Mathf.Sqrt((Constants.Space.gravityConstant * (float)massByMass) / initRadius);

        Vector3 shipDirect = Vector3.Cross(initDisplace.normalized, Vector3.back).normalized;
        shipDirect.z = 0.0f;
        //           this.transform.rotation = Quaternion.LookRotation(shipDirect);

        this.GetComponent<Rigidbody2D>().AddForce(shipDirect * initVel * (boostPercentage * 0.5f), ForceMode2D.Impulse);
    }

    public static Vector3 GravityVelocity(Vector3 objPos, Vector3 bodyPos, float mass)
    {
        Vector3 tempDisp = objPos - bodyPos;
        Vector2 tempNorm = new Vector2(tempDisp.x, tempDisp.y).normalized;
        float tempRad = tempDisp.magnitude;
        Vector2 tempForce = -tempNorm * (Constants.Space.gravityConstant * (float)(mass / Mathf.Pow(tempRad, 2)));

        return tempForce;
    }
}

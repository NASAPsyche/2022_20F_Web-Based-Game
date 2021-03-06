using System.Collections;
using UnityEngine;
using MyBox;


public class OrbitLines : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField] private int iterations = 10;

    private float mass;
    [Range(0.001f, 1.0f)]
    public float radBuffer = 1;

    [Range(0.001f, 1.0f)]
    public float resolution = 1;

    //maybe use these to better set the minimap camsize?
    [SerializeField] private float xMin = 0.0f;
    [SerializeField] private float xMax = 1.0f;
    [SerializeField] private float yMin = 0.0f;
    [SerializeField] private float yMax = 1.0f;

    [ReadOnly]
    [SerializeField] private float start2EndMag = 100.0f;
    [ReadOnly]
    [SerializeField] private float asteroidRad;

    private bool display = true;


    private struct orbital
    {
        public Vector3 pos;
        public float angle;
        public Vector3 vel;
        public Vector3 grav;
    }
    private orbital obj;


    // Start is called before the first frame update
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
        mass = OrbitalGravity.factor * LevelController.player.GetComponent<Rigidbody2D>().mass * LevelController.mainAsteroid.GetComponent<Rigidbody2D>().mass;
        asteroidRad = LevelController.mainAsteroid.GetComponent<PolygonCollider2D>().bounds.extents.x;

    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            display = !display;
            line.enabled = display;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (display)
            SimulateOrbit(LevelController.player);
    }

    public void SimulateOrbit(GameObject ship)
    {
        Vector3 physicsStep;
        asteroidRad = LevelController.mainAsteroid.GetComponent<PolygonCollider2D>().bounds.extents.x * radBuffer;

        obj.pos = ship.transform.position;
        obj.vel = ship.GetComponent<Rigidbody2D>().velocity;
        obj.grav = OrbitalGravity.GravityVelocity(obj.pos, LevelController.mainAsteroid.transform.position, mass);

        line.positionCount = iterations;

        for (int i = 0; i < iterations; i++)
        {
            line.SetPosition(i, new Vector3(obj.pos.x, obj.pos.y, 2));
            physicsStep = ((obj.vel * resolution) + (obj.grav * resolution));
            obj.vel = physicsStep;
            obj.pos += physicsStep;
            obj.grav = OrbitalGravity.GravityVelocity(obj.pos, LevelController.mainAsteroid.transform.position, mass);

            if (obj.pos.x < xMin)
                xMin = obj.pos.x;
            if (obj.pos.x > xMax)
                xMax = obj.pos.x;
            if (obj.pos.y < yMin)
                yMin = obj.pos.y;
            if (obj.pos.y > yMax)
                yMax = obj.pos.y;

            //collisioncheck
            if (obj.pos.magnitude < asteroidRad)
            {
                line.positionCount = i;
                line.loop = false;
                break;
            }
            else if (i > iterations - 5)
                line.loop = false;
            else
                line.loop = true;

            //CircleCheck
            if (i > 4)
            {
                start2EndMag = (line.GetPosition(i) - line.GetPosition(0)).magnitude;

                if (start2EndMag < 10.0f)
                {
                    line.positionCount = i;
                    break;
                }
            }
        }
    }
}

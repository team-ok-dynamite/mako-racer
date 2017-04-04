using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MakoController : MonoBehaviour
{
    public bool raceHasBegun;
    public bool finishedRace;
    public float jumpSpeed;
    public float flipTorque;
    public float maxPitchTorque;

    private uint fuelCount;
    private const uint FUEL_MAX = 100;
    private Rigidbody rb;

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    private void Start()
    {
        fuelCount = FUEL_MAX;
        rb = this.GetComponent<Rigidbody>();
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider, GameObject model)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        model.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        if (!raceHasBegun || finishedRace) {
            // TODO: should we figure out a way to make this MAKO disappear? What if they block the trigger completely?
            return;
        }

        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        uint flyingWheelCount = 0;
        WheelHit wheelHit;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel, axleInfo.leftModel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel, axleInfo.rightModel);

            if (!axleInfo.leftWheel.GetGroundHit(out wheelHit))
                flyingWheelCount++;
            if (!axleInfo.rightWheel.GetGroundHit(out wheelHit))
                flyingWheelCount++;
        }

        handleJump(flyingWheelCount >= 2);
    }

    private void handleJump(bool flying)
    {
        bool jetOn = Input.GetButton("Jump");

        if (jetOn && fuelCount > 0)
        {
            rb.AddRelativeForce(new Vector3(0, jumpSpeed, 0));
            fuelCount -= 1;
        }
        else if (fuelCount < FUEL_MAX)
        {
            fuelCount += 1;
        }

        // When flying, the forward/backward axis becomes a control on the pitch of the mako
        if (flying)
        {
            float pitch = maxPitchTorque * Input.GetAxis("Vertical");
            if (pitch != 0)
            {
                rb.AddRelativeTorque(new Vector3(pitch, 0, 0));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("From the mako, we've crossed the finish.");
        if (other.tag == "Finish")
        {
            finishedRace = true;
        }
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public GameObject leftModel;
    public GameObject rightModel;
    public bool motor;
    public bool steering;
}
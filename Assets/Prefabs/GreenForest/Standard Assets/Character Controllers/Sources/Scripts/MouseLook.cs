using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

    public float scrollSpeed;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	private float sensitivityX = 5F;
    private float sensitivityY = 5F;
    private float rotateX = 0;
    private float rotateY = 0;
    private float rotX;
    private float rotY;
    public bool move = true;

    public GameObject dreamer;
    //new Vector3 collide = new Vector3(0, 0, 0);

	float rotationY = 0F;

    void Update()
    {
        if (move == true) {
            rotX = Input.GetAxis("Mouse X") * sensitivityX;
            rotY = Input.GetAxis("Mouse Y") * sensitivityY;

            if (!Physics.Raycast(this.transform.position, -this.transform.up, Mathf.Sign(rotY) * 1f))
            {
                rotateY += -((Mathf.Abs(rotY) > 4) ? Mathf.Sign(Input.GetAxis("Mouse Y")) * 4 : rotY);
            }

            if (Physics.Raycast(this.transform.position, -Mathf.Sign(Input.GetAxis("Mouse X")) * this.transform.right, Mathf.Abs(rotX) * 5))
            {
                rotateY += Mathf.Abs(rotX) / 2;
                if (rotateY > 50 && rotateY < 180)
                    rotateY = 50;
                if (rotateY < 320 && rotateY > 180)
                    rotateY = 320;
            }

            this.transform.localEulerAngles = new Vector3(0, 0, 0);
            this.transform.localPosition = new Vector3(0, 1f, -2);
            transform.RotateAround(new Vector3(dreamer.transform.position.x, dreamer.transform.position.y + 1, dreamer.transform.position.z), dreamer.transform.right, rotateY);

            if ((this.transform.localEulerAngles.x > 50 && this.transform.localEulerAngles.x < 180 && Input.GetAxis("Mouse Y") < 0))
            {
                rotateY = 50;
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
                this.transform.localPosition = new Vector3(0, 1f, -2);
                transform.RotateAround(new Vector3(dreamer.transform.position.x, dreamer.transform.position.y + 1, dreamer.transform.position.z), dreamer.transform.right, rotateY);
            }
            if (this.transform.localEulerAngles.x < 320 && this.transform.localEulerAngles.x > 180 && Input.GetAxis("Mouse Y") > 0)
            {
                rotateY = 320;
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
                this.transform.localPosition = new Vector3(0, 1f, -2);
                transform.RotateAround(new Vector3(dreamer.transform.position.x, dreamer.transform.position.y + 1, dreamer.transform.position.z), dreamer.transform.right, rotateY);
            }

            dreamer.transform.RotateAround(dreamer.transform.localPosition, dreamer.transform.up, rotX);
        }
        else
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
            this.transform.localPosition = new Vector3(0, 1f, -2);
            transform.RotateAround(new Vector3(dreamer.transform.position.x, dreamer.transform.position.y + 1, dreamer.transform.position.z), dreamer.transform.right, rotateY);
        }
    }

    void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerWheel : MonoBehaviour
{
    public float maxRightDeg;
    public float maxLeftDeg;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, 0f, speed * Time.deltaTime, Space.Self);

            // convert the quaternion into easier to use degree values
            // xyz will now represent degrees between 0-180
            Vector3 vec = transform.rotation.eulerAngles;

            if (vec.z > maxLeftDeg && vec.z > 0 && vec.z < 180)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, maxLeftDeg);
                //Debug.Log(vec.z + " > " + maxLeftDeg);
            }
            getRotation();

        } else if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(0f, 0f, -speed * Time.deltaTime, Space.Self);
            // convert the quaternion into easier to use degree values
            // xyz will now represent degrees between 0-180
            Vector3 vec = transform.rotation.eulerAngles;

            if (vec.z < maxRightDeg && vec.z > 200)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, maxRightDeg);
                //Debug.Log(vec.z + " < " + maxRightDeg);
            }
            getRotation();
        }
   
    }

    public float getRotation()
    {
        // due to setup we need a conversion
        Vector3 rot = transform.localRotation.eulerAngles;
        float z = rot.z;

        float N = 0;
        float L1 = 45;
        float L2 = 90;
        float R1 = 315;
        float R2 = 270;

        float epsilon = 20;
        List<float> arr = new List<float> {N, L1, L2, R1, R2 };

        // find the matching nearewt rotation
        int index = 0;
        for(int i=0; i<arr.Count; i++)
        {
            if(Mathf.Abs(z - arr[i]) < epsilon)
            {
                index = i;
                break;
            }
        }
       

        //Debug.Log(arr[index]);
        return arr[index];
    }
}

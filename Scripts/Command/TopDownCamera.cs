using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopDownCamera : MonoBehaviour
{
    #region Variables

    public float height = 5f;
    public float distance = 10;
    public float angle = 45f;
    public float lookAtHeight = 2f;
    public float smothSpeed = 0.5f;
    private Vector3 refVelocity;
    public Transform target;
    #endregion Variables

    private void Awake()
    {
        
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        var obj = FindObjectsOfType<TopDownCamera>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
        
    }


    private void LateUpdate()
    {
        if (target == null && SceneManager.GetActiveScene().name != "LoadingScene")
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        HandleCamera();

       
    }

    public void HandleCamera()
    {
        if (!target)
            return;


        //Build world position vector

        Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);
        //Debug.DrawLine(target.position, worldPosition, Color.red);


        //Build our rotated vector
        Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
        //Debug.DrawLine(target.position, rotateVector, Color.green);

        //move our position 
        Vector3 finalTargetPosition = target.position;
        finalTargetPosition.y += lookAtHeight;
        Vector3 finalPosition = finalTargetPosition + rotateVector;
        //Debug.DrawLine(target.position, finalPosition, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smothSpeed);


        transform.LookAt(target.position);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f,0f,0.5f);

        if(target)
        {
            Vector3 lookAtPosition = target.position;
            lookAtPosition.y += lookAtHeight;
            Gizmos.DrawLine(transform.position, lookAtPosition);
            Gizmos.DrawSphere(lookAtPosition, 0.25f);




        }

        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraMove : MonoBehaviour
{
    public Transform player;

    Vector3 offset;

    private void Awake()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        var obj = FindObjectsOfType<CameraMove>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);


        
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(player.position.x, 30f, player.position.z);
        offset = player.position - transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(player == null&& SceneManager.GetActiveScene().name != "LoadingScene")
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (SceneManager.GetActiveScene().name != "LoadingScene")
        {
            transform.position = player.position - offset;
        }
    }
}

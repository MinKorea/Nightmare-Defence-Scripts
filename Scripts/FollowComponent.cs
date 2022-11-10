using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowComponent : MonoBehaviour
{
    Transform player;
    Vector3 offset;
    [SerializeField]
    float smooth = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, smooth * Time.deltaTime);
    }
}

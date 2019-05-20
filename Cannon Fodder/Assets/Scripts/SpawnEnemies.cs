using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefab1, enemyPrefab2, enemyPrefab3, player;
    public int maxSpawns, minSpawns;
    public Vector3 size;

    private Vector3 center;
    
    
    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        for(int i = 0; i< (int)Random.Range(minSpawns, maxSpawns); i++ )
        {
            Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
            int prefab = Random.Range(1, 3);
            if(prefab == 1)
            {  
                GameObject temp = Instantiate(enemyPrefab1, pos, Quaternion.identity);
                FocusPlayer focus = temp.transform.GetComponent<FocusPlayer>();
                focus.target = player;
            }
            if (prefab == 2)
            {
                GameObject temp = Instantiate(enemyPrefab2, pos, Quaternion.identity);
                FocusPlayer focus = temp.transform.GetComponent<FocusPlayer>();
                focus.target = player;
            }
            if (prefab == 3)
            {
                GameObject temp = Instantiate(enemyPrefab2, pos, Quaternion.identity);
                FocusPlayer focus = temp.transform.GetComponent<FocusPlayer>();
                focus.target = player;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        Gizmos.DrawCube(transform.localPosition, size);
    }
}

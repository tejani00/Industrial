using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManage.Instance.CollectMoney();
        Destroy(this.gameObject);
    }
}

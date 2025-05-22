using UnityEngine;

public interface IPoolable
{
    void InitializePool();
    GameObject GetPooledObject();
}

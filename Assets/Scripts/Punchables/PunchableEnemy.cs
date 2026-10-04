using UnityEngine;

public class Enemy : MonoBehaviour, Punchable
{
    public int hp;
    public void Hook()
    {
        throw new System.NotImplementedException();
    }

    public void Punch()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Punch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

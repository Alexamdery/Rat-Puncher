using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Punch : MonoBehaviour
{
    public float attackDuration;
    public PlayerManager playerManager;
    private float _timer = 0;

    void Start()
    {
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= attackDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Should only be able to punch PUNCHABLE layer
        playerManager.SetPunching(other.gameObject);
        Destroy(gameObject);
    }
}

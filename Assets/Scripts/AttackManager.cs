using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AttackManager : MonoBehaviour
{
    public float attackDuration;
    public PlayerManager playerManager;
    private float m_timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= attackDuration)
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

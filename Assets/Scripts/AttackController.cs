using UnityEngine;
using UnityEngine.Events;

public class AttackController : MonoBehaviour
{
    public float attackDuration;
    public UnityEvent<GameObject> doneEvent;
    private float m_timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (doneEvent == null)
            doneEvent = new UnityEvent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= attackDuration)
        {
            doneEvent.Invoke(null);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Should only be able to punch PUNCHABLE layer
        doneEvent.Invoke(other.gameObject);
        Destroy(gameObject);
    }
}

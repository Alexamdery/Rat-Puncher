using UnityEngine;
using UnityEngine.Events;

public class AttackController : MonoBehaviour
{
    public float attackDuration;
    public UnityEvent doneEvent;
    private float m_timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (doneEvent == null)
            doneEvent = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= attackDuration)
        {
            doneEvent.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Punchable punchable = other.gameObject.GetComponent<Punchable>();
        if (punchable != null)
        {
            punchable.SetupPunch(transform.forward);
        }
    }
}

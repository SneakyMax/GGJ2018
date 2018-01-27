using Assets;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    public float InitialSpeed = 1;

    public float Acceleration = 1;

    public float MaxSpeed = 10;

    public float HomingConeHalfAngle = 5;

    public float HomingTurnRate = 1;

    public Sub Parent { get; set; }

    private Rigidbody body;

    private float speed;
    private Vector3 direction;
    public TorpedoTargetable target;

    public void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        speed = InitialSpeed;
        direction = transform.forward;
    }

    public void Update()
    {
        if (target == null)
        {
            foreach (var targetable in SubManager.Instance.Targetable)
            {
                if (targetable.transform.gameObject == Parent.gameObject)
                    continue;

                var vectorTorpToTargetable = targetable.transform.position - transform.position;

                RaycastHit hit;
                Physics.Raycast(new Ray(transform.position, vectorTorpToTargetable), out hit);

                var hitTargetable = hit.collider.gameObject.GetComponent<TorpedoTargetable>();

                if (hitTargetable != null && hitTargetable == targetable)
                {
                    target = hitTargetable;
                }
            }
        }
    }

    public void FixedUpdate()
    {
        body.velocity = direction * speed;
        if (speed < MaxSpeed)
        {
            speed += Acceleration + Time.fixedDeltaTime;
        }
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
using UnityEngine;

public class PathFollowing : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _z = 10f;
    [SerializeField] private float _rotationOffset = 0f; // degrees: faces right 0, up -90, left 180, down 90

    private int _i = 1;
    private bool _hasStartOverride;
    private Summoner _summoner;

    public int CurrentIndex => _i;

    // Call right after Instantiate to spawn mid-path instead of at waypoint 0
    public void StartAt(int nextIndex, Vector3 pos)
    {
        _i = nextIndex;
        transform.position = new Vector3(pos.x, pos.y, _z);
        _hasStartOverride = true;
    }

    Vector3 Target(int i)
    {
        Vector3 p = WaypointPath.Instance.Get(i);
        return new Vector3(p.x, p.y, _z);
    }

    void Awake()
    {
        _summoner = GetComponent<Summoner>();
    }

    void Start()
    {
        if (!_hasStartOverride) transform.position = Target(0);
    }

    void Update()
    {
        if (_i >= WaypointPath.Instance.Count)
        {
            var enemy = GetComponent<Enemy>();
            if (enemy != null) enemy.ReachedEnd(); else Destroy(gameObject);
            return;
        }

        // Stand still while summoning
        if (_summoner != null && _summoner.IsSummoning) return;

        Vector3 target = Target(_i);

        // Face the waypoint we're walking toward
        Vector3 dir = target - transform.position;
        if (dir.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + _rotationOffset);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);

        if (transform.position == target) _i++;
    }
}
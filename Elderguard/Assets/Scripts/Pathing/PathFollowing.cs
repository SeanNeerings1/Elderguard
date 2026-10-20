using UnityEngine;

public class PathTester : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _z = 10f;
    private int _i = 1;

    Vector3 Target(int i)
    {
        Vector3 p = WaypointPath.Instance.Get(i);
        return new Vector3(p.x, p.y, _z);
    }

    void Start()
    {
        transform.position = Target(0);
    }

    void Update()
    {
        if (_i >= WaypointPath.Instance.Count)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 target = Target(_i);
        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);

        if (transform.position == target) _i++;
    }
}
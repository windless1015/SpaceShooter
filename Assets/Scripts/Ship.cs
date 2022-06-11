using UnityEngine;
using UnityEngine.InputSystem;


public class Ship : MonoBehaviour
{
	[SerializeField]
	PlayerInput _playerInput = null;

	[SerializeField]
	float _speed = 5.0f;

	[SerializeField]
	float _shotsPerSecond = 10.0f;

	[SerializeField]
	float _bulletThrust = 10.0f;

	[SerializeField]
	Spawner _bulletSpawner = null;

	[SerializeField]
	Transform[] _bulletEmitPoint = null;


	InputAction _steeringInput;
	InputAction _shootInput;
	float _shotDelay;
	float _lastShotTime;
	int _bulletEmitIndex;


	void Start()
	{
		_steeringInput = _playerInput.actions["SteerShip"];
		_shootInput = _playerInput.actions["Shoot"];
		_shotDelay = 1.0f / _shotsPerSecond;
		_lastShotTime = 0.0f;
		_bulletEmitIndex = 0;
	}

	void Update()
	{
		Vector2 steering = _steeringInput.ReadValue<Vector2>();
		Vector3 delta = _speed * steering * Time.deltaTime;
		transform.position = transform.position + delta;

		if( _shootInput.ReadValue<float>() == 1.0f )
		{
			float timeDelta = Time.time - _lastShotTime;
			if( timeDelta >= _shotDelay )
			{
				Transform t = _bulletEmitPoint[_bulletEmitIndex];
				Transform bullet = _bulletSpawner.Spawn( t.position, t.rotation );
				Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
				bulletBody.AddForce( transform.up * _bulletThrust );

				_bulletEmitIndex = (_bulletEmitIndex + 1) % _bulletEmitPoint.Length;
				_lastShotTime = Time.time;
			}
		}
	}
}
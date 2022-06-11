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
	Spawner _bulletSpawnerSpecial = null;

	[SerializeField]
	Transform[] _bulletEmitPoint = null;


	InputAction _steeringInput;
	InputAction _shootInput;
	InputAction _shootInputSpecial;
	float _shotDelay;
	float _lastShotTime;
	int _bulletEmitIndex;


	void Start()
	{
		_steeringInput = _playerInput.actions["SteerShip"];
		_shootInput = _playerInput.actions["Shoot"];
		_shootInputSpecial = _playerInput.actions["ShootSpecial"];
		_shotDelay = 1.0f / _shotsPerSecond;
		_lastShotTime = 0.0f;
		_bulletEmitIndex = 0;
	}

	void Update()
	{
		Vector2 steering = _steeringInput.ReadValue<Vector2>();
		Vector3 delta = _speed * steering * Time.deltaTime;
		transform.position = transform.position + delta;

		if( _shootInput.ReadValue<float>() == 1.0f)//left key control left bullets
		{
			float timeDelta = Time.time - _lastShotTime;
			if( timeDelta >= _shotDelay  && _bulletEmitIndex % 2 == 0) //even
			{
				Transform t = _bulletEmitPoint[_bulletEmitIndex];
				Transform bullet = _bulletSpawner.Spawn( t.position, t.rotation );
				Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
				bulletBody.AddForce( transform.up * _bulletThrust );

				_bulletEmitIndex = (_bulletEmitIndex + 1) % _bulletEmitPoint.Length;
				_lastShotTime = Time.time;
			}
		}

		if (_shootInputSpecial.ReadValue<float>() == 1.0f)//right key control right bullets
		{
			float timeDelta = Time.time - _lastShotTime;
			if (timeDelta >= _shotDelay && _bulletEmitIndex % 2 != 0) // odd
			{
				Transform t = _bulletEmitPoint[_bulletEmitIndex];
				//update bullet2 components since the bullet2's style has been modified
				Transform bulletSpecial = _bulletSpawnerSpecial.Spawn(t.position, t.rotation);
				Rigidbody2D bulletBodySpecial = bulletSpecial.GetComponent<Rigidbody2D>();
				//bulletBodySpecial.AddForce(transform.up * _bulletThrust);
				bulletBodySpecial.AddForce(transform.up * 5);//change the trust volume

				_bulletEmitIndex = (_bulletEmitIndex + 1) % _bulletEmitPoint.Length;
				_lastShotTime = Time.time;
			}
		}

	}
}
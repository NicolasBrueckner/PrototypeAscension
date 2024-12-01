using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public static class Utility
{
	public static bool TryValidateCollision( Collision2D collision, LayerMask mask )
	{
		GameObject collisionObject = collision.gameObject;

		return collisionObject && ValidateCollision( collisionObject, mask );
	}

	public static bool ValidateCollision( GameObject collisionObject, LayerMask mask )
	{
		int collisionLayer = collisionObject.layer;
		return ( mask.value & ( 1 << collisionLayer ) ) != 0;
	}

	public static T CreateSingleton<T>( T instance, GameObject gameObject ) where T : Component
	{
		if( !instance )
		{
			instance = gameObject.GetComponent<T>();
			if( !instance )
				instance = gameObject.AddComponent<T>();
			Object.DontDestroyOnLoad( gameObject );
			return instance;
		}

		if( instance.gameObject != gameObject )
			Object.Destroy( gameObject );
		return instance;
	}

	public static void SetRuntimeSpeed( float value )
	{
		Time.timeScale = value;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}

	public static void TryStopMovement( GameObject gameObject )
	{
		Rigidbody2D rb2D = gameObject.GetComponent<Rigidbody2D>();

		if( rb2D )
			StopMovement( rb2D );
		else
			Debug.LogError( $"No Rigidbody2D on {gameObject} found." );
	}

	public static void StopMovement( Rigidbody2D rb2D )
	{
		rb2D.velocity = Vector2.zero;
	}

	public static Transform FindParentWithLayer( Transform parent, LayerMask mask )
	{
		while( parent )
		{
			if( ( ( 1 << parent.gameObject.layer ) & mask.value ) != 0 )
				return parent;

			parent = parent.parent;
		}

		return null;
	}

	public static Vector3[] V2ToV3( Vector2[] array, float z )
	{
		return Array.ConvertAll( array, v => new Vector3( v.x, v.y, z ) );
	}

	public static Vector2[] V3ToV2( Vector3[] array )
	{
		return Array.ConvertAll( array, v => ( Vector2 )v );
	}

	public static Vector2 GetAverageCollisionNormal( Collision2D collision )
	{
		Vector2 normal = Vector2.zero;

		if( collision == null || collision.contacts.Length == 0 )
			return normal;

		foreach( ContactPoint2D point in collision.contacts )
			normal += point.normal;

		normal /= collision.contacts.Length;

		return normal;
	}

	public static bool CheckForOneWay( Collision2D collision )
	{
		PlatformEffector2D effector = collision.gameObject.GetComponent<PlatformEffector2D>();

		if( !effector || !effector.useOneWay )
			return false;

		Vector2 normal = collision.contacts[ 0 ].normal;
		Vector2 up = effector.transform.up;
		float angle = effector.surfaceArc / 2.0f;
		float angleBetween = Vector2.Angle( normal, up );

		return !( angleBetween <= angle );
	}

	public static bool ValidateVelocity( Vector2 velocity, Vector2 surfaceNormal, float threshold )
	{
		float angle = Vector2.Angle( velocity, surfaceNormal );

		return angle <= threshold;
	}
}

public class DisposableStopwatch : IDisposable
{
	private readonly Stopwatch _stopwatch;

	public DisposableStopwatch()
	{
		_stopwatch = new();
		_stopwatch.Start();
	}

	public void Dispose()
	{
		_stopwatch.Stop();
		long elapsed = _stopwatch.ElapsedMilliseconds;
		Debug.Log( $"elapsed: {elapsed} ms" );
	}
}
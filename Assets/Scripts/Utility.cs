using UnityEngine;

public static class Utility
{
	public static bool IsValidCollision( GameObject collisionObject, LayerMask mask )
	{
		int collisionLayer = collisionObject.layer;
		return ( mask.value & ( 1 << collisionLayer ) ) != 0;
	}

	public static T CreateSingleton<T>( T instance, GameObject gameObject ) where T : Component
	{
		if ( instance == null )
		{
			instance = gameObject.GetComponent<T>();
			if ( instance == null )
			{
				instance = gameObject.AddComponent<T>();
			}
			Object.DontDestroyOnLoad( gameObject );
			return instance;
		}
		else if ( instance.gameObject != gameObject )
		{
			Object.Destroy( gameObject );
		}
		return instance;
	}

	public static void SetSlowMotion( float value )
	{
		Time.timeScale = value;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}

	public static void TryStopMovement( GameObject gameObject )
	{
		Rigidbody2D rb2D = gameObject.GetComponent<Rigidbody2D>();

		if ( rb2D )
		{
			StopMovement( rb2D );
		}
		else
		{
			Debug.LogError( $"No Rigidbody2D on {gameObject} found." );
		}
	}

	public static void StopMovement( Rigidbody2D rb2D )
	{
		rb2D.velocity = Vector2.zero;
	}
}

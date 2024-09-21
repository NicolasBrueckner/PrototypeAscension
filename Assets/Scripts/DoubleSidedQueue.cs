using System.Collections.Generic;

public class DoubleSidedQueue<T>
{
	public int Count => _items.Count;

	private LinkedList<T> _items = new();

	public void EnqueueFront( T item )
	{
		_items.AddFirst( item );
	}

	public void EnqueueBack( T item )
	{
		_items.AddLast( item );
	}

	public T DequeueFront()
	{
		if ( _items.Count == 0 )
			throw new System.InvalidOperationException( "DoubleSidedQueue is empty" );

		T value = _items.First.Value;
		_items.RemoveFirst();
		return value;
	}

	public T DequeueBack()
	{
		if ( _items.Count == 0 )
			throw new System.InvalidOperationException( "DoubleSidedQueue is empty" );

		T value = _items.Last.Value;
		_items.RemoveLast();
		return value;
	}

	public T PeekFront()
	{
		if ( _items.Count == 0 )
			throw new System.InvalidOperationException( "DoubleSidedQueue is empty" );

		return _items.First.Value;
	}

	public T PeekBack()
	{
		if ( _items.Count == 0 )
			throw new System.InvalidOperationException( "DoubleSidedQueue is empty" );

		return _items.Last.Value;
	}
}

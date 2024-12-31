using System;

public abstract class Singleton<T> where T : Singleton<T>, new()
{
	private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());

	protected Singleton()
	{

	}

	public static T Instance
	{
		get { return _instance.Value; }
	}


	public void ExplicitInit() { }
}

namespace CommonBootStrapper
{
	public interface IBootStrapper
	{
		void AsyncStart();
		void StopAndWaitForCompletion();
	}
}

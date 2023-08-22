using Zoo.DataAccess.Repositories;

namespace Zoo.Logic.Validation
{
	public abstract class ValidationBL<TEntity> where TEntity : class
	{
		protected abstract void validateEntity(TEntity animal);
		protected abstract Task validateExistence(int id);
		protected void validateNull<T>(T arg)
		{
			if (arg == null) throw new ArgumentNullException(nameof(T));
		}

		protected void validateNoId(int id)
		{
			if (id != 0)
				throw new ArgumentException(ErrorMessages.ID_PROVIDED, nameof(id));
		}
	}
}

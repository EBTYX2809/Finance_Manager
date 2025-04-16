using Finance_Manager_Backend.BusinessLogic.Models;

namespace Finance_Manager_Backend.Exceptions;

public class EntityNotFoundException<TEntity> : Exception
    where TEntity : IEntity
{
    public EntityNotFoundException(int entityId)
        : base($"{typeof(TEntity).Name} with {entityId} isn't exist.") { }
}

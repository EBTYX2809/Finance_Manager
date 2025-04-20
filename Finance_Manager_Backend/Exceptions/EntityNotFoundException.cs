using Finance_Manager_Backend.BusinessLogic.Models;

namespace Finance_Manager_Backend.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, int entityId)
        : base($"{entityName} with ID {entityId} isn't exist.") { }
}

public class EntityNotFoundException<TEntity> : EntityNotFoundException
    where TEntity : IEntity
{
    public EntityNotFoundException(int entityId)
        : base(typeof(TEntity).Name, entityId) { }
}

using CarRepairServiceCode.Exceptions;
using CarRepairServiceCode.Helper;
using CarRepairServiceCode.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace CarRepairServiceCode.Services
{
    public abstract class BaseService<TLogger> where TLogger : ILogger
    {
        protected readonly TLogger _logger;
        protected readonly IPermissionRepository _permissionRepository;

        protected BaseService(TLogger logger, IPermissionRepository permissionRepository)
        {
            _logger = logger;
            _permissionRepository = permissionRepository;
        }

        protected void CheckPermissionInfo(int positionId, ActionName action, DbEntitiesName entityNameEnum)
        {
            var entityName = entityNameEnum.GetEnumDescription();
            var permissionInfo = _permissionRepository.ReturnPermissionInfoById(positionId, entityName);
            if (permissionInfo.Result == null)
                PermissionErrorRunner(Messages.DoNotHaveAccessRightsConfigured);

            switch (action)
            {
                case ActionName.CreateEntity:
                    if (permissionInfo.Result.CreateEntry != true)
                        PermissionErrorRunner(ActionName.CreateEntity);
                    break;

                case ActionName.ReadEntity:
                    if (permissionInfo.Result.ReadEntry != true)
                        PermissionErrorRunner(ActionName.ReadEntity);
                    break;

                case ActionName.UpdateEntity:
                    if (permissionInfo.Result.UpdateEntry != true)
                        PermissionErrorRunner(ActionName.UpdateEntity);
                    break;

                case ActionName.DeleteEntity:
                    if (permissionInfo.Result.DeleteEntry != true)
                        PermissionErrorRunner(ActionName.DeleteEntity);
                    break;
            }
        }

        protected void PermissionErrorRunner(ActionName actionName)
        {
            _logger.LogError(string.Format(Messages.NotEnoughPermissions,
                actionName.GetEnumDescription(), Entities.Position.GetEnumDescription()));
            throw new PermissionException(string.Format(Messages.NotEnoughPermissions,
                actionName.GetEnumDescription(), Entities.Position.GetEnumDescription()));
        }

        protected void PermissionErrorRunner(int id)
        {
            _logger.LogError(string.Format(Messages.RemoveOrUpdateServicePosition,
                ServicePositions.Manager.GetEnumDescription(), id));
            throw new PermissionException(string.Format(Messages.RemoveOrUpdateServicePosition,
                ServicePositions.Manager.GetEnumDescription(), id));
        }

        protected void PermissionErrorRunner(string message)
        {
            _logger.LogError(message);
            throw new PermissionException(message);
        }

        protected void NotFoundExceptionRunner(int id)
        {
            _logger.LogError(string.Format(Messages.NotFoundInTheSystem,
                Entities.Position.GetEnumDescription(), id));
            throw new NotFoundException(string.Format(Messages.NotFoundInTheSystem,
                Entities.Position.GetEnumDescription(), id));
        }

        protected void UnauthorizedExceptionRunner(string invalidLogin)
        {
            _logger.LogError(string.Format(Messages.AuthorizationUnsuccessful, invalidLogin));
            throw new UnauthorizedException(string.Format(Messages.AuthorizationUnsuccessful, invalidLogin));
        }
    }
}

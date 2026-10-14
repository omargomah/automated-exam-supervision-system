using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Entities
{
    public class AuditLog : Entity, IAggregateRoot
    {
        private AuditLog() : base(Guid.NewGuid()) { }

        private AuditLog(
            Guid id,
            string action,
            string entityName,
            Guid? entityId,
            string? details,
            DateTime createdAt,
            Guid? applicationUserId) : base(id)
        {
            Action = action;
            EntityName = entityName;
            EntityId = entityId;
            Details = details;
            CreatedAt = createdAt;
            ApplicationUserId = applicationUserId;
        }

        public string Action { get; private set; } = string.Empty;
        public string EntityName { get; private set; } = string.Empty;
        public Guid? EntityId { get; private set; }
        public string? Details { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid? ApplicationUserId { get; private set; }

        public ApplicationUser? ApplicationUser { get; private set; }

        public static Result<AuditLog> Create(
            string action,
            string entityName,
            Guid? entityId = null,
            string? details = null,
            Guid? applicationUserId = null,
            Guid? id = null)
        {
            if (string.IsNullOrWhiteSpace(action))
            {
                return Result.Failure<AuditLog>(Error.Create("AuditLog.ActionEmpty", "Audit action cannot be empty."));
            }

            action = action.Trim();

            if (action.Length > Constants.AuditLog.ActionMaxLength)
            {
                return Result.Failure<AuditLog>(Error.Create(
                    "AuditLog.ActionTooLong",
                    $"Audit action cannot exceed {Constants.AuditLog.ActionMaxLength} characters."));
            }

            if (string.IsNullOrWhiteSpace(entityName))
            {
                return Result.Failure<AuditLog>(Error.Create("AuditLog.EntityNameEmpty", "Entity name cannot be empty."));
            }

            entityName = entityName.Trim();

            if (entityName.Length > Constants.AuditLog.EntityNameMaxLength)
            {
                return Result.Failure<AuditLog>(Error.Create(
                    "AuditLog.EntityNameTooLong",
                    $"Entity name cannot exceed {Constants.AuditLog.EntityNameMaxLength} characters."));
            }

            if (!string.IsNullOrWhiteSpace(details))
            {
                details = details.Trim();
                if (details.Length > Constants.AuditLog.DetailsMaxLength)
                {
                    return Result.Failure<AuditLog>(Error.Create(
                        "AuditLog.DetailsTooLong",
                        $"Audit details cannot exceed {Constants.AuditLog.DetailsMaxLength} characters."));
                }
            }
            else
            {
                details = null;
            }

            return Result.Success(new AuditLog(
                id ?? Guid.NewGuid(),
                action,
                entityName,
                entityId,
                details,
                DateTime.UtcNow,
                applicationUserId));
        }
    }
}

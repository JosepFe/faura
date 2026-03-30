using Faura.Infrastructure.UnitOfWork.Repositories;
using Faura.WebAPI.Domain.Entities;

namespace Faura.WebAPI.Domain;

/// <summary>
/// Repository interface for Sample entity.
/// Inherits all common repository operations from IEntityRepository.
/// </summary>
public interface ISampleRepository : IEntityRepository<Sample> { }

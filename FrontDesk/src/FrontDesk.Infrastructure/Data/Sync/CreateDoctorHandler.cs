using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrontDesk.Infrastructure.Data.Sync
{
  public class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand>
  {
    private readonly AppDbContext _dbContext;
    private readonly ILogger<CreateDoctorHandler> _logger;

    public CreateDoctorHandler(AppDbContext dbContext,
      ILogger<CreateDoctorHandler> logger)
    {
      _dbContext = dbContext;
      _logger = logger;
    }

    public async Task<Unit> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
      _logger.LogInformation($"Creating new Doctor in FrontDesk database: {request.Name}");
      string insertSQLFormat = "SET IDENTITY_INSERT {0} ON\nINSERT INTO {0} (Id, Name) VALUES (@Id, @Name)\nSET IDENTITY_INSERT {0} OFF";

      string command = string.Format(insertSQLFormat, "Doctors");
      var idParam = new SqlParameter("@Id", request.Id);
      var nameParam = new SqlParameter("@Name", request.Name);
      await _dbContext.Database.ExecuteSqlRawAsync(command, idParam, nameParam);

      return Unit.Value;
    }
  }
}

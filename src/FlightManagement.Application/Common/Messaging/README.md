# CQRS Pattern with MediatR

This folder contains the base interfaces for implementing the CQRS (Command Query Responsibility Segregation) pattern using MediatR.

## Base Interfaces

### Commands
- **ICommand**: For commands that don't return a value (e.g., Delete, Update)
- **ICommand<TResponse>**: For commands that return a value (e.g., Create returns ID)
- **ICommandHandler<TCommand>**: Handler for commands without return value
- **ICommandHandler<TCommand, TResponse>**: Handler for commands with return value

### Queries
- **IQuery<TResponse>**: For queries that return data
- **IQueryHandler<TQuery, TResponse>**: Handler for queries

## Usage Examples

All commands and queries return a `Result` or `Result<T>` to provide consistent error handling.

### Creating a Command

```csharp
public record CreateFlightCommand(
    string FlightNumber,
    string Origin,
    string Destination,
    DateTime DepartureTime
) : ICommand<Guid>;

public class CreateFlightCommandHandler : ICommandHandler<CreateFlightCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateFlightCommandHandler> _logger;

    public CreateFlightCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateFlightCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        CreateFlightCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating flight {FlightNumber}", request.FlightNumber);

            var flight = new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = request.FlightNumber,
                Origin = request.Origin,
                Destination = request.Destination,
                DepartureTime = request.DepartureTime
            };

            var repository = _unitOfWork.Repository<Flight>();
            await repository.AddAsync(flight, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Flight created with ID: {Id}", flight.Id);

            return Result<Guid>.Success(flight.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating flight");
            return Result<Guid>.Failure($"Failed to create flight: {ex.Message}");
        }
    }
}
```

### Creating a Query

```csharp
public record GetFlightByIdQuery(Guid Id) : IQuery<FlightDto>;

public class GetFlightByIdQueryHandler : IQueryHandler<GetFlightByIdQuery, FlightDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFlightByIdQueryHandler> _logger;

    public GetFlightByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetFlightByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FlightDto>> Handle(
        GetFlightByIdQuery request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<Flight>();
            var flight = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (flight == null)
                return Result<FlightDto>.Failure($"Flight with ID {request.Id} not found");

            var dto = new FlightDto(
                flight.Id,
                flight.FlightNumber,
                flight.Origin,
                flight.Destination,
                flight.DepartureTime
            );

            return Result<FlightDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting flight");
            return Result<FlightDto>.Failure($"Failed to get flight: {ex.Message}");
        }
    }
}
```

### Using in Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FlightsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFlightCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetFlightByIdQuery(id));
        
        if (!result.IsSuccess)
            return NotFound(result.Error);
            
        return Ok(result.Data);
    }
}
```

## Examples

Check the `Examples` folder for complete examples of:
- CreateEntityCommand
- UpdateEntityCommand
- DeleteEntityCommand
- GetEntityByIdQuery
- GetAllEntitiesQuery (with pagination)


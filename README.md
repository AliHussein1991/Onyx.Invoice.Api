# Assignment - Tønsberg

This repository contains the solution to the development assignment for Tønsberg. including LINQ queries, unit testing, API development, and VAT verification.

## Project Structure

The project is organized into the following main layers:
- **API**: Contains the controllers for handling HTTP requests and API responses.
- **Core**: Contains the core business logic, including services and interfaces.
- **Infrastructure**: Contains the database context, repositories, and raw SQL queries.

The solution uses the following technologies:
- **Entity Framework Core**: For data access.
- **Fluent Validation**: For validating input models.
- **XUnit and Moq**: For unit testing.

---

## Task 1: LINQ Expressions and SQL Queries

### a) Repeated Guest Names
A LINQ expression was written to find all guest names that appear more than once across all invoice groups and invoices:
in InvoiceRepository
```csharp
public async Task<IEnumerable<string>> GetRepeatedGuestNamesAsync()
{
    IEnumerable<string> repeatedGuestNames = await _dbContext.InvoiceGroup
        .SelectMany(ig => ig.Invoices)
        .SelectMany(i => i.Observations)
        .GroupBy(o => o.GuestName)
        .Where(g => g.Count() > 1)
        .Select(g => g.Key)
        .ToListAsync();

    return repeatedGuestNames;
}
```
b) Total Number of Nights per Travel Agent for 2015
A LINQ expression was written to find the total number of nights for each travel agent for invoice groups issued in 2015:

```csharp
public async Task<IEnumerable<TravelAgentInfo>> GetTotalNightsByTravelAgentAsync(int year)
{
    IEnumerable<TravelAgentInfo> numberOfNightsByTravelAgent  = await _dbContext.InvoiceGroup
        .Where(ig => ig.IssueDate.Year == year)
        .SelectMany(ig => ig.Invoices)
        .SelectMany(i => i.Observations)
        .GroupBy(o => o.TravelAgent)
        .Select(g => new TravelAgentInfo
        {
            TravelAgent = g.Key,
            TotalNumberOfNights = g.Sum(o => o.NumberOfNights)
        })
        .ToListAsync();
    return numberOfNightsByTravelAgent;
}
```
c) SQL Query for Travel Agents with No Observations
The SQL query to find travel agents that do not have any observations is as follows:

sql
```csharp
SELECT DISTINCT t.TravelAgent
FROM TravelAgent t
LEFT JOIN Observation o ON t.TravelAgent = o.TravelAgent
WHERE o.TravelAgent IS NULL;
```
d) SQL Query for Travel Agents with More Than Two Observations
The SQL query to find travel agents that have more than two observations is as follows:

sql
```csharp
SELECT o.TravelAgent, COUNT(o.Id) AS ObservationCount
FROM Observation o
GROUP BY o.TravelAgent
HAVING COUNT(o.Id) > 2;
```
Task 2: Logger Refactoring and Unit Testing
a) Refactor Logger Class for Unit Testing
The Logger class was refactored to enable unit testing by abstracting the time provider and using dependency injection:

```csharp
public class Logger
{
    private readonly StreamWriter _writer;
    private readonly ITimeProvider _timeProvider;

    public Logger(StreamWriter writer, ITimeProvider timeProvider)
    {
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

        Log("Logger initialized");
    }

    public void Log(string str)
    {
        var timestamp = _timeProvider.GetCurrentTime();
        _writer.WriteLine($"[{timestamp:dd.MM.yy HH:mm:ss}] {str}");
    }
}
```

b) Unit Test for Logger
The unit test checks if the Log method correctly prefixes the log message with a timestamp:

```csharp
public class LoggerTests
{
    private readonly DateTime _fixedDateTime;
    private readonly Mock<ITimeProvider> _mockTimeProvider;

    public LoggerTests()
    {
        _fixedDateTime = new DateTime(2023, 12, 3, 14, 30, 0);
        _mockTimeProvider = new Mock<ITimeProvider>();
        _mockTimeProvider.Setup(tp => tp.GetCurrentTime()).Returns(_fixedDateTime);
    }

    [Fact]
    public void Log_ShouldPrefixWithTimestamp()
    {
        var logMessage = "Logger initialized";
        var expectedMessage = $"[03.12.23 14:30:00] {logMessage}";

        using (var memoryStream = new MemoryStream())
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            var logger = new Logger(streamWriter, _mockTimeProvider.Object);

            logger.Log(logMessage);
            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            var output = new StreamReader(memoryStream).ReadToEnd().Trim();
            Assert.Contains(expectedMessage, output);
        }
    }
}
```

Task 3: VAT Verification Service
a) Implementing the Verify Method for VAT Verification
The VatVerifier class was implemented to verify VAT IDs using the EU VIES web service:

```csharp
public class VatVerifier : IVatVerifier
{
    private readonly IValidator<VatVerificationRequest> _validator;
    private readonly checkVatPortTypeClient _client;
    private readonly ILogger<VatVerifier> _logger;

    public VatVerifier(IValidator<VatVerificationRequest> validator, checkVatPortTypeClient client, ILogger<VatVerifier> logger)
    {
        _validator = validator;
        _client = client;
        _logger = logger;
    }

    public async Task<VerificationStatus> VerifyAsync(VatVerificationRequest vatRequest)
    {
        _logger.LogInformation("Starting VAT verification for CountryCode: {CountryCode}, VAT ID: {VatId}",
            vatRequest.CountryCode, vatRequest.VatId);

        try
        {
            await ValidateRequestAsync(vatRequest);

            var response = await _client.checkVatAsync(new checkVatRequest
            {
                countryCode = vatRequest.CountryCode,
                vatNumber = vatRequest.VatId
            });

            var status = response.valid ? VerificationStatus.Valid : VerificationStatus.Invalid;

            return status;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during VAT verification.");
            return VerificationStatus.Unavailable;
        }
    }

    private async Task ValidateRequestAsync(VatVerificationRequest vatRequest)
    {
        var validationResult = await _validator.ValidateAsync(vatRequest);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}
```

Task 4: VAT Verification API Controller
a) API Controller for VAT Verification
The VatVerificationController handles POST requests to verify VAT IDs:

```csharp
[Route("api/[controller]")]
[ApiController]
public class VatVerificationController : ControllerBase
{
    private readonly IVatVerifier _vatVerifier;

    public VatVerificationController(IVatVerifier vatVerifier)
    {
        _vatVerifier = vatVerifier;
    }

    [HttpPost]
    public async Task<IActionResult> VerifyVatId([FromBody] VatVerificationRequest request)
    {
        try
        {
            var result = await _vatVerifier.VerifyAsync(request);

            return result switch
            {
                VerificationStatus.Valid => Ok(new VatVerificationResponse { Status = "Valid", Message = "The VAT ID is valid." }),
                VerificationStatus.Invalid => Ok(new VatVerificationResponse { Status = "Invalid", Message = "The VAT ID is invalid." }),
                VerificationStatus.Unavailable => StatusCode(503, new VatVerificationResponse { Status = "Unavailable", Message = "The service is unavailable." }),
                _ => StatusCode(500, new { Status = "Error", Message = "An unknown error occurred." })
            };
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

Dependencies
This project uses the following NuGet packages:

Microsoft.EntityFrameworkCore,
FluentValidation,
Moq,
XUnit

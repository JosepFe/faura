# Development Commands

## Build & Run

```bash
# Restore NuGet packages
dotnet restore

# Build entire solution
dotnet build

# Build specific project
dotnet build Template.API/Template.API.csproj

# Run the API
dotnet run --project Template.API/Template.API.csproj

# Run with specific environment
dotnet run --project Template.API/Template.API.csproj --environment Production

# Watch mode (auto-reload on changes)
dotnet watch --project Template.API/Template.API.csproj
```

## Docker

```bash
# Start all services (MongoDB)
docker-compose up -d

# Stop all services
docker-compose down

# View logs
docker-compose logs -f

# Restart services
docker-compose restart

# Remove volumes (clean database)
docker-compose down -v
```

## Database

### MongoDB

```bash
# Connect to MongoDB
docker exec -it template_mongodb mongosh -u admin -p password

# List databases
show dbs

# Use database
use sample_db

# List collections
show collections

# Query data
db.sample_items.find()

# Count documents
db.sample_items.countDocuments()

# Drop collection
db.sample_items.drop()
```

## Testing

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true

# Run specific test project
dotnet test Tests/YourModule.Tests/YourModule.Tests.csproj

# Run tests matching a filter
dotnet test --filter "FullyQualifiedName~CreateProduct"

# Run tests with detailed output
dotnet test --verbosity detailed
```

## Code Quality

```bash
# Format code
dotnet format

# Analyze code (StyleCop + SonarAnalyzer)
dotnet build -t:Rebuild

# List analyzers
dotnet build -getProperty:Analyzers
```

## NuGet Packages

```bash
# List outdated packages
dotnet list package --outdated

# Update package
dotnet add package PackageName --version x.x.x

# Remove package
dotnet remove package PackageName

# Clear NuGet cache
dotnet nuget locals all --clear
```

## Project Management

```bash
# Add new project to solution
dotnet sln add Path/To/Project.csproj

# Remove project from solution
dotnet sln remove Path/To/Project.csproj

# List projects in solution
dotnet sln list

# Add project reference
dotnet add Project1.csproj reference Project2.csproj
```

## Creating New Module

```bash
# Create Domain project
dotnet new classlib -n YourModule.Domain -o Modules/YourModule/YourModule.Domain

# Create Application project
dotnet new classlib -n YourModule.Application -o Modules/YourModule/YourModule.Application

# Create Infrastructure project
dotnet new classlib -n YourModule.Infrastructure -o Modules/YourModule/YourModule.Infrastructure

# Add projects to solution
dotnet sln add Modules/YourModule/YourModule.Domain/YourModule.Domain.csproj
dotnet sln add Modules/YourModule/YourModule.Application/YourModule.Application.csproj
dotnet sln add Modules/YourModule/YourModule.Infrastructure/YourModule.Infrastructure.csproj

# Add project references
dotnet add Modules/YourModule/YourModule.Application/YourModule.Application.csproj reference Modules/YourModule/YourModule.Domain/YourModule.Domain.csproj
dotnet add Modules/YourModule/YourModule.Infrastructure/YourModule.Infrastructure.csproj reference Modules/YourModule/YourModule.Application/YourModule.Application.csproj
```

## API Testing

### Using curl

```bash
# Health check
curl http://localhost:5000/health

# Create item
curl -X POST http://localhost:5000/api/sample-items \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Item",
    "description": "Description",
    "category": 1,
    "tags": ["tag1", "tag2"]
  }'

# Get item by ID
curl http://localhost:5000/api/sample-items/{id}

# Get all items
curl "http://localhost:5000/api/sample-items?pageNumber=1&pageSize=10&searchTerm=test"

# Update item
curl -X PUT http://localhost:5000/api/sample-items/{id} \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Name",
    "description": "Updated Description",
    "category": 2,
    "status": 1,
    "tags": ["updated"]
  }'

# Delete item
curl -X DELETE http://localhost:5000/api/sample-items/{id}
```

### Using PowerShell (Invoke-RestMethod)

```powershell
# Create item
$body = @{
    name = "Test Item"
    description = "Description"
    category = 1
    tags = @("tag1", "tag2")
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/sample-items" -Method Post -Body $body -ContentType "application/json"

# Get all items
Invoke-RestMethod -Uri "http://localhost:5000/api/sample-items?pageNumber=1&pageSize=10"

# Get item by ID
Invoke-RestMethod -Uri "http://localhost:5000/api/sample-items/{id}"
```

## Troubleshooting

```bash
# Clean build artifacts
dotnet clean

# Rebuild
dotnet build --no-incremental

# Clear NuGet cache
dotnet nuget locals all --clear

# Restore with verbose output
dotnet restore -v detailed

# Check .NET SDK version
dotnet --version

# List installed SDKs
dotnet --list-sdks

# List installed runtimes
dotnet --list-runtimes
```

## Useful Git Commands

```bash
# Initialize repository
git init

# Add all files
git add .

# Commit
git commit -m "Initial commit"

# Create .gitignore (already included in template)
# See .gitignore file

# Check status
git status

# View changes
git diff
```

## Performance Profiling

```bash
# Run with detailed logging
dotnet run --project Template.API/Template.API.csproj -- --Logging:MinimumLevel:Default=Debug

# Profile memory usage
dotnet-trace collect --process-id <PID>

# Profile CPU usage
dotnet-counters monitor --process-id <PID>
```

## Environment Variables

```bash
# Set environment variable (PowerShell)
$env:ASPNETCORE_ENVIRONMENT="Development"

# Set environment variable (Bash)
export ASPNETCORE_ENVIRONMENT=Development

# Use user secrets for sensitive data
dotnet user-secrets init --project Template.API/Template.API.csproj
dotnet user-secrets set "Sample:MongoDb:ConnectionString" "your-connection-string"
```

## Continuous Integration

Example GitHub Actions workflow:

```yaml
name: Build and Test

on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 8.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
```

---

Save these commands for quick reference during development!

param(
    [string]$dataDir = "C:/pantry_pulse/data/db",  # Specify the data directory
    [string]$dockerInstallerUri = "https://desktop.docker.com/win/stable/Docker%20Desktop%20Installer.exe"  # Docker Desktop Installer URL
)

# Check if Docker is installed
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Host "Docker is not found. Installing Docker..."

    # Download Docker Desktop Installer
    Invoke-WebRequest -OutFile DockerDesktopInstaller.exe -Uri $dockerInstallerUri

    # Install Docker Desktop
    Start-Process -Wait -FilePath .\DockerDesktopInstaller.exe

    # Clean up the installer
    Remove-Item -Force DockerDesktopInstaller.exe

    Write-Host "Docker installed successfully. Please restart your system for the changes to take effect."
    exit
}

# Function to check if Docker is running
function Test-DockerRunning {
    $dockerInfo = docker info 2>&1
    return ($LASTEXITCODE -eq 0)
}

# Check if Docker is running
if (-not (Test-DockerRunning)) {
    Write-Host "Docker is not running. Attempting to start Docker Desktop..."
    Write-Host "Ensure that WSL is up to date. Use 'wsl --update' to update."

    Start-Process -FilePath "C:\Program Files\Docker\Docker\Docker Desktop.exe"
    Start-Sleep -Seconds 30  # Wait for Docker to start

    # Check if Docker started successfully
    if (Test-DockerRunning) {
        Write-Host "Docker started successfully."
    } else {
        Write-Host "Failed to start Docker. Please start Docker Desktop manually."
        exit
    }
}

# Check if Docker Compose is installed
if (-not (Get-Command docker-compose -ErrorAction SilentlyContinue)) {
    Write-Host "Docker Compose is not found. Please ensure Docker Desktop is running, as it comes with Docker Compose."
    exit
}

# Create the data directory if it does not exist
if (-not (Test-Path -Path $dataDir)) {
    New-Item -ItemType Directory -Force -Path $dataDir
}

# Start Docker Compose and build the services
docker-compose up --build
# Quickstart Validation Script
# Purpose: Verify all function tools mentioned in quickstart.md are working correctly
# Usage: .\validate-quickstart.ps1 -AgentUrl "http://localhost:5000"

param(
    [Parameter(Mandatory = $false)]
    [string]$AgentUrl = "http://localhost:5000",
    
    [Parameter(Mandatory = $false)]
    [switch]$Verbose
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Quickstart Validation Script" -ForegroundColor Cyan
Write-Host "Agent URL: $AgentUrl" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Test counter
$testsPassed = 0
$testsFailed = 0
$testsTotal = 0

# Helper function to test API endpoint
function Test-AgentEndpoint {
    param(
        [string]$TestName,
        [string]$Method,
        [string]$Endpoint,
        [hashtable]$Body = @{},
        [scriptblock]$Validation
    )
    
    $script:testsTotal++
    Write-Host "Test $script:testsTotal`: $TestName" -ForegroundColor Yellow
    
    try {
        $uri = "$AgentUrl$Endpoint"
        $headers = @{
            "Content-Type" = "application/json"
        }
        
        $params = @{
            Uri     = $uri
            Method  = $Method
            Headers = $headers
        }
        
        if ($Method -eq "POST" -and $Body.Count -gt 0) {
            $params.Body = ($Body | ConvertTo-Json -Depth 10)
        }
        
        if ($Verbose) {
            Write-Host "  → Request: $Method $uri" -ForegroundColor Gray
            if ($Body.Count -gt 0) {
                Write-Host "  → Body: $($params.Body)" -ForegroundColor Gray
            }
        }
        
        $response = Invoke-RestMethod @params
        
        if ($Verbose) {
            Write-Host "  → Response: $($response | ConvertTo-Json -Depth 5)" -ForegroundColor Gray
        }
        
        # Run validation
        if ($null -ne $Validation) {
            $validationResult = & $Validation $response
            if (-not $validationResult) {
                throw "Validation failed for test: $TestName"
            }
        }
        
        Write-Host "  ✓ PASS`n" -ForegroundColor Green
        $script:testsPassed++
        return $response
    }
    catch {
        Write-Host "  ✗ FAIL: $($_.Exception.Message)`n" -ForegroundColor Red
        $script:testsFailed++
        return $null
    }
}

# Health check
Write-Host "`n=== HEALTH CHECK ===" -ForegroundColor Magenta
Test-AgentEndpoint -TestName "Agent Health Check" -Method "GET" -Endpoint "/health" -Validation {
    param($response)
    return $response.status -eq "healthy" -or $response.status -eq "OK"
}

# User Story 1: Campaign Management
Write-Host "`n=== USER STORY 1: CAMPAIGN MANAGEMENT ===" -ForegroundColor Magenta

$campaign1 = Test-AgentEndpoint `
    -TestName "Create Campaign: Navidad Solidaria" `
    -Method "POST" `
    -Endpoint "/api/agent/tool/crear-campana" `
    -Body @{ nombre = "Navidad Solidaria"; meta_euros = 5000 } `
    -Validation {
        param($response)
        return $response.exito -eq $true -and $response.id
    }

$campaign2 = Test-AgentEndpoint `
    -TestName "Create Campaign: Educación Rural" `
    -Method "POST" `
    -Endpoint "/api/agent/tool/crear-campana" `
    -Body @{ nombre = "Educación Rural"; meta_euros = 3000 } `
    -Validation {
        param($response)
        return $response.exito -eq $true -and $response.id
    }

$campaigns = Test-AgentEndpoint `
    -TestName "List All Campaigns" `
    -Method "GET" `
    -Endpoint "/api/agent/tool/listar-campanas" `
    -Validation {
        param($response)
        return $response.total -ge 2
    }

if ($campaign1 -and $campaign1.id) {
    Test-AgentEndpoint `
        -TestName "Get Campaign Details" `
        -Method "GET" `
        -Endpoint "/api/agent/tool/obtener-campana?campaña_id=$($campaign1.id)" `
        -Validation {
            param($response)
            return $response.nombre -eq "Navidad Solidaria"
        }
}

# User Story 2: Task Management
Write-Host "`n=== USER STORY 2: TASK MANAGEMENT ===" -ForegroundColor Magenta

$users = Test-AgentEndpoint `
    -TestName "List Available Users" `
    -Method "GET" `
    -Endpoint "/api/agent/tool/listar-usuarios" `
    -Validation {
        param($response)
        return $response.usuarios.Count -ge 3
    }

$task1 = $null
if ($campaign1 -and $campaign1.id) {
    $task1 = Test-AgentEndpoint `
        -TestName "Add Task to Campaign" `
        -Method "POST" `
        -Endpoint "/api/agent/tool/agregar-tarea" `
        -Body @{ 
            campaña_id  = $campaign1.id
            descripcion = "Contactar donantes principales"
        } `
        -Validation {
            param($response)
            return $response.exito -eq $true -and $response.id
        }
}

if ($task1 -and $task1.id -and $users -and $users.usuarios.Count -gt 0) {
    Test-AgentEndpoint `
        -TestName "Assign Task to User" `
        -Method "POST" `
        -Endpoint "/api/agent/tool/asignar-tarea" `
        -Body @{ 
            tarea_id   = $task1.id
            usuario_id = $users.usuarios[0].id
        } `
        -Validation {
            param($response)
            return $response.exito -eq $true
        }
    
    Test-AgentEndpoint `
        -TestName "Move Task to In Progress" `
        -Method "POST" `
        -Endpoint "/api/agent/tool/mover-tarea" `
        -Body @{ 
            tarea_id      = $task1.id
            nueva_columna = "en_progreso"
        } `
        -Validation {
            param($response)
            return $response.exito -eq $true
        }
}

if ($campaign1 -and $campaign1.id) {
    Test-AgentEndpoint `
        -TestName "Get Kanban Board" `
        -Method "GET" `
        -Endpoint "/api/agent/tool/obtener-tablero-kanban?campaña_id=$($campaign1.id)" `
        -Validation {
            param($response)
            return $response.campaña_id -eq $campaign1.id
        }
}

# User Story 3: Collaborative Comments
Write-Host "`n=== USER STORY 3: COLLABORATIVE COMMENTS ===" -ForegroundColor Magenta

$comment1 = $null
if ($task1 -and $task1.id) {
    $comment1 = Test-AgentEndpoint `
        -TestName "Add Comment to Task" `
        -Method "POST" `
        -Endpoint "/api/agent/tool/agregar-comentario" `
        -Body @{ 
            tarea_id = $task1.id
            texto    = "Llamé a 10 donantes hoy, 3 confirmaron su participación"
        } `
        -Validation {
            param($response)
            return $response.exito -eq $true -and $response.id
        }
    
    Test-AgentEndpoint `
        -TestName "List Task Comments" `
        -Method "GET" `
        -Endpoint "/api/agent/tool/listar-comentarios?tarea_id=$($task1.id)" `
        -Validation {
            param($response)
            return $response.comentarios.Count -ge 1
        }
}

# User Story 4: AI Thank-You Messages
Write-Host "`n=== USER STORY 4: AI THANK-YOU MESSAGES ===" -ForegroundColor Magenta

if ($campaign1 -and $campaign1.nombre) {
    Test-AgentEndpoint `
        -TestName "Generate Thank-You Message" `
        -Method "POST" `
        -Endpoint "/api/agent/tool/generar-agradecimiento" `
        -Body @{ 
            nombre_donante = "María García"
            campaña_nombre = $campaign1.nombre
            monto_euros    = 100
        } `
        -Validation {
            param($response)
            return $response.texto_generado -and $response.texto_generado.Length -gt 50
        }
}

# Summary
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "VALIDATION SUMMARY" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Total Tests:  $testsTotal" -ForegroundColor White
Write-Host "Passed:       $testsPassed" -ForegroundColor Green
Write-Host "Failed:       $testsFailed" -ForegroundColor $(if ($testsFailed -eq 0) { "Green" } else { "Red" })
Write-Host "Success Rate: $(if ($testsTotal -gt 0) { [math]::Round(($testsPassed / $testsTotal) * 100, 2) } else { 0 })%" -ForegroundColor White
Write-Host "========================================`n" -ForegroundColor Cyan

if ($testsFailed -eq 0) {
    Write-Host "✓ All tests passed! The agent is ready for demo." -ForegroundColor Green
    exit 0
}
else {
    Write-Host "✗ Some tests failed. Please review the errors above." -ForegroundColor Red
    exit 1
}

# Agent Fundraising - Agente de Recaudación de Fondos para ONGs

Agente conversacional de IA para gestión de campañas de recaudación de fondos, tareas y mensajes de agradecimiento.

## 🚀 Características

- **Gestión de Campañas**: Crear y listar campañas de recaudación
- **Tablero Kanban**: Gestión visual de tareas con columnas (Por Hacer, En Progreso, Completado)
- **Asignación de Tareas**: Asignar tareas a coordinadores y voluntarios
- **Comentarios**: Sistema de comunicación en tareas
- **IA Conversacional**: Interfaz en lenguaje natural (español) para todas las operaciones
- **Microsoft Agent Framework**: Integración con Azure AI Foundry y Azure OpenAI

## 📋 Requisitos

- .NET 10 SDK
- Azure AI Foundry Project (opcional - modo demo disponible)
- Azure OpenAI Service con modelo GPT-4o-mini (opcional)

## ⚙️ Configuración

### 1. Clonar el repositorio

```bash
git clone <repository-url>
cd 2025-non-profit-community-day-spain
```

### 2. Configurar Azure AI Foundry (Opcional)

Edita `src/Demo.Agent.Fundraising/appsettings.Development.json`:

```json
{
  "AzureAI": {
    "ProjectEndpoint": "https://YOUR-PROJECT.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME",
    "ModelDeploymentName": "gpt-4o-mini"
  }
}
```

**Sin configuración**: El agente funcionará en modo demo con respuestas predefinidas.

### 3. Restaurar paquetes

```bash
cd src/Demo.Agent.Fundraising
dotnet restore
```

## 🏃 Ejecución

```bash
dotnet run
```

La aplicación estará disponible en: `https://localhost:5001`

## 📡 Endpoints

### Health Check

```http
GET /health
```

Respuesta:

```json
{
  "status": "healthy",
  "timestamp": "2025-05-20T10:30:00Z",
  "service": "Fundraising Agent"
}
```

### Chat con el Agente

```http
POST /agent/chat
Content-Type: application/json

{
  "message": "Lista todas las campañas activas"
}
```

Respuesta:

```json
{
  "message": "Aquí están las campañas activas:\n1. Navidad Solidaria - Meta: 5000€...",
  "timestamp": "2025-05-20T10:31:00Z"
}
```

## 💬 Ejemplos de Conversación

```text
Usuario: "Lista todas las campañas"
Agente: [Llama a ListCampaigns() y muestra 3 campañas]

Usuario: "Muéstrame el tablero Kanban de la campaña Navidad Solidaria"
Agente: [Llama a GetKanbanBoard(campaignId) y muestra tareas por columna]

Usuario: "Agrega una nueva tarea 'Diseñar flyers' a la campaña de Educación Rural"
Agente: [Llama a AddTask(campaignId, description) y confirma creación]

Usuario: "Asigna la tarea de diseño a María"
Agente: [Llama a ListUsers(), identifica a María, llama a AssignTask(taskId, userId)]
```

## 🗂️ Datos de Ejemplo

El sistema incluye datos de ejemplo pre-cargados:

### Campañas

1. **Navidad Solidaria** - Meta: 5000€ (5 tareas)
2. **Educación Rural** - Meta: 3000€ (5 tareas)
3. **Salud Comunitaria** - Meta: 4000€ (5 tareas)

### Usuarios

- Ana García (Coordinador)
- María López (Voluntario)
- Carlos Ruiz (Voluntario)

## 🛠️ Herramientas del Agente

El agente tiene acceso a 11 funciones:

**Campañas**:

- `CreateCampaign(name, goalEuros)`
- `ListCampaigns()`
- `GetCampaign(campaignId)`

**Tareas**:

- `AddTask(campaignId, description, assignedTo?)`
- `MoveTask(taskId, newColumn)`
- `AssignTask(taskId, userId)`
- `GetKanbanBoard(campaignId)`
- `ListUsers()`

**Comentarios**:

- `AddComment(taskId, text)`
- `ListComments(taskId)`

## 📦 Arquitectura

```text
src/Demo.Agent.Fundraising/
├── Agent/
│   ├── FundraisingAgent.cs    # Clase principal del agente
│   ├── Tools.cs                # 11 function tools
│   └── Instructions.cs         # Prompts del sistema en español
├── Models/
│   ├── Campaign.cs             # Modelo de campaña
│   ├── CampaignTask.cs         # Modelo de tarea
│   ├── User.cs                 # Modelo de usuario
│   ├── TaskComment.cs          # Modelo de comentario
│   └── AgentState.cs           # Estado en memoria
├── Validators/
│   ├── CampaignValidator.cs
│   ├── TaskValidator.cs
│   └── CommentValidator.cs
├── Data/
│   └── SampleData.cs           # Datos de ejemplo
└── Program.cs                  # Configuración y endpoints
```

## 🔐 Autenticación con Azure

El agente utiliza `DefaultAzureCredential` que intenta múltiples métodos de autenticación:

1. Variables de entorno
2. Managed Identity
3. Azure CLI (`az login`)
4. Visual Studio
5. VS Code Azure Account

Para desarrollo local:

```bash
az login
```

## 🧪 Modo Demo

Sin configuración de Azure, el agente funciona en modo demo:

- ✅ Todas las function tools funcionan (gestión de campañas, tareas, comentarios)
- ✅ Datos de ejemplo precargados
- ❌ Sin comprensión de lenguaje natural
- ❌ Sin generación de respuestas con IA

Mensaje de modo demo:

```text
"Agent no inicializado. Configura las credenciales de Azure AI Foundry para habilitar la funcionalidad completa."
```

## 📝 Próximos Pasos

### En Desarrollo

- [ ] Generación de mensajes de agradecimiento con IA
- [ ] Persistencia en base de datos
- [ ] Autenticación y autorización
- [ ] M365 Copilot integration

### Infraestructura

- [ ] Azure App Service deployment
- [ ] Terraform IaC
- [ ] CI/CD pipeline
- [ ] Monitoring y Application Insights

## 📄 Licencia

Ver `LICENSE` para más detalles.

## 🤝 Contribuir

Este proyecto se desarrolla como parte del Community Day Spain 2025 para ONGs.

## 📧 Contacto

Para preguntas o sugerencias, abre un issue en GitHub.

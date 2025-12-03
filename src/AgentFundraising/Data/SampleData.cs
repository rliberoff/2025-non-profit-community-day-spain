using AgentFundraising.Models;

namespace AgentFundraising.Data;

/// <summary>
/// Provides sample data for agent demonstration.
/// Pre-loads 3 campaigns with tasks distributed across the three Kanban board columns.
/// </summary>
public static class SampleData
{
    /// <summary>
    /// Initializes the agent state with 3 sample campaigns and their associated tasks.
    /// </summary>
    /// <param name="state">Agent state to initialize.</param>
    public static void InitializeSampleData(AgentState state)
    {
        var now = DateTime.UtcNow;

        // ========== CAMPAIGN 1: Navidad Solidaria ==========
        var c1 = "c1-navidad-2025";
        var c1Tasks = new List<string>();

        // To do tasks
        var t1 = Guid.NewGuid().ToString();
        state.Tasks[t1] = new CampaignTask(
            Id: t1,
            CampaignId: c1,
            Description: "Contactar 20 donantes principales",
            Column: KanbanColumn.ToDo,
            AssignedTo: "u1-coordinadora",
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-5)
        );
        c1Tasks.Add(t1);

        var t2 = Guid.NewGuid().ToString();
        state.Tasks[t2] = new CampaignTask(
            Id: t2,
            CampaignId: c1,
            Description: "Diseñar materiales de campaña",
            Column: KanbanColumn.ToDo,
            AssignedTo: "u2-voluntario",
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-4)
        );
        c1Tasks.Add(t2);

        var t3 = Guid.NewGuid().ToString();
        state.Tasks[t3] = new CampaignTask(
            Id: t3,
            CampaignId: c1,
            Description: "Organizar evento de lanzamiento",
            Column: KanbanColumn.ToDo,
            AssignedTo: null,
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-3)
        );
        c1Tasks.Add(t3);

        // In progress tasks
        var t4 = Guid.NewGuid().ToString();
        state.Tasks[t4] = new CampaignTask(
            Id: t4,
            CampaignId: c1,
            Description: "Enviar emails de agradecimiento",
            Column: KanbanColumn.InProgress,
            AssignedTo: "u3-voluntario",
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-6)
        );
        c1Tasks.Add(t4);

        // Done tasks
        var t5 = Guid.NewGuid().ToString();
        state.Tasks[t5] = new CampaignTask(
            Id: t5,
            CampaignId: c1,
            Description: "Configurar página de donaciones",
            Column: KanbanColumn.Done,
            AssignedTo: "u1-coordinadora",
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-10)
        );
        c1Tasks.Add(t5);

        state.Campaigns[c1] = new Campaign(
            Id: c1,
            Name: "Navidad Solidaria",
            GoalEuros: 5000.0m,
            CreatedAt: now.AddDays(-10),
            TaskIds: c1Tasks
        );

        // ========== CAMPAIGN 2: Educación Rural ==========
        var c2 = "c2-educacion-rural-2025";
        var c2Tasks = new List<string>();

        // To do tasks
        var t6 = Guid.NewGuid().ToString();
        state.Tasks[t6] = new CampaignTask(
            Id: t6,
            CampaignId: c2,
            Description: "Reunión con escuelas rurales",
            Column: KanbanColumn.ToDo,
            AssignedTo: null,
            CommentIds: [],
            CreatedAt: now.AddDays(-2)
        );
        c2Tasks.Add(t6);

        var t7 = Guid.NewGuid().ToString();
        state.Tasks[t7] = new CampaignTask(
            Id: t7,
            CampaignId: c2,
            Description: "Preparar propuesta de proyecto",
            Column: KanbanColumn.ToDo,
            AssignedTo: "u1-coordinadora",
            CommentIds: [],
            CreatedAt: now.AddDays(-1)
        );
        c2Tasks.Add(t7);

        var t8 = Guid.NewGuid().ToString();
        state.Tasks[t8] = new CampaignTask(
            Id: t8,
            CampaignId: c2,
            Description: "Solicitar subvenciones",
            Column: KanbanColumn.ToDo,
            AssignedTo: null,
            CommentIds: [],
            CreatedAt: now.AddDays(-1)
        );
        c2Tasks.Add(t8);

        // In progress tasks
        var t9 = Guid.NewGuid().ToString();
        state.Tasks[t9] = new CampaignTask(
            Id: t9,
            CampaignId: c2,
            Description: "Reclutar voluntarios docentes",
            Column: KanbanColumn.InProgress,
            AssignedTo: "u2-voluntario",
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-5)
        );
        c2Tasks.Add(t9);

        // Done tasks
        var t10 = Guid.NewGuid().ToString();
        state.Tasks[t10] = new CampaignTask(
            Id: t10,
            CampaignId: c2,
            Description: "Investigar necesidades educativas",
            Column: KanbanColumn.Done,
            AssignedTo: "u3-voluntario",
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-8)
        );
        c2Tasks.Add(t10);

        state.Campaigns[c2] = new Campaign(
            Id: c2,
            Name: "Educación Rural",
            GoalEuros: 3000.0m,
            CreatedAt: now.AddDays(-8),
            TaskIds: c2Tasks
        );

        // ========== CAMPAIGN 3: Salud Comunitaria ==========
        var c3 = "c3-salud-comunitaria-2025";
        var c3Tasks = new List<string>();

        // To do tasks
        var t11 = Guid.NewGuid().ToString();
        state.Tasks[t11] = new CampaignTask(
            Id: t11,
            CampaignId: c3,
            Description: "Coordinar con centros de salud",
            Column: KanbanColumn.ToDo,
            AssignedTo: null,
            CommentIds: new List<string>(),
            CreatedAt: now.AddHours(-12)
        );
        c3Tasks.Add(t11);

        var t12 = Guid.NewGuid().ToString();
        state.Tasks[t12] = new CampaignTask(
            Id: t12,
            CampaignId: c3,
            Description: "Planificar jornadas de salud",
            Column: KanbanColumn.ToDo,
            AssignedTo: "u1-coordinadora",
            CommentIds: new List<string>(),
            CreatedAt: now.AddHours(-6)
        );
        c3Tasks.Add(t12);

        // In progress tasks
        var t13 = Guid.NewGuid().ToString();
        state.Tasks[t13] = new CampaignTask(
            Id: t13,
            CampaignId: c3,
            Description: "Comprar material médico",
            Column: KanbanColumn.InProgress,
            AssignedTo: "u2-voluntario",
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-3)
        );
        c3Tasks.Add(t13);

        var t14 = Guid.NewGuid().ToString();
        state.Tasks[t14] = new CampaignTask(
            Id: t14,
            CampaignId: c3,
            Description: "Formar voluntarios sanitarios",
            Column: KanbanColumn.InProgress,
            AssignedTo: "u3-voluntario",
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-2)
        );
        c3Tasks.Add(t14);

        // Done tasks
        var t15 = Guid.NewGuid().ToString();
        state.Tasks[t15] = new CampaignTask(
            Id: t15,
            CampaignId: c3,
            Description: "Identificar comunidades objetivo",
            Column: KanbanColumn.Done,
            AssignedTo: "u1-coordinadora",
            CommentIds: new List<string>(),
            CreatedAt: now.AddDays(-7)
        );
        c3Tasks.Add(t15);

        state.Campaigns[c3] = new Campaign(
            Id: c3,
            Name: "Salud Comunitaria",
            GoalEuros: 4000.0m,
            CreatedAt: now.AddDays(-7),
            TaskIds: c3Tasks
        );
    }
}

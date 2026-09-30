# VenEl Modular MCP: Master Vision & Execution Plan

## 1. The Grand Idea
**The Problem:** The original `VenEl.MCPAssistant` was a monolith. It bundled 16 different toolsets (AWS, Azure, Atlassian, SQL, etc.) into a single, massive deployment. When users only wanted to use the Atlassian tools, they were still forced to install the entire monolithic package with all its heavy dependencies.

**The Solution:** A true "Hub and Spoke" modular architecture. We are decoupling the heavy monolithic runner into a lightweight, extensible **Modular Host** (`VenEl.ModularMCP`). 
Users will install the extremely light core host (`dotnet tool install -g VenEl.ModularMCP`), and then dynamically install *only* the specific plugins they need (e.g., `venel-modular-mcp plugin install VenEl.ModularMCP.Atlassian`). 

## 2. Phase 1: The Architectural Overhaul (✅ COMPLETED)
We have successfully decoupled the business logic from the runner.

*   **The Monolith (`VenEl.MCPAssistant`):** Safely renamed from `VenEl.AssistantMCP.*` to `VenEl.MCP.*`. This repository now acts purely as a library of business logic. It no longer acts as the final executable host.
*   **The Modular Engine (`VenEl.ModularMCP`):** A completely new repository built from scratch.
    *   **Core:** A dynamic runner that uses `AssemblyLoadContext` to scan its plugin folder and load `IVenElPlugin` DLLs dynamically at runtime.
    *   **Shared:** A minimalist class library holding the `IVenElPlugin` contract and centralized boilerplate logic (`PluginExecutableBase`) to keep the codebase perfectly DRY.
    *   **Plugins (The Wrappers):** 16 ultra-lightweight Console Apps. They contain zero business logic. They are just empty shells with a `<ProjectReference>` linking back to the heavy business logic in the old repository.

## 3. Phase 2: The Final Wiring (🚀 UP NEXT)
The blueprint is perfect, but the plugins need to be hooked up to their respective Dependency Injection (DI) containers.

**Step 1: Expand the Contract**
*   **Goal:** Allow plugins to read from `appsettings.json`.
*   **Action:** Open `src/VenEl.ModularMCP.Shared/IVenElPlugin.cs` and add `IConfiguration` to the signature: 
    `void ConfigureServices(IServiceCollection services, IConfiguration configuration);`

**Step 2: Inject the Logic**
*   **Goal:** Actually register the business logic into the dynamic host.
*   **Action:** For every plugin (e.g., `MSSqlPlugin.cs`), implement the interface and call the specific extension method from the old codebase:
    `services.AddMSSqlFeature(configuration);`

## 4. Phase 3: Packaging & Distribution (🔮 FUTURE)
Once the plugins are wired up and tested, we move to distribution.

*   **NuGet Packaging:** Each of the 16 thin wrapper projects will be packed into standalone `.nupkg` files.
*   **The CLI Installer:** The `VenEl.ModularMCP.Core` host needs a CLI command (e.g., `plugin install <name>`) that downloads the `.nupkg`, extracts the `.dll`, and places it in the Core's dynamic loading directory.
*   **The End-User Experience:** The user gets a lightning-fast host, and can customize their AI's toolset a la carte.

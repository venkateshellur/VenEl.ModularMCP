# VenEl.ModularMCP

Welcome to **VenEl.ModularMCP** — a highly extensible, dynamically loaded Model Context Protocol (MCP) host engine.

This project introduces a "Hub-and-Spoke" architecture for deploying and consuming MCP tools. It is designed to act as a lightweight, scalable engine (the Hub) that discovers, loads, and executes modular plugins (the Spokes) entirely at runtime.

> ⚠️ **Important Note for NuGet Users:** The `VenEl.ModularMCP.*` packages published to NuGet.org from this repository are **not standalone libraries**. They are dynamically-loaded plugin modules designed exclusively to be consumed by the VenEl.ModularMCP host engine.

## 🌟 The Vision: ModularMCP vs. AssistantMCP

If you are familiar with our foundational project, **VenEl.AssistantMCP**, you know it is an incredibly powerful, all-in-one monolith. `AssistantMCP` is the ultimate "batteries-included" solution: it ships with all 17+ integrations (AWS, Azure, Docker, GitHub, Slack, MSSql, etc.) baked directly into a single unified server. It is perfect for deployments where you want a comprehensive suite of tools ready to go out-of-the-box.

**VenEl.ModularMCP** takes those exact same world-class integrations and reimagines how they are delivered. 

Instead of compiling everything into a single binary, `ModularMCP` decouples the core host engine from the business logic. It allows developers and administrators to selectively install *only* the plugins they need, exactly when they need them.

### Key Benefits

1. **True Runtime Modularity:** 
   Plugins are distributed as standalone `.nupkg` files. You can drop a new plugin into the `plugins/` directory, and the Core Engine will dynamically load it via isolated `AssemblyLoadContexts` on its next boot—no recompilation of the host required.

2. **Decoupled Lifecycles:**
   Because plugins are just lightweight shells referencing the core `VenEl.MCP.*` libraries, a bug fix or feature addition in the `Slack` tool doesn't require rebuilding the entire server. You simply update the `Slack` plugin package.

3. **Leaner Deployments:**
   Only need GitHub and MSSql capabilities for a specific AI agent? You no longer need to deploy the dependencies for Kubernetes, GCP, and Docker. `ModularMCP` ensures your runtime footprint is strictly limited to the capabilities you actively install.

4. **100% Code Reuse:**
   `ModularMCP` does not rewrite or replace the battle-tested logic of `AssistantMCP`. Instead, it acts as a sleek wrapper. The heavy lifting is still performed by the original `VenEl.MCP.*` libraries, which are now seamlessly consumed as independent SDKs.

## 🚀 How It Works

The architecture consists of three main pieces:
- **VenEl.ModularMCP.Core:** The central host engine. It sets up Dependency Injection, Configuration, and the MCP server lifecycle. It scans the `plugins/` folder at startup.
- **VenEl.ModularMCP.Shared:** The contract library containing `IVenElPlugin`. Both the Core and the Plugins reference this to ensure safe type-casting across boundaries.
- **The Plugins (e.g., VenEl.ModularMCP.Slack):** Thin wrapper libraries that implement `IVenElPlugin`. They contain the DI registration instructions necessary to wire up the heavy `VenEl.MCP.*` business logic into the Core's service collection.

### CLI Installation
The Core engine comes with a built-in CLI to manage plugins. To install a plugin locally, simply run:
```bash
venel-mcp plugin install Slack
```
This command automatically unpacks the designated NuGet package and places it in the dynamic loading directory.

---
## ⚡ Quick Start

To run the ModularMCP Core server locally:

```bash
# 1. Clone the repository
git clone https://github.com/venkateshellur/VenEl.ModularMCP.git
cd VenEl.ModularMCP

# 2. Build the Core Host
dotnet build src/VenEl.ModularMCP.Core

# 3. Install a Plugin (e.g., Slack)
dotnet run --project src/VenEl.ModularMCP.Core -- plugin install Slack

# 4. Start the Server
dotnet run --project src/VenEl.ModularMCP.Core
```

## 📦 Available Official Plugins

The following plugins are officially supported and available on NuGet.org. They can be installed dynamically into your Core engine:

- `VenEl.ModularMCP.AWS`
- `VenEl.ModularMCP.Azure`
- `VenEl.ModularMCP.Atlassian`
- `VenEl.ModularMCP.Bitwarden`
- `VenEl.ModularMCP.Databricks`
- `VenEl.ModularMCP.Docker`
- `VenEl.ModularMCP.Email`
- `VenEl.ModularMCP.FTP`
- `VenEl.ModularMCP.GCP`
- `VenEl.ModularMCP.GitHub`
- `VenEl.ModularMCP.Kubernetes`
- `VenEl.ModularMCP.LocalOffice`
- `VenEl.ModularMCP.Logging`
- `VenEl.ModularMCP.MSSql`
- `VenEl.ModularMCP.MicrosoftTeams`
- `VenEl.ModularMCP.Slack`
- `VenEl.ModularMCP.WebAutomator`

## 🛠️ Building a Custom Plugin

Extending the ecosystem is incredibly easy. Just create a standard Class Library, reference `VenEl.ModularMCP.Shared`, and implement `IVenElPlugin`:

```csharp
using Microsoft.Extensions.DependencyInjection;
using VenEl.ModularMCP.Shared;

namespace MyCustomPlugin;

public class MyPlugin : IVenElPlugin
{
    public string Name => "MyCustomPlugin";
    public string Version => "1.0.0";

    public void ConfigureServices(IServiceCollection services)
    {
        // Register your tool logic here!
        services.AddSingleton<IMyCustomTool, MyCustomTool>();
    }
}
```

Pack it as a `.nupkg`, drop it into the `plugins/` directory of the Core host, and it will be loaded automatically!

---
*Built with ❤️ to push the boundaries of Agentic Tooling and the Model Context Protocol.*

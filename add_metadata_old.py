import os

repo_path = "/Users/venkateshellur/Venky/Git/VenEl.MCPAssistant/VenEl.MCPAssistant"

readme_content = """# VenEl.MCP Core Library

⚠️ **Notice:** This package is the underlying business logic library for the VenEl Model Context Protocol ecosystem.
It is primarily intended to be consumed by the **VenEl.ModularMCP** runtime host and is generally not used as a standalone independent package.

For more information, please visit the main GitHub repositories:
- Modular Hub: https://github.com/venkateshellur/VenEl.ModularMCP
- Monolithic Server: https://github.com/venkateshellur/VenEl.AssistantMCP
"""

props_content = """<Project>
  <PropertyGroup>
    <PackageReadmeFile>MODULE_README.md</PackageReadmeFile>
    <Description>Core business logic library for the VenEl MCP ecosystem. Not intended for standalone use outside of VenEl.ModularMCP or VenEl.AssistantMCP.</Description>
    <Authors>Venkatesh Ellur</Authors>
    <Company>VenEl</Company>
  </PropertyGroup>
  
  <ItemGroup>
    <None Include="$(MSBuildThisFileDirectory)MODULE_README.md" Pack="true" PackagePath="\\" />
  </ItemGroup>
</Project>"""

with open(os.path.join(repo_path, "MODULE_README.md"), "w") as f:
    f.write(readme_content)

with open(os.path.join(repo_path, "Directory.Build.props"), "w") as f:
    f.write(props_content)

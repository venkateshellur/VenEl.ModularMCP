import re
path = "src/VenEl.ModularMCP.Core/Program.cs"
with open(path, "r") as f: content = f.read()

# Add builder.Services.AddCoreSecurity() before mcpBuilder
content = re.sub(r"var mcpBuilder = builder\.Services\s*\n\s*\.AddMcpServer", "builder.Services.AddCoreSecurity();\n\n            var mcpBuilder = builder.Services\n                .AddMcpServer", content)

with open(path, "w") as f: f.write(content)

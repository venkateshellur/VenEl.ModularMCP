import glob
import re

def bump_versions(repo_path, old_version, new_version):
    for fp in glob.glob(f'{repo_path}/src/**/*.csproj', recursive=True):
        with open(fp, 'r') as f:
            content = f.read()
        
        # If no Version tag exists, we can add it, but we know we added <Version>1.0.0</Version> earlier
        if f"<Version>{old_version}</Version>" in content:
            new_content = content.replace(f"<Version>{old_version}</Version>", f"<Version>{new_version}</Version>")
            # Also replace any PackageReference versions
            new_content = re.sub(rf'Version="{old_version}"', f'Version="{new_version}"', new_content)
            
            with open(fp, 'w') as f:
                f.write(new_content)

# Bump AssistantMCP to 1.0.1
bump_versions("/Users/venkateshellur/Venky/Git/VenEl.MCPAssistant/VenEl.MCPAssistant", "1.0.0", "1.0.1")

# Bump ModularMCP to 1.0.1
bump_versions("/Users/venkateshellur/Venky/Git/VenEl.ModularMCP", "1.0.0", "1.0.1")

import glob
import re

def fix_external_versions(repo_path, current_version, original_version):
    for fp in glob.glob(f'{repo_path}/src/**/*.csproj', recursive=True):
        with open(fp, 'r') as f:
            content = f.read()
        
        # We want to change Version="1.0.1" (or 1.0.2) back to "1.0.0" 
        # BUT ONLY IF it is NOT a VenEl.* package
        
        def replace_match(m):
            pkg_name = m.group(1)
            if not pkg_name.startswith("VenEl."):
                return f'Include="{pkg_name}" Version="{original_version}"'
            return m.group(0)
            
        new_content = re.sub(rf'Include="([^"]+)" Version="{current_version}"', replace_match, content)
        
        if new_content != content:
            with open(fp, 'w') as f:
                f.write(new_content)

# AssistantMCP was bumped to 1.0.1
fix_external_versions("/Users/venkateshellur/Venky/Git/VenEl.MCPAssistant/VenEl.MCPAssistant", "1.0.1", "1.0.0")

# ModularMCP was bumped to 1.0.1
fix_external_versions("/Users/venkateshellur/Venky/Git/VenEl.ModularMCP", "1.0.1", "1.0.0")

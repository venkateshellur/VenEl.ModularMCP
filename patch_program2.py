import re
path = "src/VenEl.ModularMCP.Core/Program.cs"
with open(path, "r") as f: content = f.read()

handler = """
        AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
        {
            Console.Error.WriteLine($"[FirstChanceException] {eventArgs.Exception.GetType().Name}: {eventArgs.Exception.Message}\\n{eventArgs.Exception.StackTrace}");
        };
"""

content = re.sub(r"AppDomain\.CurrentDomain\.FirstChanceException \+=[\s\S]*?};", handler.strip(), content)

with open(path, "w") as f: f.write(content)

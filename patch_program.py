import re
path = "src/VenEl.ModularMCP.Core/Program.cs"
with open(path, "r") as f: content = f.read()

handler = """
        AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
        {
            if (eventArgs.Exception.StackTrace != null && eventArgs.Exception.StackTrace.Contains("ModelContextProtocol"))
            {
                Console.Error.WriteLine($"[FirstChanceException] {eventArgs.Exception.GetType().Name}: {eventArgs.Exception.Message}\\n{eventArgs.Exception.StackTrace}");
            }
        };
"""

content = re.sub(r"public static async Task Main\(string\[\] args\)\s*{", "public static async Task Main(string[] args)\n    {" + handler, content)

with open(path, "w") as f: f.write(content)

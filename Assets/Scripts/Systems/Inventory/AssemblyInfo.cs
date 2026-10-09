// Goes in the Dialogue assembly (next to its .asmdef), NOT in the test folder.
//
// Lets the test assembly see `internal` members. Tests use this ONLY to construct
// fixture graphs (nodes, links). Every assertion goes through the public API,
// so refactoring how the graph is built breaks one helper file, not every test.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Recalled.Systems.Tests.EditMode")]

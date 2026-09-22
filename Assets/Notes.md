# My Sticky Notes because i forget all the staff

## Scriptable Objects

Scriptable objects are assets used to store data. Largely it's shared data persistent between play sessions. While components all belong to its own gameobject the SOs belong to the whole porject. They can be loaded and deloaded from memory.

Unity callbacks calls:
- `Awake` - first created via Editor view or `CreateInstance` in code
- `OnEnable` - domain load hence is not called on entering play mode if auto domain reload is off
- `OnDisable` - domain unload.
- `OnDestroy` - object was destroyed via `Destroy()` or deleted in the Editor

SOs areloaded lazily when they're accessed. They can be unloaded from memory when nothing references them.
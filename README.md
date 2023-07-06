# Terrain-Tinker-Turbo

| Game Play                                                       | Descriptive Doc                                                                                    | Game Design Document                                                                                   | Video Demo           |
|-----------------------------------------------------------------|----------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------|----------------------|
| [WebGL Game Play](https://www.brando.dev/Terrain-Tinker-Turbo/) | [Google Doc](https://docs.google.com/document/d/1Vv43XOBFkLTBURJkr4-mtASEsxzbv6IdImxQkb_WFcw/edit) | [GDD Google Doc](https://docs.google.com/document/d/1muZIKU-rg9XkjcEP2YsbQW13vPxFwVih8k33z1c64DY/edit) |                      |

## Game Mechanics (How to Play)

- Track construction mechanic:
  - Use the mouse to drag-and-drop track blocks
  - Use `R` to rotate a selected track block.
- Movement mechanic:
  - Uses the keyboard `W`, `A`, `S`, `D` to control Player 1’s vehicle.
  - Uses the keyboard `I`, `J`, `K`, `L` to control Player 2’s vehicle.
- Game end mechanic:
  - Whoever arrives at the destination wins the game.

## File Structure
```bash
.
├── README.md
├── Terrain-Tinker-Turbo-Source  # Open this folder in your Unity Hub
│   ├── Assembly-CSharp-Editor.csproj
│   ├── Assembly-CSharp.csproj
│   ├── Assets
│   ├── Library
│   ├── Logs
│   ├── Packages
│   ├── ProjectSettings
│   ├── Temp
│   ├── Terrain-Tinker-Turbo-Source.sln
│   ├── UserSettings
│   ├── WebGL Builds  # was set to be gitignore
│   └── obj
└── docs  # GitHub Pages Deployment Source (Manually updated)
    ├── Build
    ├── GUID.txt
    ├── ProjectVersion.txt
    ├── TemplateData
    ├── dependencies.txt
    └── index.html
```

## Reference
- [Unity Assets- Lowpoly Terrain Track](https://assetstore.unity.com/packages/3d/environments/roadways/lowpoly-terrain-track-165394): Assets to build terrain quickly.

- [Realistic Car Kit 2](https://assetstore.unity.com/packages/tools/physics/realistic-car-kit-18421): Assets to implement vehicle control.

- [Unity Assets - US Road Signs Free](https://assetstore.unity.com/packages/3d/props/exterior/us-road-signs-free-164941): Models of traffic signs.

- [TouchScript](https://github.com/TouchScript/TouchScript): Assets to reimplement drag-and-drop function.
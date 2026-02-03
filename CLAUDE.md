# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ZDPS is a Damage Meter and Companion Tool for Blue Protocol: Star Resonance (Season 1 version). It captures network packets to parse combat data and provides real-time metrics including DPS, healing, tanking, and NPC damage tracking. The application automatically splits encounters by phases/wipes and stores all encounter history in a local database for later analysis.

## Build Commands

```bash
# Restore NuGet dependencies
dotnet restore

# Build the entire solution
dotnet build

# Run the main application
dotnet run --project BPSR-ZDPS/BPSR-ZDPS.csproj

# Publish single-file executable (used for releases)
publish.bat
```

The `publish.bat` script creates a self-contained executable in `./publish` and copies the `Data` folder.

## Technology Stack

- **Language**: C# with .NET 9.0
- **GUI**: Hexa.NET.ImGui (Immediate Mode GUI) with Direct3D11 (Silk.NET)
- **Database**: SQLite with Dapper ORM
- **Packet Capture**: SharpPcap, PacketDotNet
- **Serialization**: Protocol Buffers (protobuf-net, Google.Protobuf)
- **Compression**: ZstdSharp
- **Logging**: Serilog

## Solution Structure

The solution contains 3 projects:

1. **BPSR-ZDPS** - Main desktop application executable
   - UI windows implemented with ImGui
   - Managers for game state (encounters, entities, chat, etc.)
   - Database layer with migrations

2. **BPSR-ZDPSLib** - Core library
   - Packet capture and TCP reassembly
   - Protocol buffer definitions (`protos/`)
   - Game data blob structures (`Blobs/`)
   - gRPC service definitions

3. **BPSR-ZDPSServ** - Deduplication server component

## Key Architecture Components

### Packet Capture ([BPSR-ZDPSLib/NetCap.cs](BPSR-ZDPSLib/NetCap.cs))

The packet capture system uses SharpPcap to intercept network traffic. Key components:

- `NetCap` - Main capture class that manages the packet capture device
- `TcpReassembler` - Reassembles TCP streams into complete messages
- Connection filtering to isolate game traffic from other network activity
- ZstdSharp decompression for compressed game packets

Game messages are dispatched via registered handlers using `NotifyId` (serviceId + methodId pairs).

### Managers ([BPSR-ZDPS/Managers/](BPSR-ZDPS/Managers/))

Core game state management:

- **EncounterManager** - Tracks combat encounters, auto-splits by phases/wipes
- **MessageManager** - Processes game network messages and updates entity states
- **ChatManager** - Chat overlay functionality
- **MatchManager** - Matchmaking queue detection for alert sounds
- **IntegrationManager** - Third-party service integrations (BPTimer spawn tracking)
- **ModuleOptimizer** - Fast module combination optimizer using AVX2 CPU optimizations

### UI Windows ([BPSR-ZDPS/Windows/](BPSR-ZDPS/Windows/))

All windows use ImGui immediate mode rendering:

- **MainWindow** - Main DPS/healing/tank meters display
- **EncounterHistoryWindow** - Browse stored encounters from database
- **EntityInspector** - Detailed breakdown of entity performance (damage, healing, buffs)
- **SettingsWindow** - Application configuration
- **Raid Manager Windows** - Cooldown tracker, countdowns, raid warnings, threat meter

### Database ([BPSR-ZDPS/Database/](BPSR-ZDPS/Database/))

- SQLite with Dapper ORM
- Zstandard compression for encounter data storage
- Migration system for schema updates
- Can run in-memory or file-backed mode

### Data Directory ([BPSR-ZDPS/Data/](BPSR-ZDPS/Data/))

JSON data files for:
- Skills, buffs, monsters
- Localized strings (AppStrings.json)
- Must be copied to output directory (see publish.bat)

## Important Notes

- Uses `unsafe` code blocks for performance-critical packet processing
- Multi-language support via embedded fonts (JP, SC, TC, KR)
- No test framework configured - relies on manual testing
- This is the **Season 1** branch - only works with Season 1 versions of BPSR

## Prerequisites for Development

- Npcap installed (required for packet capture functionality) - https://npcap.com
- .NET 9 SDK or Desktop Runtime

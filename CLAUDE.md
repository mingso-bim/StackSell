# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**StackSell** is a Unity 6 (6000.5.0f1) 3D game project using the Universal Render Pipeline (URP). It is in early development with two scenes: `Main` and `SampleScene`.

## Unity Version & Key Packages

- **Unity**: 6000.5.0f1
- **Render Pipeline**: URP 17.5.0 (separate PC and Mobile renderer/asset configs in `Assets/Settings/`)
- **Input**: Unity Input System 1.19.0 (new input system — `UnityEngine.InputSystem`, not legacy `Input`)
- **AI Navigation**: 2.0.13
- **Timeline**: 1.8.12
- **Test Framework**: 1.7.0

## How to Build and Run

Unity projects are built and run through the Unity Editor, not the command line:

1. Open the project in Unity 6000.5.0f1 via Unity Hub.
2. Use **File → Build Settings** to configure and build for a target platform.
3. Use **Edit → Play** (or the Play button) to run the game in-editor.

To run tests: **Window → General → Test Runner**, then run EditMode or PlayMode tests.

## Code Architecture

All game scripts use the `SS` prefix (StackSell). Scripts live in `Assets/Scripts/`.

- **`SSPlayerController`** — Attached to the player GameObject (requires `CharacterController`). Reads WASD keyboard input directly via `Keyboard.current` from the new Input System and moves the character. Rotates the player to face the movement direction.
- **`SSCameraFollow`** — Attached to the camera. Follows a target `Transform` with a configurable offset, updating in `LateUpdate` to stay in sync with player movement.

### Input System Note

The project uses the **new Unity Input System** (`UnityEngine.InputSystem`). Do not use `UnityEngine.Input` (legacy). The input actions asset is at `Assets/InputSystem_Actions.inputactions`.

### Render Pipeline

Two URP configurations exist for different platforms:
- `Assets/Settings/PC_RPAsset.asset` + `PC_Renderer.asset` — PC quality settings
- `Assets/Settings/Mobile_RPAsset.asset` + `Mobile_Renderer.asset` — Mobile quality settings

Post-processing volumes are set up via `Assets/Settings/DefaultVolumeProfile.asset` and per-scene profiles (e.g., `SampleSceneProfile.asset`).

## Development Rules

### Scope
- 7일짜리 Unity 3D 모바일 캐주얼 게임 범위를 유지한다. 현재 요구사항 외의 기능을 추가하지 않는다.

### Simplicity
- Unity 초보자가 이해하고 설명할 수 있는 수준의 단순한 구현을 우선한다.
- 과도한 추상화, 디자인 패턴, Manager 클래스, Interface를 불필요하게 추가하지 않는다.
- 작은 로직을 과도하게 함수나 클래스로 쪼개지 않는다.

### Code Style
- Unity 6 기준으로 작성한다. 일반적인 C# / Unity 컨벤션을 따른다.
- `public` field보다 `[SerializeField] private`을 우선한다.
- `Update`에는 긴 로직을 직접 작성하지 않고 의미 있는 `private` 메서드로 분리한다.
- 매 프레임 `GetComponent`, `Find` 계열 호출을 반복하지 않는다 (`Awake`/`Start`에서 캐싱).

### Editing Approach
- 기존 코드가 정상 동작하면 전체 파일을 재작성하지 말고 필요한 부분만 최소 수정한다.
- 수정 전에 변경 이유와 수정할 파일을 먼저 설명한다.

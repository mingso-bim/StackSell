# Stack & Sell

> Unity로 제작한 3D 모바일 캐주얼 게임. 과일을 모아 등에 쌓고, 옮겨 팔아 Gold를 벌고, 능력치를 강화한다.

플레이어는 생성 존에서 과일을 모아 캐릭터 뒤에 쌓고, 판매 존으로 옮겨 팔아 Gold를 얻습니다.
모은 Gold로 이동 속도(Speed)와 적재량(Capacity)을 강화하며, 두 능력치를 모두 최대로 올리면 게임을 클리어합니다.

- 개인 포트폴리오용 소규모 프로젝트
- Unity 6000.5.0f1 · Universal Render Pipeline · Android 타깃

---

## Gameplay Flow

```
Move  →  Collect  →  Stack  →  Sell  →  Earn Gold  →  Upgrade  →  Game Clear
```

| 단계 | 내용 |
| --- | --- |
| **Move** | Floating Joystick으로 매장을 이동 |
| **Collect** | 생성 존에 머무르면 과일이 일정 간격으로 생성되어 수집 |
| **Stack** | 수집한 과일이 캐릭터 뒤 스택에 쌓임 (적재량 한계 존재) |
| **Sell** | 판매 존에 머무르면 스택이 하나씩 팔리며 Gold 증가 |
| **Upgrade** | Gold를 소비해 Speed / Capacity 강화 |
| **Game Clear** | Speed·Capacity가 모두 최대가 되면 클리어 |

---

## 구현 특징

**플레이어 이동 & 입력**
- 화면을 누른 지점에 조이스틱이 생성되는 Floating Joystick을 직접 구현 (`IPointerDownHandler` / `IDragHandler` / `IPointerUpHandler`)
- 조이스틱 입력과 에디터 테스트용 키보드(WASD) 입력을 합산해 `CharacterController`로 이동
- 이동 속도를 Animator `Speed` 파라미터로 전달해 Idle ↔ Walk 를 블렌드

**아이템 생성 / 수집 / 스택**
- `SSItemZone` 하나가 `isSellZone` 플래그로 생성 존과 판매 존을 겸함. 존에 머무는 동안 코루틴으로 일정 간격 처리
- `SSPlayerCollector` 가 수집한 과일을 `stackRoot` 아래 일정 간격으로 쌓고 Lerp 기반 이동으로 자연스러운 수집 애니메이션 구현, `maxCapacity` 로 적재량 제한
- 판매 시 스택 상단부터 하나씩 제거되어 판매 존으로 날아가 사라지고 Gold가 증가

**성장 & 밸런스 데이터**
- Speed / Capacity 2종 업그레이드, 각각 Gold 비용과 최대 횟수 제한
- 판매가, 기본 스탯, 업그레이드 비용·증가량·최대 횟수 등 밸런스 수치를 ScriptableObject(`SSItemData`, `SSPlayerUpgradeData`)로 분리해 코드 수정 없이 조정 가능

**저장 / 진행 유지**
- `SSSaveSystem` 이 Gold, 스탯, 업그레이드 횟수, 클리어 여부를 `JsonUtility` 로 직렬화해 `Application.persistentDataPath` 에 저장
- 앱 일시정지 / 종료 / 클리어 시점에 저장하고, 씬 시작 시 자동으로 로드

**게임 클리어 / 튜토리얼 / 연출**
- Speed·Capacity가 모두 최대가 되는 순간 클리어 오버레이 표시. 판정 기준은 `SSPlayerUpgrade` 한 곳으로 통일하고, 세이브에 기록되어 재실행 시 다시 뜨지 않음
- 최초 실행에서만 전체화면 튜토리얼 오버레이 표시(`PlayerPrefs` 기록). 화면을 탭하면 닫히며 게임 UI가 활성화
- URP Outline Renderer Feature로 상호작용 오브젝트에 외곽선 표시
- 수집 / 판매 / 업그레이드 효과음과 루프 BGM

---

## 조작법

| 입력 | 동작 |
| --- | --- |
| 화면 터치 & 드래그 | 누른 지점에 Floating Joystick 생성, 드래그 방향으로 이동 |
| `W` `A` `S` `D` | 이동 (에디터 테스트용) |

- 생성 존 / 판매 존은 해당 구역 안에 있으면 자동으로 동작
- 업그레이드는 화면의 Speed / Capacity 버튼으로 실행

---

## 기술 / 환경

| 항목 | 내용 |
| --- | --- |
| Engine | Unity 6000.5.0f1 |
| Render Pipeline | Universal Render Pipeline 17.5.0 (PC / Mobile 렌더러·에셋 분리) |
| Input | Input System 1.19.0 (신 입력 시스템) |
| UI | uGUI + TextMeshPro |
| Outline | CristianQiu/Unity-URP-Outline (Renderer Feature) |
| Platform | Android (Min SDK 26) |

---

## 프로젝트 구조

```
Assets/
├── Scenes/Main.unity      # 게임 씬 (빌드 대상)
├── Scripts/               # 게임 로직 (SS* 접두사)
├── Settings/              # URP PC / Mobile 설정
├── Animations/Player/     # Idle / Walk 애니메이션 + Animator
├── Prefabs/               # Player, Apple, Orange, SellZone 등
├── Models/                # 캐릭터 / 과일 / 존 모델
└── Sounds/                # BGM, SFX
```

핵심 스크립트

| 스크립트 | 역할 |
| --- | --- |
| `SSPlayerController` | `CharacterController` 이동, 조이스틱 + 키보드 입력 처리, 애니메이션 파라미터 전달 |
| `SSFloatingJoystick` | 플로팅 조이스틱 UI 입력 |
| `SSItemZone` | 아이템 생성 존 / 판매 존 (겸용) |
| `SSPlayerCollector` | 아이템 수집 및 스택 관리, 적재량 제한 |
| `SSPlayerUpgrade` / `SSPlayerUpgradeData` | Speed·Capacity 업그레이드 로직 / 밸런스 데이터 |
| `SSPlayerWallet` | Gold 보유 및 증감 |
| `SSSaveSystem` / `SSSaveData` | 진행 상황 저장 · 로드 |
| `SSTutorialUI` | 최초 1회 튜토리얼 |
| `SSGameClearUI` | 클리어 조건 판정 및 오버레이 |

---

## 실행 방법

1. Unity Hub에서 **6000.5.0f1** 버전으로 프로젝트를 연다.
2. `Assets/Scenes/Main.unity` 를 열고 Play.
3. Android 빌드: Build Profiles(또는 Build Settings)에서 Android 플랫폼 선택 → `Main` 씬만 포함된 상태로 Build.

---

## Third-Party Assets / Credits

### 3D

| 에셋 | 제작자 | 출처 | 라이선스 |
| --- | --- | --- | --- |
| Stickman (Player) | KOMiRA | [Sketchfab](https://sketchfab.com/3d-models/stickman-cd59f48ed6a7492da639efdc55f2c1f8) | CC Attribution (CC BY) |
| Low-Poly Stylized Fruits Collection (Apple, Orange) | Dimitri Matcharashvili (ditomatch) | [Sketchfab](https://sketchfab.com/3d-models/low-poly-stylized-fruits-collection-f2f0f61bfba54a018db74ef4ccaffc8b) | Sketchfab Free Standard License |
| Low Poly Wooden Crate (생성 존) | MasterYogurt | [Sketchfab](https://sketchfab.com/3d-models/low-poly-wooden-crate-6958abcb7b7043b99709f944cc42a6d0) | Sketchfab Free Standard License |
| Wooden Market Stalls (판매 존) | hadoukengames737 | [Sketchfab](https://sketchfab.com/3d-models/wooden-market-stalls-5a75792bec2a45c798794d134213ff7a) | CC Attribution (CC BY) |
| RPG Poly Pack - Lite (환경) | Gigel3d | [Unity Asset Store](https://assetstore.unity.com/packages/3d/environments/landscapes/rpg-poly-pack-lite-148410) | Standard Unity Asset Store EULA |

### Audio

| 에셋 | 제작자 | 출처 | 라이선스 |
| --- | --- | --- | --- |
| Pick Up SFX — Retro Arcade Item Pickup | Vadim_Makes_Sound | [Pixabay](https://pixabay.com/sound-effects/film-special-effects-retro-arcade-item-pickup-554465/) | Pixabay Content License |
| Sell SFX — Drop or Pickup item (1) | Yodguard | [Pixabay](https://pixabay.com/sound-effects/film-special-effects-drop-or-pickup-item-1-387916/) | Pixabay Content License |
| Upgrade SFX — Item Collected (1) | ALEXIS_GAMING_CAM | [Pixabay](https://pixabay.com/sound-effects/film-special-effects-item-collected-1-367087/) | Pixabay Content License |
| BGM — Mobile Casual Video Game Music | Ivan_Luzan | [Pixabay](https://pixabay.com/music/upbeat-mobile-casual-video-game-music-158301/) | Pixabay Content License |

### Package

| 에셋 | 제작자 / 저장소 | 출처 | 라이선스 |
| --- | --- | --- | --- |
| URP Outline | CristianQiu / Unity-URP-Outline | [GitHub](https://github.com/CristianQiu/Unity-URP-Outline) | MIT License |

> CC BY로 표기된 에셋(Stickman, Wooden Market Stalls)은 배포 시 제작자 표기가 필요합니다.

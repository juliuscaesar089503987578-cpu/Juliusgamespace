# Scripts Directory Structure

All C# scripts follow SOLID principles and are production-ready.

```
Scripts/
├── Core/              # Core game systems
│   ├── GameManager.cs         # Main game controller
│   ├── SaveManager.cs         # Save/Load system
│   ├── EventManager.cs        # Event system
│   └── TimeManager.cs         # Time and weather
├── Player/            # Player-related systems
│   ├── PlayerController.cs    # Player movement and input
│   ├── PlayerStats.cs         # Health, stats, attributes
│   ├── SurvivalSystem.cs      # Hunger, thirst, fatigue
│   └── CombatSystem.cs        # Combat mechanics
├── Character/         # Character creation and management
│   ├── CharacterCreator.cs    # Character customization UI
│   ├── Character.cs           # Character data structure
│   └── CharacterDatabase.cs   # Preset data
├── Inventory/         # Inventory system
│   ├── InventorySystem.cs     # Inventory management
│   ├── Item.cs                # Item data structure
│   ├── ItemDatabase.cs        # Item definitions
│   └── InventoryUI.cs         # UI display
├── Crafting/          # Crafting system
│   ├── CraftingSystem.cs      # Recipe management
│   ├── Recipe.cs              # Recipe data
│   ├── RecipeDatabase.cs      # Recipe definitions
│   └── CraftingUI.cs          # Crafting interface
├── Building/          # Building system
│   ├── BuildingSystem.cs      # Building mechanics
│   ├── Building.cs            # Building data
│   ├── BuildingDatabase.cs    # Building types
│   ├── BuildingUI.cs          # Building mode UI
│   └── GridSystem.cs          # Grid placement
├── NPC/               # NPC system
│   ├── NPCManager.cs          # NPC management
│   ├── NPC.cs                 # NPC data
│   ├── NPCBehavior.cs         # AI behavior tree
│   ├── NPCDialogue.cs         # Dialogue system
│   └── NPCTask.cs             # Task assignment
├── Combat/            # Combat system
│   ├── Combat.cs              # Combat mechanics
│   ├── Weapon.cs              # Weapon data
│   ├── WeaponDatabase.cs      # Weapon definitions
│   ├── Enemy.cs               # Enemy base class
│   ├── EnemyAI.cs             # Enemy AI
│   └── DamageCalculator.cs    # Damage system
├── Exploration/       # World exploration
│   ├── WorldGenerator.cs      # Procedural generation
│   ├── Location.cs            # Location data
│   ├── LocationDatabase.cs    # Location definitions
│   ├── LootSystem.cs          # Loot generation
│   └── Minimap.cs             # Minimap system
├── Progression/       # Progression system
│   ├── ExperienceSystem.cs    # XP and leveling
│   ├── SkillTree.cs           # Skill tree
│   ├── TechnologyTree.cs      # Tech tree
│   ├── QuestSystem.cs         # Quest management
│   └── ReputationSystem.cs    # Faction reputation
├── Audio/             # Audio system
│   ├── AudioManager.cs        # Audio management
│   ├── MusicManager.cs        # Music transitions
│   └── SoundEffect.cs         # Sound effects
├── UI/                # User interface
│   ├── MainMenuUI.cs          # Main menu
│   ├── HUDManager.cs          # In-game HUD
│   ├── MenuUI.cs              # Menu panels
│   └── Notifications.cs       # Notification system
├── Utilities/         # Helper classes
│   ├── ObjectPool.cs          # Object pooling
│   ├── RandomGenerator.cs     # RNG utilities
│   ├── MathUtils.cs           # Math helpers
│   ├── SerializationHelper.cs # Save/load helpers
│   └── DebugManager.cs        # Debug utilities
└── Constants/         # Game constants
    ├── GameConstants.cs       # Game settings
    └── TagsAndLayers.cs       # Tags and layers
```

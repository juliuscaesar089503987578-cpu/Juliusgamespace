# Julius Game Space - 2D Survival Apocalypse
## Complete Game Design Document

### 1. CORE VISION
**Genre:** 2D Survival Apocalypse
**Platform:** PC (Windows, Mac, Linux via Unity)
**Target Audience:** Survival game enthusiasts, ages 16+
**Target Duration:** 100+ hours per playthrough

### 2. SETTING & ATMOSPHERE
- **World:** Post-apocalyptic Earth, year 2087
- **Theme:** Dark, tense, immersive, realistic survival
- **Art Style:** High-quality pixel art with dynamic lighting
- **Locations:** Ruined cities, forests, military bases, laboratories, bunkers, villages, wastelands
- **Tone:** Gritty, challenging, rewarding progression

### 3. CORE GAMEPLAY LOOP
1. **Explore** - Find resources and loot in procedurally generated world
2. **Survive** - Manage health, hunger, thirst, temperature, diseases
3. **Craft** - Create weapons, armor, tools, medicines
4. **Build** - Construct and expand survivor base
5. **Combat** - Fight zombies, mutants, raiders, animals
6. **Progress** - Gain experience, unlock skills, recruit NPCs
7. **Repeat** - Endless survival challenges

### 4. CHARACTER CREATION SYSTEM
#### Customization Options:
- **Gender:** Male / Female
- **Face:** 12 face presets
- **Hairstyle:** 15 hairstyles
- **Hair Color:** 20 colors
- **Eye Color:** 15 colors
- **Skin Tone:** 12 tones
- **Facial Hair:** 8 beard styles (male only)
- **Clothing:** 20 base outfit options
- **Armor:** Plate, leather, cloth variants
- **Accessories:** Scarf, goggles, mask, belt, etc.
- **Backpack:** Different styles and capacities
- **Name:** Custom text input

#### Starting Profession:
- **Medic:** +30% healing, +20% disease resistance
- **Engineer:** +30% crafting speed, +20% building speed
- **Hunter:** +30% ranged damage, +20% resource detection
- **Soldier:** +30% melee damage, +20% armor effectiveness
- **Farmer:** +30% farming output, +20% food quality
- **Mechanic:** +30% vehicle repair, +20% electronics crafting
- **Scientist:** +30% research speed, +20% technology unlock rate
- **Scavenger:** +30% loot find rate, +20% item durability

#### Attributes (20 points to allocate):
- **Strength:** Melee damage, carry capacity, climbing
- **Agility:** Movement speed, dodge chance, stealth
- **Intelligence:** Crafting quality, skill unlock rate, resource efficiency
- **Endurance:** Health, stamina, disease resistance
- **Charisma:** NPC recruitment, trading prices, quest rewards

### 5. SURVIVAL SYSTEMS
#### Core Needs (deplete over time):
- **Hunger:** 0-100, depletes at 0.5/min
  - Starving: <20 → health damage
  - Satisfied: 50-100 → regenerate health
- **Thirst:** 0-100, depletes at 1/min
  - Dehydrated: <20 → stamina damage
  - Hydrated: 50-100 → stamina regenerate
- **Fatigue:** 0-100, depletes based on activity
  - Exhausted: <20 → movement speed -50%
  - Rested: >80 → all actions +20% speed
- **Temperature:** -50 to +50°C
  - Too cold: <5°C → health damage over time
  - Comfortable: 10-25°C → no penalties
  - Too hot: >35°C → fatigue drain faster

#### Status Conditions:
- **Bleeding:** -2 HP/sec until bandaged
- **Infection:** -1 HP/sec, reduced healing effectiveness
- **Broken Bones:** -50% movement speed
- **Poison:** Variable damage based on poison type
- **Radiation:** -1 HP/sec, can cause mutations
- **Disease:** Multiple types with different effects
- **Sleep Deprivation:** Reduced stats, hallucinations
- **Morale:** Affects NPC happiness and combat effectiveness

### 6. CRAFTING SYSTEM
#### Categories:
**Weapons (50+ recipes):**
- Melee: Axe, Sword, Spear, Club, Knife, Hammer, Crowbar
- Ranged: Bow, Crossbow, Rifle, Shotgun, Pistol, SMG, Sniper
- Throwables: Grenade, Molotov, Poison dart

**Armor (40+ recipes):**
- Helmet, Chest plate, Leggings, Boots, Gloves
- Materials: Leather, Cloth, Metal, Composite
- Variants: Light, Medium, Heavy

**Medical (30+ recipes):**
- Bandage, Splint, Antidote, Antibiotic
- Pain killer, Stimulant, Vaccine, Medical kit

**Food (50+ recipes):**
- Cooked meat, Canned food, Dried fruit
- Bread, Soup, Stew, Energy bars

**Water (10+ recipes):**
- Boiled water, Filtered water, Purification tablets
- Water treatment systems

**Electronics (40+ recipes):**
- Radio, Lamp, Circuit board, Battery
- Computer, Detector, Alarm system

**Furniture (30+ recipes):**
- Bed, Chair, Table, Desk, Shelf, Crate
- Workbench, Anvil, Furnace, Crafting table

**Traps (20+ recipes):**
- Bear trap, Spike trap, Alarm trap, Gas trap
- Turret, Mine, Explosive trap

**Vehicles (5+ recipes):**
- Cart, Motorcycle, Truck, Car, Bus

**Structure (60+ recipes):**
- Wall, Floor, Door, Gate, Window, Roof
- Storage, Generator, Solar panel, Wind turbine
- Farm plot, Water tank, Silo

### 7. BUILDING SYSTEM
#### Base Features:
- **Grid-based placement** with snap-to-grid
- **Multiple floors** (up to 10 levels)
- **Customizable dimensions** (no size limit)
- **Real-time preview** before placement
- **Rotation and flip** options
- **Material selection** and upgrade paths

#### Building Types:
1. **Residential:** Bedrooms, bathrooms, living areas
2. **Production:** Workshops, crafting areas, kitchens
3. **Storage:** Warehouses, lockers, crates
4. **Agriculture:** Farms, greenhouses, water collectors
5. **Defense:** Walls, gates, guard towers, turrets
6. **Utility:** Generator rooms, power lines, water systems
7. **Recreation:** Recreation room, library, gym
8. **Medical:** Hospital, pharmacy, surgery room

#### Systems:
- **Electricity:** Solar panels, wind turbines, generators
- **Water:** Collectors, tanks, purification, distribution
- **Food Production:** Farms, fishponds, hydroponics
- **Defense:** Walls, turrets, traps, sensors
- **Comfort:** Heating, lighting, furniture

### 8. NPC SURVIVOR SYSTEM
#### NPC Attributes:
- **Name & Appearance:** Randomized or custom
- **Profession:** Same as player professions
- **Skills:** 10 skill categories, each 0-100
- **Personality:** 5 traits affecting behavior
- **Happiness:** 0-100 (affects productivity)
- **Loyalty:** 0-100 (affects reliability)
- **Health:** Full survival system like player
- **Inventory:** 30-slot personal inventory
- **Relationships:** With player and other NPCs

#### NPC Capabilities:
- **Gather resources** from assigned zones
- **Defend base** during attacks
- **Cook meals** from raw ingredients
- **Farm crops** in designated areas
- **Heal teammates** using medical supplies
- **Craft items** in workshops
- **Repair buildings** and structures
- **Explore autonomously** for loot
- **Follow orders** from player commands
- **Guard positions** and patrol routes

#### NPC Happiness Factors:
- Good food and water (+10/day)
- Comfortable bed in bedroom (+5/day)
- Recreation activities (+5/day)
- Successful combat (+10/victory)
- Difficult work (-5/day)
- Poor conditions (-10/day)
- Injuries (-5 per injury)
- Morale events (+/-10-20)

### 9. COMBAT SYSTEM
#### Melee Combat:
- **Attack Speed:** Varies by weapon
- **Damage:** Based on Strength stat + weapon
- **Critical Hit Chance:** Based on Agility
- **Critical Multiplier:** 1.5x - 2.5x damage
- **Blocking:** Reduces incoming damage 30-50%
- **Stamina Cost:** Each attack consumes stamina

#### Ranged Combat:
- **Accuracy:** Based on Agility + weapon type
- **Damage:** Based on weapon quality
- **Ammo Types:** Different damage types (AP, explosive, etc.)
- **Reload Time:** Varies by weapon
- **Headshot Bonus:** 1.5x-3x damage
- **Falloff Damage:** Decreases with distance

#### Enemy Types:
1. **Zombies:**
   - Slow, weak, melee attack
   - Pack behavior (attract others)
   - Variants: Crawler, Runner, Brute, Spitter

2. **Mutants:**
   - Fast, intelligent, ranged/melee
   - Special abilities (leap, slam, poison)
   - Variants: Scout, Warrior, Behemoth, Alpha

3. **Raiders:**
   - Intelligent, use weapons, tactical
   - Patrol routes, communication
   - Variants: Scavenger, Raider, Mercenary, Leader

4. **Animals:**
   - Territorial, avoid if possible
   - Variants: Wolf, Bear, Boar, Wild dog

5. **Bosses:**
   - Unique enemies with special abilities
   - Higher rewards
   - Examples: Zombie King, Mutant Alpha, Raider Warlord

#### Combat Mechanics:
- **Stamina System:** Limited action points
- **Armor Penetration:** Different weapons penetrate differently
- **Status Effects:** Can apply bleeding, poison, etc.
- **Environmental Damage:** Hazards, fire, radiation
- **Stealth Kills:** 1 hit if undetected

### 10. EXPLORATION SYSTEM
#### World Locations:
1. **Cities (50+ buildings each):**
   - Apartments, shops, hospitals, offices
   - Loot: Medical supplies, electronics, money

2. **Villages (20+ buildings):**
   - Houses, farms, church, general store
   - Loot: Food, seeds, tools

3. **Military Bases:**
   - Barracks, armory, command center, laboratory
   - Loot: Weapons, armor, ammunition, tech
   - Enemies: Heavy resistance

4. **Laboratories:**
   - Research rooms, storage, testing chambers
   - Loot: Technology, mutations, chemicals
   - Enemies: Mutants, security systems

5. **Hospitals:**
   - Operating rooms, pharmacy, wards
   - Loot: Medicine, medical equipment
   - Enemies: Zombies, infected

6. **Police Stations:**
   - Armory, cells, evidence room
   - Loot: Weapons, ammunition, supplies
   - Enemies: Raiders, zombies

7. **Forests:**
   - Open area with trees, rocks, water
   - Loot: Wood, plants, animals
   - Enemies: Animals, mutants

8. **Sewers/Tunnels:**
   - Underground network, multiple levels
   - Loot: Rare materials, lost supplies
   - Enemies: Mutants, zombies

9. **Underground Bunkers:**
   - Vault system, sealed doors, technology
   - Loot: Pre-war supplies, advanced tech
   - Enemies: Robots, mutants

10. **Wastelands:**
    - Barren area, radiation zones, anomalies
    - Loot: Radiation suits, mutant parts
    - Enemies: Mutants, radiation

#### Procedural Generation:
- **World Size:** 1000x1000 chunks (expandable)
- **Chunk System:** Each chunk loaded dynamically
- **Location Seeding:** Pseudo-random but deterministic
- **Loot Randomization:** Item types and quantities vary
- **Difficulty Scaling:** Harder the farther from base

### 11. PROGRESSION SYSTEM
#### Experience & Leveling:
- **Total Levels:** 1-100
- **XP Sources:** Combat kills, exploration, crafting, tasks
- **Stat Growth:** +5 points per level to distribute
- **Level Benefits:** Skill unlock, stat increase, new recipes

#### Skill Tree (10 categories, each 0-100):
1. **Combat:** Melee, Ranged, Defense, Critical Strike
2. **Crafting:** Weapons, Armor, Medicine, Food
3. **Building:** Structure, Electricity, Water, Defense
4. **Survival:** Hunting, Farming, Cooking, Navigation
5. **Science:** Medicine, Chemistry, Mechanics, Electronics
6. **Stealth:** Sneaking, Lockpicking, Hacking, Camouflage
7. **Leadership:** NPC management, morale, trade
8. **Exploration:** Loot detection, hazard resistance
9. **Driving:** Vehicle control, repair, customization
10. **Endurance:** Health increase, stamina, disease resistance

#### Technology Tree:
- **Tiers:** 1-10 (unlocks new recipes)
- **Research Time:** Hours of gameplay
- **Resources Required:** Materials to unlock
- **Categories:** Weapons, Armor, Tools, Electronics, Agriculture, Defense

#### Reputation System:
- **Factions:** Survivors, Raiders, Mutants, Government
- **Reputation Range:** -100 to +100
- **Effects:** Quest availability, NPC recruitment, prices, locations

#### Quests:
- **Main Quests:** Story-driven, 20+ quests
- **Side Quests:** Various NPCs, 50+ quests
- **Random Events:** Dynamic missions, repeatable
- **Objectives:** Kill, fetch, rescue, build, defend

### 12. DYNAMIC WORLD EVENTS
#### Time System:
- **Day/Night Cycle:** 48 minute cycle (2 min/hour)
- **Day Phase:** 6am-6pm (normal gameplay)
- **Night Phase:** 6pm-6am (harder, more enemies)

#### Weather System:
- **Clear:** Normal conditions
- **Rain:** Reduced visibility, slippery movement
- **Storm:** Heavy rain, lightning, wind
- **Fog:** Very low visibility
- **Heat Wave:** High temperature, faster dehydration
- **Snow:** Cold, reduced movement, visibility
- **Radiation Storm:** Radiation damage, suit required

#### Seasons (each 7 days):
- **Spring:** Moderate, good for farming
- **Summer:** Hot, fast crop growth
- **Fall:** Cool, good harvests
- **Winter:** Cold, food scarce, survival hard

#### Dynamic Events:
- **Zombie Hordes:** 100+ zombies attack base
- **Raider Attacks:** 10-20 raiders assault
- **Supply Drops:** Loot falls from sky
- **Mutant Sighting:** Rare mutant appears
- **Natural Disaster:** Earthquake, fire, etc.
- **NPC Incident:** Injury, illness, betrayal
- **Equipment Failure:** Tools break at critical moment

### 13. INVENTORY SYSTEM
#### Features:
- **Grid-based:** 10x10 cells (100 slots)
- **Item Weight:** Each item has weight
- **Carry Limit:** 100kg base (upgradeable)
- **Rarity Tiers:** Common, Uncommon, Rare, Epic, Legendary
- **Durability:** Items degrade with use
- **Stacking:** Similar items stack to 99
- **Quick Slots:** 8 hotkeys for quick access
- **Equipment Slots:** Head, chest, legs, feet, hands, back, accessories (7 total)

#### Item Properties:
- **Name & Icon:** Visual identification
- **Description:** Detailed information
- **Weight:** kg per item
- **Durability:** 0-100% condition
- **Rarity:** Visual indicator
- **Special Properties:** Enchantments, bonuses
- **Crafting Ingredients:** Can be used in recipes

### 14. AUDIO DESIGN
#### Music System:
- **Exploration:** Calm, ambient (50-80 BPM)
- **Combat:** Intense, energetic (120-160 BPM)
- **Boss Battle:** Epic, powerful (140-180 BPM)
- **Shelter/Base:** Peaceful, warm (60-90 BPM)
- **Night:** Tense, dark (80-110 BPM)
- **Transitions:** Smooth fade between states

#### Ambient Sounds:
- **Rain:** Realistic rain ambience
- **Wind:** Variable based on weather
- **Fire:** Crackling fire sounds
- **Nature:** Birds, insects, animals
- **Urban:** Echoes, creaks, distant sounds
- **Technology:** Hum, beep, alert sounds

#### Sound Effects:
- **Footsteps:** Different per surface
- **Weapons:** Unique per weapon type
- **UI:** Click, confirm, error, notification
- **Crafting:** Sizzle, hammer, etc.
- **Building:** Placement, demolition, upgrade
- **Damage:** Hit, impact, pain
- **Equipment:** Equip, unequip, swap

### 15. USER INTERFACE
#### HUD Elements:
- **Health Bar:** Red, 0-100 HP
- **Hunger Bar:** Orange, 0-100
- **Thirst Bar:** Blue, 0-100
- **Stamina Bar:** Green, 0-100
- **Temperature Indicator:** -50 to +50°C
- **Experience Bar:** Current level progress
- **Compass:** Orientation indicator
- **Mini-map:** Local area, enemies, NPCs
- **Quest Tracker:** Current objectives
- **Notification Area:** Status alerts

#### Menus:
- **Inventory:** Item management
- **Character:** Stats, equipment, appearance
- **Crafting:** Recipe browsing, creation
- **Building:** Structure placement, management
- **NPC Management:** Survivor control panel
- **Skills:** Skill tree, experience spending
- **Technology:** Research unlocks
- **Settings:** Graphics, audio, gameplay
- **Map:** World exploration, markers
- **Quests:** Objective tracking, rewards

### 16. GRAPHICS & VISUAL EFFECTS
#### Art Style:
- **Resolution:** 16x16 pixel tiles (scalable)
- **Color Palette:** Limited, atmospheric
- **Animation:** Frame-based, 4-8 frames per action
- **Weather Effects:** Particle system for rain, snow, dust
- **Lighting:** Dynamic shadows, time-based
- **Damage Indication:** Blood, destruction visuals
- **UI Design:** Minimalist, information-dense

#### Performance Optimization:
- **Sprite Batching:** Reduce draw calls
- **Object Pooling:** Reuse common objects
- **Chunk Loading:** Only load visible areas
- **Particle Culling:** Disable off-screen effects
- **Animation Optimization:** Frame reduction

### 17. OPTIMIZATION STRATEGY
#### Performance Targets:
- **FPS:** 60 FPS on medium hardware
- **Memory:** <500MB RAM usage
- **Save File:** <10MB
- **Load Time:** <5 seconds

#### Techniques:
- **Chunk System:** Unload distant chunks
- **Object Pooling:** Pre-allocate common objects
- **Sprite Atlasing:** Combine textures
- **Physics Optimization:** Simpler colliders
- **AI Optimization:** Simplified pathfinding
- **Save Compression:** Binary format

### 18. SAVE SYSTEM
#### Features:
- **Auto-save:** Every 5 minutes
- **Manual Save:** Player initiated
- **Multiple Slots:** 10 save files
- **Backup System:** Automatic backups
- **Progress Tracking:** Hours played, statistics

#### Save Data:
- **Player State:** Position, inventory, stats
- **World State:** NPCs, buildings, loot
- **Time:** Day, season, weather
- **Progress:** Quests, technology, reputation
- **Game State:** Difficulty, settings

### 19. DIFFICULTY SYSTEM
#### Modes:
- **Story Mode:** Easy, forgiving
- **Standard:** Balanced
- **Hard:** Challenging
- **Nightmare:** Extreme, permadeath
- **Custom:** Player-defined difficulty

#### Scalable Parameters:
- Enemy health and damage
- Resource availability
- Survival drain rates
- Crafting recipes and times
- NPC availability

---

## DEVELOPMENT PHASES

### Phase 1: Foundation (Weeks 1-3)
- Character creation system
- Basic survival mechanics
- Inventory system
- Simple crafting
- Save/load system
- Basic UI

### Phase 2: World & Building (Weeks 4-6)
- World generation
- Building system
- NPC system basics
- Simple exploration

### Phase 3: Combat & Progression (Weeks 7-9)
- Combat system
- Enemy AI
- Skill tree
- Progression mechanics

### Phase 4: Polish & Features (Weeks 10-12)
- Dynamic events
- Audio system
- Visual effects
- Optimization
- Final balancing

---

## CONCLUSION

This design document provides comprehensive guidance for creating a AAA-quality 2D Survival Apocalypse game. Each system is detailed and interconnected, creating a deep, immersive gameplay experience. The phased development approach allows for iterative testing and refinement.

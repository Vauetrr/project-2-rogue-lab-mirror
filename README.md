**The University of Melbourne**
# COMP30019 – Graphics and Interaction

<p align="center">
  <img width="649" alt="image" src="https://user-images.githubusercontent.com/102025260/198919429-9c2618a6-b1d9-401e-835a-92a1ecd48924.png">
</p>

## Teamwork plan/summary

<!-- [[StartTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

<!-- Fill this section by Milestone 1 (see specification for details) -->

The general roles are split into two game developers (Max Semmler, Changmin Lee) and two game artists (Ishaann Cheema, Shuaixian Li). 
While each of the team members will focus on different aspects of the project, team members will also try to work together in different fields for a flexible workflow.

<!-- [[EndTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

## Final report

### Table of contents
* [Game Summary](#game-summary)
* [How To Play](#how-to-play)
* [Gameplay Design](#gameplay-design)
* [Asset Design](#asset-design)
* [Graphics Pipeline and Shaders](#graphics-pipeline-and-shaders)
* [Scene Transitions and UI Design](#scene-transitions-and-ui-design)
* [Procedural Generation](#procedural-generation)
* [Particle System](#particle-system)
* [Evaluation Techniques](#evaluation-techniques)
* [Feedback Implemented](#feedback-implemented)
* [References](#references)
* [Technologies](#technologies)
* [Images](#images)

### Game Summary
_Downfall_ is an isometric roguelite game, set in a kingdom that has fallen to ruin after the queen has died. The
protagonist has been framed for the murder, and is now trapped in the palace dungeon which they must escape from.

### How To Play
The goal of _Downfall_ is to defeat the final boss, reached by accessing a "warp zone" in the main level.
The warp zone will be placed somewhere in the procedurally generated map, which the player must find.

Enemies will block the path of the player. Defeating them will grant experience.
Gain enough experience to increase your level. Each level will provide the player with an option to increase
Health, Mana or Stamina/Stamina Regeneration.

Reaching a health of 0 ends the current "run", and the player will be brought to the start of a newly generated level.
Levels, experience, and increases in any stats are carried over in all runs.


#### Visual/Modifiable Player Stats

<p align="center">
  <img width="1440" alt="image" src="https://user-images.githubusercontent.com/102025260/198920058-a1488396-64ed-417c-92fb-5946b0baba22.png">
</p>

`Health` Red slider bar displays Health. Health will be lowered from taking enemy damage, and reaching 0 ends the current run.

  Health can be replenished by picking up HP healing items.


`Mana` Blue slider bar displays Mana. Mana is consumed from using Magic attacks. A magic weapon must be picked up to use Mana.

  Mana can be replenished by picking up magic weapons, or by successfully attacking with the Sword attack.


`Stamina` Thin green slider bar displays Stamina. Stamina is used for Rolling and Dashing. 

  Stamia will gradually recharge when not being consumed.


`Level` Level of the player. 

  Each level grants the player a new upgrade, and fully replenishes Health, Mana and Stamina.


`Experience` Thin transparent yellow bar at the bottom displays the experience of the player. The entire bottom of the gameplay screen denotes the progression of the player's experience

  Reaching max experience increases the player's level, and resets exp to 0.
  

#### General Movement

`[W]` `[A]` `[S]` `[D]` Move Player

`[Left Click]` Sword Attack (when a Sword is picked up)

`[Right Click]` Magic Attack (when a Magic item is picked up)

`[Space]` Roll

Hold `[Space]` while Rolling to Dash

`[Shift]` Guard

`[E]` Interact (Resulting action varies)


#### Upgrades (Available after Levelling up)

`[1]` Increase Health

`[2]` Increase Mana

`[3]` Increase Stamina/Stamina Regeneration


#### UI Settings

`[M]` Show/Hide minimap  _(available in the Main level, after the tutorial)_

`[P]` Show/Hide tutorial Text


#### Cheat Codes

`[Z]+[J]` Gain a huge amount of Health, Mana and Stamina

`[Z]+[L]` Warp to the final boss


### Gameplay Design

As a nature of Roguelites, _Downfall_ is designed to be very difficult for a new player to complete the game in their first attempt. The player gains experience, and eventually levels that grant the player more Health/Mana/Stamina that will aid the player in progressing further into the game: eventually beating the game. With usage of a `static GamePlayManager` with property `DontDestroyOnLoad`, the main statistics (Ex. Max Health/Mana/Stamina, Level, Exp) of the player carries over between level progressions and game overs. 

**Attacking** (which has a moderate delay) can be animation cancelled by **Rolling**, making the overall combat smoother. Animation of the `Player` mso cancels out appropriately. While **Rolling**, the player is immune to damage; however, this costs a considerable amount of `Stamina`. **Dashing**, which also consumes `Stamina`, is implemented for the player to move across the map quickly.

The player character is modified through a `PlayerMovementScript` controls the state of the player using booleans that describe the state of the player.

```
    private bool guarding = false;
    private bool dashing = false;
    private bool sprinting = false;
    public bool iframed = false;
    private bool defaultState = true;
    private bool attacking = false;
    private bool alive = true;
```
Depending on the state of the player, methods act differently: for example, `DecreaseHealth()` returns without decreasing health if `iframed = true`.


### Asset Design
One of the most important focus of this project is the design of map using procedural generation system. The original system uses a sets of planes with different orientations to procedurally generate the map each time the scene runs by assembling them randomly. In order to match the theme of dungeon and kingdom, this map is further improved by using our original creations of models. To improve the experience of exploration, more visiable/ accessable surfaces are used, horizontally and vertically, giving the sense of volume and allowing the change of levels to provide a more "dungeon-like" spatial experience in graphic perspective. More detailed models are used to replace universal planes as well to have better visual experience such as columns with plates, flooring with decoration, and walls with windows and balconies. With the increase of the complexity of the models, we decided to use another third-party modeling software, "Rhinoceros", to work models as NURB then import them into unity as [meshes](https://github.com/COMP30019/project-2-rogue-lab/tree/main/Assets/Temporary%20Assets/WaveFunctionCollapse/Old/Rhino%20Model). Thus, each plane is refined to a more vivid tile with our original models, and provides the final scenes.

Texture is another focus of the assets design. All [materials](https://github.com/COMP30019/project-2-rogue-lab/tree/main/Assets/Temporary%20Assets/Materials) use both main map and normal map to provide better visual experience while keeping the size of the project. To match with the theme, textures are selected based on medieval material reference and adjusted in image editor to have approximate colors, finally work together to provide concordant materials in the same scene.

Animation is implemented according to player's operation including walking, running, rolling and two kinds of attack. Animations are improved by selecting precise frame to implement code, which allows more sensible damage of the attacks instead of keeping damaging during the whole animation. This also helps to implement precise sound effects like footsteps which could change based on the pace, improving the gaming experience both visually and audibly.

The golum was made in blender with animations imported from mixamo.
The cloak, the sword and the breakable boxes were also made in blender.
The tiles and the tutorial scene were created in rhino.
The textures, sounds, knight and sorcerer models and all the animations were imported assets.

### Graphics Pipeline and Shaders
This project uses the Built-in Render Pipeline, which allowed the project to utilise custom vertex/fragment shaders.
Two of these shaders, which are the ones to be marked, are the ["low health shader"](/Assets/Shaders/LowHealth.shader)
and the ["depth of field shader"](/Assets/Shaders/DoF.shader).

The "low health shader" creates multiple post-processing effects in the fragment shader to graphically show the player
they are hurt and close to dying. It creates an inverted-colour and red-tinted vignette around the screen, 
shifts the colours towards a greyscale, and applies a "double vision" effect when the player is hit at low health.
These effects also scale with the severity of the situation, i.e., how low in health the player is. These effects are 
achieved by applying a mask to the screen, by smooth stepping between a radius value and radius+feather value,
depending on how close the current pixel is to the centre of the screen. This creates a separation between the normal
area on the inside, vignette area on the outside, and by smooth stepping, creates a transition between the two. The
vignette area has its colour inverted, then multiplied by a specified tint colour. The normal area has its colour
lerped between a left and right offset (for double vision), then lerped again between itself and a greyscale version
of itself. These areas are then combined to create the low health shader effect.

<p align="center">
  <img src="Gifs\lowhealth.gif" width="300">
</p>

The "depth of field shader" applies a depth of field post-processing effect in the fragment shader by bluring the screen
depending on the depth of the object at that point on the screen, and how far away that depth according to a specified
focal distance/plane and focal range. In practice, each pixel has a "circle of confusion" value calculated, indicating
how wide an area is used by the camera to capture light for that pixel. When blurring each pixel, this value was used
to indicate how blurry that pixel should be (i.e., how far an offset should be used when applying the gaussian blur).
Thus, this creates a depth of field effect, and helps the player in focusing on the player character and the action
surrounding them.

<p align="center">
  <img src="Gifs\dof.jpg" width="300">
</p>

Shader paths:
* Low Health Shader: Assets/Shaders/LowHealth.shader
* Depth of Field Shader: Assets/Shaders/DoF.shader

### Scene Transitions and UI Design


<img width="500" alt="image" src="https://user-images.githubusercontent.com/102025260/198918507-2ae7e566-0a6d-4aab-8aad-ff8930acea7d.png">
For a more polished gameplay experience, the screen fades in/out between scene transitions with a loading screen. <br/><br/><br/>


<img width="220" alt="image" src="https://user-images.githubusercontent.com/102025260/198918955-5fff7e5b-2992-4190-84c7-c6bc3d42a835.png">
A map of the overall level is avaiable on the main level for better traversal through the game. <br/><br/><br/>


<img width="500" alt="image" src="https://user-images.githubusercontent.com/102025260/198918900-e6f306a5-36d7-4ca4-95db-d3f02c1fdd9c.png">
<img width="500" alt="image" src="https://user-images.githubusercontent.com/102025260/198918931-07d78a80-77f7-4f64-882d-131ca11f2022.png">
The tutorial text and the map can be enabled and disabled as the user prefers. <br/><br/>

For a more visually appealing UI, TextMeshPro was utilised for the UI text, with custom fonts imported from Google Fonts. (ref 1)

### Procedural Generation
for procedural generation we are using the wavefunction collapse algorithm. (ref 2)
before the algorithm starts tiles are made then rules for how those tiles can be placed together are given. The tiles are then split into the 4 rotations around the y axis. Tiles are then preselected such as lava being placed around the map. The entropy (the number of diffrent tiles that can be placed in a given slot) of each slot is then calculated and the slot with the lowest entropy is chosen and a random tiles that can fit in the slot is selected. This is repeated until every tile is filled in.
All the tiles have information about which adjacent tiles the player can move to from the tile. Using this a graph can be produced and the breadth first search algorithm is used to calculate which tiles the player can reach and how many tiles the player must walk to reach a given tile this is used to scale the difficult as you go further through the dungeon (loot boxes have a higher chance of spawning an enemy). 
Sometimes the wavefunction collapse algorithm can fail or the map can have few walkable tiles if this is the case the algorithm is run again. The tile with the furthest walk distance to the start tile is chosen to contain the boss.
(Note that we are using an experimental unity package to generate nav meshs at run time (ref 3) )

### Particle System
The particle system created for this project is a [fire particle system](/Assets/Particle%20Systems/Fire.prefab). This
particle system was created by making four abstract "fire" shapes, then generating particles of one of the four shapes
in an upwards cone with slight randomised rotation. The size, colour, speed, lifetime, and rotation were fine tuned to 
have one subsystem look like smoke, and one to look like the fire itself. These attributes were also modified over time
to help make the fire look alive and growing. Additionally two other subsystems were used. One generated the standard 
particle with a large size to simulate a fire's glow, and the other generated many small standard particles as embers. 
The embers utilised the built-in noise feature to sway the embers' travel paths and make it truly look like they were 
randomly influenced by the wind.

<p align="center">
  <img src="Gifs\fire.gif" width="300">
</p>

Particle system path: Assets/Particle Systems/Fire.prefab

### Evaluation Techniques
For evaluation purposes, 5 participants were selected. Each participant played a section of the game and were asked to 
particpate in the "think aloud" querying method. Hence, their footage was recorded, and their thoughts the players said
out loud were transcribed to paper. Afterwards, the participants were asked 10 questions from the System Usability Scale 
(SUS) which were rated from 1-5, and each of them had a calculated SUS score. To calculate the SUS score, odd-numbered 
questions had 1 subtracted from their score,and even-numbered questions had their score subtracted from 5, before adding 
the scores together and multiplying the total by 2.5.

The 10 questions used were:
1. I think that I would like to play this game frequently.
2. I found the game unnecessarily complex.
3. I thought the game was easy to play.
4. I think that I woul dneed the support of a technical person to be able to play this game.
5. I found the various functions/systems in the game were well integrated.
6. I thought there was too much inconsistency in the game.
7. I would imagine that most people would learn how to play this game very quickly.
8. I found the game was very cumbersome to use.
9. I felt very confident in being able to play the game.
10. I needed to learn a lot of things before I could start playing well in the game.

The participants were:
1. Participant A - 21 years old, male, familiar with games, 92.5 SUS score
2. Participant B - 20 years old, male, familiar with games, 82.5 SUS score
3. Participant C - 20 years old, female, unfamiliar with games, 60 SUS score
4. Participant D - 21 years old, male, unfamiliar with games, 75 SUS score
5. Participant E - 22 years old, female, familiar with games, 87.5 SUS score

Lots of valuable feedback was gleaned from the participants. High SUS scores and thoughts from the participants 
showed that the game's movement and combat were straightforward, and it wasn't too hard to learn the core gameplay loop.
However, Participant C, who was unfamiliar with games, found the system's usabililty lacking (shown by the lower SUS score), 
stating it was "a bit too complex", and found the tutorial text to be overwhelming at the start. Beyond that, the 
observational methods were able to shed light on some significant issues not found when developing the game:
* No death zone existed below the level, leading to stuck states
* Graphical bug in objects occluding when touching them, rather than when they are occluding the player
* Graphical bug in the golem's melee attack lacking a texture
* Initial bridge has a collision gap, leading to a stuck state
* Increasing health from health pickups caused the double vision effect to trigger (from the low health shader)

### Feedback Implemented
By utilising the feedback outlined above, substantial changes were made to the game to improve on it. Participants A, B, 
and C first played the game. This lead to some improvements, before letting participants D and E provide even more 
feedback. 

From the initial round of feedback, the following was implemented due to the participants' evaluations:
* Participant C's comment on the game being initially too overwhelming prompted the integration of a dedicated tutorial level.
* Making the player die when the fall off the level
* Improving the object dithering and occluding object detection
* Fixing the graphical bug relating to the golem's melee attack

After listening to the feedback from participant D and E, the following was fixed:
* Health pickups only reduce the low health shader effect, and don't trigger any double vision effects
* Initial bridge doesn't have a collision gap anymore, preventing the stuck state

Beyond this, improvements were made to the game to try and make it a more enjoyable experience, such as introducing a
mini-map, and providing a levelling system to make the player progress and become stronger.

### References
#### Learning
1. https://www.youtube.com/watch?v=_1fvJ5sHh6A

2. https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.0/manual/index.html

3. https://www.ronja-tutorials.com/post/042-dithering/

4. https://www.ronja-tutorials.com/post/023-postprocessing-blur/

5. https://catlikecoding.com/unity/tutorials/advanced-rendering/depth-of-field/

6. https://www.youtube.com/watch?v=GiQ5OvDN8dE

7. https://www.youtube.com/watch?v=5Mw6NpSEb2o

#### Graphics
1. https://fonts.google.com/

#### Sound
1. "All This"
Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 3.0
http://creativecommons.org/licenses/by/3.0/

2. "Constance"
Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 3.0
http://creativecommons.org/licenses/by/3.0/

3. "Killers"
Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 3.0
http://creativecommons.org/licenses/by/3.0/

4. "bensound-epic" - Benjamin Tissot (bensound.com)

### Technologies
Project is created with:
* Unity 2022.1.9f1 
* Rhinoceros version: 7.4
* Photoshop 2022.4.2

### Images

<p align="center">
  <img src="Gifs\00.jpg" width="300">
</p>
<p align="center">
  <img src="Gifs\01.jpg" width="300">
</p>
<p align="center">
  <img src="Gifs\02.jpg" width="300">
</p>
<p align="center">
  <img src="Gifs\03.jpg" width="300">
</p>

<p align = "center"><img width="600" alt="image" src="https://user-images.githubusercontent.com/102025260/198920700-4b5c44d2-2340-4e60-aa79-0aa9ba8195ca.png"></p>


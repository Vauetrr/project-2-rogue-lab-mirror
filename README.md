

**The University of Melbourne**
# COMP30019 – Graphics and Interaction

## Teamwork plan/summary

<!-- [[StartTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

<!-- Fill this section by Milestone 1 (see specification for details) -->

The general roles are split into two game developers (Max Semmler, Changmin Lee) and two game artists (Ishaann Cheema, Shuaixian Li). 
While each of the team members will focus on different aspects of the project, team members will also try to work together in different fields for a flexible workflow.

<!-- [[EndTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

## Final report

Read the specification for details on what needs to be covered in this report... 

Remember that _"this document"_ should be `well written` and formatted **appropriately**. 
Below are examples of markdown features available on GitHub that might be useful in your report. 
For more details you can find a guide [here](https://docs.github.com/en/github/writing-on-github).

### Table of contents
* [Game Summary](#game-summary)
* [How To Play](#how-to-play)
* [Gameplay Design](#gameplay-design)
* [Asset Design](#asset-design)
* [Graphics Pipeline and Shaders](#graphics-pipeline-and-shaders)
* [Procedural Generation](#procedural-generation)
* [Particle System](#particle-system)
* [Evaluation Techniques](#evaluation-techniques)
* [Feedback Implemented](#feedback-implemented)
* [References](#references)
* [Technologies](#technologies)
* [Using Images](#using-images)
* [Code Snipets](#code-snippets)

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
`Health` Health will be lowered from taking enemy damage, and reaching 0 ends the current run.

  Health can be replenished by picking up HP healing items.


`Mana` Mana is consumed from using Magic attacks. A magic weapon must be picked up to use Mana.

  Mana can be replenished by picking up magic weapons, or by successfully attacking with the Sword attack.


`Stamina` Stamina is used for Rolling and Dashing. 

  Stamia will gradually recharge when not being consumed.


`Level` Level of the player. 

  Each level grants the player a new upgrade, and fully replenishes Health, Mana and Stamina.


`Experience` Experience of the player. 

  Reaching max experience increases the player's level, and resets exp to 0.
  

#### General Movement

`[W]` `[A]` `[S]` `[D]` Move Player

`[Left Click]` Sword Attack (when a Sword is picked up)

`[Right Click]` Magic Attack (when a Magic item is picked up)

`[Space]` Roll

Hold `[Space]` while Rolling to Dash

`[Shift]` Guard


#### Upgrades (Available after Levelling up)

`[1]` Increase Health

`[2]` Increase Mana

`[3]` Increase Stamina/Stamina Regeneration


#### UI Settings

`[M]` Show/Hide minimap  _(available in the Main level, after the tutorial)_

`[P]` Show/Hide tutorial Text


#### Cheat Codes, available after the game has begun

`[Z]+[J]` Gain a huge amount of Health, Mana and Stamina

`[Z]+[L]` Warp to the final boss


### Gameplay Design

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
Two of these shaders, which are the ones to be marked, are the ["low health shader"](Assets\Shaders\LowHealth.shader)
and the ["depth of field shader"](Assets\Shaders\DoF.shader).

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

The "depth of field shader" applies a depth of field post-processing effect in the fragment shader by bluring the screen
depending on the depth of the object at that point on the screen, and how far away that depth according to a specified
focal distance/plane and focal range. In practice, each pixel has a "circle of confusion" value calculated, indicating
how wide an area is used by the camera to capture light for that pixel. When blurring each pixel, this value was used
to indicate how blurry that pixel should be (i.e., how far an offset should be used when applying the gaussian blur).
Thus, this creates a depth of field effect, and helps the player in focusing on the player character and the action
surrounding them.

### Procedural Generation
for procedural generation we are using the wavefunction collapse algorithm. (ref 1)
before the algorithm starts tiles are made then rules for how those tiles can be placed together are given. The tiles are then split into the 4 rotations around the y axis. Tiles are then preselected such as lava being placed around the map. The entropy (the number of diffrent tiles that can be placed in a given slot) of each slot is then calculated and the slot with the lowest entropy is chosen and a random tiles that can fit in the slot is selected. This is repeated until every tile is filled in.
All the tiles have information about which adjacent tiles the player can move to from the tile. Using this a graph can be produced and the breadth first search algorithm is used to calculate which tiles the player can reach and how many tiles the player must walk to reach a given tile this is used to scale the difficult as you go further through the dungeon (loot boxes have a higher chance of spawning an enemy). 
Sometimes the wavefunction collapse algorithm can fail or the map can have few walkable tiles if this is the case the algorithm is run again. The tile with the furthest walk distance to the start tile is chosen to contain the boss.

### Particle System
The particle system created for this project is a [fire particle system](Assets\Particle%20Systems\Fire.prefab). This
particle system was created by making four abstract "fire" shapes, then generating particles of one of the four shapes
in an upwards cone with slight randomised rotation. The size, colour, speed, lifetime, and rotation were fine tuned to 
have one subsystem look like smoke, and one to look like the fire itself. These attributes were also modified over time
to help make the fire look alive and growing. Additionally two other subsystems were used. One generated the standard 
particle with a large size to simulate a fire's glow, and the other generated many small standard particles as embers. 
The embers utilised the built-in noise feature to sway the embers' travel paths and make it truly look like they were 
randomly influenced by the wind.

### Evaluation Techniques

### Feedback Implemented

### References
1 - https://www.youtube.com/watch?v=_1fvJ5sHh6A

### Technologies
Project is created with:
* Unity 2022.1.9f1 
* Rhinoceros version: 7.4
* Photoshop 2022.4.2

### Images

Project Processes:

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


### Code Snippets 

You may wish to include code snippets, but be sure to explain them properly, and don't go overboard copying
every line of code in your project!

```c#
public class CameraController : MonoBehaviour
{
    void Start ()
    {
        // Do something...
    }
}
```

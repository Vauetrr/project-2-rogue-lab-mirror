

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

### Gameplay Design

### Asset Design
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
* Ipsum version: 2.33
* Ament library version: 999

### Using Images

You can include images/gifs by adding them to a folder in your repo, currently `Gifs/*`:

<p align="center">
  <img src="Gifs/sample.gif" width="300">
</p>

To create a gif from a video you can follow this [link](https://ezgif.com/video-to-gif/ezgif-6-55f4b3b086d4.mov).

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

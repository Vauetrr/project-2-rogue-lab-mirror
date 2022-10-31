

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
* [Graphical Design](#graphical-design)
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

### Graphical Design

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

The "depth of field shader" applies a depth of field post-processing effect in the fragment shader by bluring the screen
depending on the depth of the object at that point on the screen, and how far away that depth according to a specified
focal distance/plane and focal range. In practice, each pixel has a "circle of confusion" value calculated, indicating
how wide an area is used by the camera to capture light for that pixel. When blurring each pixel, this value was used
to indicate how blurry that pixel should be (i.e., how far an offset should be used when applying the gaussian blur).
Thus, this creates a depth of field effect, and helps the player in focusing on the player character and the action
surrounding them.

### Procedural Generation

### Particle System
The particle system created for this project is a [fire particle system](/Assets/Particle%20Systems/Fire.prefab). This
particle system was created by making four abstract "fire" shapes, then generating particles of one of the four shapes
in an upwards cone with slight randomised rotation. The size, colour, speed, lifetime, and rotation were fine tuned to 
have one subsystem look like smoke, and one to look like the fire itself. These attributes were also modified over time
to help make the fire look alive and growing. Additionally two other subsystems were used. One generated the standard 
particle with a large size to simulate a fire's glow, and the other generated many small standard particles as embers. 
The embers utilised the built-in noise feature to sway the embers' travel paths and make it truly look like they were 
randomly influenced by the wind.

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
showed the game's movement and combat were straightforward, and it wasn't too hard to learn the core gameplay loop.
However, Participant C, who was unfamiliar with games, found the system's usabililty lacking, stating it was "a bit
too complex", and found the tutorial text to be overwhelming at the start. Beyond that, the observational methods were
able to shed light on some significant issues. For example, 

### Feedback Implemented
By utilising the feedback outlined above, substantial changes were made to the game to improve on it. For instance,
Participant C's comment on the game being initially too overwhelming prompted the integration of a dedicated tutorial
level.

### References

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

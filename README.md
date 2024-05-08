# SFA STEM Circuit Challenge 2 (SSCC2)
 A simple puzzle game for designed for students based on completing circuits.

## The Basics
 SFA STEM Circuit Challenge 2 is a 2D puzzle game designed for students aged 8-12 about completing circuits. The students play as a robot who can move using the arrow keys (or WASD keys) to push computer chips that conduct electricity throughout a circuit-themed labyrinth. The chips are color-coded, and will only work if pushed to their correct spot. The robot can also use his laser eyes (the Space key) to blast destructible red bug blocks out of his way in his mission! The robot must avoid the mysterious purple hazards on his journey, but if he touches them more than 3 times, he will explode!

 In the event that players get stuck and can no longer complete a level, the 'R' key may be used to instantly restart the level. If you want to reset the game to the title screen, the 'F2' key may be used to do so.

 Levels are divided into 2 difficulties, the 'introduction' and the 'challenge' levels. Challenge levels are, well, very challenging, and should only be attempted by students if they have beaten all four introduction levels. The level labeled 'custom' will be discussed later in the "Custom Levels" section of this document.

## Use with the Makey Makey device
 SSCC2 is designed to be functional with the Makey Makey educational device. It can be used as a controller for the game by using the Directional Pad on the Makey Makey, as well as the Space button for firing the robot's laser and navigating menus. Further information about the Makey Makey, such as how to set the device up, can be found in the documentation that came with the device.

## Custom Levels
 SSCC2 supports user modified levels, and fully custom levels! This gives older students a chance to practice their design and logic abilities by creating circuit puzzles of their very own to solve! A video version of this guide can also be found at [this link](https://youtu.be/KnSneu8d9TQ).

 When downloading the game, open the folder that contains the game's executible file ('SFA STEM CC 2.exe'), and open the folder titled 'SFA STEM CC 2_Data'. When inside of this folder, open the folder titled 'Levels'. This will bring you to a list of all levels in the games, labeled by "level<packNumber>-<levelNumber>.txt". Any of these levels may be modified as desired by the user, but to modify the level pack shown in the game under the name 'Custom', you must begin with the file named "levelc-1.txt". Double click this file to open it in a text editor.

 ### Level File Structure
 Levels in SSCC2 follow the following format. Try to make sure that you follow this structure, or else the game might not run your custom level!

 The first row contains a number telling the game how large your level will be on the X axis, or how many blocks 'long' your level will be. The second row contains a number telling the game how large your level will be on the Y axis, or how many blocks 'tall' your level will be.

 The next line contains the name of your level, so call it whatever you like! Just know that the font used in game doesn't support numbers or special characters, so those may appear in a different font!

 The meat and potatoes of the level structure begins now, so get ready for a bit of a list!
 - 'R's designate the starting location of the robot, you can have multiple robots in a level
 - '0's designate an open part of the level that the robot can walk on
 - '1's designate a part of the wall that the robot cannot walk through
 - '2's designate a breakable bug block that the robot can blast through using their laser
 - 'X's designate a purple hazard that will damage the robot when they touch it
 - 'B's designate a battery that should ideally be placed within a wall, wires will need to be in the spot above and below the battery to carry connections to doors
 - 'W's designate a wire with a wall behind it
  - Be careful not to put wires that run alongside each other too close, or the game won't know which wires go with which path, and will likely lead to unexpected results! Be sure to leave gaps between separate wire paths.
 - 'w's designate a wire without a wall behind it so that the player can walk through it
 - 'D's designate a door which must have two wires coming into it to carry a possible connection to open the door
 - '(', '\[', '{', and '<' all designate chips that the robot can push around to complete circuits made of wires
 - ')', '\]', '}', and '>' all designate spots that the chips must go into to complete circuits, these should have two wires touching them to carry a connection
 - 'E's designate a portal that lets the robot exit the level and go to the next one

These elements must be laid out in a grid that's the size you specified with the X axis and Y axis sizes on the first two lines of the file.

Finally, if this isn't the last level in the level pack, use the final line of the file to write the name of a different .txt file with another level that's in the same 'Levels' folder. If this is the final level of your level pack, leave this area blank, and the game will know this is the end of your level pack!

## Disclaimers
This game is released under Creative Commons (CC-BY-SA-4.0). This means that you are free to modify, change, and distribute this game however you please! However, if you do so, the project must be distributed under the same license, meaning that your changes must also be public for other people to change as they please! Keep the open-source train going and support developers!

This game is done as a passion project in collaboration with my volunteering with Stephen F. Austin State University's STEM Reserach and Learning Center. It is used heavily by the center with the Makey Makey device, but I wanted to release it for all to use as wanted! If you feel the desire to support me further, please check out my game Frog Flip! on itch.io or the Google Play Store.
 

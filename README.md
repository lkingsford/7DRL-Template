7DRL18 Template

This is a basic C#/Monogame project to provide a basis for me for 7DRL 2018.
It may be helpful to other people too.

It currently includes:
- Pathfinding
- Basic state machine

I'll be using it with the game logic in the Game project. The Desktop will only
have the Desktop specific code. If you were to add an Android version, add a 
reference to the Game project and put the Android specific code in it. If I do
it, I'll add a reference to the same monogame content file to any other
projects. Any common presentation can go into another shared project or libary
per your preference.

Transfer between states by adding new states to State.StateStack.

Any useful fairly bug free pull requests will probably be accepted.
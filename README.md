# Siegebound

A 3D tower defense game built in Unity and C# by a 2-person team for Game Programming at Northeastern University (Summer 2026).

**[▶ Play it here](https://play.unity.com/en/games/e96c235e-12b7-4b23-8040-9a7728654566/siege-bound)**
<img width="1292" height="674" alt="image" src="https://github.com/user-attachments/assets/835614ed-9eef-4f3b-b82b-99367da9603e" />

## About the Game
Defend your base against waves of enemies by placing Mage Towers, Archery Towers, and Cannons along their path. Each level throws increasingly tough waves at you, including flying enemies that only certain towers can hit and a Skeleton King boss.

## My Contributions
- **Enemy system:** spawning logic and behavior scripts for 4 enemy types (Crow, Goblin, Skeleton, Skeleton King AI)
- **Wave manager:** configurable waves per level, enemy composition, and spawn timing, all set in the Unity Inspector without code changes
- **Game-state logic:** win/loss conditions, damage handling, and flying enemies targetable only by specific towers
- **Level design:** designed Level 2 and balanced the final waves, enemy prefabs, and animations
- **Version control:** handled nearly all merges and merge conflicts for the team

Built with Edmund Chin.

## Tech
Unity 6 (6000.4.6f1), C#, NavMeshAgent pathfinding, Universal Render Pipeline

## Note
Third-party art assets (Synty Studios, Simple Fantasy) are not included due to licensing, so the project will be missing models if opened directly in Unity. Use the play link above to try the game.

# 🏃 Unity Character Controller 🏃

![banner](https://github.com/akihiko47/Character-Controllers/blob/main/Images/running.gif)

In this repository, I implemented a character controller with animations and several camera modes.

## Installation 🔧
There are 2 ways:
- import `CharacterController.unitypackage` via *Assets-Import* Package
- clone/download this repository and move the `Assets/Scripts/Character`, `Assets/Animations/Character` and `Assets/Prefabs/Character` folders to your Unity project's Assets folder

## Camera types 📷
You can add camera scripts from `Assets/Scripts/Character/Camera` to your camera GameObject and use them. Cams have settings such as sensitivity, smoothing time, bounds, ...
1) **Third-person**
2) **First-person**

## Animations 〰️
In folder `Assets/Animations/Character` you can find different animations for some character states. Right now animations graph looks like this:
![animations](https://github.com/akihiko47/Character-Controllers/blob/main/Images/animations.jpg)

## Usage 🎮
1) Add character prefab from `Assets/Prefabs/Character` to your scene
2) Add camera script to your main camera
3) Configure character and camera settings

I should note that this system works well with other humanoid character models.
![characters](https://github.com/akihiko47/Character-Controllers/blob/main/Images/characters.jpg)


Thank you for reading this 😊!
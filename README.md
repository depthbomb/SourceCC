<div align="center">
	<h1><code>Source</code> <code>C</code>ache <code>C</code>leaner</h1>
</div>

This project was inspired by [Cache File Cleaner by Ah_Roon](https://gamebanana.com/tools/6688) to expand on the functionality and to practice my C#.

### Usage
Simply choose the game you wish to clean up and press the big button.
The directories that are set by default are the most common ones that the games may be located. You may change these directories by clicking the **Settings** button.

For the best result, choose the `Team Fortress 2\tf` folder for TF2 and the `\Left 4 Dead 2\left4dead2` folder for L4D2.

As far as I know, you shouldn't find any .cache files in the *left4dead2_dlc#* folders.

### Features
- Support for multiple Source games
  - TF2 and L4D2 currently available
- No installation required, run the app from anywhere
- Ability to change directories if your game installation differs from the norm

### Requirements
- This app requires _.NET Framework 4.5_ or higher.
  - It is highly likely that you already have this installed. If you don't, however, you may get it [here.](https://www.microsoft.com/en-us/download/details.aspx?id=30653)
- It runs on Windows 8.1 and Windows 10.
  - It will likely run on Windows 7 and Windows Vista but I am unable to test them.

### Disclaimer
This program does save info to your system, however this is merely configuration data such as custom paths that you set. If you wish to see the files that the program may write, you can find them in `C:\Users\USERNAME\AppData\Local\Caprine_Logic\`.

Want a simpler way to clean up cache files? Go check out [Ah_Roon's solution](https://gamebanana.com/tools/6688) instead!
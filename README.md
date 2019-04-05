This repository is used as a shear modulus.
It **must** be included in an editor folder.

For Example Assets/Editor/Ci

# How to Use
Start unity in an Console Applicaiton and execute the command "AutoBuilder.<BuildPlatform>"
Or Click in the Toolbar "Tools/Build/<BuildPlatform>"

# Supported Platforms
- Android
- AndroidGradle
- IOS
- Linux
- PS4
- XboxOne
- WebGl
- Windows 64bit

# Andoird Setup

JDK needs to be setup. Recomendet is the default Folder
you also need some spesific Environment variables

| Key                                         | Description                                  |
|---------------------------------------------|----------------------------------------------|
| ANDROID_KEYSTORE_ + ProductName + _NAME     | Generate Keystone path on the build computer |
| ANDROID_KEYSTORE_ + ProductName + _PASSWORD | Generate Keystone password                   |
| ANDROID_KEYALIAS_ + ProductName + _NAME     | Generate Keyalias name                       |
| ANDROID_KEYALIAS_ + ProductName + _PASSWORD | Generate Keyalias password                   |
| ANDROID_SDK_PATH                            | Path to the Android SDK                      |
| ANDROID_NDK_PATH                            | Path to the Android JDK                      |
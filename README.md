# IDMapping

IDMapping for Unity3D

**Odin Inspector Asset recommended to usage with this Package (https://odininspector.com)**

# Table of Contents

- [Overview](#Overview)
- [Getting Started](#Getting-Started)
  - [Generating Scripts](#Generate-scripts)
  - [Creating the ISO File](#Create-ISO)
- [ISO Settings](#ISO-Settings)
- [DEMO](#DEMO)

# Overview
- This tool serves as an alternative to enums with enhanced functionality.
- You can use Scriptable Objects both for storing IDs and for keeping additional data.
- It provides convenient visualization in the Editor and represents values as integers in the code.

# Getting Started
To use the module, you need to define **GAME_MODULES_ENABLE** in **Edit -> Project Settings -> Player -> Other Settings -> Script Compilation**.

Don’t forget to click the **Apply** button.

![image](https://github.com/user-attachments/assets/f9999909-7479-40a4-ab2b-de55a3f267e3)

## Generate scripts

**Okay! Now you can start working with the tool!**

**Right-click** on any folder (except `Assets`) in the **Project** window.
Select **Create -> Id Generator**.

![image](https://github.com/user-attachments/assets/c207dec2-a29e-4bd3-9598-bb49f0d17001)

A dialog will open where you need to enter the desired class name.
Name the class as you would normally, for example, `Car`, `House`, `Shape`, etc., and click the **Create Map** button.

![image](https://github.com/user-attachments/assets/84eb4283-a215-4a83-8689-2d092b7b2a75)

After generating the scripts, you will see four scripts appear in the folder you selected.

![image](https://github.com/user-attachments/assets/67ae0a6a-591a-4495-8d5a-473861ac826f)

## Create ISO

Now that we have the necessary scripts, we need to create an ISO file.

1. **Right-click** on the folder where you want to store it.
2. Select **Create -> Games -> Maps -> Your Map**.

![image](https://github.com/user-attachments/assets/3d16677d-9420-45c4-a59b-d0a9722fb3a5)


# ISO Settings
Now, in the configuration we created, you can add any number of elements to the list.
Each element serves as an independent data container.

![image](https://github.com/user-attachments/assets/b4153ca3-4c33-4c40-a8ff-5f063ddabbd7)


By default, the container includes only an `id` and a `name`, but you can add any additional fields that are required for your logic.

![image](https://github.com/user-attachments/assets/997dbc51-41e4-4fa2-a82b-041751755243)


The **Generate Static Properties** button generates a `partial struct`, allowing you to access the ID of an element from your map anywhere in the code.
This is particularly useful in cases where you need to compare an object's ID or assign an ID in the code.

![image](https://github.com/user-attachments/assets/356f5f78-0e84-4108-a342-c3b33fada1d4)


# DEMO

In the module, you can find a demo at **GameModules -> IdMapping -> Demo**. Simply open the scene and click the **Play** button. 
If you don't have **Text Mesh Pro** installed, Unity may prompt you to install this plugin when you load the scene.

![demo](https://github.com/user-attachments/assets/79d85f49-72f5-48d3-856c-c1e39182dc47)





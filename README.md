EventManager
============

A type-safe event system for Unity3D based on the event listener pattern. The
code in this repository is a minor variation of the event system originally
described here:  http://www.willrmiller.com/?p=87

This repository was created with Unity version 5.3.1

Features
--------

* Loosely coupled
* Type-safe
* Avoids breaking changes when new event parameters are added

Installation
------------

Copy the Assets/EventManager folder to your project's Assets folder.

### Source and Unit Tests:

NOTE: Unit tests are stored in an 'Editor' folder so they are not added to your build

    mkdir tmp
    cd tmp
    git clone https://github.com/robertwahler/EventManager
    cp -R EventManager/Assets/EventManager ~/your_unity_project/Assets/

### Examples (optional)

    cp -R EventManager/Assets/Examples ~/your_unity_project/Assets/EventManager/

Testing
-------

The NUnit test framework is included in Unity 5.3 and higher.  Tests require
installation of the UnityTestTools asset for Unity 5.2 and lower. 

There is no Unity hotkey for running tests. Instead, manually use this menu sequence:

    Main Menu: Window, Editor Tests Runner
    Editor Tests: Run All

Usage
-----

See Assets/Examples

License
-------

MIT, see ./LICENSE for details.

EventManager
============

A type-safe event system for Unity3D based on the event listener pattern.
Original blog article http://saltydog.digital/usage-pattern-for-a-type-safe-unity-event-system/

The code in this repository is a minor variation with added examples and tests
of the event system originally described here:
http://www.willrmiller.com/?p=87

Features
--------

EventManager is useful when you want to keep your codebase loosely coupled.
Publishers and subscribers don't need to know anything about each other.
EventManager is not a solution for exposing callbacks in the Unity Editor,
Unity's own EventSystem may be more appropriate for that use case.

* Loosely coupled
* Type-safe
* Avoids breaking changes when new event parameters are added

Installation
------------

Install the source or build a DLL.

### Option 1: Source Installation

Copy the Assets/EventManager folder to your project's Assets folder.

#### Source and Unit Tests

NOTE: Unit tests are stored in an 'Editor' folder so they are not added to your build

    mkdir tmp
    cd tmp
    git clone https://github.com/robertwahler/EventManager
    cp -R EventManager/Assets/EventManager ~/your_unity_project/Assets/

#### Examples (optional)

    cp -R EventManager/Assets/Examples ~/your_unity_project/Assets/EventManager/

#### Record Version SHA (optional)

    git --git-dir=./EventManager/.git log --pretty=format:%h -1 > ~/your_unity_project/Assets/EventManager/VERSION

### Option 2: DLL Installation

Create a DLL and copy it to your Assets folder.  These instructions expect the
standard Mono mcs compiler.  If you need it on OS X, you can install it with
HomeBrew ```brew install mono```

    mkdir build

    mcs -recurse:'Assets/EventManager/Source/*.cs' \
        -lib:/Applications/Unity/Unity.app/Contents/Frameworks/ \
        -lib:/Applications/Unity/Unity.app/Contents/Frameworks/Managed/ \
        -r:UnityEngine \
        -r:UnityEditor \
        -target:library \
        -out:build/SDD.EventManager.dll

    mkdir Assets/Lib

    cp build/SDD.EventManager.dll ~/your_unity_project/Assets/Lib/

### Option 3: Installation via NPM

To install via NPM the following is needed:
* Install the [Node.js package manager](https://nodejs.org/en/download/)
* In your Unity-project run `npm install --save unity3d.eventmanager`
* Extend your `.gitignore` (or whatever else you're using)
  * Add `node_modules`
  * Add `package-lock.json`
  * Add `Assets/packages/`
  * Add `Assets/packages.meta`
  
You can also add a `package.json` to your Unity project that contains the following content:
```
{
  "dependencies": {
    "unity3d.eventmanager": "1.0.2"
  }
}
```
This way you can define what dependencies you need in a reproducible way.

    
Testing
-------

The NUnit test framework is included in Unity 5.3 and higher.  Tests require
installation of the UnityTestTools asset for Unity 5.2 and lower.

### Running tests from Unity IDE

There is no Unity hotkey for running tests. Instead, manually use this menu sequence:

    Main Menu: Window, Editor Tests Runner
    Editor Tests: Run All

### Running tests from the command line

All command line test scripts require a Ruby Thor scripting environment with
Ruby > 2.0. These commands are configured for execution on Mac OS X. Other
environments will need to modify the Ruby source in the ./tasks folder.

    gem install bundler
    bundle install

#### Using Unity to run the tests (SLOW)

You need to shutdown the Unity IDE to run this command.

    thor test:unit

#### Using NUnit-console (FAST)

The Unity IDE can be running.

    thor test:nunit

#### Using Guard and the NUnit-console (CI)

This command will watch for file changes and automatically run the unit test
suite. The Unity IDE can be running.

    bundle exec guard

Syntax checking with Vim
------------------------

Do you use Vim instead of MonoDevelop/Visual Studio? Install
https://github.com/neomake/neomake and add this to your .vimrc

    let g:neomake_cs_mcs_maker = {
      \ 'args': ['@.mcs'],
      \ 'errorformat': '%f(%l\,%c): %trror %m',
      \ }

Usage
-----

See Assets/Examples

License
-------

MIT, see ./LICENSE for details.

TidalUnity
---

TidalUnity is the middle-ware of visual livecode tool. It bridges control patterns that was generated with [TidalCycles](https://tidalcycles.org) to visual renderer on Unity engine.


![demo.gif](media/demo.gif)

## demo

[https://vimeo.com/198314806](https://vimeo.com/198314806)

[https://vimeo.com/198320403](https://vimeo.com/198320403)

## Installation

put```Module/UnityOsc.hs```file to your $HOME```~```

then, boot TidalCycles.

excute below in your editor within TidalCycles context.

```
:load ~/UnityOsc

v1 <- unityStream
```

livecode in Tidal.

```
v1 $ thing "apple"
```

## Prepare Assets

TidalUnity attempts to load all '.prefab' file in

``Assets/Resources/Things``

when it boot.

Please make ``Assets/Resources/Things`` directory and put your game-objects. 

Tidal's parameter ```thing```will match prefab file's name(whithout extension).

## Parameters

```
x 
```

- value : Float : x position in 3D space. (default 0)

```
y
```

- value : Float : y position in 3D space. (default 0)

```
z
```

- value : Float : z position in 3D space. (default 0)


```
duration
```

- value : Float : object's life time in second. 1 is 1 second. (default 0.5)


```
rigid
```

- value : Int : if 0 < rigid, object will be calicurated phisics.


```
randCam
```

- speed : Float : speed of random camera transition.


```
vortex
```

- x : Float : x scale of vortex distortion. (default 0.5)
- y : Float : y scale of vortex distortion. (default 0.5)
- angle : Float : angle of vortex distortion. (default 0)


TidalUnity
---

##Installation

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

##Prepare Assets

TidalUnity attempts to load all '.prefab' file in

``Assets/Resources/Things``

when it boot.

Please make ``Assets/Resources/Things`` directory and put your game-objects. 

Tidal's parameter ```thing```will match prefab file's name(whithout extension).

##Parameters

```
x 
```

- Float : x position in 3D space. (default 0)

```
y
```

- Float : y position in 3D space. (default 0)

```
z
```

- Float : z position in 3D space. (default 0)


```
duration
```

- Float : object's life time in cycle. 1 is 1cycle. (default 0.5)



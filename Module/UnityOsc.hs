{-# LANGUAGE TypeSynonymInstances, FlexibleInstances, OverloadedStrings #-}

-- custom osc send

module UnityOsc where
import Sound.Tidal.Stream
import Sound.Tidal.Pattern
import Sound.Tidal.ParseBP
import Sound.Tidal.Stream
import Sound.Tidal.Context

tidalUnityTarget :: OSCTarget
tidalUnityTarget = OSCTarget {oName = "tidal-unity", 
                          oAddress = "127.0.0.1",
                          oPort = 5000,
                          oPath = "/unity_osc",
                          oShape = Just [
                            ("thing", Just $ VS ""),
                            ("x", Just $ VF 0),
                            ("y", Just $ VF 0),
                            ("z", Just $ VF 0),
                            ("moveX", Just $ VF 0),
                            ("moveY", Just $ VF 0),
                            ("moveZ", Just $ VF 0),
                            ("vscale", Just $ VF 1.0),
                            ("duration", Just $ VF 0.5),
                            ("twist", Just $ VF 0),
                            ("rigid", Just $ VI 0),
                            ("randCam", Just $ VF 0),
                            ("vortexRadX", Just $ VF 0.5),
                            ("vortexRadY", Just $ VF 0.5),
                            ("vortexAngle", Just $ VF 0),
                            ("ripple", Just $ VF 0),
                            ("dlight", Just $ VS "r")
                          ],
                          oLatency = 0.1,
                          oPreamble = [],
                          oTimestamp = MessageStamp
                          }

thing = pS "thing"
x = pF "x"
y = pF "y"
z = pF "z"
moveX = pF "moveX"
moveY = pF "moveY"
moveZ = pF "moveZ"
vscale = pF "vscale"
duration = pF "duration"
twist = pF "twist"
rigid = pI "rigid"
randCam = pF "randCam"
vortexRadX = pF "vortexRadX"
vortexRadY = pF "vortexRadY"
vortexAngle = pF "vortexAngle"
ripple = pF "ripple"
dlight = pS "dlight"

vortex x y angle = vortexRadX x |+| vortexRadY y |+| vortexAngle angle
move x y z = moveX x |+| moveY y |+| moveZ z

-- skybox = makeS unityShape "skybox"


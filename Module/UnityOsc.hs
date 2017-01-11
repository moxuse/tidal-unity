-- custom osc send

module UnityOsc where
import Sound.Tidal.Stream
import Sound.Tidal.Pattern
import Sound.Tidal.Parse
import Sound.Tidal.OscStream
import Sound.Tidal.Context

port = 5000

unityShape = Shape {

  params = [
    S "thing" Nothing,
    F "x" (Just 0),
    F "y" (Just 0),
    F "z" (Just 0),
    F "duration" (Just 0.5)
  ],
  cpsStamp = True,
  latency = 0.02

}

unitySlang = OscSlang {
  path = "/unity_osc",
  timestamp = NoStamp,
  namedParams = False,
  preamble = []
}

unityStream = do
  s <- makeConnection "127.0.0.1" port unitySlang
  stream (Backend s $ (\_ _ _ -> return ())) unityShape

thing = makeS unityShape "thing"
x = makeF unityShape "x"
y = makeF unityShape "y"
z = makeF unityShape "z"
duration = makeF unityShape "duration"


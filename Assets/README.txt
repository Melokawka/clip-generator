Uruchomienie:
1. Open the scene: GeneratorScene
2. Play/run the scene
3. Select PointRandomizer in gameobject hierarchy and check checkbox 'Randomize'
4. Select BtnShotsPlanner in gameobject hierarchy and check checkbox 'PlanArc'

Default settings:
- batch size 5
- environments: 2 - high quality forest environment (URP required) and a simple village environment
- number of shots - 15 (environment weights {2,1} - gives {10,5} clips in each environment)
- timescale - 1
- fps - 5 (each camera in a batch takes 5 screenshots per second. With batch size 5 and fps 5 there are 25 screenshots taken per second)
- duration - 5 (seconds - per batch - with three batches it takes 15 seconds for recording to complete)

Options:
- select TimeScale object in gameobject hierarchy - timescale can be modified (even during runtime)
- select ShotsPlanner object in the hierarchy to modify shot parameters and the number of shots
- select ShotsPlanner object in the hierarchy to change fps (how many screenshots a camera takes per second) and duration (seconds per 1 batch)
- select MainRecorder object in the hierarchy to modify batch size (number of simultaneously recorded clips / cameras recording at a time)
- select GlobalSettings object in the hierarchy to switch out the environments
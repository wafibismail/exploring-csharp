Focus: coroutines, i.e. methods that can pause in its execution to allow the processor to handle other methods
- allowing developers to:
  - have control over repeating tasks in code, or
  - handle very large tasks e.g. loading large files, parsing a large amount of data
- initiated with a call to StartCoroutine(),
  - callable only within a class that extends MonoBehaviour
- a coroutine executes until it encounters a yield statement
  - leading to pause in its execution for a certain amount of time
    - during which other code are allowed to be executed.
    - after which the coroutine continues on the line after the yield statemens
- more evidently useful for devices with slower processors

yield
- return null
  - means yield execution until next frame
- return new waitForSeconds(1) 
  - means wait for at least 1 second before continuing coroutine execution
    - note that this is reasonably accurate but not exact

Unity Profiler
- is one of the most powerful tools for optimizing game perfomance
- maintains stats on the amount of time spent on each C# function for EVERY FRAME
- add via top right of current Scene pane
  - Add Tab > Profiler
  - Pause first, then Play
  - types of processes can be hidden by clicking the color boxes
  - drag the mouse along the graph in CPU Usage to see how much processing time is taken by a function or background process
    - can also use search field
- more useful info in Unity documentation
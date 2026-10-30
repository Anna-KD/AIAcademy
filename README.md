# AI Academy (Crazy Universe Vol.3)

## Description

AI Academy is a humorous WPF application where robot teachers perform different actions and consume battery power.
Vol.2 extended Vol.1 with generic repositories, events, LINQ and the MVVM pattern.
Vol.3 adds CPU-heavy calculations that run in the background (sequential and parallel), with cancellation and error handling.

The project demonstrates:

- Inheritance, interfaces and polymorphism
- Generics (`IRepository<T>`, `Repository<T>` with `where T : RobotTeacher`)
- Events and reactions between objects
- LINQ (Where, OrderBy, GroupBy)
- MVVM (Model, ViewModel, View with bindings and commands)
- async/await, `Task.Run`, `Task.WhenAll`, `CancellationToken`, `Stopwatch` (Vol.3)

## World

All robot teachers inherit from the abstract class `RobotTeacher`.

| Type | Repository | Added in |
|---|---|---|
| MathTeacherBot | `Repository<MathTeacherBot>` | Vol.1 |
| LanguageTeacherBot | `Repository<LanguageTeacherBot>` | Vol.1 |
| ScienceTeacherBot | `Repository<ScienceTeacherBot>` | Vol.1 |
| HistoryTeacherBot | `Repository<HistoryTeacherBot>` | Vol.1 (partner) |
| ArtTeacherBot | `Repository<ArtTeacherBot>` | Vol.2 (partner) |
| MusicTeacherBot | `Repository<MusicTeacherBot>` | Vol.2 (partner) |
| SportTeacherBot | `Repository<SportTeacherBot>` | Vol.2 (partner) |

Each robot type has its own repository.

## Interfaces

- `IQuizCreator`: the robot can create a quiz.
- `IRechargeable`: the robot can recharge its battery.
- `IHeavyWorker` (Vol.3): the robot can run a CPU-heavy calculation on a snapshot of its data.

All seven types implement `IQuizCreator` and `IRechargeable`. A robot that does not implement an interface would have the matching button disabled (`CanExecute`).
`IHeavyWorker` is implemented by `MathTeacherBot`, `LanguageTeacherBot` and `ScienceTeacherBot`.

## Object State

Each robot has a name and a battery level (0-100). The battery level changes when actions are performed.

| Action | Battery |
|---|---|
| Teach | -10 |
| Create Quiz | -5 |
| Crazy Action | -20 |
| Reaction to a neighbour's Crazy Action | -5 (all types) |
| Recharge | restores to 100% |

Robots cannot perform actions when the battery level is insufficient.
Heavy calculations (Vol.3) do not change the battery level.

## Vol.3: CPU-heavy processing

### Calculations

Each calculation is a simple loop whose length is set by **Workload**.

| Robot type | What it calculates | Result for Workload = 1 000 000 000 |
|---|---|---|
| MathTeacherBot | Sum of all numbers up to Workload that are divisible by 3 or 5 | 233 333 334 166 666 656 |
| LanguageTeacherBot | Total number of letters in Workload words (word length 3-10) | 6 500 000 000 |
| ScienceTeacherBot | Number of perfect squares (1, 4, 9, 16...) up to Workload | 31 622 |

### Snapshot

When Sequential or Parallel is pressed, the ViewModel creates a `RobotSnapshot` (name, type, battery level) for every selected robot on the UI thread.
The background calculation works only with this copy and never touches the live robot object.

### Two modes

- **Sequential:** the robots are processed one by one. The ViewModel awaits each robot before the next one starts.
- **Parallel:** all calculations are started with `Task.Run` (inside `HeavyWorkRunner`) and awaited together with `Task.WhenAll`.

Both modes use the same input (selected robots, snapshots and Workload). The total time of each run is measured with `Stopwatch`.
After both modes were run with the same input, the log shows a `COMPARE` line with both times and whether the results are identical.
The UI stays responsive while the calculation is running.

### Cancellation

The **Cancel** button cancels one shared `CancellationTokenSource`. Every loop iteration calls `token.ThrowIfCancellationRequested()`.
Robots that are still working get the status `Cancelled`; robots that already finished keep `Success`.

### Error handling

Every robot ends with one of three statuses: `Success`, `Error` or `Cancelled`.
An error of one robot is written to the report and the other robots continue.
Error demo: `LanguageTeacherBot` throws `Language database error: battery too low` when the battery level in its snapshot is below 30.

### Report

Each log line contains: robot name, status, result or error message, duration.
At the end of a run the total time is written (`SEQUENTIAL TOTAL` or `PARALLEL TOTAL`).

Example (Workload = 1 000 000 000):

- Sequential: Albert 6309 ms, Luna 5240 ms, Nova 8072 ms, total 19636 ms
- Parallel: total 11195 ms, `same results: True`

### New classes (Vol.3)

| Class | Project | Purpose |
|---|---|---|
| `IHeavyWorker` | Core | Interface: `DoHeavyWorkAsync(snapshot, workload, token)` |
| `WorkResult` | Core | Result of one robot: name, status, result, error message, duration |
| `RobotSnapshot` | Core | Copy of robot data taken when the calculation starts |
| `HeavyWorkRunner` | Core | Runs a calculation with `Task.Run`, `Stopwatch` and converts success / error / cancellation to `WorkResult` |
| `RobotSelection` | WPF | Wrapper for the checkbox list (robot + `IsSelected`) |

## How to run

1. Open `AIAcademy.sln` in Visual Studio.
2. Set `AIAcademy.WPF` as the startup project and press F5.
3. In the **Vol.3 Processing** block choose robots (checkboxes) and enter **Workload** (for example `1000000000`).
4. Press **Sequential** or **Parallel**. The result appears in the Activity Log.
5. Run both modes with the same robots and Workload to see the `COMPARE` line.
6. **Error demo:** select Luna, press Crazy Action 4 times (battery drops below 30), then start a calculation. Luna shows `Error`, the other robots finish with `Success`. Press Recharge to restore Luna.
7. **Cancel demo:** start a calculation and press **Cancel** in the first seconds. Robots that are still working show `Cancelled`.

Note: do not use Teach, Crazy Action or Recharge between the two runs if you want the `COMPARE` line, because they change the battery level and therefore the input.

## World rules from Vol.2

### Events and reactions (Core)

All of this logic lives in `Repository<T>` and the models, not in the WPF code-behind.

- **Add:** existing robots of the same repository greet the new robot. The new robot does not greet itself.
- **Remove:** the remaining robots say goodbye. The removed robot is unsubscribed and no longer reacts.
- **CrazyAction:** `RobotTeacher.CrazyActionPerformed` is raised with `RobotEventArgs` (the robot that caused it). Other robots in the same repository react and lose battery. The actor does not react to itself.
- Robots in other repositories never react.

### LINQ

- **Where:** search robots by name.
- **OrderBy:** sort robots by name (A-Z).
- **GroupBy:** summary of robots per type across all repositories.

### MVVM

- **Model (Core):** robots, repositories, events, reaction rules, heavy calculations.
- **ViewModel:** `MainViewModel` (selected repository and robot, lists, log, summary, Vol.3 run logic), `RelayCommand`.
- **View:** `MainWindow.xaml` uses bindings and commands only. The code-behind contains no business logic.

### Validation Rules

- Robot names cannot be empty.
- Robot names cannot consist only of digits.
- Add, Delete, Teach and Crazy Action are disabled when there is no valid input or selection.
- Sequential and Parallel are disabled while a calculation is running; Cancel is enabled only while it is running.
- Workload must be greater than 0, and at least one robot must be selected.

### Crazy Actions

- **MathTeacherBot:** solves 100 equations in one second and becomes confused by 2 + 2.
- **LanguageTeacherBot:** translates one word into 20 languages and forgets the original.
- **ScienceTeacherBot:** creates glowing tea during an experiment.
- **HistoryTeacherBot:** confidently places a famous historical event in the wrong century.
- **ArtTeacherBot:** paints the entire classroom blue, including the desks and the door.
- **MusicTeacherBot:** plays a dramatic symphony on a broken piano.
- **SportTeacherBot:** organises a full marathon through the academy corridors.

## WPF Elements Used

- `ProgressBar`: shows the battery level of the selected robot.
- `ComboBox`: selects the repository.
- `ListBox`: shows robots of the selected repository.
- `CheckBox` list (`ItemsControl`): selects robots for the Vol.3 calculation.

## Main Features

Users can select a repository and a robot, add and delete robots, teach, create quizzes, recharge, use Crazy Action, search, sort, view the summary and read the activity log.
In Vol.3 users can also choose robots and workload, run heavy calculations sequentially or in parallel, cancel them and read the report.

## Tested Use Cases

1. Teaching decreases battery by 10.
2. Quiz creation decreases battery by 5.
3. Crazy Action decreases battery by 20.
4. Recharge restores battery to 100%.
5. Adding a robot: only robots of the same repository greet it.
6. Crazy Action: only robots of the same repository react, and their battery decreases.
7. Removing a robot: the others say goodbye, and the removed robot no longer reacts.
8. Other repositories keep their state and log unchanged.
9. Search, sort A-Z and the summary work.
10. Invalid names are rejected.
11. The three partner types (Art, Music, Sport) work with add, remove and Crazy Action in the same way as the original types.
12. Sequential mode: robots are processed one by one, total time is about the sum of the robot times.
13. Parallel mode: total time is about the time of the slowest robot; results are identical to Sequential (`same results: True`).
14. Error: Luna with battery below 30 gets `Error`, the other robots finish with `Success`.
15. Cancel: robots that are still working get `Cancelled`, buttons become active again.
16. The UI stays responsive during the calculation.
17. A robot added or deleted in the UI appears in or disappears from the Vol.3 selection list

## Team work (Vol.2)

A second student (arinashishkova) added three new robot types in a separate branch
(`feature/arina-new-teacher-bots`) and opened Pull Request #4 into `Vol.2`.
The pull request was reviewed, approved and merged by the main author.

| Type | TypeName | Crazy Action | Reaction to a neighbour's Crazy Action |
|---|---|---|---|
| ArtTeacherBot | Art Teacher | -20 battery, paints the classroom blue | -5 battery, adds colorful details |
| MusicTeacherBot | Music Teacher | -20 battery, plays a symphony on a broken piano | -5 battery, joins with a drum solo |
| SportTeacherBot | Sport Teacher | -20 battery, organises a marathon in the corridors | -5 battery, warms up to join |

Each new type:

- inherits `RobotTeacher` and implements `IQuizCreator` and `IRechargeable`;
- has its own `Teach()` with a battery check (-10);
- raises the `CrazyActionPerformed` event, so only robots of the same repository react;
- has its own `Repository<T>` registered in `MainViewModel`;
- has one starting robot.

## AI Usage

AI was used to help with project planning, code examples, problem solving, debugging, user interface improvements and documentation. All generated code was reviewed, tested and adapted for the final project by the student.
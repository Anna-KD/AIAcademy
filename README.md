# AI Academy (Crazy Universe Vol.2)

## Description

AI Academy is a humorous WPF application where robot teachers perform different actions and consume battery power.
Vol.2 extends Vol.1 with generic repositories, events, LINQ and the MVVM pattern.

The project demonstrates:

- Inheritance, interfaces and polymorphism
- Generics (`IRepository<T>`, `Repository<T>` with `where T : RobotTeacher`)
- Events and reactions between objects
- LINQ (Where, OrderBy, GroupBy)
- MVVM (Model, ViewModel, View with bindings and commands)

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

All seven types implement both interfaces. A robot that does not implement an interface would have the matching button disabled (`CanExecute`).

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

## Events and reactions (Core)

All of this logic lives in `Repository<T>` and the models, not in the WPF code-behind.

- **Add:** existing robots of the same repository greet the new robot. The new robot does not greet itself.
- **Remove:** the remaining robots say goodbye. The removed robot is unsubscribed and no longer reacts.
- **CrazyAction:** `RobotTeacher.CrazyActionPerformed` is raised with `RobotEventArgs` (the robot that caused it). Other robots in the same repository react and lose battery. The actor does not react to itself.
- Robots in other repositories never react.

## LINQ

- **Where:** search robots by name.
- **OrderBy:** sort robots by name (A-Z).
- **GroupBy:** summary of robots per type across all repositories.

## MVVM

- **Model (Core):** robots, repositories, events, reaction rules.
- **ViewModel:** `MainViewModel` (selected repository and robot, lists, log, summary), `RelayCommand`.
- **View:** `MainWindow.xaml` uses bindings and commands only. The code-behind contains no business logic.

## Validation Rules

- Robot names cannot be empty.
- Robot names cannot consist only of digits.
- Add, Delete, Teach and Crazy Action are disabled when there is no valid input or selection.

## Crazy Actions

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

## Main Features

Users can select a repository and a robot, add and delete robots, teach, create quizzes, recharge, use Crazy Action, search, sort, view the summary and read the activity log.

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
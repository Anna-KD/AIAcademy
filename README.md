# AI Academy

## Description

AI Academy is a humorous WPF application about robot teachers.

The application demonstrates:

- Inheritance
- Interfaces
- Polymorphism
- Validation
- ObservableCollection
- WPF UI elements

## Robot Types

The application contains three robot teachers:

- MathTeacherBot
- LanguageTeacherBot
- ScienceTeacherBot

All robots inherit from the abstract class `RobotTeacher`.

## Interfaces

- `IQuizCreator` – allows robots to create quizzes.
- `IRechargeable` – allows robots to recharge their battery.

## Features

Users can:

- View robots
- Select robots
- Add new robots
- Delete robots
- Teach lessons
- Create quizzes
- Recharge batteries
- Use CrazyAction()
- View activity log

## Crazy Actions

### MathTeacherBot
Solves 100 equations in one second and gets confused by 2 + 2.

### LanguageTeacherBot
Translates one word into 20 languages and forgets the original word.

### ScienceTeacherBot
Creates glowing tea during an experiment.

The ScienceTeacherBot CrazyAction decreases battery level and changes the robot's state.

## State and Validation

Each robot has a battery level from 0 to 100.

Validation rules:

- Robot names cannot contain only numbers.
- Battery level cannot become negative.
- Robots cannot perform actions if the battery level is insufficient.
- Invalid actions do not change the object's valid state.

## WPF Elements Used

### ProgressBar
Used to visually display the current battery level of the selected robot.

### TabControl
Used to separate robot actions from robot management, making the interface more organized and user-friendly.

## Tested Use Cases

1. Teaching decreases battery level.
2. Quiz creation decreases battery level.
3. Recharge restores battery level to 100%.
4. Adding a robot updates the robot list.
5. Deleting a robot removes it from the list.
6. ScienceTeacherBot CrazyAction decreases battery level.
7. Robots cannot perform actions when the battery is too low.

## How to Run

1. Open the solution in Visual Studio.
2. Build the solution.
3. Run the WPF project.
4. Use the interface to manage robot teachers and perform actions.

## AI Usage

AI was used to help with project planning, code examples, problem solving, and documentation. All generated code was reviewed, tested, and adapted for the final project by the student.
# AI Academy

## Description

AI Academy is a humorous WPF application where robot teachers perform different actions and consume battery power.

The project demonstrates:

- Inheritance
- Interfaces
- Polymorphism
- Validation
- ObservableCollection
- WPF user interface development

## World

The application contains three robot teachers:

- MathTeacherBot
- LanguageTeacherBot
- ScienceTeacherBot

All robot teachers inherit from the abstract class `RobotTeacher`.

## Interfaces

### IQuizCreator

Allows robots to create quizzes.

### IRechargeable

Allows robots to recharge their batteries.

All robot types implement both interfaces.

## Object State

Each robot has:

- Name
- BatteryLevel

Battery level changes when actions are performed.

### Battery Costs

- Teach: -10
- Create Quiz: -5
- Crazy Action: -20
- Recharge: restores battery to 100%

Robots cannot perform actions when the battery level is insufficient.

## Validation Rules

- Robot names cannot be empty.
- MathTeacherBot and ScienceTeacherBot names cannot consist only of numbers.
- LanguageTeacherBot names may contain letters only.
- Invalid input does not change application state.

## Crazy Actions

### MathTeacherBot

Solves 100 equations in one second and becomes confused by 2 + 2.

### LanguageTeacherBot

Translates one word into 20 languages and forgets the original word.

### ScienceTeacherBot

Creates glowing tea during an experiment.

CrazyAction consumes battery power and depends on the robot's current battery level.

## WPF Elements Used

### ProgressBar

ProgressBar is used to display the battery level of the selected robot.

The control provides a clear visual representation of the robot's current state.

## Main Features

Users can:

- View all robots
- Select robots
- Add new robots
- Delete robots
- Teach lessons
- Create quizzes
- Recharge batteries
- Use CrazyAction
- View activity logs

## Tested Use Cases

1. Teaching decreases battery level by 10.
2. Quiz creation decreases battery level by 5.
3. CrazyAction decreases battery level by 20.
4. Recharge restores battery level to 100%.
5. New robots can be added successfully.
6. Invalid robot names are rejected.
7. Robots can be removed from the academy.
8. Robots cannot perform actions with insufficient battery.

## AI Usage

AI was used to help with project planning, code examples, problem solving, debugging, user interface improvements, and documentation. All generated code was reviewed, tested, and adapted for the final project by the student.
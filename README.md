# SharpETL

SharpETL is a simple and flexible .NET library designed to aid in process of extract, transform and load of an unstructured data.
It supports simple Python scripts to help with data extraction and manupulation.

### Example

The very basic and straightforward configuration with 3 actions.

1. Configure data source
```c#
ISource source1 = sourceFactory.CreateXlsSource(@"..\..\RandomData.xlsx", true);
```

2. Configure processing script
```c#
string scriptText = @"
def OnElement(action, element):
  print '---OnElement'
  yield element          # just push recieved element forward
def OnCompleted(action):
  print '===OnCompleted'
  yield
  return
";
IScript script1 = scriptFactory.CreatePythonScript("pythonScript1", scriptText);
```

3. Configure generator action
```c#
IAction action1 = actionFactory.CreateSourceAction("sourceAction1", source1);
```

4. Configure transform action
```c#
ISource source2 = sourceFactory.CreateNullSource("nullSource1");
IAction action2 = actionFactory.CreateScriptAction("scriptAction1", source2, script1);
```

5. Configure output action
```c#
IAction action3 = actionFactory.CreateBinarySerializeAction("binaryAction1", @"Data.bin");
```

6. Create processing engine
```c#
IEngine engine = engineFactory.Create(c => {
    c.UseDefaultServiceResolver();
    c.UseDefaultContextProvider();
    c.UseReactivePlanner();
    c.AddLink(action1, action2);
    c.AddLink(action2, action3);
});
```

7. Run it!
```c#
engine.Run();
```

### Project Roadmap

- **Windows service**.

    It would be very useful to have a windows service application that could run a number of preconfigured `IEngine`s.

- **Visual configuration tool**.

    UI is needed for creating and setting up configurations.  
    It would greatly simplify process of setting up `IEngine` for non-programmers.

- Documentation.

    The project really lacks documentation and comments throughout the code.  
    Some demo projects would be nice too.

- Tests.

    Project could use more tests.  
    First of all tests of `IAction` derived classes.

- XML configuration.

    Add `IConfigurationData<T>` for most of actions.

- Fluent configuration.

    Add simplified *fluent* styled configuration.  
    Should look like `c.Fluently.FromXls().Debug().Join(...).ToBinary()`.

- Messaging.

    Add support for sending and recieving `IElement` objects as messages.  
    Potential candidates include [ZeroMQ](http://zeromq.org/), [MassTransit](http://masstransit-project.com/).

- SQLite support.

    Due to missing freely available OleDB driver for SQLite support for accessing SQLite databases needs to be added manually.

- Internal logging.

    Better internal logging needed to help debug misbehaving configurations.

- Multithreading.

    As of now `IEngine` actions run in sequental single threaded mode because of debugging issues.  
    Actions should be switched to multithreaded mode using `.ObserveOn(Scheduler.NewThread)` and tested under load.

- Planners.

    `EnumerablePlanner` needs attention. Right now it is in unusable state.  
    Implement and experiment with planner based on [Disruptor-net](https://github.com/odeheurles/Disruptor-net) technology.  
    Together with ZeroMQ messaging could result in extremely fast data processing.

- Code cleanup.

    All `DataElement` specific actions (SharpETL.Actions.Db) should be separated from SharpETL.Actions into a new project (SharpETL.DbActions).

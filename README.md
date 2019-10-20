# StringEnum

StringEnum is a base class for creating string-valued enums in .NET. It is just one file that you can copy & paste into your projects, or install via NuGet package named **StringEnum**.

## Usage:

``` csharp
///<completionlist cref="HexColor"/> 
class HexColor : StringEnum<HexColor>
{
    public static readonly HexColor Blue = New("#FF0000");
    public static readonly HexColor Green = New("#00FF00");
    public static readonly HexColor Red = New("#000FF");
}
```

## Features

- Implicit conversion from `StringEnum` **to** `string`

``` csharp
string myString1 = HexColor.Red; // implicit cast to string => "#FF0000"
string myString2 = HexColor.Red.ToString(); // also returns => "#FF0000"

string myCss = $"{background-color: " + HexColor.Red + ";}";
```

- Conversion *from* `string` to the `StringEnum` class with `Parse` and `TryParse` static methods.  It does not use reflection. O(N) time. Always returns preexistent instances. Only matches with defined members of the user StringEnum.  

``` csharp
HexColor MyParam = HexColor.TryParse("#FF0000"); // returns HexColor.Red
HexColor MyParam = HexColor.TryParse("#ff0000", caseSensitive: false); // => HexColor.Red ;
HexColor MyParam = HexColor.TryParse("invalid"); // => null;
HexColor MyParam = HexColor.Parse("invalid"); // throws InvalidOperationException;
object.ReferenceEquals(HexColor.Parse("#FF0000"), HexColor.Red) // => true
```

- Comparison by underlying string value:

``` csharp
Console.WriteLine(HexColor.Red == "#FF0000") // => true (case sensitive)
Console.WriteLine(HexColor.Red == HexColor.Parse("#ff0000", caseSensitive: false)) // => true (case insensitive)
```

- Intellisense will suggest the enum name if the class is annotated with the `<completitionlist>` tag:
`///<completionlist cref="HexColor"/>`

### Instalation

Either:

- Install **StringEnum** NuGet package, it's based on `.Net Standard 1.0` so it runs on `.Net Core` >= 1.0, `.Net Framework` >= 4.5, `Mono` >= 4.6, etc.
- Or copy [StringEnum.cs](StringEnum\StringEnum.cs) base class to your project. 
- For `Newtonsoft.Json` serialization support, copy this extended version instead. [StringEnum.cs](StringEnum.Sample.NewtonsoftSerialization\StringEnum.cs).

### Extensibility tips

- **`Newtonsoft.Json`** serialization support explained [here](StringEnum.Sample.NewtonsoftSerialization\README.md) 

- You may add **implicit casting from string to your StringEnum**, by binding the implicit cast operator to the Parse method in the target class.

``` csharp
class Color : StringEnum<Color>
{
    public static readonly Color Blue = New("Blue");
    ..other members...
    // Add the following line
    public static implicit operator Color(string stringValue) 
        => Parse(stringValue, caseSensitive: false); 
}

// usage:
Color x = "Blue"; // same as Color.Parse("Blue", caseSensitive: false)
// or given:
void PaintWall(Color wallColor)
// you can call
PaintWall("Blue");
```

- To allow any string to be a casted as the enum, expose the New() method:

``` csharp
public new static TestEnum New(string value) => StringEnum<MyEnum>.New(value);
```

Or implement a special implicit cast:

``` csharp
public static implicit operator MyEnum(string stringValue) => TryParse(stringValue, true) ?? New(stringValue);
```

But do not use this implicit cast, that calls the New() method, with user-entered strings, as they will be stored in memory in the internal ValueList used for Parse and TryParse .

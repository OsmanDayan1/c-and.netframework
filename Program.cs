using System;

// Main program class that serves as the entry point
class Program
{
    static void Main(string[] args)
    {
        // Create shipping quote builder and director
        var builder = new ShippingQuoteBuilder();
        var director = new ShippingQuoteDirector(builder);
        
        // Build and process the shipping quote
        director.ConstructShippingQuote();
    }
}

// Class representing a shipping quote
class ShippingQuote
{
    public double Weight { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Length { get; set; }
    public double? Cost { get; set; }
    public bool IsValid { get; set; }
    public string Error { get; set; }
}

// Builder interface for shipping quote
interface IShippingQuoteBuilder
{
    void Reset();
    void BuildWelcomeMessage();
    bool BuildWeight();
    bool BuildDimensions();
    void BuildQuote();
    ShippingQuote GetResult();
}

// Concrete builder implementing the shipping quote construction
class ShippingQuoteBuilder : IShippingQuoteBuilder
{
    private ShippingQuote _quote;
    private const double MaxWeight = 50;
    private const double MaxDimensions = 50;

    public ShippingQuoteBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _quote = new ShippingQuote { IsValid = true };
    }

    public void BuildWelcomeMessage()
    {
        Console.WriteLine("Welcome to Package Express. Please follow the instructions below.");
    }

    public bool BuildWeight()
    {
        while (true)
        {
            Console.WriteLine("Please enter the package weight:");
            if (!double.TryParse(Console.ReadLine(), out double weight))
            {
                Console.WriteLine("Invalid input. Please enter a numeric value.");
                continue;
            }

            _quote.Weight = weight;

            if (weight > MaxWeight)
            {
                _quote.IsValid = false;
                _quote.Error = "Package too heavy to be shipped via Package Express. Have a good day.";
                return false;
            }

            return true;
        }
    }

    public bool BuildDimensions()
    {
        // Get width
        while (true)
        {
            Console.WriteLine("Please enter the package width:");
            if (double.TryParse(Console.ReadLine(), out double width))
            {
                _quote.Width = width;
                break;
            }
            Console.WriteLine("Invalid input. Please enter a numeric value.");
        }

        // Get height
        while (true)
        {
            Console.WriteLine("Please enter the package height:");
            if (double.TryParse(Console.ReadLine(), out double height))
            {
                _quote.Height = height;
                break;
            }
            Console.WriteLine("Invalid input. Please enter a numeric value.");
        }

        // Get length
        while (true)
        {
            Console.WriteLine("Please enter the package length:");
            if (double.TryParse(Console.ReadLine(), out double length))
            {
                _quote.Length = length;
                break;
            }
            Console.WriteLine("Invalid input. Please enter a numeric value.");
        }

        // Validate total dimensions
        double totalDimensions = _quote.Width + _quote.Height + _quote.Length;
        if (totalDimensions > MaxDimensions)
        {
            _quote.IsValid = false;
            _quote.Error = "Package too big to be shipped via Package Express.";
            return false;
        }

        return true;
    }

    public void BuildQuote()
    {
        _quote.Cost = (_quote.Width * _quote.Height * _quote.Length * _quote.Weight) / 100;
    }

    public ShippingQuote GetResult()
    {
        return _quote;
    }
}

// Director class that controls the building process
class ShippingQuoteDirector
{
    private readonly IShippingQuoteBuilder _builder;

    public ShippingQuoteDirector(IShippingQuoteBuilder builder)
    {
        _builder = builder;
    }

    public void ConstructShippingQuote()
    {
        _builder.Reset();
        _builder.BuildWelcomeMessage();

        // Build and validate weight
        if (!_builder.BuildWeight())
        {
            DisplayError();
            return;
        }

        // Build and validate dimensions
        if (!_builder.BuildDimensions())
        {
            DisplayError();
            return;
        }

        // Calculate and display quote
        _builder.BuildQuote();
        DisplayQuote();
    }

    private void DisplayError()
    {
        var quote = _builder.GetResult();
        Console.WriteLine(quote.Error);
    }

    private void DisplayQuote()
    {
        var quote = _builder.GetResult();
        Console.WriteLine($"Your estimated total for shipping this package is: ${quote.Cost:F2}");
        Console.WriteLine("Thank you!");
    }
}
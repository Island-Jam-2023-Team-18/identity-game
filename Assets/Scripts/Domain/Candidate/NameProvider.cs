using System.Collections;
using System.Collections.Generic;

public class NameProvider
{
  public class NameProviderBuilder
  {
    private IRandomProvider randomProvider;

    public NameProviderBuilder()
    {
      randomProvider = RandomProvider.GetInstance();
    }

    public NameProviderBuilder SetRandomProvider(IRandomProvider randomProvider)
    {
      this.randomProvider = randomProvider;
      return this;
    }

    public NameProvider Build()
    {
      return new NameProvider(randomProvider);
    }
  }

  private readonly IRandomProvider randomProvider;
  private readonly string[] names;
  private readonly string[] surnames;

  private NameProvider(IRandomProvider randomProvider)
  {
    this.randomProvider = randomProvider;

    names = new string[] {
            "Emma", "David", "Aria", "Samuel", "Isabella", "Ethan", "Mia", "Noah", "Sophia", "Liam",
            "Ava", "Alexander", "Olivia", "Benjamin", "Charlotte", "James", "Amelia", "William", "Harper",
            "Elijah", "Emily", "Michael", "Abigail", "Lucas", "Elizabeth", "Mason", "Sofia", "Daniel",
            "Madison", "Gabriel", "Chloe", "Matthew", "Lily", "Jackson", "Ella", "Sebastian", "Grace",
            "Joseph", "Victoria", "Andrew", "Zoey", "Caleb", "Natalie", "Henry", "Scarlett", "Nicholas",
            "Hannah", "Joshua", "Addison", "Christopher", "Brooklyn", "Jonathan", "Claire", "Ryan",
            "Audrey", "Isaac", "Ava", "Dylan", "Eleanor", "Kevin", "Maya", "Thomas", "Aaliyah",
            "Connor", "Leah", "Wyatt", "Layla", "Zachary", "Samantha", "Julian", "Gabriella", "Levi",
            "Skylar", "Aaron", "Arianna", "Owen", "Addison", "Anthony", "Paisley", "Evan", "Brielle",
            "Lucas", "Violet", "Jordan", "Stella", "Brandon", "Bella", "Oliver", "Isabelle", "Jason",
            "Adalyn", "Charles", "Eva", "Cameron", "Everly", "Kyle", "Kinsley", "Eric", "Penelope",
            "Tyler"
        };

    surnames = new string[] {
            "Smith", "Garcia", "Brown", "Johnson", "Miller", "Gonzalez", "Davis", "Martinez", "Rodriguez", "Wilson",
            "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Moore", "Young", "Allen",
            "King", "Wright", "Scott", "Green", "Baker", "Adams", "Nelson", "Carter", "Mitchell", "Perez",
            "Roberts", "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Sanchez",
            "Morris", "Rogers", "Reed", "Cook", "Morgan", "Cooper", "Ramirez", "Peterson", "Bailey", "Flores",
            "Gray", "Gomez", "Price", "Bennett", "Wood", "Barnes", "Ross", "Henderson", "Coleman", "Jenkins",
            "Perry", "Powell", "Long", "Patterson", "Hughes", "Flores", "Washington", "Butler", "Simmons", "Foster",
            "Gonzales", "Bryant", "Alexander", "Russell", "Griffin", "Diaz", "Hayes", "Myers", "Ford", "Hamilton",
            "Graham", "Sullivan", "Wallace", "Woods", "Cole", "West", "Jordan", "Owens", "Reynolds", "Fisher",
            "Ellis", "Harrison", "Gibson", "Mcdonald", "Cruz", "Marshall", "Ortiz", "Medina", "Gomez", "Murray"
        };
  }

  public string GetFullName()
  {
    int nameIndex = randomProvider.GetNumber(0, 99);
    string name = names[nameIndex];

    int surnameIndex = randomProvider.GetNumber(0, 99);
    string surname = surnames[surnameIndex];

    return name + " " + surname;
  }
}

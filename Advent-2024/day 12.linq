<Query Kind="Program">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>


void Main()
{
    part1();
}

static string[] Lines;
static int Height, Width;

void part1()
{
    var lines = data1
                .Split(Environment.NewLine)
                //.Split(':')
                //.Select(x => )
                //.Select(x => new thing())
                .ToArray()
                .Dump()
                ;
    
    
}

record thing(int num1, int num2, string str1)
{

}

void part2()
{

}

// example data
const string data1 = """
AAAA
BBCD
BBCC
EEEC
""";

const string data2 = """

""";

const string data3 = """

""";

const string data4 = """

""";


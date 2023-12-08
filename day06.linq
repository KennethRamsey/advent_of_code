<Query Kind="Program" />

void Main()
{
    part1();
    part2();
}

void part1()
{
    var nums = data
                .Split(Environment.NewLine)
                .Select(x => x.Split(':')[1].Trim()
                                .Split(' ')
                                .Select(x => x.Trim())
                                .WhereNot(string.IsNullOrWhiteSpace)
                                .Select(int.Parse)
                )
                .ToArray();

    var races = nums[0].Zip(nums[1])
                   .Select(x => (time: x.First, dist: x.Second))
                   .Dump("races");
    races
    .Select(x => winCounts(x.time, x.dist))
    .Aggregate((x, y) => x * y)
    .Dump("sum");
}

long winCounts(long time, long dist)
{
    var wins = 0;
    for (long hold = 0; hold < time; hold++)
    {
        var goTime = time - hold;
        var speed = hold;
        if (speed * goTime > dist)
            wins++;
    }
    return wins;
}

void part2()
{
    var nums = data
                .Split(Environment.NewLine)
                .Select(x => x.Split(':')[1].Replace(" ", ""))
                .Select(long.Parse)
                .ToArray()
                .Dump("big race");

    winCounts(nums[0], nums[1])
    .Dump("counts");
}



const string data2 = """
Time:      7  15   30
Distance:  9  40  200
""";

const string data = """
Time:        46     68     98     66
Distance:   358   1054   1807   1080
""";
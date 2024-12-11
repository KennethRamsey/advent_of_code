<Query Kind="Program">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>


void Main()
{
    part1();
    part2();
}

void part1()
{
    data2
    .Split(Environment.NewLine)
    .Select(line =>
    {
        return line.Split(' ')
            .Select(int.Parse)
            .ToArray();
    })
    .Select(nums => new report(nums))
    .Count(x => x.Safe)
    .Dump();
}

record report
{
    public int[] Nums;
    public bool AllInc, AllDec, Safe;

    public report(int[] nums)
    {
        Nums = nums;

        AllInc = nums.Zip(nums.Skip(1))
                    .All(n => n.Second > n.First && goodDif(n));
        AllDec = nums.Zip(nums.Skip(1))
                    .All(n => n.Second < n.First && goodDif(n));

        Safe = AllInc || AllDec;
    }

    bool goodDif((int First, int Second) n)
    {
        var diff = Math.Abs(n.First - n.Second);
        return 1 <= diff && diff <= 3;
    }
}

void part2()
{
    data2
    .Split(Environment.NewLine)
    .Select(line =>
    {
        return line.Split(' ')
            .Select(int.Parse)
            .ToList();
    })
    .Select(nums => new report2(nums, true))
    .Count(x => x.Safe)
    .Dump();
}

record report2
{
    public List<int> Nums;
    public bool AllInc, AllDec, Safe;
    public int RemoveAt;

    public report2(List<int> nums, bool allowOneError)
    {
        Nums = nums;

        AllInc = nums.Zip(nums.Skip(1))
                    .All(n => n.Second > n.First && goodDif(n));
        AllDec = nums.Zip(nums.Skip(1))
                    .All(n => n.Second < n.First && goodDif(n));

        Safe = AllInc || AllDec;

        if (!Safe && allowOneError)
        {
            // brute force search.
            for (int i = 0; i < nums.Count; i++)
            {
                var next = nums.ToList();
                next.RemoveAt(i);
                if (new report2(next, false).Safe)
                {
                    Safe = true;
                    RemoveAt = i;
                    break;
                }
            }
        }
    }

    bool goodDif((int First, int Second) n)
    {
        var diff = Math.Abs(n.First - n.Second);
        return 1 <= diff && diff <= 3;
    }
}


// example data
const string data1 = """
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
""";

const string data2 = """
73 75 78 81 80
81 82 83 86 89 89
66 67 68 71 75
66 67 69 70 72 74 77 83
88 90 93 90 91 92
61 62 60 63 60
39 41 44 47 48 45 47 47
63 66 63 66 70
92 95 92 93 98
86 88 91 91 92
39 41 41 42 44 46 49 46
79 81 82 82 83 84 84
58 60 63 65 67 67 71
63 65 66 67 67 70 76
83 85 88 90 94 96 97
27 30 34 36 39 40 41 40
76 78 82 85 86 88 90 90
57 60 61 63 67 69 73
63 66 70 71 73 74 80
42 43 45 51 53 55
82 85 86 89 92 98 97
44 45 47 54 54
31 33 40 43 47
54 56 59 66 71
84 82 83 84 85 88 90
60 59 61 63 66 69 70 69
33 31 34 35 35
39 36 38 41 44 46 48 52
82 81 84 87 93
63 62 59 62 65 66
20 18 21 19 17
43 40 41 40 40
88 86 87 90 88 91 94 98
36 34 36 33 34 41
17 15 16 18 18 20 23 24
80 77 79 79 77
93 92 93 95 95 95
29 27 29 30 31 31 35
30 29 31 34 36 36 38 43
31 30 33 37 38 41 42
12 10 11 15 16 13
54 51 55 57 59 60 63 63
60 58 61 65 66 70
86 83 85 89 90 92 93 99
28 27 33 36 37 40 42
47 46 49 51 52 53 58 56
22 21 23 24 27 34 37 37
39 38 45 46 48 49 53
54 51 53 54 57 64 69
24 24 25 27 29 31 34 37
69 69 72 75 77 78 77
16 16 19 21 24 27 27
26 26 27 29 32 36
60 60 62 63 65 68 71 76
71 71 72 73 74 72 75 77
68 68 67 68 69 70 73 71
53 53 52 55 58 58
54 54 52 55 56 60
32 32 34 35 38 40 38 45
73 73 73 75 76
5 5 7 8 8 10 9
9 9 10 12 15 15 15
5 5 7 7 10 14
77 77 80 80 83 86 91
73 73 77 78 80 83
8 8 10 11 15 18 21 19
22 22 24 26 30 30
68 68 71 75 79
83 83 87 88 90 95
33 33 38 40 42
71 71 74 79 76
18 18 19 24 25 25
85 85 90 92 96
41 41 48 50 57
54 58 59 61 62
67 71 72 74 75 77 74
24 28 31 32 32
48 52 55 57 58 62
68 72 74 75 77 78 83
46 50 49 51 52 54 57 58
5 9 10 9 10 9
36 40 43 44 45 46 45 45
28 32 34 33 35 37 41
81 85 84 87 88 93
5 9 12 15 15 17
41 45 48 48 49 52 49
8 12 13 15 18 18 18
16 20 20 23 26 27 29 33
75 79 80 80 87
79 83 86 88 91 95 97 98
59 63 67 70 69
52 56 59 63 63
8 12 14 17 20 22 26 30
74 78 82 84 91
79 83 85 86 91 92 93
68 72 73 80 77
71 75 77 84 86 87 89 89
70 74 75 81 85
2 6 11 14 19
52 59 62 63 65 66 67
74 80 83 85 86 89 88
44 51 52 54 55 58 58
58 64 65 68 69 70 74
38 45 48 49 50 52 57
45 51 54 57 60 62 59 60
41 46 48 47 50 48
18 23 25 27 26 28 29 29
84 89 90 92 90 91 95
28 33 36 34 37 44
38 44 45 45 47 50
5 11 12 12 14 17 16
45 52 55 55 55
78 84 86 86 88 91 95
67 73 73 74 80
1 8 11 12 16 18
8 13 17 19 21 22 21
48 54 58 60 63 65 65
29 36 40 43 47
14 21 23 24 28 35
81 87 89 95 97 98 99
43 50 57 60 62 61
44 51 54 55 61 62 65 65
18 25 32 33 37
37 43 46 53 59
72 69 66 64 61 59 56 59
87 86 83 82 79 76 76
82 80 78 76 75 73 70 66
23 22 19 16 11
59 58 57 55 58 56 53 50
16 13 10 7 10 9 7 9
93 92 90 91 91
24 21 20 22 20 16
49 47 46 47 44 43 37
96 95 93 91 91 88
94 91 90 90 89 92
73 71 69 69 68 65 65
57 54 51 51 47
66 64 64 62 55
26 25 21 18 16 15 13 12
53 51 47 46 44 41 39 40
32 29 27 23 22 20 20
42 41 38 35 31 29 28 24
36 33 30 28 24 22 17
29 28 22 21 18
77 75 73 70 65 62 61 63
42 40 38 31 29 28 26 26
38 35 30 27 24 20
35 34 28 25 22 21 15
12 13 10 8 6
93 95 92 90 87 89
63 65 63 62 60 57 57
98 99 97 95 93 91 87
58 61 59 56 53 51 44
92 93 91 93 92 90 87 85
82 85 84 83 80 83 86
42 45 44 42 45 45
42 43 41 44 41 38 34
96 99 97 94 91 90 92 87
96 98 95 95 93 92
23 26 25 22 21 19 19 20
35 36 34 31 31 31
22 23 22 22 18
57 58 57 54 54 51 46
83 84 80 77 74 72 71
69 70 67 63 66
83 86 83 79 79
14 17 16 12 9 7 5 1
26 28 24 21 14
66 67 62 61 60 57
59 62 61 60 55 54 53 56
23 25 20 17 16 14 11 11
30 32 30 28 27 20 17 13
62 65 64 57 55 52 51 44
74 74 73 71 69
59 59 58 55 57
28 28 25 23 20 17 15 15
32 32 31 30 27 26 22
69 69 68 65 59
27 27 25 24 26 24 23
37 37 36 38 35 37
10 10 11 8 8
65 65 67 65 63 62 58
76 76 75 77 71
61 61 59 56 56 54 52 50
18 18 16 15 14 13 13 14
69 69 69 67 66 63 60 60
72 72 70 67 65 62 62 58
88 88 88 87 80
69 69 66 63 59 58
94 94 93 89 92
85 85 81 78 78
20 20 16 14 10
77 77 74 72 70 66 60
54 54 52 45 44 43 42 40
40 40 33 32 34
67 67 60 59 59
19 19 14 12 8
78 78 76 73 71 65 60
35 31 28 27 24
35 31 29 28 25 24 23 26
58 54 52 51 49 49
52 48 46 44 42 39 35
51 47 44 42 41 34
59 55 52 50 53 51
45 41 43 40 41
61 57 58 56 56
25 21 24 23 21 20 16
35 31 28 31 28 25 20
23 19 16 16 15
91 87 86 85 82 81 81 84
57 53 53 51 50 49 49
66 62 62 60 57 54 50
56 52 49 49 48 45 38
24 20 17 14 10 8
23 19 16 15 13 10 6 8
61 57 56 52 50 47 47
83 79 77 75 71 67
27 23 21 17 11
74 70 68 63 62 61 59 57
48 44 38 35 38
50 46 45 39 39
68 64 57 54 50
78 74 73 72 67 61
66 61 58 57 54
37 30 28 26 28
31 25 22 19 17 17
79 73 71 68 67 63
66 61 59 56 50
34 28 26 24 25 24 22
15 8 9 8 10
71 66 65 64 61 63 61 61
24 17 15 13 11 9 11 7
97 90 88 87 89 84
67 60 59 59 57
46 41 39 36 36 39
14 9 8 8 7 7
86 79 78 75 75 74 70
67 60 60 59 52
92 87 86 83 80 76 74 73
29 23 21 19 16 12 15
83 76 72 70 70
80 75 74 72 69 66 62 58
47 40 39 35 32 26
32 26 19 16 14 12
40 34 33 27 26 25 23 26
52 47 45 44 41 34 33 33
20 14 13 11 5 1
55 50 48 47 42 37
52 55 56 57 60 61 63 62
19 21 22 23 26 29 29
70 73 75 78 81 83 87
59 62 64 65 66 68 74
47 48 50 49 52
37 38 41 44 42 43 44 42
18 20 19 22 24 27 27
53 56 57 58 60 61 60 64
57 59 60 58 59 66
52 53 53 56 58 59
45 48 49 49 50 47
21 23 24 24 24
26 28 31 31 32 34 38
18 19 21 21 26
52 54 56 60 63 65 66 69
26 29 31 32 33 37 40 38
31 32 35 39 42 42
57 59 60 61 65 68 71 75
41 44 47 51 53 59
23 25 28 34 37 40 42
59 60 62 63 65 72 69
11 13 15 22 23 25 25
35 36 42 43 45 48 50 54
54 57 58 60 67 69 74
44 41 42 43 45
78 76 78 81 83 84 82
48 45 46 48 51 54 54
62 61 64 67 68 72
26 23 24 26 29 30 37
26 23 20 23 26 27
45 43 45 46 48 47 48 47
41 40 42 40 43 43
22 21 18 21 23 24 25 29
88 85 87 88 89 90 88 94
40 38 39 39 40 43 45
53 51 54 57 57 55
53 52 54 54 54
30 29 29 30 34
17 16 16 17 24
2 1 4 8 9 11 14
56 53 54 58 61 60
52 51 54 58 60 60
43 42 45 49 53
54 53 56 60 65
62 60 66 69 72
13 10 11 17 19 20 18
11 10 11 14 17 24 26 26
41 40 42 45 47 50 56 60
45 44 50 51 56
3 3 4 5 7 9 12 13
74 74 75 76 79 78
81 81 84 85 88 89 89
28 28 29 31 33 34 38
44 44 46 49 52 54 57 62
5 5 8 7 9
22 22 20 23 25 23
51 51 50 51 54 57 57
52 52 49 51 55
28 28 30 29 36
38 38 39 41 41 43 46
53 53 54 54 56 54
15 15 18 18 19 22 22
9 9 9 11 14 18
2 2 5 8 8 11 16
49 49 50 53 54 58 59 60
84 84 87 91 92 90
4 4 8 11 11
8 8 12 15 19
52 52 55 59 60 66
6 6 7 8 13 16
67 67 68 74 73
32 32 39 40 43 43
43 43 46 53 55 58 61 65
83 83 88 90 92 99
70 74 77 78 81 82 85
17 21 24 26 28 30 28
43 47 49 52 52
62 66 67 69 73
55 59 62 63 65 68 73
29 33 35 37 34 36 38 40
54 58 55 57 60 58
26 30 28 31 31
41 45 47 46 49 53
30 34 37 35 37 40 47
71 75 77 79 82 82 83
47 51 51 54 57 60 63 61
82 86 88 89 89 89
27 31 32 32 34 38
39 43 44 44 47 48 54
47 51 54 55 58 62 64 67
23 27 28 32 33 30
38 42 45 46 50 53 53
25 29 33 36 39 43
18 22 23 27 34
80 84 86 87 88 95 98
25 29 31 37 35
30 34 36 37 44 44
67 71 74 81 82 83 87
20 24 27 34 37 42
34 41 43 45 46 47
41 46 48 51 53 54 55 52
26 31 33 35 37 39 42 42
62 69 72 73 76 80
53 59 62 65 71
41 46 47 48 49 50 47 49
68 74 72 73 74 76 79 78
42 49 47 50 51 52 54 54
6 11 9 11 15
19 26 25 27 29 34
39 46 47 47 49
82 88 91 93 96 96 98 95
2 9 9 11 11
60 66 67 67 71
17 23 23 26 27 33
5 11 14 15 19 20 22
5 10 12 16 17 15
63 70 74 77 77
30 36 39 43 47
6 11 15 16 17 24
15 21 24 30 31
72 79 81 88 87
31 36 42 45 48 49 51 51
18 24 31 32 36
26 31 32 38 39 40 41 48
43 41 39 36 35 38
75 74 71 69 66 64 64
39 36 33 32 29 26 25 21
35 32 31 28 26 20
24 23 22 24 23 20 19 18
30 29 28 29 28 29
62 60 59 58 57 60 60
44 42 41 38 40 36
89 88 87 85 83 84 81 74
52 49 48 45 45 44
41 38 37 37 35 32 31 34
57 55 54 54 54
46 45 44 42 40 40 36
25 23 21 21 15
74 73 70 66 63 60
55 53 51 47 44 41 43
69 67 66 62 60 60
97 96 94 90 89 86 82
44 41 40 36 35 32 25
44 43 37 35 32
74 72 65 62 60 63
58 55 52 47 46 43 40 40
89 87 81 78 77 73
74 71 68 61 55
31 32 31 28 27 25
8 10 9 8 10
79 80 78 76 74 72 72
90 92 90 89 86 85 81
92 95 94 93 91 90 87 82
18 21 23 20 17
69 70 67 65 67 64 66
95 97 94 93 91 94 94
39 41 44 41 39 35
18 21 24 22 16
3 5 4 4 2 1
27 30 30 29 30
6 9 8 7 7 7
78 79 78 78 77 75 71
98 99 98 97 96 96 89
21 23 19 17 16 13 10
44 46 44 41 39 36 32 35
13 15 11 9 7 7
78 81 77 74 70
91 94 92 88 83
85 88 85 79 78
92 93 90 83 84
85 88 86 80 78 77 75 75
25 26 24 23 21 18 11 7
17 18 11 10 9 2
72 72 71 68 67
64 64 62 60 62
54 54 52 49 49
95 95 94 91 90 86
17 17 14 11 5
37 37 38 35 34
99 99 96 93 94 97
78 78 81 78 75 73 73
84 84 81 78 80 76
85 85 83 85 82 79 77 71
92 92 92 91 90 88 86 85
75 75 75 74 71 70 67 69
5 5 3 3 3
64 64 62 62 59 55
87 87 85 85 84 77
40 40 39 35 34
60 60 57 54 50 47 45 47
44 44 42 40 37 36 32 32
53 53 51 49 48 44 40
56 56 55 52 51 47 42
20 20 18 13 10 9
18 18 13 10 9 7 6 8
96 96 93 90 85 82 80 80
68 68 65 59 57 53
97 97 92 89 84
53 49 46 43 40 37 35
39 35 32 31 29 26 24 27
32 28 27 26 25 24 24
77 73 70 68 67 63
46 42 41 38 36 34 29
29 25 24 27 25 22 20 17
54 50 51 49 46 48
75 71 68 71 70 70
20 16 19 16 12
93 89 88 86 83 86 84 78
58 54 54 53 50
59 55 52 49 49 48 49
75 71 71 70 67 67
24 20 19 19 15
54 50 48 48 43
54 50 46 45 42 39 38 37
22 18 14 13 14
62 58 57 53 53
21 17 14 10 8 4
38 34 30 27 26 24 17
61 57 55 54 47 44
59 55 48 45 43 42 43
71 67 64 61 56 55 53 53
99 95 93 91 85 83 79
85 81 80 78 71 65
34 27 26 25 24
44 39 37 34 37
58 53 50 49 47 45 43 43
25 18 15 14 10
66 59 58 56 55 50
46 41 38 37 35 37 34 31
93 87 89 86 89
18 11 10 11 9 7 6 6
94 88 91 88 87 86 82
30 24 23 21 22 15
74 67 65 65 62
78 73 72 72 75
69 62 62 60 58 55 55
90 83 83 80 78 74
51 45 45 44 41 40 37 32
33 27 23 21 18 16
27 20 19 15 17
92 87 83 82 79 77 77
88 83 79 78 77 73
46 41 37 34 32 29 23
70 65 62 57 55 52 49 46
95 88 81 80 83
93 87 82 79 78 75 74 74
98 91 86 84 83 79
80 73 66 63 58
21 14 11 8 1
29 23 22 20 17 12 8
39 41 39 38 34 32 29
1 2 5 6 12 16
23 29 31 35 37 35
64 64 62 58 56 55 55
80 78 76 75 74 75
53 53 52 53 50 48 47 43
70 72 78 79 81 83
38 34 29 27 24 22 18
46 47 46 43 40 39 37
12 12 14 11 13
13 18 21 26 30
71 66 61 60 59 62
32 29 32 33 34 36 40
58 51 48 43 42 39 39
5 5 6 5 7
84 83 82 84 82 77
60 53 49 48 45 44 37
28 27 30 32 32 32
11 15 15 16 17 23
43 47 48 53 52
67 71 73 74 74 75 77 74
10 13 14 15 18 18
58 62 63 67 64
74 80 82 84 87 84 84
67 63 61 60 58 60 59 54
21 25 28 31 32 35 38 37
90 88 87 90 87 84 81 77
19 23 28 31 34
63 59 59 58 58
83 82 79 79 78 80
3 7 13 15 16 18 21 25
12 12 14 17 18 21 19
96 96 90 89 84
51 55 57 59 62 66 68 72
7 11 14 16 15 17 19 19
57 52 51 49 48 46 39 38
85 84 86 88 92 97
16 12 8 7 6 3
78 71 68 66 62 61 58 59
15 21 24 26 29 26 29 30
17 13 10 9 6 4 3
17 18 19 21 19 22 22
18 18 21 18 21 22 23 27
63 67 72 75 78 80 82 88
44 40 38 36 36 34
59 55 52 51 48 45 42 42
95 94 91 90 89 83
61 61 66 69 70 72 76
61 62 60 58 54 52 49 42
64 64 63 62 61 61 57
69 72 71 71 70 70
49 48 49 51 51
50 44 44 41 37
76 79 83 86 89 92 95 95
16 13 11 11 9 8
92 95 92 90 89 82
37 33 32 35 34 32 30
5 9 11 13 15 18 22
6 10 13 12 14 17
62 63 65 68 69 68 71 70
18 14 13 10 9 11
23 30 37 38 41 44 47 49
15 18 17 15 13 11 11 8
25 18 15 11 7
64 64 67 67 70 72 76
56 59 57 56 53 52 51 51
56 52 53 51 49 46 49
20 16 12 10 10
44 50 53 54 57 58 61 58
44 42 40 35 36
84 80 77 73 76
60 65 69 71 73 75 78 82
98 93 91 88 86 88
74 74 74 77 80 81 81
69 70 67 68 67 67
44 44 42 39 38 37 33
86 80 77 74 73 73
57 56 53 55 61
16 13 9 7 9
3 3 4 7 10 12 12
49 51 52 52 55 56 62
5 12 14 15 16 17 17
40 40 42 41 48
86 89 87 83 79
66 64 67 64 68
78 78 79 81 84 88
84 84 85 86 93 94 97 99
24 30 34 35 37 37
83 89 89 92 94 96 94
33 33 26 25 22 21
60 59 56 53 50 44 40
56 63 64 66 63 65 70
30 37 39 42 43 42 45 49
35 40 45 46 52
6 6 6 8 11 16
74 70 63 62 57
52 52 54 60 63 66 66
57 62 62 63 63
65 64 67 69 69 71 72
61 61 59 56 54 51 51
16 22 25 28 28 32
15 19 23 26 27 29 31 32
58 58 60 61 62 67
43 41 43 46 46 51
42 41 43 45 50
48 48 50 53 54 56 59
72 67 68 67 64 62 58
63 68 71 76 79 81 78
26 20 17 14 12 9 12 11
63 59 54 52 50 47 44
91 91 93 91 92 95 93
83 79 76 73 67 65 65
20 23 26 28 31 33 36
59 60 62 64 66 68 69
52 55 58 60 63 64
68 69 71 72 74 76 79 80
72 73 75 77 80 82 84 85
14 11 9 6 5
10 11 13 16 18 20 22
32 29 28 27 26 25 24 23
14 15 16 18 21
45 42 39 36 33 32 30 29
19 16 15 12 10 9
92 93 94 96 98
25 23 20 17 14
44 46 49 50 51 53 55
50 47 46 44 42 39 37
9 11 14 15 18 20 23 24
22 20 17 15 14 12 10 9
59 58 55 52 49 48 46
35 32 31 30 27
56 58 61 63 65 67 68
73 72 71 70 69 67 64 62
86 85 82 81 80 77 74 71
42 40 38 36 33 30
63 61 58 57 54 52
81 80 77 74 71 70 67
21 24 25 27 29 30
16 19 22 25 26
23 25 26 29 32 35 36
66 67 69 71 73 76 78
36 34 32 30 28 27 25
48 46 45 44 41 38 37
32 35 36 39 40
11 14 16 19 20 21 22 23
24 26 27 30 32 33 34
23 26 27 30 32 34
44 45 48 50 53 56
35 38 39 42 44
29 26 25 23 21 19
1 2 4 6 9 12
8 11 13 14 17
79 77 76 75 72 71
92 90 87 84 82 81
83 85 88 89 90 93
10 13 14 16 18 20 23 25
49 52 54 55 57 59 60 63
27 30 32 35 38 40 42 44
86 85 83 81 79 77
21 18 16 15 12 10
86 84 81 78 77
56 59 60 61 64 67
16 13 11 8 7 6 5 4
46 45 42 41 38 36 33 32
42 39 38 35 33 32
43 45 46 48 50 52 53
65 66 68 70 72 75
57 55 52 51 50 49 47
42 39 36 33 30 28
29 30 31 33 35
96 93 92 89 86 84 81 80
7 9 10 12 15
93 92 90 87 86 84 83
67 65 62 59 57 56
95 94 91 90 87 85
33 32 29 28 27 25
62 61 59 58 55 53 51
85 83 80 79 77 75
47 48 50 51 52 54
15 17 18 19 21 24 27 28
83 80 78 77 75
64 62 59 58 55 53 51
90 88 87 85 83
70 71 73 75 78 81 82
80 77 76 74 71 68 66
16 17 18 20 23
50 53 54 56 59 60 62 63
29 32 34 37 40 42
58 57 56 54 53 52
42 43 45 46 49 50 51 52
47 50 53 56 59 61 63 65
24 25 28 29 32 33 35 36
31 28 26 23 20 17 14
74 77 79 81 83
56 57 60 61 63 66 67 70
62 65 68 69 70 73
19 18 16 15 14
13 12 11 10 8 6 3
42 45 48 50 52 55 57 59
29 30 32 34 35 37 38 40
19 20 21 22 24 27 29
76 75 73 72 70 67 64
52 55 56 57 59 61 64 67
88 90 91 93 96 97 99
54 51 48 46 44 41 40
84 85 88 91 93 95 98
79 76 74 72 70
27 25 22 21 19 16
74 75 77 78 80 82 84
74 75 76 77 80 81 82 83
30 33 35 36 39 40
11 13 16 18 20 23 25
26 29 31 32 33
79 77 74 73 70 68 67
2 5 6 7 9 10
17 19 22 24 27 28
47 45 44 43 40
91 88 87 86 85
49 50 53 54 55 58 60 63
12 13 14 16 17
88 90 93 94 97
89 91 92 94 95
8 7 6 5 3
84 86 87 90 92 94
84 85 86 87 89
17 16 14 13 11
37 36 35 34 32 30 29 27
89 87 84 81 78 76 75
80 81 84 87 89 92
64 62 60 58 57
62 61 59 56 53 51
35 34 31 28 25 24 23
76 79 80 82 83 84 86 89
94 91 90 89 87 85 83
82 80 78 77 74 73 71 69
38 36 34 33 32 31 30 28
55 57 58 60 62 65 68
55 56 57 59 60 63
70 69 66 64 62 61 59
67 68 71 74 77 79 81
70 68 65 63 61
19 16 15 14 11
24 23 22 21 20 18
52 49 46 43 40
79 81 83 86 87 88 90
18 16 15 14 12 9 8
38 35 33 30 28 26 23 22
44 41 40 38 37
81 82 84 85 86 87 89 91
85 84 81 79 76 75
50 51 53 54 55 56 57 60
13 16 18 21 23 24 26
13 15 16 19 22
79 77 75 74 72 69 66 64
23 26 28 29 30
94 92 90 88 85 83 80 78
51 49 47 45 44 42 40 37
35 33 32 29 26
81 80 79 77 74 71
88 87 85 83 82
20 18 16 14 13 12 9 8
17 16 13 10 9 6 4 2
50 47 46 43 42 41 39
71 74 75 76 79 80
75 76 79 80 83 86 88
53 56 58 59 60 63 64
66 64 61 59 56 55 54
78 79 82 83 86 87 88
24 27 28 31 33 35 37 40
13 14 17 18 19 20 21 22
73 75 76 77 78 81 84
61 58 56 55 53 52
14 16 17 18 19 22 24 27
71 73 76 78 80 81 84
6 8 9 10 12 14 15 18
42 39 37 36 35 33 32
24 22 21 18 15 14
76 74 73 71 70 68 67 64
10 9 8 7 6 5 4 2
4 6 8 10 12 14 15 16
16 13 12 11 9 8 5 3
76 78 81 82 85 88 91
9 11 12 14 16 18 19 21
57 58 59 61 62 63 65 67
31 29 28 27 25 22 20
65 68 71 73 75 76 77 80
98 96 93 91 88 87 84 81
48 51 54 57 58 59
23 22 19 17 15 14
99 97 95 93 90 89 88 87
76 74 72 70 69
53 56 59 61 62 64 65
61 60 57 56 53 50 47
21 23 26 29 31
63 62 61 59 56 54 51 50
17 18 21 23 25 27 29 30
81 80 79 77 76 75 74
49 47 44 43 40
42 41 39 37 35
27 29 32 35 36 37 38 41
62 65 66 69 71 74 76 79
5 8 10 13 16
16 13 11 8 5 4 2
65 67 69 71 73
77 78 81 83 84 86
65 66 68 70 71 73
44 41 40 39 38 37 35 34
32 31 29 28 25 22 21 20
29 26 25 22 21 18
53 55 57 58 60 62
43 42 39 37 34 32
9 10 12 14 16 17 20 21
5 7 9 10 11
52 50 48 45 42 41 39 38
74 77 78 81 83 86 88
69 70 72 73 76 78 81 82
82 80 78 77 74 71
55 58 59 60 63
76 77 78 80 82
65 63 62 61 59 57 54 52
62 64 65 68 69
74 71 70 68 66 64
75 76 79 82 83
57 55 53 52 49 48 46 43
11 12 15 16 19 21 24
45 48 50 51 54
89 88 87 85 83 80 78 77
76 77 79 80 81
50 47 44 42 41
66 69 72 73 76 79 81
72 70 69 68 65 64 61 60
28 30 31 32 34 36 39
24 21 20 18 16
15 14 11 8 7
64 62 60 57 56 55 53
90 93 94 95 98
34 35 37 40 43
24 22 19 18 17 16 14 13
40 39 37 35 34 31 28
55 54 52 51 48 47
28 30 32 35 37
10 12 14 15 18 20
56 58 61 62 65
83 85 86 88 91 94 97
74 71 69 66 63
64 67 69 71 74
85 83 82 80 77 75 72 69
12 13 16 19 22
92 89 87 86 84
41 44 47 50 52 53 55 57
28 31 34 37 39 42 44
47 44 42 40 39 36
21 18 16 13 10 9 7
97 94 92 89 88 86 83
22 23 25 27 30 31 34
75 72 71 70 67 66 65 63
54 52 50 49 47 46 43
48 46 43 40 37
54 57 58 61 62 64
72 71 68 66 64 62 60
17 19 20 21 24 25 28
53 54 55 57 60 62 64 65
20 17 16 15 12 11 10 9
55 58 61 64 65
29 32 33 35 36
62 64 65 67 70
20 22 23 24 25
76 79 82 83 85 87
53 55 56 59 61 64 66 69
4 5 6 8 9
69 70 72 74 76 77 79 81
74 76 79 82 83 86
87 86 85 82 79 77 75 74
19 21 22 25 26 29 30 31
52 55 57 58 60 61 62
73 71 70 68 67
41 39 37 34 31 29 27 24
40 39 38 36 34 33
57 60 61 63 64 67 69 72
67 68 71 73 76 77 79 81
78 75 73 71 68 66
39 37 35 32 31
13 16 19 20 22
40 43 44 47 48 51
72 69 66 63 60 57
64 65 66 68 70 73 75 76
21 20 17 15 14 12
83 82 80 78 77 74
15 16 18 19 21 24 27 30
68 66 65 63 61 60 58 55
67 65 62 60 58 55 52 49
95 93 91 89 87 86 84 83
16 15 14 13 12
36 39 42 45 48 51
28 30 32 35 36 37 38
73 74 76 79 80 83
18 16 13 11 8 5
97 94 92 91 89 87 86
80 82 84 87 90 93 95 96
34 36 38 39 40 43
60 57 56 54 51 50 48 46
38 41 43 45 46 48 50 52
67 66 64 63 61
51 48 46 43 41 40 39 37
81 80 78 76 73 70 67
56 53 51 50 47
27 29 31 32 34 37 38
81 79 77 74 71
17 16 14 12 9 6 4 1
94 91 90 87 85
45 47 50 53 55 57
22 25 27 28 29 30 31 34
87 86 85 83 82 81 78 76
23 26 28 30 33
96 94 92 91 90 87
23 26 28 30 32 34 37 38
35 33 30 27 24 21 20
83 85 86 88 89 91 93
57 59 62 65 67
80 79 78 75 74 71 70
1 3 4 6 8 9 11 14
83 82 81 78 77 74 72
95 94 93 91 89 87 85 82
36 39 42 45 47 50
53 55 56 59 60 63 65
84 82 80 78 77 76
63 61 60 59 56 54
88 87 86 84 83
69 72 74 75 78 81
82 81 78 77 76 75 73
34 33 30 28 27 25 22 20
65 68 70 73 76 79 80 81
81 78 77 74 72 71
60 58 55 53 51
66 64 63 62 61 59 57 55
7 8 11 13 14 15 16
55 54 53 51 49 48 46
1 2 3 4 7 8
52 54 56 58 61 64
29 30 31 33 36 37
50 53 54 57 58 61 64 65
36 35 33 30 29 26
67 68 69 70 71
71 73 76 78 81
91 88 85 83 81
68 67 66 63 61
78 79 80 81 83 85 87 89
90 88 87 84 83 81
7 9 10 12 15 18 19
38 36 33 31 29 26 25
78 76 74 72 71 70
35 34 32 30 29 28 26 25
46 43 42 40 37 34
55 53 51 49 47 44 42
49 50 52 54 55 56 59 60
43 41 39 36 35 32 31
73 74 75 78 79 82 83 85
36 39 41 43 44 46 49
28 25 24 21 18
81 83 84 86 89 90 91 92
36 38 39 41 42 45 48 49
73 72 69 67 65 63
91 90 88 87 85 82 81 80
42 43 45 47 49 51
88 86 83 80 79 77
59 56 54 52 49 47
78 80 83 84 87 90
49 52 54 56 59 62 64 66
57 58 61 63 65 68 70 71
31 34 35 37 40 42 43
43 41 39 38 36 34 31 29
25 28 31 32 34
50 49 48 45 44 41 39 37
48 50 52 54 55 57
49 52 55 56 58
21 24 27 29 32 34
19 21 22 23 26
71 74 77 78 80 81 82
67 69 70 71 73 74 77 79
95 92 89 87 85 84 81 78
22 25 27 29 31 33 36 38
70 72 74 76 79
79 77 75 74 71
34 31 28 25 22 20 17 16
1 4 7 8 10 12 14 17
14 16 18 19 20 22 23 25
29 28 27 26 24
66 65 63 60 58 55
66 65 64 63 60 57 56
38 40 43 46 47
32 29 28 25 22 21
55 57 60 62 65 68 69
58 59 62 64 66 69 71 74
32 30 27 26 23 21 18 15
58 55 52 49 46 43
89 91 93 94 96
21 23 24 27 30
30 29 26 23 22 21 20
64 62 59 58 56 55 54
32 31 28 27 25 24 23 21
47 49 52 55 57 59 61 63
73 75 77 78 80 82 83
""";

const string data3 = """

""";

const string data4 = """

""";


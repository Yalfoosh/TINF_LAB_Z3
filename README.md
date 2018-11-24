# Information Theory - laboratory exercise, 3rd exercise

This is the C# implementation of the Shannon-Fano coder for the aforementioned class in the 5th semester of FER's undergraduate program. You are free to use any part of this as you wish as long as you notify me first. The code is as-is and I am not responsible for any eventual damage from using this code.

# Input

There are two types of input: sample and description. The entry point of the program is in EntryPoint.cs.

## Sample imput

The user enters a string that statistically represents the model of the coder input. For an example, the user inputs "Hakuna matata" as input. This will equate to:

```
<a> 5/13
<t> 2/13
< > 1/13
<H> 1/13
<k> 1/13
<m> 1/13
<n> 1/13
<u> 1/13
```

## Description input

The user can enter the description of the input in a format similar to the one above. The rules are as follows:

- Each stochastic element is delimited by a semicolon (`;`).
- Each stochastic element has exactly one char as the value and a number representing the probability of it appearing.
- Each value and probabikity is separated by a single space (` `).
- Each value is written as `<value>`. In other words, the value is contained between < and >.
- Each probability is written either as a real number (format 0.000... or 0,000...) or as a fraction (format 0/0).
  
Here is an example of valid input:

```
<a> 0.5;
<b> 0,25;
<c> 1/4
```
  
Additionally, since I've noticed that input checking is a must, I have added input normalization - if the program detects that the probability sum of the random variable created upon input description is not equal to 1, it will ask the user whether or not he wants to normalize it. If yes, the program divides every value by the probability sum, so the new probability sum is closer to 1 than the previous one, if not exactly one (you have to take into account rounding errors which are even present in decimal, a 128-bit real number. If you choose not to normalize the values, the program will display a message that the sum of probabilities is not one and will offer you to enter a new input description.

## Final words for input

Since the input reads both command line entries and whole file loaded into the stdin, it accepts ends as both EOF and "" read on a line. As such, this usually means that while files loaded into stdin are going to give you a result instantly, loading data through the command line will require you to enter an empty line and the initial data.

To clarify, this means that if you want to enter `abcde` through the command line window, you will actually have to enter

```
abcde

```


# Output

The output is as follows:

- First, the original input description is printed, sorted so the probabilities are descending.
- Then, the coder does its job and codes every stochastic element, showing the sign and its respective code.
- Finally, the median code length is printed, as well as the efficiency of the Shannon-Fano code.

An example of output for `abcde` is as follows:

```
The given description is as follows:

<a>    0,200
<b>    0,200
<c>    0,200
<d>    0,200
<e>    0,200

For the given description, the Shannon Fano code is:

----------------------------------------------------------------------
<a>    ->    00
<b>    ->    01
<c>    ->    10
<d>    ->    110
<e>    ->    111

Median code word length = 2,400
Code efficiency: 96,747%
----------------------------------------------------------------------
```

A slightly more complex output for the string
```
Peter Piper picked a a peck of pickled peppers, a peck of picked peppers Peter Piper Picked. If Peter Piper picked a peck of pickled peppers, where's the peck of pickled peppers Peter Piper picked?
```

would be this:

```
The given description is as follows:

<e>    0,178
< >    0,173
<p>    0,137
<r>    0,066
<c>    0,061
<i>    0,061
<k>    0,061
<P>    0,046
<d>    0,041
<f>    0,025
<s>    0,025
<t>    0,025
<a>    0,020
<o>    0,020
<l>    0,015
<,>    0,010
<h>    0,010
<'>    0,005
<.>    0,005
<?>    0,005
<I>    0,005
<w>    0,005

For the given description, the Shannon Fano code is:

----------------------------------------------------------------------
<e>    ->    00
< >    ->    010
<p>    ->    011
<r>    ->    1000
<c>    ->    1001
<i>    ->    1010
<k>    ->    1011
<P>    ->    11000
<d>    ->    11001
<f>    ->    11010
<s>    ->    11011
<t>    ->    11100
<a>    ->    111010
<o>    ->    111011
<l>    ->    111100
<,>    ->    111101
<h>    ->    1111100
<'>    ->    1111101
<.>    ->    11111100
<?>    ->    11111101
<I>    ->    11111110
<w>    ->    11111111

Median code word length = 3,756
Code efficiency: 98,836%
----------------------------------------------------------------------
```

# Executable

For windows folk, I always build the .exe in the bin/Release folder. It's on .NET 4.7.2, and it's C# 7.1. People with other OS' will need to build this for themselves. I recommend JetBrains Rider.

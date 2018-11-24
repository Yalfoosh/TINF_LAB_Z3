# Information Theory - laboratory exercise, 3rd exercise

This is the C# implementation of the Shannon-Fano coder for the aforementioned class in the 5th semester of FER's undergraduate program. You are free to use any part of this as you wish as long as you notify me first. The code is as-is and I am not responsible for any eventual damage from using this code.

# Input

There are two types of input: sample and description. The entry point of the program is in EntryPoint.cs.

# Sample imput

The user enters a string that statistically represents the model of the coder input. For an example, the user inputs "Hakuna matata" as input. This will equate to:

<a> 5/13
<t> 2/13
< > 1/13
<h> 1/13
<k> 1/13
<m> 1/13
<n> 1/13
<u> 1/13

# Description input

The user can enter the description of the input in a format similar to the one above. The rules are as follows:

- Each stochastic element is delimited by a semicolon (";").
- Each stochastic element has exactly one char as the value and a number representing the probability of it appearing.
- Each value is written as <value>. In other words, the value is contained between < and >
- Each probability is written either as a real number (format 0.000... or 0,000...) or as a fraction (format 0/0).

# Output

The output is as follows:

- First, the original input description is printed, sorted so the probabilities are descending.
- Then, the coder does its job and codes every stochastic element, showing the sign and its respective code.
- Finally, the median code length is printed, as well as the efficiency of the Shannon-Fano code.

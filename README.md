# Sparse Matrix

A simple C# class that implements compressed sparse row (CSR) format to store a
matrix where the majority of elements are zeros.

## Motivation

The first time I interviewed at a FAANG company, I was asked to represent a 
matrix with mostly zero elements in a way that would use less memory. 
The chosen data structure had to support matrix addition, transposition and 
multiplication.

At that time, my idea was to use a dictionary to store a tuple of row and 
column indices. However I soon realized it was very difficult to 
perform operations, as the dictionary did no preserve any lexicographical order.

I didn't make the cut, that goes without saying, but the experience motivated me 
to investigate such representations and to implement this one.
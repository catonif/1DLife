# 1DLife
A little C# console application that simulates life in one dimension from a rule.\
You can try its TypeScript port [here](https://catonif.github.io/life).

## Usage
The compiled executable can be ran from the command line with arguments. If no argument is given, they will be asked after the program has started.
The arguments, in order, are:
1. [Neighbour distance](#Neighbours-distance)
2. [Rule](#Rule)
3. [Render in 2D](#Render-in-2D), optional, default value is `y`.

## Arguments
### Rule
Like the original Conway's Game of Life, each cell future state is determined by the cell itself and its neighbours.

A cell has 2[*](#L30) neighbours. The rule is formed by 6[*](#L30) bits (`1` or `0`).

The 1st (most relevant) bit says the state that an already alive cell with 0 neighbours alive should have in the next generation. The bit after it says the state that a dead cell with 0 neighbours alive should have.\
The 3rd and the 4th bit work respectevly the same way for cells with 1 neighbours alive, and for the ones with 2 neighbours alive there are the 5th and 6th bit.\
These bits also rapresent a number in binary, so, so you can also write the number in the decimal scale with a dollar sign (`$`) before it, and the program will do the rest.

To recap, the rule `011010`, equivalent of `$26`, means that:
| number of neighbours alive | current state | future state |
| :------------------------: | :-----------: | :----------: |
|             0              |      `1`      |     `0`      |
|             0              |      `0`      |     `1`      |
|             1              |      `1`      |     `1`      |
|             1              |      `0`      |     `0`      |
|             2              |      `1`      |     `1`      |
|             2              |      `0`      |     `0`      |

---

\*: these numbers are only true if `neighbour distance` is `1`. The actual values are:
- cell's neighbours count: `(neighbour distance) * 2`
- rule's bits count: `(neighbour distance) * 4 + 2`
- rule's maximum decimal value: `1 << (bits count)` or `Math.Pow(2, (bits count))` for who isn't familiar with bitwise operations.
- the table actualle keeps going, with `bits count` rows with the bits as the `future state` column, following the alternating pattern for the `current state` column.

### Neighbours distance
In the note above I mentioned the `neighbour distance` variable. This is the maximum distance of a cell to another for them to be considered neighbours. If it's `1` a cell considers neighbours only the two other cells adiacent to itself. If it's `2` a cell considers neighbours the two other cells adiacent to itself and the two cells "at distance 2", adiacent to the initial two neghbours.

### Render in 2D
If it is `n` the program will just render one line, with the current generation, but since your screen has two dimension, you can set this to `y` to use the y axis for time.

**TL;DR** it render the new generations in a new line.

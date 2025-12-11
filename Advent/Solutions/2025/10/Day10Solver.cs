using System.Text.RegularExpressions;

namespace Advent.Solutions._2025._10;

// NOT MY CODE! Day 10 part two finally made me throw in the towel JEEEESUS
public static class Day10Solver
{
    private const double Epsilon = 1e-9;

    // -----------------------------
    // Parse Input
    // -----------------------------
    public static (int[] target, int[][] buttons, int[] joltage)[] Parse(IEnumerable<string> lines)
    {
        var list = new List<(int[], int[][], int[])>();

        foreach (var line in lines)
        {
            var patternMatch = Regex.Match(line, @"\[([.#]+)\]");
            var pattern = patternMatch.Groups[1].Value
                .Select(ch => ch == '#' ? 1 : 0)
                .ToArray();

            var buttonMatches = Regex.Matches(line, @"\(([0-9,]+)\)");
            var buttons = buttonMatches
                .Select(m => m.Groups[1].Value.Split(',').Select(int.Parse).ToArray())
                .ToArray();

            var targetMatch = Regex.Match(line, @"\{([0-9,]+)\}");
            var joltage = targetMatch.Groups[1].Value.Split(',').Select(int.Parse).ToArray();

            list.Add((pattern, buttons, joltage));
        }

        return list.ToArray();
    }

    // -----------------------------
    // Build Matrix
    // -----------------------------
    private static T[][] BuildMatrix<T>(
        int rowCount,
        int buttonCount,
        int[][] buttons,
        T zeroValue,
        T oneValue,
        Func<int, T> getTarget)
    {
        var matrix = new T[rowCount][];

        for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
        {
            matrix[rowIndex] = new T[buttonCount + 1];

            for (int colIndex = 0; colIndex < buttonCount; colIndex++)
            {
                matrix[rowIndex][colIndex] = buttons[colIndex].Contains(rowIndex) ? oneValue : zeroValue;
            }

            matrix[rowIndex][buttonCount] = getTarget(rowIndex);
        }

        return matrix;
    }

    // -----------------------------
    // Swap Rows
    // -----------------------------
    private static T[][] SwapRows<T>(int rowA, int rowB, T[][] matrix)
    {
        if (rowA == rowB) return matrix;

        var tmp = matrix[rowA];
        matrix[rowA] = matrix[rowB];
        matrix[rowB] = tmp;

        return matrix;
    }

    // -----------------------------
    // GF(2) Gaussian Elimination
    // -----------------------------
    private static (int[][] matrix, List<int> pivots, int nextRow) GaussGF2(int rowCount, int colCount, int[][] matrix)
    {
        int pivotRow = 0;
        var pivots = new List<int>();

        for (int colIndex = 0; colIndex < colCount; colIndex++)
        {
            int? pivotCandidate = Enumerable.Range(pivotRow, rowCount - pivotRow)
                .FirstOrDefault(r => matrix[r][colIndex] == 1);

            if (pivotCandidate == null || pivotCandidate == default) continue;

            int pivot = pivotCandidate.Value;
            SwapRows(pivotRow, pivot, matrix);

            // Eliminate
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                if (rowIndex != pivotRow && matrix[rowIndex][colIndex] == 1)
                {
                    for (int k = 0; k <= colCount; k++)
                    {
                        matrix[rowIndex][k] ^= matrix[pivotRow][k];
                    }
                }
            }

            pivots.Add(colIndex);
            pivotRow++;

            if (pivotRow >= rowCount) break;
        }

        return (matrix, pivots, pivotRow);
    }

    private static int[] FindFreeVariables(int colCount, List<int> pivots)
    {
        var pivotSet = new HashSet<int>(pivots);
        return Enumerable.Range(0, colCount).Where(c => !pivotSet.Contains(c)).ToArray();
    }

    // -----------------------------
    // Solve Part 1 (GF2)
    // -----------------------------
    private static int? SolvePattern(int[] target, int[][] buttons)
    {
        int rowCount = target.Length;
        int colCount = buttons.Length;

        var matrix = BuildMatrix(rowCount, colCount, buttons, 0, 1, r => target[r]);
        var (reduced, pivots, nextRow) = GaussGF2(rowCount, colCount, matrix);

        // Check inconsistency
        for (int r = nextRow; r < rowCount; r++)
        {
            if (reduced[r][colCount] == 1)
                return null;
        }

        var freeVars = FindFreeVariables(colCount, pivots);
        var pivotMap = pivots
            .Select((col, index) => (col, index))
            .ToDictionary(x => x.col, x => x.index);

        int MinSolutionSum = int.MaxValue;

        int freeCount = freeVars.Length;
        int maskLimit = 1 << freeCount;

        for (int mask = 0; mask < maskLimit; mask++)
        {
            var freeValues = new Dictionary<int, int>();

            for (int k = 0; k < freeCount; k++)
            {
                freeValues[freeVars[k]] = (mask >> k) & 1;
            }

            int[] solution = new int[colCount];

            for (int j = 0; j < colCount; j++)
            {
                if (freeValues.TryGetValue(j, out int fv))
                {
                    solution[j] = fv;
                }
                else
                {
                    int pivotRowIndex = pivotMap[j];
                    int value = reduced[pivotRowIndex][colCount];

                    foreach (int free in freeVars)
                    {
                        value ^= reduced[pivotRowIndex][free] * freeValues[free];
                    }

                    solution[j] = value;
                }
            }

            int sum = solution.Sum();
            MinSolutionSum = Math.Min(MinSolutionSum, sum);
        }

        return MinSolutionSum;
    }

    public static int Part1((int[], int[][], int[])[] input)
    {
        return input.Sum(block =>
            SolvePattern(block.Item1, block.Item2) ?? 0
        );
    }

    // -----------------------------
    // Gaussian Elimination (Real)
    // -----------------------------
    private static (double[][] matrix, List<int> pivots) GaussReal(int rowCount, int colCount, double[][] matrix)
    {
        int pivotRow = 0;
        var pivots = new List<int>();

        for (int colIndex = 0; colIndex < colCount; colIndex++)
        {
            int bestRow = Enumerable.Range(pivotRow, rowCount - pivotRow)
                .OrderByDescending(r => Math.Abs(matrix[r][colIndex]))
                .FirstOrDefault();

            if (Math.Abs(matrix[bestRow][colIndex]) <= Epsilon)
                continue;

            SwapRows(pivotRow, bestRow, matrix);

            double divisor = matrix[pivotRow][colIndex];
            for (int c = 0; c <= colCount; c++)
                matrix[pivotRow][c] /= divisor;

            // Eliminate
            for (int r = 0; r < rowCount; r++)
            {
                if (r != pivotRow && Math.Abs(matrix[r][colIndex]) > Epsilon)
                {
                    double factor = matrix[r][colIndex];
                    for (int c = 0; c <= colCount; c++)
                    {
                        matrix[r][c] -= factor * matrix[pivotRow][c];
                    }
                }
            }

            pivots.Add(colIndex);
            pivotRow++;

            if (pivotRow >= rowCount) break;
        }

        return (matrix, pivots);
    }

    // -----------------------------
    // Solve Part 2 (Joltage, Real Gauss)
    // -----------------------------
    private static int SolveJoltage(int[] joltageLevels, int[][] buttons)
    {
        int rowCount = joltageLevels.Length;
        int colCount = buttons.Length;

        var matrix = BuildMatrix(rowCount, colCount, buttons, 0.0, 1.0, r => (double)joltageLevels[r]);
        var (reduced, pivots) = GaussReal(rowCount, colCount, matrix);

        var freeVars = FindFreeVariables(colCount, pivots);
        int freeCount = freeVars.Length;

        var pivotMap = pivots
            .Select((col, index) => (col, index))
            .ToDictionary(x => x.col, x => x.index);

        bool Valid(double[] solution)
        {
            return solution.All(x => x >= -Epsilon && Math.Abs(x - Math.Round(x)) < Epsilon);
        }

        int Total(double[] solution) => solution.Sum(x => (int)Math.Round(x));

        double BackSubstituteValue(int col, Dictionary<int, double> partial)
        {
            if (partial.ContainsKey(col)) return partial[col];

            int pr = pivotMap[col];
            double value = reduced[pr][colCount];

            foreach (var fv in freeVars)
            {
                value -= reduced[pr][fv] * partial[fv];
            }

            return value;
        }

        double[] Back(Dictionary<int, double> fm)
        {
            double[] result = new double[colCount];
            for (int j = 0; j < colCount; j++)
            {
                if (fm.ContainsKey(j))
                    result[j] = fm[j];
                else
                    result[j] = BackSubstituteValue(j, fm);
            }
            return result;
        }

        if (freeCount == 0)
        {
            var sol = Back(new Dictionary<int, double>());
            if (Valid(sol)) return Total(sol);
            throw new Exception("No valid solution");
        }

        int maxJ = joltageLevels.Max();
        int sumJ = joltageLevels.Sum();

        double Coeff(int pivotIndex, int k) => reduced[pivotIndex][freeVars[k]];

        bool Feasible(int depth, int[] freeValues)
        {
            for (int pr = 0; pr < pivots.Count; pr++)
            {
                double pv = reduced[pr][colCount];
                for (int k = 0; k < depth; k++)
                {
                    pv -= Coeff(pr, k) * freeValues[k];
                }

                if (pv >= -Epsilon)
                    continue;

                bool ok = false;
                for (int k = depth; k < freeCount; k++)
                {
                    if (Coeff(pr, k) < -Epsilon)
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok) return false;
            }
            return true;
        }

        int bestSolution = int.MaxValue;

        void Search(int depth, int[] values, int runningSum)
        {
            if (runningSum >= bestSolution || runningSum > sumJ || !Feasible(depth, values))
                return;

            if (depth == freeCount)
            {
                var fm = new Dictionary<int, double>();
                for (int i = 0; i < freeCount; i++)
                    fm[freeVars[i]] = values[i];

                var sol = Back(fm);
                if (Valid(sol))
                    bestSolution = Math.Min(bestSolution, Total(sol));

                return;
            }

            for (int x = Math.Min(maxJ, sumJ - runningSum); x >= 0; x--)
            {
                values[depth] = x;
                Search(depth + 1, values, runningSum + x);
            }
        }

        Search(0, new int[freeCount], 0);
        return bestSolution;
    }

    public static int Part2((int[], int[][], int[])[] input)
    {
        return input.Sum(block =>
            SolveJoltage(block.Item3, block.Item2)
        );
    }

    // -----------------------------
    // Combined Entry Point
    // -----------------------------
    public static (int Part1, int Part2) Solve(IEnumerable<string> inputLines)
    {
        var parsed = Parse(inputLines);
        return (Part1(parsed), Part2(parsed));
    }
}
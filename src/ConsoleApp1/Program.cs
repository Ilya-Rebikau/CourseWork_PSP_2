using ConsoleApp1;
using System.Diagnostics;
using System.Linq;

var matrix = new float[][]
{
    new float[] { 4, 7, 13 },
    new float[] { 9, 15, 12 },
    new float[] { 5, 1, 8 },
};
var vector = new float[] { 14, 26, 32 };
for (int i = 0; i < matrix.Length; i++)
{
    matrix[i] = matrix[i].Append(vector[i]).ToArray();
}

var servers = new List<Server>
{
    new Server(),
    new Server(),
    new Server()
};

int serversCount = servers.Count;
for (int i = 0, k = 0; i < matrix.Length; i++, k++)
{
    if (k == servers.Count)
    {
        k = 0;
    }

    for (int z = 0; z < serversCount; z++)
    {
        servers[z].stroku = new Dictionary<int, float[]>();
    }

    servers[k].strokaToSend = CopyArray(matrix[i]);
    for (int j = 0; j < matrix.Length;)
    {
        for (int z = 0; z < serversCount; z++)
        {
            if (j == matrix.Length)
            {
                break;
            }

            servers[z].stroku.Add(j, CopyArray(matrix[j]).ToArray());
            j++;
        }
    }

    for (int z = 0; z < serversCount; z++)
    {
        servers[z].strokaToGet = servers[k].strokaToSend;
        servers[z].Minus(i);
    }

    var totalNumber = matrix.Length;
    matrix = Array.Empty<float[]>();
    for (int z = 0, v = 0, x = 0; x < totalNumber; z++, x++)
    {
        if (z == serversCount)
        {
            v++;
            z = 0;
        }

        if (servers[z].stroku.Count != v && !Contains(matrix, servers[z].stroku.Values.ElementAt(v)))
        {
            matrix = matrix.Append(CopyArray(servers[z].stroku.Values.ElementAt(v))).ToArray();
        }
    }
}

for (int i = 0; i < matrix.Length; i++)
{
    vector[i] = matrix[i].Last();
    Array.Resize(ref matrix[i], matrix.Length);
}

MatrixToDebug(matrix);
var vectorX = CopyArray(vector);
for (int i = vector.Length - 1, k = serversCount - 1; i >= 0; i--, k--)
{
    if (k == 0)
    {
        k = serversCount - 1;
    }

    vectorX[i] = servers[k].GetX(vectorX[i], matrix[i][i]);
    var currentX = vectorX[i];
    var currentIndex = i;
    for (int j = i - 1; j >= 0; j--)
    {
        vectorX[j] = servers[k].GetX(vectorX[j], currentX, matrix[j][currentIndex]);
    }
}

VectorToDebug(vectorX);

Console.Read();

bool Contains(float[][] matrix, float[] stroka)
{
    for (int i = 0; i < matrix.Length; i++)
    {
        var x = 0;
        for (int j = 0; j < matrix[i].Length; j++)
        {
            if (matrix[i][j] == stroka[j])
            {
                x++;
            }
        }

        if (x == matrix[i].Length)
        {
            return true;
        }
    }

    return false;
}

float[] CopyArray(float[] arrayToCopy)
{
    var newArray = new float[arrayToCopy.Length];
    for (int i = 0; i < arrayToCopy.Length; i++)
    {
        newArray[i] = arrayToCopy[i];
    }

    return newArray;
}

float[][] CopyMatrix(float[][] matrixToCopy)
{
    var newMatrix = new float[matrixToCopy.Length][];
    for (int i = 0; i < matrixToCopy.Length; i++)
    {
        newMatrix[i] = new float[matrixToCopy[i].Length];
        for (int j = 0; j < matrixToCopy[i].Length; j++)
        {
            newMatrix[i][j] = matrixToCopy[i][j];
        }
    }

    return newMatrix;
}

void VectorToDebug(float[] vector)
{
    for (int i = 0; i < vector.Length; i++)
    {
        Debug.Write($"{vector[i]}   ");
    }

    Debug.WriteLine("\n________________________________________");
}

void MatrixToDebug(float[][] matrix)
{
    for (int i = 0; i < matrix.Length; i++)
    {
        for (int j = 0; j < matrix[i].Length; j++)
        {
            Debug.Write($"{matrix[i][j]}   ");
        }

        Debug.WriteLine("");
    }

    Debug.WriteLine("________________________________________");
}
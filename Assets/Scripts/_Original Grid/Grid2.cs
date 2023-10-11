using System;
using System.Collections.Generic;

/// <summary>
/// Source https://github.com/lordjesus/Packt-Introduction-to-graph-algorithms-for-game-developers
/// </summary>
public class Point2
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point2(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj is Point)
        {
            Point p = obj as Point;
            return this.X == p.X && this.Y == p.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 6949;
            hash = hash * 7907 + X.GetHashCode();
            hash = hash * 7907 + Y.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return "P(" + this.X + ", " + this.Y + ")";
    }
}

public enum CellType2
{
    Empty,
    Road,
    Structure,
    SpecialStructure,
    None
}

public class Grid2
{
    private Tuple<CellType2,int>[,] _grid;
    private int _width;
    public int Width { get { return _width; } }
    private int _height;
    public int Height { get { return _height; } }

    private List<Point> _roadList = new List<Point>();
    private List<Point> _specialStructure = new List<Point>();

    public Grid2(int width, int height)
    {
        _width = width;
        _height = height;
        _grid = new Tuple<CellType2,int>[height,width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                _grid[i,j] = Tuple.Create(CellType2.Empty,0);
            }
        }
    }

    // Adding index operator to our Grid class so that we can use grid[][] to access specific cell from our grid. 
    public Tuple<CellType2,int> this[int i, int j]
    {
        get
        {
            return _grid[i, j];
        }
        set
        {
            if (value.Item1 == CellType2.Road)
            {
                _roadList.Add(new Point(i, j));
            }
            else
            {
                _roadList.Remove(new Point(i, j));
            }
            if (value.Item1 == CellType2.SpecialStructure)
            {
                _specialStructure.Add(new Point(i, j));
            }
            else
            {
                _specialStructure.Remove(new Point(i, j));
            }
            _grid[i, j] = value;
        }
    }

    public static bool IsCellWakable(CellType2 cellType, bool aiAgent = false)
    {
        if (aiAgent)
        {
            return cellType == CellType2.Road;
        }
        return cellType == CellType2.Empty || cellType == CellType2.Road;
    }

    public Point GetRandomRoadPoint()
    {
        Random rand = new Random();
        return _roadList[rand.Next(0, _roadList.Count - 1)];
    }

    public Point GetRandomSpecialStructurePoint()
    {
        Random rand = new Random();
        return _roadList[rand.Next(0, _roadList.Count - 1)];
    }

    public List<Point> GetAdjacentCells(Point cell, bool isAgent)
    {
        return GetWakableAdjacentCells((int)cell.X, (int)cell.Y, isAgent);
    }

    public float GetCostOfEnteringCell(Point cell)
    {
        return 1;
    }

    public List<Point> GetAllAdjacentCells(int x, int y)
    {
        List<Point> adjacentCells = new List<Point>();
        if (x > 0)
        {
            adjacentCells.Add(new Point(x - 1, y));
        }
        if (x < _width - 1)
        {
            adjacentCells.Add(new Point(x + 1, y));
        }
        if (y > 0)
        {
            adjacentCells.Add(new Point(x, y - 1));
        }
        if (y < _height - 1)
        {
            adjacentCells.Add(new Point(x, y + 1));
        }
        return adjacentCells;
    }

    public List<Point> GetWakableAdjacentCells(int x, int y, bool isAgent)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if(IsCellWakable(_grid[adjacentCells[i].X, adjacentCells[i].Y].Item1, isAgent)==false)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    public List<Point> GetAdjacentCellsOfType(int x, int y, CellType2 type)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (_grid[adjacentCells[i].X, adjacentCells[i].Y].Item1 != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    public List<Point> GetAdjacentCellsOfColor(int x, int y, int color)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (_grid[adjacentCells[i].X, adjacentCells[i].Y].Item2 != color)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    /// <summary>
    /// Returns array [Left neighbour, Top neighbour, Right neighbour, Down neighbour]
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public CellType2[] GetAllAdjacentCellTypes(int x, int y)
    {
        CellType2[] neighbours = { CellType2.None, CellType2.None, CellType2.None, CellType2.None };
        if (x > 0)
        {
            neighbours[0] = _grid[x - 1, y].Item1;
        }
        if (x < _width - 1)
        {
            neighbours[2] = _grid[x + 1, y].Item1;
        }
        if (y > 0)
        {
            neighbours[3] = _grid[x, y - 1].Item1;
        }
        if (y < _height - 1)
        {
            neighbours[1] = _grid[x, y + 1].Item1;
        }
        return neighbours;
    }
    public int[] GetAllAdjacentCellColor(int x, int y)
    {
        int[] neighbours = { 0, 0, 0, 0 };
        if (x > 0)
        {
            neighbours[0] = _grid[x - 1, y].Item2;
        }
        if (x < _width - 1)
        {
            neighbours[2] = _grid[x + 1, y].Item2;
        }
        if (y > 0)
        {
            neighbours[3] = _grid[x, y - 1].Item2;
        }
        if (y < _height - 1)
        {
            neighbours[1] = _grid[x, y + 1].Item2;
        }
        return neighbours;
    }
}

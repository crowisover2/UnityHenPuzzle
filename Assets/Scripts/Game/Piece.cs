using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    #region Board Component
    [Serializable]
    public class Board
    {
        [SerializeField] private List<Vertical> row = new();

        public Board()
        {
            for (int row = 0; row < Constants.MaxRows; row++)
            {
                Vertical tempRow = new();
                for (int column = 0; column < Constants.MaxColumns; column++)
                {
                    Piece tempPiece = new();
                    
                    //tempPiece.CurrColumn = 0;
                    //tempPiece.CurrRow = 0;
                    //tempPiece.OriColumn = 0;
                    //tempPiece.OriColumn = 0;
                    //tempPiece.GameObject = null;

                    tempRow.column.Add(tempPiece);
                }
                this.row.Add(tempRow);
            }
        }

        public void SetGameObject(int _row, int _column, GameObject _object) => row[_row].column[_column].GameObject = _object;
        public void SetPosition(int _row, int _column, Vector2 _pos)
        {
            row[_row].column[_column].GameObject.transform.position = _pos;
        }

        public Piece GetPiece(int _row, int _column)
        {
            return row[_row].column[_column];
        }

        public Piece GetPiece(GameObject _gameObject)
        {
            for (int row = 0; row < Constants.MaxRows; row++)
            {
                for (int column = 0; column < Constants.MaxColumns; column++)
                {
                    if (this.row[row].column[column].GameObject.Equals(_gameObject))
                    {
                        return this.row[row].column[column];
                    }
                }
            }

            return null;
        }

        public void SetEmpty(int _row, int _column, bool _value = false)
        {
            row[_row].column[_column].GameObject.SetActive(_value);
        }

        public bool IsEmpty(int _row, int _column)
        {
            return row[_row].column[_column].GameObject.activeSelf;
        }

        public void SetOriginal(int _row, int _column) {
            row[_row].column[_column].OriColumn = _column;
            row[_row].column[_column].OriRow = _row;
        }

        public void SetCurrent(int _row, int _column, int _id)
        {
            int row = (_id - 1) / Constants.MaxColumns;
            int column = (_id - 1) % Constants.MaxColumns;

            this.row[_row].column[_column].CurrColumn = column;
            this.row[_row].column[_column].CurrRow = row;

            //Debug.Log("ORI " + row[_row].column[_column].OriColumn
            //+" ORI " + row[_row].column[_column].OriRow +
            //"| Curr " + row[_row].column[_column].CurrColumn
            //+" Curr " + row[_row].column[_column].CurrRow);
        }

        public Piece Swap(Piece _cur, Piece _nul)
        {
            int tempRow = _nul.CurrRow;
            int tempCol = _nul.CurrColumn;

            _nul.CurrRow = _cur.CurrRow;
            _nul.CurrColumn = _cur.CurrColumn;

            _cur.CurrRow = tempRow;
            _cur.CurrColumn = tempCol;

            return _cur;
        }

        public bool IsTheSame(int _row, int _column)
        {
            if (row[_row].column[_column].CurrRow == row[_row].column[_column].OriRow && row[_row].column[_column].CurrColumn == row[_row].column[_column].OriColumn) return true;
            else return false;
        }
    }

    [Serializable]
    class Vertical { [SerializeField] public List<Piece> column = new(); }

    [Serializable]
    public class Piece
    {
        public int OriRow;
        public int OriColumn;

        public int CurrRow;
        public int CurrColumn;

        public GameObject GameObject;
    }
#endregion

    #region Matrix Component
    [Serializable]
    public class Matrix
    {
        [SerializeField]
        private List<Row> row = new();

        public Matrix()
        {
            int value = 1;
            for (int row = 0; row < Constants.MaxRows; row++)
            {
                Row tempRow = new Row();
                for (int column = 0; column < Constants.MaxColumns; column++)
                {
                    Coor tempCoor = new Coor();
                    tempCoor.ID = value++;
                    tempCoor.pos = Vector3.zero;

                    tempRow.column.Add(tempCoor);
                }
                this.row.Add(tempRow); // Add the constructed row to the matrix
            }
        }

        public int GetID(int _row, int _column)
        {
            return row[_row].column[_column].ID;
        }

        public void SetID(int _row, int _column, int _value) => row[_row].column[_column].ID = _value;

        public Vector2 GetVector(int _id)
        {
            for (int column = 0; column < Constants.MaxColumns; column++)
            {
                for (int row = 0; row < Constants.MaxRows; row++)
                {
                    if (this.row[row].column[column].ID.Equals(_id))
                    {
                        return this.row[row].column[column].pos;
                    }
                }
            }

            return Vector2.zero;
        }

        public Vector2 GetVector(int _row, int _column) => row[_row].column[_column].pos;
        public Vector2 SetVector(int _row, int _column, Vector2 _value) => row[_row].column[_column].pos = _value;
    }

    [Serializable]
    class Row
    {
        [SerializeField]
        public List<Coor> column = new();
    }

    [Serializable]
    class Coor
    {
        public int ID;
        public Vector2 pos;
    }
    #endregion
}

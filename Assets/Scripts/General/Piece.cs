using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleHen.Game
{
    #region Board Component
    [Serializable]
    public class Board
    {
        //[SerializeField] private List<Vertical> row = new();
        [SerializeField] private List<Piece> pieces;

        public int IdEmpty { set; get; }

        private UnityAction winAction;

        public Board(UnityAction _action)
        {
            pieces = new List<Piece>();
            winAction = _action;
        }

        public void InitPerPiece(int _id, RectTransform _rect)
        {
            foreach (var piece in pieces) {
                if (piece.IdOri == _id)
                {
                    piece.Rect = _rect;
                    piece.OriPos = _rect.position;
                    return;
                }
            }

            Piece pi = new Piece()
            {
                IdOri = _id,
                OriPos = _rect.position,
                Rect = _rect
            };

            pieces.Add(pi);
        }

        public Piece GetPiece(int _idOri) => pieces.Find((x) => x.IdOri.Equals(_idOri));
        public Piece GetPieceCurr(int _idCurr) => pieces.Find((x) => x.IdCurr.Equals(_idCurr));

        public void SetCurrent(int _idOri, int _idCurr)
        {
            pieces[_idOri].Rect.position = pieces[_idCurr].OriPos;
            pieces[_idOri].IdCurr = _idCurr; 
        }

        public void Swap(int _idCurr, IEnumerable<int> _ids)
        {
            Piece actor = GetPieceCurr(_idCurr);
            foreach (int id in _ids)
            {
                if (id.Equals(Constants.IdIgnore)) continue;

                Piece target = GetPieceCurr(id);
                if (target.IdOri.Equals(IdEmpty))
                {
                    int _idActor = actor.IdCurr;
                    int _idTarget = target.IdCurr;
                    SetCurrent(actor.IdOri, _idTarget);
                    SetCurrent(target.IdOri, _idActor);
                    break;
                }
            }

            CheckBoard();
        }

        private void CheckBoard()
        {
            foreach (var piece in pieces) {
                if (!piece.IdCurr.Equals(piece.IdOri)) return;
            }

            winAction?.Invoke();
        }
    }

    [Serializable]
    public class Piece : Column
    {
        public RectTransform Rect;
        public Vector2 OriPos;
    }

#endregion

    #region Matrix Component
    [Serializable]
    public class Matrix
    {
        [SerializeField] private int idEmpty;
        [SerializeField] private List<Row> row = new();
        private int numSlices => PuzzleDataMiner.Current.NumSlices;

        public Matrix()
        {
            int value = -1;
            idEmpty = -1;

            for (int row = 0; row < numSlices; row++)
            {
                Row tempRow = new Row();
                for (int column = 0; column < numSlices; column++)
                {
                    value++;

                    Column tempCoor = new Column();
                    tempCoor.IdOri = value;
                    tempCoor.IdCurr = value;

                    tempRow.column.Add(tempCoor);
                }
                this.row.Add(tempRow); // Add the constructed row to the matrix
            }
        }
        public bool InitEmpty(int _row, int _column, int _emptyTileValue)
        {
            if(GetID(_row, _column) == _emptyTileValue)
            {
                idEmpty = _emptyTileValue;
                return true;
            }
            return false;
        }
        public int GetID(int _row, int _column)
        {
            return row[_row].column[_column].IdOri;
        }
        public void GetID(int _row, int _column, out int _idOri, out int _idCurr)
        {
            _idOri = row[_row].column[_column].IdOri;
            _idCurr = row[_row].column[_column].IdCurr;
        }
        public int GetEmptyPos(out int _row, out int _column)
        {
            for (int i = 0; i < row.Count; i++)
            {
                for (int h = 0; h < row[i].column.Count; h++)
                {
                    if (row[i].column[h].IdCurr == idEmpty)
                    {
                        _row = i;
                        _column = h;
                        return row[i].column[h].IdOri;
                    }
                }
            }

            _row = 99;
            _column = 99;
            return 99;
        }
        public int IdEmpty => idEmpty;
        public void SwapIdCurr(int _rowOri, int _columnOri, int _rowTarget, int _columnTarget)
        {
            var oriID = row[_rowOri].column[_columnOri].IdCurr;
            var targetID = row[_rowTarget].column[_columnTarget].IdCurr;

            row[_rowOri].column[_columnOri].IdCurr = targetID;
            row[_rowTarget].column[_columnTarget].IdCurr = oriID;
        }
    }

    [Serializable]
    class Row
    {
        [SerializeField]
        public List<Column> column = new();
    }

    [Serializable]
    public class Column
    {
        public int IdCurr;
        public int IdOri;
    }
    #endregion
}

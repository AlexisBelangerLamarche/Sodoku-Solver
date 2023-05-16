using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardSolver : MonoBehaviour
{
    private static BoardSolver _instance;

    public static BoardSolver Instance { get { return _instance; } }

    public Cell[] Cells;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ResetBoard()
    {
        foreach (Cell cell in Cells)
        {
            cell.ResetCell();
        }
    }

    public void SolveBoard()
    {
        foreach (Cell cell in Cells)
        {
            if (cell.Solved == true)
            {
                continue;
            }

            if (cell.PredictedNumbers.Count == 1)
            {
                cell.SolveCell(cell.PredictedNumbers[0]);
                SolveBoard();
            }
        }

        foreach (Cell cell in Cells)
        {
            List<int> AllBoxPredictions = new List<int>();
            bool foundUnique = false;

            if (cell.Solved == true)
            {
                continue;
            }

            AllBoxPredictions.AddRange(cell.PredictedNumbers);

            foreach (Cell boxCell in Cells)
            {
                if (boxCell == cell)
                {
                    continue;
                }

                if (boxCell.Solved == true)
                {
                    continue;
                }

                if (boxCell.Box == cell.Box)
                {
                    AllBoxPredictions.AddRange(boxCell.PredictedNumbers);
                }
            }

            //Code stolen from here https://stackoverflow.com/questions/292307/selecting-unique-elements-from-a-list-in-c-sharp
            var uniqueNumbers =
                from n in AllBoxPredictions.ToArray()
                group n by n into nGroup
                where nGroup.Count() == 1
                select nGroup.Key;

            if (uniqueNumbers.Count() == 1)
            {

                int numbertofind = uniqueNumbers.First();

                foreach (Cell boxCell in Cells)
                {
                    if (boxCell == cell)
                    {
                        continue;
                    }

                    if (boxCell.Solved == true)
                    {
                        continue;
                    }

                    if (boxCell.Box == cell.Box)
                    {
                        foreach (int number in cell.PredictedNumbers)
                        {
                            if (number == numbertofind)
                            {
                                cell.SolveCell(number);
                                SolveBoard();
                                foundUnique = true;
                                break;
                            }
                        }
                    }
                }

                if (foundUnique == false)
                {
                    foreach (int number in cell.PredictedNumbers)
                    {
                        if (number == numbertofind)
                        {
                            cell.SolveCell(number);
                            SolveBoard();
                            foundUnique = true;
                            break;
                        }
                    }
                }
            }



        }


    }
}

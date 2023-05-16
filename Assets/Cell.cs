using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Column;
    public int Row;
    public int Box;
    public List<int> PredictedNumbers = new List<int>();
    private bool Locked;
    public bool Solved;
    public int CellValue;
    [SerializeField] private GameObject DisplayNumber;
    [SerializeField] private GameObject DisplayPredictedNumber;
    public delegate void CellChanged();
    public static CellChanged cellChanged;

    // Start is called before the first frame update
    void Start()
    {
        DisplayPredictedNumber = transform.Find("PredictionNumber").gameObject;

        HideDiplayNumbers();

        HidePredictions();

        Cell.cellChanged += UpdatePredictions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        CellClicked();
    }

    public void CellClicked()
    {
        if (Locked == true)
            return;

        SolveCell(CellValue + 1);

    }

    public void UpdatePredictions()
    {
        HidePredictions();
        if (Solved == true)
        {
            return;
        }

        Cell[] cells = BoardSolver.Instance.Cells;
        List<int> Predictions = new List<int>() {9, 8, 7, 6, 5, 4, 3, 2, 1};

        foreach (Cell cell in cells)
        {
            if (cell == this)
            {
                continue;
            }

            if (cell.Solved == false)
            {
                continue;
            }

            if (cell.Row == Row || cell.Column == Column || cell.Box == Box)
            {

                Predictions.Remove(cell.CellValue);
            }

        }

        foreach (Transform numbers in DisplayPredictedNumber.transform)
        {
            foreach (int prediction in Predictions)
            {
                if (numbers.name == prediction.ToString())
                {
                    numbers.gameObject.SetActive(true);
                }    
            }
        }

        PredictedNumbers = Predictions;
    }

    public void HideDiplayNumbers()
    {
        foreach (Transform numbers in DisplayNumber.transform)
        {
            numbers.gameObject.SetActive(false);
        }
    }

    public void HidePredictions()
    {
        foreach (Transform numbers in DisplayPredictedNumber.transform)
        {
            numbers.gameObject.SetActive(false);
        }
    }

    public void ResetCell()
    {
        CellValue = 0;

        Solved = false;

        PredictedNumbers.Clear();

        HideDiplayNumbers();

        HidePredictions();
    }

    public void SolveCell(int Value)
    {
        if (Value > 9 || Value < 0)
        {
            Value = 0;
            Solved = false;
        }
        else
        {
            Solved = true;
            PredictedNumbers.Clear();
        }

        
        CellValue = Value;

        foreach (Transform numbers in DisplayNumber.transform)
        {
            if (numbers.name == Value.ToString())
            {
                numbers.gameObject.SetActive(true);
                continue;
            }

            numbers.gameObject.SetActive(false);
        }
        cellChanged?.Invoke();
    }
}

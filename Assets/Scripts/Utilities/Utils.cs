using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Utils : MonoBehaviour
{
    public static ValidationResult ValidateJSONData(List<BlockData> blocks, int rows, int cols)
    {
        int totalElements = rows * cols;

        if (totalElements < 4 || totalElements > 64)
        {
            return ValidationResult.ERROR_INVALID_ELEMENT_COUNT;
        }

        if (rows < 2 || rows > 8 || cols < 2 || cols > 8)
        {
            return ValidationResult.ERROR_INVALID_DIMENSIONS;
        }

        if (totalElements % 2 != 0)
        {
            return ValidationResult.ERROR_ODD_ELEMENT_COUNT;
        }

        var numberCounts = blocks.GroupBy(block => block.number)
                                          .ToDictionary(g => g.Key, g => g.Count());

        foreach (var kvp in numberCounts)
        {
            if (kvp.Value != 2)
            {
                return ValidationResult.ERROR_INVALID_OCCURRENCES;
            }
        }


        var seenPositions = new HashSet<(int, int)>();

        foreach (var block in blocks)
        {
            var position = (block.R, block.C);
            if (seenPositions.Contains(position))
            {
                return ValidationResult.ERROR_DUPLICATE_POSITION;
            }
            seenPositions.Add(position);
        }


        return ValidationResult.SUCCESS;
    }
}

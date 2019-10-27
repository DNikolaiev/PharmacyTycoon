using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Regression  {

	public void LinearRegression(int[] xArray, int[] yArray, out float a, out float b, out float rSq) // a - 0, b - 1
    {
        a = 0;b = 0; rSq = 0;
        if (xArray.Length != yArray.Length) return;
        float sumX = 0;
        float sumY = 0;
        float sumYsq = 0;
        float sumXsq = 0;
        float sumCodeviates = 0;

        for(var i=0; i<xArray.Length;i++)
        {
            var x = xArray[i];
            var y = yArray[i];
            sumCodeviates += x * y;
            sumX += x;
            sumY += y;
            sumXsq += x * x;
            sumYsq += y * y;
        }
        int count = xArray.Length;
        var ssX = sumXsq - ((sumX * sumX) / count);
        var ssY = sumYsq - ((sumY * sumY) / count);
        var rNumerator = (count * sumCodeviates) - (sumX * sumY);
        var rDenom = (count * sumXsq - (sumX * sumX)) * (count * sumYsq - (sumY * sumY));
        var sCo = sumCodeviates - (sumX * sumY / count);
        var meanX = sumX / count;
        var meanY = sumY / count;
        var dblR = rNumerator / Mathf.Sqrt(rDenom);
        rSq = dblR * dblR;
        /*  a = ((count * sumCodeviates) - (sumX * sumY)) /
                     (count * sumXsq - (sumX * sumX));
         b = (sumY - a * sumX) / count; */

        /*a = ((count * sumCodeviates) - (sumX * sumY)) /
                    (count * sumXsq - (sumX * sumX));
        b = sumY * (sumXsq) - (sumX * sumCodeviates) /
            (count * sumXsq - (sumX * sumX)); */
            b= meanY - ((sCo / ssX) * meanX);
        a = sCo / ssX;
    }
    public List<float> Function(List<float> x, float a, float b)
    {
        List<float> y = new List<float>();
        for(int i=0; i<x.Count; i++)
        {
            y.Add(a * x[i] + b);
        }
        return y;

    }
}

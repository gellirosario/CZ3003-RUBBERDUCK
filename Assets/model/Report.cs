using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Report
{
    public int w1s1,w1s2,w1s3;
    public int w2s1, w2s2, w2s3;
    public int w3s1, w3s2, w3s3;
    public int w4s1, w4s2, w4s3;
    public int w5s1, w5s2, w5s3;

    public double worldStageReport;


    public Report(double wsReport)
    {
        worldStageReport = wsReport;
       
        //this.w1s1= ReportController.w1s1R;
    }
    public Report(IDictionary<string, object> dict)
    {
        //this.worldStageReport = dict["Appear"].ToString();
        this.worldStageReport = double.Parse(dict["Appear"].ToString());

    }
}
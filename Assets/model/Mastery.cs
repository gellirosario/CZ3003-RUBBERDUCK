using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mastery
{
    public int world1stage1;
    public int world1stage2;
    public int world1stage3;
    public int world2stage1;
    public int world2stage2;
    public int world2stage3;
    public int world3stage1;
    public int world3stage2;
    public int world3stage3;
    public int world4stage1;
    public int world4stage2;
    public int world4stage3;
    public int world5stage1;
    public int world5stage2;
    public int world5stage3;

    public Mastery()
    {
    }

    public Mastery(int w1s1, int w1s2, int w1s3, int w2s1, int w2s2, int w2s3, int w3s1, int w3s2, int w3s3, int w4s1, int w4s2, int w4s3, int w5s1, int w5s2, int w5s3)
    {
        this.world1stage1 = w1s1;
        this.world1stage2 = w1s2;
        this.world1stage3 = w1s3;
        this.world2stage1 = w2s1;
        this.world2stage2 = w2s2;
        this.world2stage3 = w2s3;
        this.world3stage1 = w3s1;
        this.world3stage2 = w3s2;
        this.world3stage3 = w3s3;
        this.world4stage1 = w4s1;
        this.world4stage2 = w4s2;
        this.world4stage3 = w4s3;
        this.world5stage1 = w5s1;
        this.world5stage2 = w5s2;
        this.world5stage3 = w5s3;
    }
    
    // Updating IDictionary to Class
    public Mastery(IDictionary <string, object> dict)
    {
        //pls refer to question model
    }
}


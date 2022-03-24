using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public List<Vector2> genes = new List<Vector2>();
    public DNA(int length = 20) {
        for (int i = 0; i < length; i++) {
            genes.Add(new Vector2(Random.Range(-0.5f, 1.5f),
                Random.Range(-0.5f, 1.5f)));
        }
    }

    /* contains crossover dna */
    public DNA(DNA parent, DNA parent2, float mutationRate = 0.01f) {
        for (int i = 0; i < parent.genes.Count; i++)
        {
            /* add the parents genes */
            float chance = Random.Range(0.0f, 1.0f);
            /* make new random gene */
            if (chance <= mutationRate) { genes.Add(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f))); }
            /* copy parent gene */
            else {
                /* crossover */
                int x = Random.Range(0, 2);
                if (x == 0) { genes.Add(parent.genes[i]); }
                else { genes.Add(parent2.genes[i]); }
            }
        }
    }
}

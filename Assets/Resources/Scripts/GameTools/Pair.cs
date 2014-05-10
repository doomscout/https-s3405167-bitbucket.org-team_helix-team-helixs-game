using UnityEngine;
using System.Collections;

//I can't seem to use the Tuple class, so this will have to do.
//I could make it generic, but meh
public class Pair {

    public int First { get; private set; }
    public int Second { get; private set; }

    public Pair(int t, int u) {
        First = t;
        Second = u;
    }

    public override bool Equals(object o) {
        if (o == null) {
            return false;
        }
        return this.First == ((Pair)o).First && this.Second == ((Pair)o).Second;
    }

    public override int GetHashCode() {
        return (First << 16) | Second;
    }
}

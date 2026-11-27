using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DungeonGenerator : MonoBehaviour
{

    [SerializeField] private int m_height;
    [SerializeField] private int m_width;
    [SerializeField] private int m_seed;
    [SerializeField] private int m_splits;
    [SerializeField] private int m_roomSize;
    [SerializeField] private int m_leafSize;

    private List<RectInt> m_leaves;
    private List<RectInt> m_rooms;

    private List<RectInt> m_pairA;
    private List<RectInt> m_pairB;

    private int[,] m_map;

    private System.Random m_rng;

    private void OnValidate()
    {
       if (m_leafSize < 4 ) m_leafSize = 4;
    }

    private void Generate()
    {
        m_rng = new System.Random(m_seed);

        SplitSpace();

        MakeRooms();

    }

    // min = leaf size
    // max = m_width - leaf size

    void SplitSpace()
    {
        //  pairA and pairB
        m_pairA.Clear();
        m_pairB.Clear();

        // leaves starts with ONE region: the whole map
        m_map = new int[m_width, m_height];

        // repeat 'splits' times:
        for (int i = 0; i < m_splits; i++)
        {
            //     a NEW empty list, next
            List<RectInt> next = new List<RectInt>();

            //     for every region in leaves:  Split(region, next)
            foreach(RectInt r in m_leaves)
            {
                Split(r, next);
            }

            //     swap: leaves becomes next

            m_leaves = next;
        }
    }

    void Split(RectInt r, List<RectInt> next)
    {

        RectInt a;
        RectInt b;

        bool canCutWidth = r.width > 2 * m_leafSize;
        bool canCutHeight = r.height > 2 * m_leafSize;



        // cut: a random number from minLeaf to (size - minLeaf), both allowed
        // both: cut the LONGER side (the same length: pick at random)
        // only one: cut that one
        // make the two halves, a and b: two new RectInt
        if (canCutWidth && canCutHeight)
        {
            if (r.x > r.y)
            {
                int sliceX = m_rng.Next(m_leafSize, r.width - m_leafSize);
                a = new RectInt(r.x, r.y, sliceX, r.height);
                b = new RectInt(sliceX, r.y, r.x - sliceX, r.height);
            }
            else if (r.x < r.y)
            {
                int sliceY = m_rng.Next(m_leafSize, r.height - m_leafSize);
                a = new RectInt(r.x, r.y, r.width, sliceY);
                b = new RectInt(r.x, sliceY, r.width, r.y - sliceY);
            }
            else
            {
                
                if (m_rng.Next(2) == 0)
                {
                    int sliceX = m_rng.Next(m_leafSize, r.width - m_leafSize);
                    a = new RectInt(r.x, r.y, sliceX, r.height);
                    b = new RectInt(sliceX, r.y, r.x - sliceX, r.height);
                }
                else
                {
                    int sliceY = m_rng.Next(m_leafSize, r.height - m_leafSize);
                    a = new RectInt(r.x, r.y, r.width, sliceY);
                    b = new RectInt(r.x, sliceY, r.width, r.y - sliceY);
                }
            }
        }
        // can it be cut across its width?    width  is at least 2 * minLeaf
        else if (canCutWidth)
        {
            int sliceX = m_rng.Next(m_leafSize, r.width - m_leafSize);
            a = new RectInt(r.x, r.y, sliceX, r.height);
            b = new RectInt(sliceX, r.y, r.x - sliceX, r.height);
        }
        // can it be cut across its height?   height is at least 2 * minLeaf
        else if (canCutHeight)
        {
            int sliceY = m_rng.Next(m_leafSize, r.height - m_leafSize);
            a = new RectInt(r.x, r.y, r.width, sliceY);
            b = new RectInt(r.x, sliceY, r.width, r.y - sliceY);
        }
        // neither: add r to next, and stop here
        else
        {
            next.Add(r);
            return;
        }


        // add both to next;  add a to pairA and b to pairB
        m_pairA.Add(a);
        m_pairB.Add(b);

        next.Add(a);
        next.Add(b);
    }

    void MakeRooms()
    {
        // fill the map with 1 (wall)

        // for every leaf:
        for (int i = 0; i < m_leaves.Count; i++)
        {
            //     w: a random number from minRoom to leaf.width - 2
            //     h: the same with height
            //     x: a random number from leaf.x + 1 to leaf.xMax - w - 1
            //     y: the same with y
            //     the room is a new RectInt: add it to rooms
            //     carve it: every cell inside becomes 0
        }


        // for every pair i:
        //     look at every room whose centre is inside pairA[i]
        //     and every room whose centre is inside pairB[i]
        //     keep the two whose centres are the fewest steps apart
        //     dig a corridor between those two centres

        // digging from one centre to the other, in an L:
        //     along:  every x between the two, at the first centre's y, becomes 0
        //     then:   every y between the two, at the second centre's x, becomes 0

    }

}

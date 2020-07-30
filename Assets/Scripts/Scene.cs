using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public const float xDistanceHex = 1.1f;
    public const float zDistanceHex = 1.73205080757f / 2 * 1.1f; //Mathf.Sqrt(3) = 1.73205080757
    public const int areaLength = 75;
    public static GameObject[] map;
    public static bool[] mapOccupied;
    public struct hexNum
    {
        public int num_x;
        public int num_z;

        public static bool operator< (hexNum a, hexNum b)
        {
            if(a.num_x < b.num_x) return true;
            if(a.num_x == b.num_x && a.num_z < b.num_z) return true;
            return false;
        }

        public static bool operator> (hexNum a, hexNum b)
        {
            if(a.num_x > b.num_x) return true;
            if(a.num_x == b.num_x && a.num_z > b.num_z) return true;
            return false;
        }

    }

    public struct hexNumFloat
    {
        public float num_x_float;
        public float num_z_float;
    }

    public struct hexAxis
    {
        public float axis_x;
        public float axis_z;

        public static bool operator< (hexAxis a, hexAxis b)
        {
            if(a.axis_x < b.axis_x) return true;
            if(a.axis_x == b.axis_x && a.axis_z < b.axis_z) return true;
            return false;
        }

        public static bool operator> (hexAxis a, hexAxis b)
        {
            if(a.axis_x > b.axis_x) return true;
            if(a.axis_x == b.axis_x && a.axis_z > b.axis_z) return true;
            return false;
        }

    }
    public static hexNum[][] hexCenter = new hexNum[9][];
    public static hexNum[] checkpointPos;
    public static hexAxis[] checkpointPosAxis;

    void Awake()
    {
        Initialize();
    }

    public static void Initialize()
    {
        map = new GameObject[areaLength*areaLength];
        mapOccupied = new bool[areaLength*areaLength];
        for (int i=0; i<areaLength*areaLength; i++)
            mapOccupied[i] = false;
        for (int i=0; i<9; i++)
            hexCenter[i] = new hexNum[0];
    }

    public static void GetCheckpointPosValue()
    {
        if (checkpointPos == null)
            checkpointPos = new hexNum[] {
                new hexNum {
                    num_x = Random.Range(3, 7),
                    num_z = areaLength*2/3 + Random.Range(-2, 3)
                },
                new hexNum {
                    num_x = areaLength*2/3 - Random.Range(1, 4),
                    num_z = areaLength - Random.Range(3, 7)
                },
                new hexNum {
                    num_x = areaLength/3 + Random.Range(1, 4),
                    num_z = Random.Range(3, 7)
                },
                new hexNum {
                    num_x = areaLength - Random.Range(3, 7),
                    num_z = areaLength/3 + Random.Range(-2, 3)
                },
            };
        if (checkpointPosAxis == null)
            checkpointPosAxis = Hexagon.NumToAxisArray(checkpointPos);
    }

    public static void SortHexagonCenter()
    {
        for (int i=0; i<9; i++)
            Utils.Sort(ref hexCenter[i]);
    }

    public static hexNum GetNextHexagonCenter(int area, int num)
    {
        // area: 0-8
        if (num+1<hexCenter[area].Length)
            return hexCenter[area][num+1];
        // search next subarea
        while(area<8)
        {
            if (hexCenter[++area].Length > 0)
                return hexCenter[area][0];
        }
        return new hexNum{
            num_x = -1,
            num_z = -1
        }; // the last one, no next element
    }

    public static void AddHexagonCenter(hexNum hexN)
    {
        if (hexN.num_x>=0 && hexN.num_x<areaLength/3)
        {
            if (hexN.num_z>=0 && hexN.num_z<areaLength/3)
                Utils.Add(ref hexCenter[0], hexN);
            else if (hexN.num_z>=areaLength/3 && hexN.num_z<areaLength*2/3)
                Utils.Add(ref hexCenter[1], hexN);
            else //hexN.num_z>=areaLength*2/3 && hexN.num_z<areaLength
                Utils.Add(ref hexCenter[2], hexN);
        }

        else if (hexN.num_x>=areaLength/3 && hexN.num_x<areaLength*2/3)
        {
            if (hexN.num_z>=0 && hexN.num_z<areaLength/3)
                Utils.Add(ref hexCenter[5], hexN);
            else if (hexN.num_z>=areaLength/3 && hexN.num_z<areaLength*2/3)
                Utils.Add(ref hexCenter[4], hexN);
            else //hexN.num_z>=areaLength*2/3 && hexN.num_z<areaLength
                Utils.Add(ref hexCenter[3], hexN);
        }

        else //hexN.num_x>=areaLength*2/3 && hexN.num_x<areaLength
        {
            if (hexN.num_z>=0 && hexN.num_z<areaLength/3)
                Utils.Add(ref hexCenter[6], hexN);
            else if (hexN.num_z>=33 && hexN.num_z<areaLength*2/3)
                Utils.Add(ref hexCenter[7], hexN);
            else //hexN.num_z>=areaLength*2/3 && hexN.num_z<areaLength
                Utils.Add(ref hexCenter[8], hexN);
        }
    }

    public static bool JudgeRectAreaOccupancy(int start_x, int start_z, int height, int width)
    {
        if (start_x+height >= areaLength) return false;
        if (start_z+width >= areaLength) return false;
        for (int i=0; i<height; i++)
            for (int j=0; j<width; j++)
            {
                int num = GetOneDimensionVal(start_x+i, start_z+j);
                if (mapOccupied[num]) return false;
            }
        return true;
    }

    public static void SetRectAreaOccupancy(int start_x, int start_z, int height, int width)
    {
        for (int i=0; i<height; i++)
            for (int j=0; j<width; j++)
            {
                int num = GetOneDimensionVal(start_x+i, start_z+j);
                Debug.Assert(!mapOccupied[num], "Block already occupied in SetRectAreaOccupancy");
                mapOccupied[num] = true;
            }
    }

    public static bool JudgeAreaOccupancy(hexNum[] area)
    {
        for (int i=0; i<area.Length; i++)
        {
            int num = GetOneDimensionVal(area[i].num_x, area[i].num_z);
            if (mapOccupied[num]) return false;
        }
        return true;
    }

    public static void SetAreaOccupancy(hexNum[] area)
    {
        for (int i=0; i<area.Length; i++)
        {
            Debug.Assert(area[i].num_x >= 0 && area[i].num_x < areaLength && area[i].num_z >= 0 && area[i].num_z < areaLength, "Area out of range in SetAreaOccupancy");
            int num = GetOneDimensionVal(area[i].num_x, area[i].num_z);
            Debug.Assert(!mapOccupied[num], "Block already occupied in SetAreaOccupancy");
            mapOccupied[num] = true;
        }
    }

    public static bool JudgeRectInRing(int rect_start_x, int rect_start_z, int height, int width, hexNum[] ring)
    {
        // detect 4 angles
        bool rectInRing = SearchHexNum(ring, rect_start_x, rect_start_z) &&
            SearchHexNum(ring, rect_start_x+height-1, rect_start_z) &&
            SearchHexNum(ring, rect_start_x, rect_start_z+width-1) &&
            SearchHexNum(ring, rect_start_x+height-1, rect_start_z+width-1);
        return rectInRing;
    }

    public static hexAxis GetRectAreaCenter(int start_x, int start_z, int height, int width)
    {
        //return Hexagon.NumToAxis(start_x, start_z);
        hexAxis hexA_1, hexA_2, hexA_3, hexA_4, hexA_5, hexA_6, hexA_7;
        if( width == 1)
        {
            if (height % 2 == 1)
                return Hexagon.NumToAxis(start_x+height/2, start_z);
            // height % 2 == 0
            hexA_1 = Hexagon.NumToAxis(start_x+height/2-1, start_z);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2, start_z);
            hexA_3 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x)/2,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z)/2
            };
            return hexA_3;
        }
        //height == 1 && width > 1 -> no such area

        if (width % 2 == 0 && height % 2 ==1)
        {
            hexA_1 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2-1);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2);
            hexA_3 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x)/2,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z)/2
            };
            return hexA_3;
        }
        else if (width % 2 == 0 && height % 2 ==0)
        {
            hexA_1 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2-1);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2-1);
            hexA_3 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2);
            hexA_4 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2);
            hexA_5 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x + hexA_3.axis_x + hexA_4.axis_x)/4,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z + hexA_3.axis_z + hexA_4.axis_z)/4
            };
            return hexA_5;
        }
        else if (width % 2 ==1 && height % 2 ==1)
        {
            hexA_1 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2-1);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2);
            hexA_3 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2+1);
            hexA_4 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x + hexA_3.axis_x)/3,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z + hexA_3.axis_z)/3
            };
            return hexA_4;
        }
        else if (width % 2 ==1 && height % 2 ==0)
        {
            hexA_1 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2-1);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2);
            hexA_3 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2+1);
            hexA_4 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2-1);
            hexA_5 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2);
            hexA_6 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2+1);
            hexA_7 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x + hexA_3.axis_x + hexA_4.axis_x + hexA_5.axis_x + hexA_6.axis_x)/6,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z + hexA_3.axis_z + hexA_4.axis_z + hexA_5.axis_z + hexA_6.axis_z)/6
            };
            return hexA_7;
        }
        // won't reach here
        hexA_1 = new hexAxis {axis_x=-1, axis_z=-1};
        return hexA_1;
    }

    public static int GetOneDimensionVal(int x, int z)
    {
        return x*areaLength + z;
    }

    public static bool SearchHexNum(hexNum[] array, int x, int z)
    {
        for (int i=0; i<array.Length; i++)
        {
            if (array[i].num_x == x && array[i].num_z == z) return true;
        }
        return false;
    }

    public static hexNumFloat HexNumToFloat(hexNum point)
    {
        return new hexNumFloat {
            num_x_float = point.num_x,
            num_z_float = point.num_z
        };
    }

    public static hexNumFloat Lerp(hexNumFloat start, hexNumFloat end, float ratio)
    {
        Debug.Assert(ratio>=0 && ratio<=1, "ratio out of [0, 1] in Lerp");
        hexNumFloat tmp;
        tmp.num_x_float = ratio*start.num_x_float + (1-ratio)*end.num_x_float;
        tmp.num_z_float = ratio*start.num_z_float + (1-ratio)*end.num_z_float;
        return tmp;
    }

    public static hexNumFloat RandomLerp(hexNumFloat start, hexNumFloat end, float range)
    {
        float division = UnityEngine.Random.Range(0.5f-range/2, 0.5f+range/2);
        //Debug.Log("division: "+division);
        hexNumFloat tmp = Lerp(start, end, division);
        return tmp;
    }

    public static hexNumFloat HalfLerp(hexNumFloat start, hexNumFloat end)
    {
        return Lerp(start, end, 0.5f);
    }

}
